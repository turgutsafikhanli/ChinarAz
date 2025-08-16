using ChinarAz.Application.Abstracts.Services;
using ChinarAz.Infrastructure.Services;
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
        #endregion

        #region Repositories
        #endregion
    }
}
