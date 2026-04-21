using CoreApp.Repositories;
using CoreApp.Services;
using Infrastructure.EntityFramework.Context;
using Infrastructure.EntityFramework.Entities;
using Infrastructure.EntityFramework.Repositories;
using Infrastructure.EntityFramework.UnitOfWork;
using Infrastructure.Memory;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class ContactsInfrastructureModule
{
    public static IServiceCollection AddContactsEfModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<IPersonRepositoryAsync, EfPersonRepository>();
        services.AddScoped<ICompanyRepositoryAsync, EfCompanyRepository>();
        services.AddScoped<IOrganizationRepositoryAsync, EfOrganizationRepository>();
        services.AddScoped<IContactRepositoryAsync, EfContactRepository>();

        services.AddScoped<IContactUnitOfWork, EfContactsUnitOfWork>();

        services.AddDbContext<ContactsDbContext>(options =>
            options.UseSqlite(configuration.GetConnectionString("CrmDb")));

        services.AddIdentity<CrmUser, CrmRole>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = true;
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
            })
            .AddEntityFrameworkStores<ContactsDbContext>()
            .AddDefaultTokenProviders();

        services.AddScoped<IPersonService, PersonService>();

        return services;
    }

    public static IServiceCollection AddContactsMemoryModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<IPersonRepositoryAsync, MemoryPersonRepository>();
        services.AddScoped<ICompanyRepositoryAsync, MemoryCompanyRepository>();
        services.AddScoped<IOrganizationRepositoryAsync, MemoryOrganizationRepository>();
        services.AddScoped<IContactRepositoryAsync, MemoryContactRepository>();

        services.AddScoped<IContactUnitOfWork, MemoryContactUnitOfWork>();
        services.AddScoped<IPersonService, PersonService>();

        return services;
    }
}