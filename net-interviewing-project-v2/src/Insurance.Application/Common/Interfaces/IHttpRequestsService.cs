using Insurance.Domain.Entities;

namespace Insurance.Application.Common.Interfaces;

public interface IHttpRequestsService
{
    Task<Product> GetProduct(int productId);
    Task<List<Product>> GetProducts();
    Task<ProductType> GetProductType(int productTypeId);
    Task<List<ProductType>> GetProductsTypes();
}
