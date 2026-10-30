using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace MobileStore.API.Controllers
{
    /// <summary>
    /// Base controller that provides common helper methods for all controllers
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseApiController : ControllerBase
    {
        /// <summary>
        /// Get the current authenticated user's ID from the JWT token
        /// </summary>
        protected int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                throw new UnauthorizedAccessException("User ID not found in token.");

            return int.Parse(userIdClaim);
        }

        /// <summary>
        /// Get the current authenticated user's role from the JWT token
        /// </summary>
        protected string GetCurrentUserRole()
        {
            var roleClaim = User.FindFirst(ClaimTypes.Role)?.Value;
            return roleClaim ?? "Customer";
        }

        /// <summary>
        /// Check if the current user is an admin (case-insensitive)
        /// </summary>
        protected bool IsCurrentUserAdmin()
        {
            var role = GetCurrentUserRole();
            return role.Equals("Admin", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Check if the current user is active.
        /// </summary>
        protected bool IsCurrentUserActive()
        {
            var isActiveClaim = User.FindFirst("IsActive")?.Value;
            if (string.IsNullOrEmpty(isActiveClaim))
                throw new UnauthorizedAccessException("IsActive claim not found in token.");

            return bool.Parse(isActiveClaim);
        }

        /// <summary>
        /// Check if the current user is the owner of the resource
        /// </summary>
        protected bool IsCurrentUserOwner(int resourceUserId)
        {
            return GetCurrentUserId() == resourceUserId;
        }

        /// <summary>
        /// Check if the current user is authorized to access/modify a user's data
        /// (Allows access if user is the owner OR is an admin)
        /// </summary>
        protected bool IsAuthorizedToAccessUser(int userId)
        {
            return IsCurrentUserOwner(userId) || IsCurrentUserAdmin();
        }

        /// <summary>
        /// Return a 403 Forbidden response with a custom message
        /// </summary>
        protected IActionResult Forbidden(string message = "You are not authorized to perform this action.")
        {
            return StatusCode(403, new { message });
        }
    }
}