using Insurance.Application.UseCases.OrderInsurance.Commands.CalculateOrderInsuranceCommand;
using Insurance.Application.UseCases.ProductInsurance.Queries.GetProducInsurance;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Insurance.Api.Controllers;

public class InsuranceController : ControllerBase
{

    [HttpPost]
    [Route("api/insurance/product")]
    public async Task<ActionResult<ProducInsuranceVm>> CalculateInsurance(GetProductInsuranceQuery toInsure)
    {
        if (toInsure == null || toInsure.ProductId == 0)
            return null;

        return await Mediator.Send(toInsure);
    }


    [HttpPost]
    [Route("api/insurance/Order")]
    public async Task<ActionResult<OrderInsuranceVm>> CalculateOrderInsurance([FromBody] CalculateOrderInsuranceCommand orderToInsure)
    {
        if (orderToInsure == null || orderToInsure.orderProductInsurances.Count == 0)
            return null;

        return await Mediator.Send(orderToInsure);
    }
}