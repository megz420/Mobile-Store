using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MobileStore.Infrastructure.Persistence
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<AppDbContext>();
            builder.UseSqlServer("Server=DESKTOP-UH84CRS\\SQLEXPRESS01;Database=mobile_store_db;Integrated Security=true;TrustServerCertificate=true;Encrypt=true;");
            return new AppDbContext(builder.Options);
        }
    }
}

