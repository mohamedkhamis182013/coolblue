using Insurance.Application.UseCases.OrderInsurance.Commands.CalculateOrderInsuranceCommand;
using Insurance.Application.UseCases.ProductInsurance.Queries.GetProducInsurance;
using Insurance.Domain.Entities;

namespace Insurance.Application.Common.Interfaces;

public interface IProductService
{
    ProductInsuranceDto GetProductDto(Product product, ProductType productType);
    float GetProductInsurance(float productPrice);
    float GetSensetiveProductsInsurance(string productTypeName);
    float GetOrderProductInsurance(OrderProductInsuranceDto orderProduct);
    float GetAdditionalInsurance(string productTpeName, Order order);
}
