using MobileStore.API.Extensions;
using MobileStore.API.Middlewares;
using MobileStore.Application.DependencyInjection;
using MobileStore.Infrastructure.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// ============================================
// 1. Add Controllers (المهم!)
// ============================================
builder.Services.AddControllers();

// ============================================
// 2. Infrastructure Services
// ============================================
builder.Services.AddInfrastructureServices(builder.Configuration);

// ============================================
// 3. Application Services
// ============================================
builder.Services.AddApplicationServices();

// ============================================
// 4. Swagger Documentation (Clean!)
// ============================================
builder.Services.AddSwaggerDocumentation();

// ============================================
// 5. JWT Authentication (Clean!)
// ============================================
builder.Services.AddJwtAuthentication(builder.Configuration);

// ============================================
// 5. Build
// ============================================
var app = builder.Build();

// ============================================
// 6. Global Exception Middleware
// ============================================
app.UseMiddleware<GlobalExceptionMiddleware>();

// ============================================
// 7. Swagger (Development only)
// ============================================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ============================================
// 8. HTTPS
// ============================================
app.UseHttpsRedirection();


app.UseAuthentication();
app.UseAuthorization();
// ============================================
// 9. Map Controllers (المهم!)
// ============================================
app.MapControllers();

// ============================================
// 10. Run
// ============================================
app.Run();