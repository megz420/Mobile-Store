using MobileStore.Application.DTOs.User;

namespace MobileStore.Application.Services.Interfaces
{
    public interface IUserService
    {
        // ============================================
        // Basic CRUD
        // ============================================
        Task<UserResponseDto> GetUserByIdAsync(int id);
        Task<IEnumerable<UserResponseDto>> GetAllUsersAsync();
        Task<IEnumerable<UserResponseDto>> GetActiveUsersAsync();
        Task<UserResponseDto> CreateUserAsync(CreateUserDto createDto);
        Task<UserResponseDto> UpdateUserAsync(int id, UpdateUserDto updateDto);
        Task<bool> DeleteUserAsync(int id);

        // ============================================
        // Authentication
        // ============================================
        Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
        Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword);
        Task<bool> ResetPasswordAsync(string email, string newPassword);

        // ============================================
        // User Management
        // ============================================
        Task<UserResponseDto> GetUserByEmailAsync(string email);
        Task<UserResponseDto> GetUserByUsernameAsync(string username);
        Task<bool> ActivateUserAsync(int userId);
        Task<bool> DeactivateUserAsync(int userId);

        // ============================================
        // Validation Helpers
        // ============================================
        Task<bool> UserExistsAsync(int id);
        Task<bool> EmailExistsAsync(string email);
        Task<bool> UsernameExistsAsync(string username);

        // ============================================
        // Analytics
        // ============================================
        Task<int> GetTotalUserCountAsync();
        Task<int> GetActiveUserCountAsync();
        Task<Dictionary<int, int>> GetUserCountByRoleAsync();
    }
}