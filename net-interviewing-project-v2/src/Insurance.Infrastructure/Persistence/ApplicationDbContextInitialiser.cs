using Insurance.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Insurance.Infrastructure.Persistence;

public class ApplicationDbContextInitialiser
{
    private readonly ILogger<ApplicationDbContextInitialiser> _logger;
    private readonly ApplicationDbContext _context;
    private readonly IHttpRequestsService _httpRequestsService;

    public ApplicationDbContextInitialiser(ILogger<ApplicationDbContextInitialiser> logger, ApplicationDbContext context, IHttpRequestsService httpRequestsService)
    {
        _logger = logger;
        _context = context;
        _httpRequestsService = httpRequestsService;
    }
    public async Task InitialiseAsync()
    {
        try
        {
            if (_context.Database.IsSqlServer())
            {
                await _context.Database.MigrateAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    public async Task TrySeedAsync()
    {

        if (!_context.ProductList.Any())
        {
            _context.ProductList.AddRange(await _httpRequestsService.GetProducts());

            await _context.SaveChangesAsync();
        }
        if (!_context.ProductTypeList.Any())
        {
            _context.ProductTypeList.AddRange(await _httpRequestsService.GetProductsTypes());

            await _context.SaveChangesAsync();
        }
    }


}
