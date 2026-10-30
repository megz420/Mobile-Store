using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MobileStore.Application.DTOs.User;
using MobileStore.Application.Services.Interfaces;
using MobileStore.Domain.Enums;

namespace MobileStore.API.Controllers
{
    /// <summary>
    /// Controller for managing users and authentication
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(IUserService userService) : ControllerBase
    {
        private readonly IUserService _userService = userService;


        // ============================================
        // Helper Methods
        // ============================================

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                throw new UnauthorizedAccessException("User ID not found in token.");
            
            return int.Parse(userIdClaim);
        }

        private string GetCurrentUserRole()
        {
            var roleClaim = User.FindFirst(ClaimTypes.Role)?.Value;
            return roleClaim ?? "customer";
        }

        // ============================================
        // Public Endpoints (No Authorization Required)
        // ============================================

        /// <summary>
        /// Register a new user account
        /// </summary>
        [HttpPost("register")]
        [ProducesResponseType(typeof(UserResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserResponseDto>> Register([FromBody] CreateUserDto createDto)
        {
            var user = await _userService.CreateUserAsync(createDto);
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }

        /// <summary>
        /// Login and receive JWT token
        /// </summary>
        [HttpPost("login")]
        [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto loginDto)
        {
            var authResponse = await _userService.LoginAsync(loginDto);
            return Ok(authResponse);
        }

        // ============================================
        // Admin Only Endpoints
        // ============================================

        /// <summary>
        /// Get all users (requires admin role)
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UserResponseDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<UserResponseDto>>> GetAll()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        /// <summary>
        /// Get only active users (requires admin role)
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpGet("active")]
        [ProducesResponseType(typeof(IEnumerable<UserResponseDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<UserResponseDto>>> GetActive()
        {
            var users = await _userService.GetActiveUsersAsync();
            return Ok(users);
        }

        /// <summary>
        /// Get user by email (requires admin role)
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpGet("by-email/{email}")]
        [ProducesResponseType(typeof(UserResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserResponseDto>> GetByEmail(string email)
        {
            var user = await _userService.GetUserByEmailAsync(email);
            return Ok(user);
        }

        /// <summary>
        /// Get user by username (requires admin role)
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpGet("by-username/{username}")]
        [ProducesResponseType(typeof(UserResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserResponseDto>> GetByUsername(string username)
        {
            var user = await _userService.GetUserByUsernameAsync(username);
            return Ok(user);
        }

        /// <summary>
        /// Soft delete a user (requires admin role)
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            await _userService.DeleteUserAsync(id);
            return NoContent();
        }

        /// <summary>
        /// Activate a user account (requires admin role)
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpPatch("{id:int}/activate")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ActivateUser(int id)
        {
            await _userService.ActivateUserAsync(id);
            return NoContent();
        }

        /// <summary>
        /// Deactivate a user account (requires admin role)
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpPatch("{id:int}/deactivate")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeactivateUser(int id)
        {
            await _userService.DeactivateUserAsync(id);
            return NoContent();
        }

        /// <summary>
        /// Get total number of registered users (requires admin role)
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpGet("count")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        public async Task<ActionResult<int>> GetTotalCount()
        {
            var count = await _userService.GetTotalUserCountAsync();
            return Ok(count);
        }

        /// <summary>
        /// Get number of active users (requires admin role)
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpGet("count/active")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        public async Task<ActionResult<int>> GetActiveCount()
        {
            var count = await _userService.GetActiveUserCountAsync();
            return Ok(count);
        }

        /// <summary>
        /// Get number of users grouped by role (requires admin role)
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpGet("count-by-role")]
        [ProducesResponseType(typeof(Dictionary<string, int>), StatusCodes.Status200OK)]
        public async Task<ActionResult<Dictionary<string, int>>> GetCountByRole()
        {
            var counts = await _userService.GetUserCountByRoleAsync();
            return Ok(counts);
        }

        // ============================================
        // User + Admin Endpoints
        // ============================================

        /// <summary>
        /// Get user by ID (user can get their own profile, admin can get any)
        /// </summary>
        [Authorize]
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(UserResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserResponseDto>> GetById(int id)
        {
            var currentUserId = GetCurrentUserId();
            var currentUserRole = GetCurrentUserRole();

            if (currentUserId != id && currentUserRole != "Admin")
                return Forbid("You can only access your own profile.");

            var user = await _userService.GetUserByIdAsync(id);
            return Ok(user);
        }

        /// <summary>
        /// Update user details (user can update their own profile only)
        /// </summary>
        [Authorize]
        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(UserResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserResponseDto>> Update(
            int id,
            [FromBody] UpdateUserDto updateDto)
        {
            var currentUserId = GetCurrentUserId();
            
            if (currentUserId != id)
                return Forbid("You can only update your own profile.");

            var user = await _userService.UpdateUserAsync(id, updateDto);
            return Ok(user);
        }

        /// <summary>
        /// Change password for the authenticated user
        /// </summary>
        [Authorize]
        [HttpPatch("{id:int}/change-password")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ChangePassword(
            int id,
            [FromBody] ChangePasswordDto changePasswordDto)
        {
            var currentUserId = GetCurrentUserId();
            
            if (currentUserId != id)
                return Forbid("You can only change your own password.");

            var result = await _userService.ChangePasswordAsync(
                id,
                changePasswordDto.CurrentPassword,
                changePasswordDto.NewPassword);

            if (!result)
                return BadRequest(new { message = "Password change failed." });

            return NoContent();
        }
    }
}