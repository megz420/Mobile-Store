using AutoMapper;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using MobileStore.Application.DTOs.User;
using MobileStore.Application.Helpers;
using MobileStore.Application.Services.Interfaces;
using MobileStore.Domain.Entities;
using MobileStore.Domain.Enums;
using MobileStore.Infrastructure.Repositories.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace MobileStore.Application.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public UserService(
            IUserRepository userRepository,
            IMapper mapper,
            IConfiguration configuration)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _configuration = configuration;
        }

        // ============================================
        // Basic CRUD
        // ============================================

        public async Task<UserResponseDto> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"User with ID {id} not found.");

            return _mapper.Map<UserResponseDto>(user);
        }

        public async Task<IEnumerable<UserResponseDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<UserResponseDto>>(users);
        }

        public async Task<IEnumerable<UserResponseDto>> GetActiveUsersAsync()
        {
            var users = await _userRepository.GetActiveUsersAsync();
            return _mapper.Map<IEnumerable<UserResponseDto>>(users);
        }

        public async Task<UserResponseDto> CreateUserAsync(CreateUserDto createDto)
        {
            // Validate email uniqueness
            if (await _userRepository.EmailExistsAsync(createDto.Email))
                throw new InvalidOperationException($"Email '{createDto.Email}' is already registered.");

            // Validate username uniqueness
            if (await _userRepository.UsernameExistsAsync(createDto.Username))
                throw new InvalidOperationException($"Username '{createDto.Username}' is already taken.");

            // Validate role
            if (!IsValidRole(createDto.Role))
                throw new ArgumentException($"Invalid role: {createDto.Role}");

            var user = _mapper.Map<User>(createDto);
            var created = await _userRepository.CreateAsync(user);

            return _mapper.Map<UserResponseDto>(created);
        }

        public async Task<UserResponseDto> UpdateUserAsync(int id, UpdateUserDto updateDto)
        {
            var user = await _userRepository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"User with ID {id} not found.");

            // Validate email uniqueness if email is being updated
            if (!string.IsNullOrEmpty(updateDto.Email) &&
                await _userRepository.EmailExistsAsync(updateDto.Email, id))
                throw new InvalidOperationException($"Email '{updateDto.Email}' is already taken.");

            // Validate username uniqueness if username is being updated
            if (!string.IsNullOrEmpty(updateDto.Username) &&
                await _userRepository.UsernameExistsAsync(updateDto.Username, id))
                throw new InvalidOperationException($"Username '{updateDto.Username}' is already taken.");

            // Validate role if being updated
            if (updateDto.Role.HasValue && !IsValidRole(updateDto.Role.Value))
                throw new ArgumentException($"Invalid role: {updateDto.Role}");

            _mapper.Map(updateDto, user);
            var updated = await _userRepository.UpdateAsync(user);

            return _mapper.Map<UserResponseDto>(updated);
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            if (!await _userRepository.ExistsAsync(id))
                throw new KeyNotFoundException($"User with ID {id} not found.");

            return await _userRepository.DeleteAsync(id);
        }

        // ============================================
        // Authentication
        // ============================================

        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _userRepository.GetByEmailAsync(loginDto.Email)
                ?? throw new UnauthorizedAccessException("Invalid email or password.");

            if (!user.IsActive)
                throw new UnauthorizedAccessException("Account is deactivated.");

            // Verify password
            if (!PasswordHashHelper.VerifyPassword(loginDto.Password, user.PasswordHash))
                throw new UnauthorizedAccessException("Invalid email or password.");


            // Generate token (you'll need JWT configuration)
            var token = GenerateJwtToken(user);

            return new AuthResponseDto
            {
                Token = token,
                RefreshToken = PasswordHashHelper.GenerateRandomToken(),
                User = _mapper.Map<UserResponseDto>(user),
                ExpiresAt = DateTime.UtcNow.AddHours(24)
            };
        }

        public async Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword)
        {
            var user = await _userRepository.GetByIdAsync(userId)
                ?? throw new KeyNotFoundException($"User with ID {userId} not found.");

            // Verify current password
            if (!PasswordHashHelper.VerifyPassword(currentPassword, user.PasswordHash))
                throw new UnauthorizedAccessException("Current password is incorrect.");

            // Hash new password
            var newHash = PasswordHashHelper.HashPassword(newPassword);

            return await _userRepository.UpdatePasswordAsync(userId, newHash);
        }

        public async Task<bool> ResetPasswordAsync(string email, string newPassword)
        {
            var user = await _userRepository.GetByEmailAsync(email)
                ?? throw new KeyNotFoundException($"User with email '{email}' not found.");

            var newHash = PasswordHashHelper.HashPassword(newPassword);
            return await _userRepository.UpdatePasswordAsync(user.Id, newHash);
        }

        // ============================================
        // User Management
        // ============================================

        public async Task<UserResponseDto> GetUserByEmailAsync(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email)
                ?? throw new KeyNotFoundException($"User with email '{email}' not found.");

            return _mapper.Map<UserResponseDto>(user);
        }

        public async Task<UserResponseDto> GetUserByUsernameAsync(string username)
        {
            var user = await _userRepository.GetByUsernameAsync(username)
                ?? throw new KeyNotFoundException($"User with username '{username}' not found.");

            return _mapper.Map<UserResponseDto>(user);
        }

        public async Task<bool> ActivateUserAsync(int userId)
        {
            if (!await _userRepository.ExistsAsync(userId))
                throw new KeyNotFoundException($"User with ID {userId} not found.");

            return await _userRepository.ActivateUserAsync(userId);
        }

        public async Task<bool> DeactivateUserAsync(int userId)
        {
            if (!await _userRepository.ExistsAsync(userId))
                throw new KeyNotFoundException($"User with ID {userId} not found.");

            return await _userRepository.DeactivateUserAsync(userId);
        }

        // ============================================
        // Validation Helpers
        // ============================================

        public async Task<bool> UserExistsAsync(int id)
        {
            return await _userRepository.ExistsAsync(id);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _userRepository.EmailExistsAsync(email);
        }

        public async Task<bool> UsernameExistsAsync(string username)
        {
            return await _userRepository.UsernameExistsAsync(username);
        }

        // ============================================
        // Analytics
        // ============================================

        public async Task<int> GetTotalUserCountAsync()
        {
            return await _userRepository.GetTotalUserCountAsync();
        }

        public async Task<int> GetActiveUserCountAsync()
        {
            return await _userRepository.GetActiveUserCountAsync();
        }

        public async Task<Dictionary<int, int>> GetUserCountByRoleAsync()
        {
            return await _userRepository.GetUserCountByRoleAsync();
        }

        // ============================================
        // Private Helpers
        // ============================================

        private static bool IsValidRole(UserRole role)
        {
            return Enum.IsDefined(typeof(UserRole), role);
        }

        private string GenerateJwtToken(User user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"]
                ?? throw new InvalidOperationException("JWT SecretKey is missing from configuration.");
            var issuer = jwtSettings["Issuer"]
                ?? throw new InvalidOperationException("JWT Issuer is missing from configuration.");
            var audience = jwtSettings["Audience"]
                ?? throw new InvalidOperationException("JWT Audience is missing from configuration.");
            var expiryMinutes = int.TryParse(jwtSettings["ExpiryMinutes"], out var configuredExpiryMinutes)
                ? configuredExpiryMinutes
                : 60;

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim("IsActive", user.IsActive.ToString())
            };

            var credentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}