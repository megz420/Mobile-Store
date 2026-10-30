using MobileStore.Domain.Entities;
using MobileStore.Infrastructure.Repositories.Interfaces;
using MobileStore.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MobileStore.Infrastructure.Repositories.Implementations
{
    public class BrandRepository : IBrandRepository
    {
        private readonly AppDbContext _context;  // DbContext instance for database operations
        public BrandRepository(AppDbContext context)  // Constructor that takes a DbContext instance as a parameter
        {
            _context = context;
        }

        public async Task<Brand?> GetByIdAsync(int id) // Method to retrieve a Brand entity by its ID
        {
            return await _context.Brands
                .Include(b => b.Products)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<IEnumerable<Brand>> GetAllAsync()
        {
            return await _context.Brands
                .Include(b => b.Products)
                .ToListAsync();  // Retrieve all Brand entities from the database, including their associated Products
        }

        public async Task<IEnumerable<Brand>> GetActiveBrandsAsync()
        {
            return await _context.Brands
                .Where(b => b.IsActive)
                .Include(b => b.Products)
                .ToListAsync();
        }

        public async Task<int> GetProductCountAsync(int brandId)
        {
            return await _context.Products
                .CountAsync(p => p.BrandId == brandId);
        }

        public async Task<Brand> CreateAsync(Brand brand)
        {
            _context.Brands.Add(brand);
            await _context.SaveChangesAsync();
            return brand;
        }

        public async Task<Brand> UpdateAsync(Brand brand)
        {
            _context.Brands.Update(brand);
            await _context.SaveChangesAsync();
            return brand;
        }

        public async Task<bool> DeleteAsync(int id)
        {
                var brand = await _context.Brands.FindAsync(id);
                if (brand == null)
                    return false;
                
                _context.Brands.Remove(brand);
                await _context.SaveChangesAsync();
                return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            var Brand = await _context.Brands.FindAsync(id);
            return Brand != null;
        }

        public Task<bool> ExistsByNameAsync(string name, int? excludeId)
        {
            var query = _context.Brands.AsQueryable();

            if (excludeId.HasValue)
            {
                query = query.Where(b => b.Id != excludeId.Value);
            }

            var exists = query.Any(b => b.Name == name);
            return Task.FromResult(exists);
        }
        }
    }

