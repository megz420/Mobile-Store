using MobileStore.Domain.Entities;

namespace MobileStore.Infrastructure.Repositories.Interfaces
{
    public interface IUserRepository
    {
        // ============================================
        // Basic CRUD
        // ============================================
        Task<User?> GetByIdAsync(int id);
        Task<IEnumerable<User>> GetAllAsync();
        Task<IEnumerable<User>> GetActiveUsersAsync();
        Task<User> CreateAsync(User user);
        Task<User> UpdateAsync(User user);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);

        // ============================================
        // Authentication & Validation
        // ============================================
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByUsernameAsync(string username);
        Task<bool> EmailExistsAsync(string email, int? excludeId = null);
        Task<bool> UsernameExistsAsync(string username, int? excludeId = null);

        // ============================================
        // User Management
        // ============================================
        Task<bool> UpdatePasswordAsync(int userId, string newPasswordHash);
        Task<bool> ActivateUserAsync(int userId);
        Task<bool> DeactivateUserAsync(int userId);

                // ============================================
        // Analytics
        // ============================================
        Task<int> GetTotalUserCountAsync();
        Task<int> GetActiveUserCountAsync();
        Task<Dictionary<int, int>> GetUserCountByRoleAsync();
    }
}