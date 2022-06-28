using Insurance.Application.Common.Interfaces;
using Insurance.Infrastructure.Persistence;
using Insurance.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace Insurance.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
               options.UseInMemoryDatabase("CoolBlueDb"));
        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
        services.AddScoped<ApplicationDbContextInitialiser>();
        services.AddHttpClient<IHttpRequestsService, HttpRequestsService>(x => x.BaseAddress = new Uri(configuration.GetSection("ProductApi").Value));
        services.AddTransient<IProductService, ProductService>();
        services.AddTransient<IOrderService, OrderService>();
        return services;
    }

}
