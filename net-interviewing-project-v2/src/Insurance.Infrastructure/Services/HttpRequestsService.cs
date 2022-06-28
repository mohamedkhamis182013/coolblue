using Insurance.Application.Common.Interfaces;
using Insurance.Domain.Entities;
using Newtonsoft.Json;

namespace Insurance.Infrastructure.Services;

public class HttpRequestsService : IHttpRequestsService
{

    private readonly HttpClient _httpClient;
    public HttpRequestsService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    public async Task<Product> GetProduct(int productId)
    {
        var result = await _httpClient.GetAsync(string.Format("/products/{0:G}", productId));
        string json = await result.Content.ReadAsStringAsync();
        var product = JsonConvert.DeserializeObject<Product>(json);
        return product;
    }

    public async Task<List<Product>> GetProducts()
    {
        var result = await _httpClient.GetAsync("/products");
        string json = await result.Content.ReadAsStringAsync();
        var products = JsonConvert.DeserializeObject<List<Product>>(json);
        return products;
    }

    public async Task<List<ProductType>> GetProductsTypes()
    {
        var result = await _httpClient.GetAsync("/product_types");
        string json = await result.Content.ReadAsStringAsync();
        var productTypes = JsonConvert.DeserializeObject<List<ProductType>>(json);
        return productTypes;
    }

    public async Task<ProductType> GetProductType(int productTypeId)
    {
        var result = await _httpClient.GetAsync($"/product_types/{productTypeId}");
        string json = await result.Content.ReadAsStringAsync();
        var productType = JsonConvert.DeserializeObject<ProductType>(json);
        return productType;
    }
}
