using Insurance.Application.Common.Interfaces;
using Insurance.Application.UseCases.OrderInsurance.Commands.CalculateOrderInsuranceCommand;
using Insurance.Application.UseCases.ProductInsurance.Queries.GetProducInsurance;
using Insurance.Domain.Entities;
using Insurance.Infrastructure.InsurenceChainOfResponsibilities;

namespace Insurance.Infrastructure.Services;

public class ProductService : IProductService
{
    private readonly List<string> _sensitiveDevices;
    private readonly IApplicationDbContext _applicationDbContext;
    public ProductService(IApplicationDbContext applicationDbContext)
    {
        _sensitiveDevices = new List<string>
        {
            "Laptops",
            "Smartphones"
        };
        _applicationDbContext = applicationDbContext;
    }
    public ProductInsuranceDto GetProductDto(Product product, ProductType productType)
    {
        return new ProductInsuranceDto
        {
            ProductId = product.Id,
            ProductTypeHasInsurance = productType.CanBeInsured,
            ProductTypeName = productType.Name,
            SalesPrice = product.SalesPrice
        };
    }

    public float GetProductInsurance(float productPrice)
    {
        Link chain = new FirstLink();
        Link secondLink = new SecondLink();
        Link thirdLink = new ThirdLink();
        chain.SetSuccessor(secondLink);
        secondLink.SetSuccessor(thirdLink);
        return chain.Execute(productPrice);
    }

    public float GetSensetiveProductsInsurance(string productTypeName)
    {
        if (_sensitiveDevices.Count(x => x.Equals(productTypeName)) == 1)
            return 500;
        else
            return 0;
    }

    private Product? GetProduct(int id)
    {
        return _applicationDbContext.ProductList.FirstOrDefault(x => x.Id == id);
    }

    private ProductType? GetProductType(int id)
    {
        return _applicationDbContext.ProductTypeList.FirstOrDefault(x => x.Id == id);
    }
    private int? GetProductTypeIdByName(string productTypeName)
    {
        return _applicationDbContext.ProductTypeList.FirstOrDefault(x => x.Name == productTypeName)?.Id;
    }
    public float GetOrderProductInsurance(OrderProductInsuranceDto orderProduct)
    {
        var product = GetProduct(orderProduct.ProductId);
        if (product == null)
            return 0;
        var productType = GetProductType(product.ProductTypeId);
        if (productType == null)
            return 0;
        var productDto = GetProductDto(product, productType);
        var productInsurance = GetProductInsurance(productDto.SalesPrice);
        productInsurance += GetSensetiveProductsInsurance(productDto.ProductTypeName ?? "");

        return productInsurance * orderProduct.Quantity;
    }

    public float GetAdditionalInsurance(string productTpeName, Order order)
    {
        var productTypeId = GetProductTypeIdByName(productTpeName);
        if (productTypeId == null)
            return 0;
        var ProductsIdList = order.OrderOrderProducts.Select(x => x.ProductId).Distinct().ToList();
        var productTypesIdList = _applicationDbContext.ProductList.Where(x => ProductsIdList.Contains(x.Id)).Select(y => y.ProductTypeId).ToList();
        if (productTypesIdList.Any(x => x == productTypeId))
            return 500;
        else return 0;

    }
}
