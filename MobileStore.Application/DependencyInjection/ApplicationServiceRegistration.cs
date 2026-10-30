using MobileStore.Application.Mappings;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using MobileStore.Application.Services.Interfaces;
using MobileStore.Application.Services.Implementations;
using MobileStore.Application.DTOs.Brand;

namespace MobileStore.Application.DependencyInjection
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(
            this IServiceCollection services)
        {
            // ============================================
            // 1. Register AutoMapper
            // ============================================
            services.AddAutoMapper(cfg => cfg.AddProfile<AutoMapperProfile>());

            // ============================================
            // 2. Register FluentValidation (if using)
            // ============================================
            services.AddValidatorsFromAssemblyContaining<CreateBrandDto>();

            // ============================================
            // 3. Register Services (Scoped)
            // ============================================
            services.AddScoped<IBrandService, BrandService>();
            services.AddScoped<ICategoryService, CategoryService>(); // if exists
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IProductImageService, ProductImageService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IOrderItemService, OrderItemService>();
            services.AddScoped<IReviewService, ReviewService>();
            services.AddScoped<IUserService, UserService>();

            return services;
        }
    }
}