using ChinarAz.Application.Abstracts.Repositories;
using ChinarAz.Application.Abstracts.Services;
using ChinarAz.Infrastructure.Services;
using ChinarAz.Persistence.Repositories;
using ChinarAz.Persistence.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ChinarAz.Persistence;

public static class ServiceRegistration
{
    public static void RegisterService(this IServiceCollection services)
    {
        #region Services
        services.AddScoped<IFileUploadService, FileUploadService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IBioService, BioService>();
        services.AddScoped<IBlogService, BlogService>();
        services.AddScoped<IFavouriteService, FavouriteService>();
        services.AddScoped<ICacheService, CacheService>();
        services.AddScoped<IElasticService, ElasticService>();
        #endregion

        #region Repositories
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IOrderProductRepository, OrderProductRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IBioRepository, BioRepository>();
        services.AddScoped<IBlogRepository, BlogRepository>();
        services.AddScoped<IFavouriteRepository, FavouriteRepository>();
        #endregion
    }
}
