using Insurance.Application.Common.Interfaces;
using MediatR;

namespace Insurance.Application.UseCases.OrderInsurance.Commands.CalculateOrderInsuranceCommand;

public record CalculateOrderInsuranceCommand : IRequest<OrderInsuranceVm>
{
    public List<OrderProductInsuranceDto> orderProductInsurances { get; set; } = new List<OrderProductInsuranceDto>();
}

public class CalculateOrderInsuranceCommandHandler : IRequestHandler<CalculateOrderInsuranceCommand, OrderInsuranceVm>
{
    private readonly IOrderService _orderService;

    public CalculateOrderInsuranceCommandHandler(IOrderService orderService)
    {
        _orderService = orderService;
    }

    public async Task<OrderInsuranceVm> Handle(CalculateOrderInsuranceCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderService.SaveOrderInsurance(request.orderProductInsurances, cancellationToken);
        order = await _orderService.CalculateAdditionalInsurance(order, cancellationToken);
        return new OrderInsuranceVm
        {
            orderId = order.Id,
            AdditionalInsurance = order.AdditionalInsurance,
            productsInsurance = order.Insurance,
            totalInsurance = order.AdditionalInsurance + order.Insurance
        };
    }

}