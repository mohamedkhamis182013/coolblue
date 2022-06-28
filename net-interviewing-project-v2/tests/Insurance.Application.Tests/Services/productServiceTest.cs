using Insurance.Application.Common.Interfaces;
using Insurance.Application.UseCases.OrderInsurance.Commands.CalculateOrderInsuranceCommand;
using Insurance.Application.UseCases.ProductInsurance.Queries.GetProducInsurance;
using Insurance.Domain.Entities;
using Insurance.Infrastructure.Persistence;
using Insurance.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Insurance.Application.Tests.Services;

[TestClass]
public class productServiceTest
{
    [TestMethod]
    public void WhenProductAndProductType_AreNotNull_GetInsuranceDto()
    {
        //Arrange
        var dbContextMock = new Mock<IApplicationDbContext>();
        var productService = new ProductService(dbContextMock.Object);
        var productEntity = new Domain.Entities.Product { Id = 1, Name = "Test", ProductTypeId = 2, SalesPrice = 10 };
        var productTypeEntity = new Domain.Entities.ProductType { Id = 2, Name = "TestPT", CanBeInsured = true };
        var productInsuranceDto = new ProductInsuranceDto { ProductId = productEntity.Id, ProductTypeHasInsurance = productTypeEntity.CanBeInsured, ProductTypeName = productTypeEntity.Name, SalesPrice = productEntity.SalesPrice };
        //Act
        var result = productService.GetProductDto(productEntity, productTypeEntity);
        //Assert
        Assert.AreEqual(result.SalesPrice, productInsuranceDto.SalesPrice);
        Assert.AreEqual(result.ProductTypeName, productInsuranceDto.ProductTypeName);
        Assert.AreEqual(result.ProductTypeHasInsurance, productInsuranceDto.ProductTypeHasInsurance);
        Assert.AreEqual(result.ProductId, productInsuranceDto.ProductId);
        Assert.AreEqual(result.ProductTypeName, productInsuranceDto.ProductTypeName);
    }

    [TestMethod]
    [DataRow(420, 0)]
    [DataRow(500, 1000)]
    [DataRow(600, 1000)]
    [DataRow(1000, 1000)]
    [DataRow(2000, 2000)]
    public void WhensalesPrice_Exist_CalculateInsurance(float salesPrice, float insurance)
    {
        //Arrange
        var dbContextMock = new Mock<IApplicationDbContext>();
        var productService = new ProductService(dbContextMock.Object);
        //Act
        var result = productService.GetProductInsurance(salesPrice);
        //Assert
        Assert.AreEqual(result, insurance);
    }

    [TestMethod]
    [DataRow("test", 0)]
    [DataRow("Laptops", 500)]
    [DataRow("Smartphones", 500)]
    public void WhenProductTypeName_Exist_CalculateIfHasInsurance(string ProductType, float insurance)
    {
        //Arrange
        var dbContextMock = new Mock<IApplicationDbContext>();
        var productService = new ProductService(dbContextMock.Object);
        //Act
        var result = productService.GetSensetiveProductsInsurance(ProductType);
        //Assert
        Assert.AreEqual(result, insurance);
    }
    [TestMethod]
    public void WhenGetAdditionalInsurance_hasDigitalCamera_Add500Insurance()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "cooolblueDb")
            .Options;
        var _context = new ApplicationDbContext(options);
        _context.ProductTypeList.Add(new ProductType { CanBeInsured = true, Name = "digital Cameras", Id = 2 });
        _context.ProductList.Add(new Product { Id = 2, ProductTypeId = 2 });
        _context.SaveChanges();
        float expectedResult = 500;
        var productService = new ProductService(_context);
        var order = new Order();
        order.OrderOrderProducts.Add(
                new OrderProduct
                {
                    ProductId = 1,
                    InsuranceValue = 1000,
                    Quantity = 1
                }
                );
        var result = productService.GetAdditionalInsurance("digital Cameras", order);

        Assert.AreEqual(result, expectedResult);
    }

    [TestMethod]
    public void WhenGetAdditionalInsurance_DonthasDigitalCamera_Add0Insurance()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "cooolblueDb")
            .Options;
        var _context = new ApplicationDbContext(options);
        _context.ProductTypeList.Add(new ProductType { CanBeInsured = true, Name = "digital Cameras", Id = 1 });
        _context.ProductList.Add(new Product { Id = 1, ProductTypeId = 1 });
        _context.SaveChanges();
        float expectedResult = 0;
        var productService = new ProductService(_context);
        var order = new Order();
        order.OrderOrderProducts.Add(
                new OrderProduct
                {
                    ProductId = 1,
                    InsuranceValue = 1000,
                    Quantity = 1
                }
                );
        var result = productService.GetAdditionalInsurance("test", order);

        Assert.AreEqual(result, expectedResult);
    }

    [TestMethod]
    public void WhenGetorderProductExist_CalculateInsurance_ShouldBeCalculated()
    {
        //Arrange
        float expectedResult = 5000;
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "cooolblueDb")
            .Options;
        var _context = new ApplicationDbContext(options);
        _context.ProductTypeList.Add(new ProductType { CanBeInsured = true, Name = "Laptops", Id = 3 });
        _context.ProductList.Add(new Product { Id = 3, ProductTypeId = 3, SalesPrice = 2000 });
        _context.SaveChanges();
        var Orderproduct = new OrderProductInsuranceDto { ProductId = 3, Quantity = 2 };
        var productService = new ProductService(_context);
        //Act
        var result = productService.GetOrderProductInsurance(Orderproduct);
        //Assert
        Assert.AreEqual(result, expectedResult);
    }
}
