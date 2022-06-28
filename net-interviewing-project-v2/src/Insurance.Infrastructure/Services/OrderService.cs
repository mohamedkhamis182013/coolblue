using Insurance.Application.Common.Interfaces;
using Insurance.Application.UseCases.OrderInsurance.Commands.CalculateOrderInsuranceCommand;
using Insurance.Domain.Entities;

namespace Insurance.Infrastructure.Services;

public class OrderService : IOrderService
{
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly IProductService _productService;
    private readonly string _productTypeAdditionalInsurance;
    public OrderService(IApplicationDbContext applicationDbContext, IProductService productService)
    {
        _applicationDbContext = applicationDbContext;
        _productService = productService;
        _productTypeAdditionalInsurance = "Digital cameras";
    }

    public async Task<Order> SaveOrderInsurance(List<OrderProductInsuranceDto> orderProductInsurances, CancellationToken cancellationToken)
    {
        var order = new Order();
        float orderInsurance = 0;
        foreach (var orderProduct in orderProductInsurances)
        {
            var orderProductInsurance = _productService.GetOrderProductInsurance(orderProduct);
            orderInsurance += orderProductInsurance;
            order.OrderOrderProducts.Add(
                new OrderProduct
                {
                    ProductId = orderProduct.ProductId,
                    InsuranceValue = orderProductInsurance,
                    Quantity = orderProduct.Quantity
                }
                );
        }
        order.Insurance = orderInsurance;
        _applicationDbContext.OrderList.Add(order);
        await _applicationDbContext.SaveChangesAsync(cancellationToken);
        return order;
    }

    public async Task<Order> CalculateAdditionalInsurance(Order order, CancellationToken cancellationToken)
    {
        var additionalInsurance = _productService.GetAdditionalInsurance(_productTypeAdditionalInsurance, order);
        if (additionalInsurance > 0)
        {
            var orderEntity = await _applicationDbContext.OrderList
          .FindAsync(new object[] { order.Id }, cancellationToken);
            if (orderEntity != null)
            {
                orderEntity.AdditionalInsurance = additionalInsurance;
                await _applicationDbContext.SaveChangesAsync(cancellationToken);
            }

        }
        return order;
    }
}
