using Insurance.Application.UseCases.OrderInsurance.Commands.CalculateOrderInsuranceCommand;
using Insurance.Domain.Entities;

namespace Insurance.Application.Common.Interfaces;

public interface IOrderService
{
    Task<Order> SaveOrderInsurance(List<OrderProductInsuranceDto> orderProductInsurances, CancellationToken cancellationToken);
    Task<Order> CalculateAdditionalInsurance(Order order, CancellationToken cancellationToken);
}
