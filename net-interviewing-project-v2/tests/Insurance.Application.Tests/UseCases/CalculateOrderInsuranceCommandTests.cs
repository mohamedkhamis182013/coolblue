
using Insurance.Application.Common.Interfaces;
using Insurance.Application.UseCases.OrderInsurance.Commands.CalculateOrderInsuranceCommand;
using Moq;
using Insurance.Domain.Entities;

namespace Insurance.Application.Tests.UseCases;

[TestClass]
public class CalculateOrderInsuranceCommandTests
{
    [TestMethod]
    public async Task WhenOrder_hasorderProducts_CalculateTotalInsurance()
    {
        //Arrange
        var orderService = new Mock<IOrderService>();
        var cancellationtoken = new CancellationToken();
        var orderProducts = new List<OrderProductInsuranceDto> 
        {
            new OrderProductInsuranceDto{ProductId = 1 , Quantity=1 },
            new OrderProductInsuranceDto{ProductId = 2 , Quantity=1 },
        };
        var order = new Order { 
        Id = 1,
        Insurance=1000,
        };
        orderService.Setup(x => x.SaveOrderInsurance(orderProducts, cancellationtoken)).ReturnsAsync(order);
        orderService.Setup(x => x.CalculateAdditionalInsurance(order, cancellationtoken)).ReturnsAsync(order);
        var command = new CalculateOrderInsuranceCommand { orderProductInsurances = orderProducts };
        var handler = new CalculateOrderInsuranceCommandHandler(orderService.Object);
        //Act
        var result = await handler.Handle(command, cancellationtoken);
        //Assert
        Assert.AreEqual(result.productsInsurance, order.Insurance);
        
    }

    [TestMethod]
    public async Task WhenOrder_HasDigitalCamerasInOrderProducts_CalculateAdditionalInsurance()
    {
        //Arrange
        var orderService = new Mock<IOrderService>();
        var cancellationtoken = new CancellationToken();
        var orderProducts = new List<OrderProductInsuranceDto>
        {
            new OrderProductInsuranceDto{ProductId = 1 , Quantity=1 },
            new OrderProductInsuranceDto{ProductId = 2 , Quantity=1 },
        };
        var order = new Order
        {
            Id = 1,
            Insurance = 1000,
        };
        var orderWithAdditionalInsurance = new Order
        {
            Id = 1,
            Insurance = 1000,
            AdditionalInsurance=500
        };
        orderService.Setup(x => x.SaveOrderInsurance(orderProducts, cancellationtoken)).ReturnsAsync(order);
        orderService.Setup(x => x.CalculateAdditionalInsurance(order, cancellationtoken)).ReturnsAsync(orderWithAdditionalInsurance);
        var command = new CalculateOrderInsuranceCommand { orderProductInsurances = orderProducts };
        var handler = new CalculateOrderInsuranceCommandHandler(orderService.Object);
        //Act
        var result = await handler.Handle(command, cancellationtoken);
        //Assert
        Assert.AreEqual(result.productsInsurance, orderWithAdditionalInsurance.Insurance);
        Assert.AreEqual(result.AdditionalInsurance, orderWithAdditionalInsurance.AdditionalInsurance);
        Assert.AreEqual(result.totalInsurance, orderWithAdditionalInsurance.Insurance+ orderWithAdditionalInsurance.AdditionalInsurance);

    }

}
