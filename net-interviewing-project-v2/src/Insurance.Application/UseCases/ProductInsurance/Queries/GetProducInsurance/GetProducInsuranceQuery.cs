using Insurance.Application.Common.Interfaces;
using MediatR;

namespace Insurance.Application.UseCases.ProductInsurance.Queries.GetProducInsurance;

public record GetProductInsuranceQuery : IRequest<ProducInsuranceVm>
{
    public int ProductId { get; set; }
    public float InsuranceValue { get; set; }
}

public class GetProductInsuranceQueryHandler : IRequestHandler<GetProductInsuranceQuery, ProducInsuranceVm>
{
    private readonly IHttpRequestsService _httpRequestsService;
    private readonly IProductService _productService;

    public GetProductInsuranceQueryHandler(IHttpRequestsService httpRequestsService, IProductService productService)
    {
        _httpRequestsService = httpRequestsService;
        _productService = productService;
    }

    public async Task<ProducInsuranceVm> Handle(GetProductInsuranceQuery request, CancellationToken cancellationToken)
    {
        var product = await _httpRequestsService.GetProduct(request.ProductId);
        if (product == null || product.Id == 0) return new ProducInsuranceVm();
        var productType = await _httpRequestsService.GetProductType(product.ProductTypeId);
        if (productType == null || productType.Id == 0) return new ProducInsuranceVm();
        var productInsuranceDto = _productService.GetProductDto(product, productType);
        if (!productInsuranceDto.ProductTypeHasInsurance)
            return new ProducInsuranceVm
            {
                ProductId = product.Id,
                InsuranceValue = 0
            };
        var insuranceValue = _productService.GetProductInsurance(productInsuranceDto.SalesPrice);
        insuranceValue += _productService.GetSensetiveProductsInsurance(productInsuranceDto.ProductTypeName ?? "");
        return new ProducInsuranceVm
        {
            ProductId = product.Id,
            InsuranceValue = insuranceValue
        };
    }
}