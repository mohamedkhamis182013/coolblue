using FluentValidation.AspNetCore;
using Insurance.Api.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace Insurance.Api;

public static class ConfigureServices
{
    public static IServiceCollection AddWebUIServices(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddControllersWithViews(options =>
         options.Filters.Add<ApiExceptionFilterAttribute>())
             .AddFluentValidation(x => x.AutomaticValidationEnabled = true);
        services.AddSwaggerGen();
        return services;
    }
}
