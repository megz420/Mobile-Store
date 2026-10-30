using Microsoft.EntityFrameworkCore;
using MobileStore.Domain.Entities;
using MobileStore.Infrastructure.Repositories.Interfaces;
using MobileStore.Infrastructure.Persistence;

namespace MobileStore.Infrastructure.Repositories.Implementations
{
    public class ProductImageRepository : IProductImageRepository
    {
        private readonly AppDbContext _context;

        public ProductImageRepository(AppDbContext context)
        {
            _context = context;
        }

        // ============================================
        // Basic CRUD
        // ============================================

        public async Task<ProductImage?> GetByIdAsync(int id)
        {
            return await _context.ProductImages
                .Include(pi => pi.Product)
                .FirstOrDefaultAsync(pi => pi.Id == id);
        }

        public async Task<IEnumerable<ProductImage>> GetAllAsync()
        {
            return await _context.ProductImages
                .Include(pi => pi.Product)
                .OrderBy(pi => pi.ProductId)
                .ToListAsync();
        }

        public async Task<IEnumerable<ProductImage>> GetImagesByProductIdAsync(int productId)
        {
            return await _context.ProductImages
                .Where(pi => pi.ProductId == productId)
                .OrderBy(pi => pi.CreatedAt)
                .ToListAsync();
        }

        public async Task<ProductImage> CreateAsync(ProductImage image)
        {
            image.CreatedAt = DateTime.UtcNow;
            await _context.ProductImages.AddAsync(image);
            await _context.SaveChangesAsync();
            return image;
        }

        public async Task<ProductImage> UpdateAsync(ProductImage image)
        {
            _context.ProductImages.Update(image);
            await _context.SaveChangesAsync();
            return image;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var image = await _context.ProductImages.FindAsync(id);
            if (image == null)
                return false;

            _context.ProductImages.Remove(image);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.ProductImages.AnyAsync(pi => pi.Id == id);
        }

        // ============================================
        // Product Image Management
        // ============================================

        public async Task<ProductImage?> GetPrimaryImageByProductIdAsync(int productId)
        {
            return await _context.ProductImages
                .Where(pi => pi.ProductId == productId)
                .OrderBy(pi => pi.CreatedAt)
                .FirstOrDefaultAsync();
        }


        public async Task<bool> DeleteAllImagesByProductIdAsync(int productId)
        {
            var images = _context.ProductImages.Where(pi => pi.ProductId == productId);
            if (!await images.AnyAsync())
                return false;

            _context.ProductImages.RemoveRange(images);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> GetImageCountByProductIdAsync(int productId)
        {
            return await _context.ProductImages
                .CountAsync(pi => pi.ProductId == productId);
        }
    }
}