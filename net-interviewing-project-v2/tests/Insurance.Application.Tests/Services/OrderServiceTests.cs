using Insurance.Application.Common.Interfaces;
using Insurance.Application.UseCases.OrderInsurance.Commands.CalculateOrderInsuranceCommand;
using Insurance.Domain.Entities;
using Insurance.Infrastructure.Persistence;
using Insurance.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Insurance.Application.Tests.Services;

[TestClass]
public class OrderServiceTests
{
    private ApplicationDbContext _context;
    [TestInitialize]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "cooolblueDb")
            .Options;
        _context = new ApplicationDbContext(options);

    }

    [TestMethod]
    public async Task WhenOrderProducts_AreNotNull_GetInsurancevalue()
    {
        //Arrange
        float firstOrderProductInsurance = 1000;
        float secondOrderProductInsurance = 500;
        var cancellationToken = new CancellationToken();
        var orderProducts = new List<OrderProductInsuranceDto>
        {
            new OrderProductInsuranceDto{ProductId = 1 , Quantity=1 },
            new OrderProductInsuranceDto{ProductId = 2 , Quantity=1 },
        };
        var productService = new Mock<IProductService>();
        productService.Setup(x => x.GetOrderProductInsurance(orderProducts[0])).Returns(firstOrderProductInsurance);
        productService.Setup(x => x.GetOrderProductInsurance(orderProducts[1])).Returns(secondOrderProductInsurance);
        var orderService = new OrderService(_context, productService.Object);
        //Act
        var result = await orderService.SaveOrderInsurance(orderProducts, cancellationToken);
        //Assert
        Assert.AreEqual(result.Insurance, firstOrderProductInsurance + secondOrderProductInsurance);
    }
    [TestMethod]
    public async Task WhenOrderHasDigitalCameras_GetAdditionalInsurance_Equal500 ()
    {
        //Arrange
        var order = new Order {
            Id = 2,
            Insurance = 1000
        };
        _context.OrderList.Add(order);
        _context.SaveChanges();
        float additionalInsurance = 500;
        var cancellationToken = new CancellationToken();
        var productService = new Mock<IProductService>();
        productService.Setup(x => x.GetAdditionalInsurance("Digital cameras", order)).Returns(additionalInsurance);
        var orderService = new OrderService(_context, productService.Object);
        //Act
        var result = await orderService.CalculateAdditionalInsurance(order, cancellationToken);
        //Assert
        Assert.AreEqual(result.AdditionalInsurance, additionalInsurance);
    }
    [TestMethod]
    public async Task WhenOrderDoesntHasDigitalCameras_GetAdditionalInsurance_Equal0()
    {
        //Arrange
        var order = new Order
        {
            Id = 1,
            Insurance = 1000
        };
        _context.OrderList.Add(order);
        _context.SaveChanges();
        float additionalInsurance = 0;
        var cancellationToken = new CancellationToken();
        var productService = new Mock<IProductService>();
        productService.Setup(x => x.GetAdditionalInsurance("Test", order)).Returns(additionalInsurance);
        var orderService = new OrderService(_context, productService.Object);
        //Act
        var result = await orderService.CalculateAdditionalInsurance(order, cancellationToken);
        //Assert
        Assert.AreEqual(result.AdditionalInsurance, additionalInsurance);
    }
}
