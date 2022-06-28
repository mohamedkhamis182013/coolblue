using Insurance.Application.Common.Interfaces;
using Insurance.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Insurance.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }
    public DbSet<Product> ProductList => Set<Product>();

    public DbSet<ProductType> ProductTypeList => Set<ProductType>();

    public DbSet<Order> OrderList => Set<Order>();

    public DbSet<OrderProduct> OrderProductList => Set<OrderProduct>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }


    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) => await base.SaveChangesAsync(cancellationToken);
}
