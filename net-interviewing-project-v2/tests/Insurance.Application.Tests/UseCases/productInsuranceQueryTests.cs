using Insurance.Application.Common.Interfaces;
using Insurance.Application.UseCases.ProductInsurance.Queries.GetProducInsurance;
using Moq;

namespace Insurance.Application.Tests.UseCases;

[TestClass]
public class productInsuranceQueryTests
{
    [TestMethod]
    [DataRow(450, 0, 0, true, 0)]
    [DataRow(500, 1000, 0, true, 1000)]
    [DataRow(2000, 2000, 0, true, 2000)]
    [DataRow(2500, 2000, 500, true, 2500)]
    [DataRow(400, 0, 500, true, 500)]
    [DataRow(2000, 1000, 500, false, 0)]
    public async Task WhenProductAndProductType_AreNotNull_CalculateInsurance
        (float salesPrice, float productInsurance, float sensitiveProductInsurance, bool canBeInsured, float totalInsurance)
    {
        //Arrange
        var httpHandler = new Mock<IHttpRequestsService>();
        var productService = new Mock<IProductService>();
        var productEntity = new Domain.Entities.Product { Id = 1, Name = "Test", ProductTypeId = 2, SalesPrice = salesPrice };
        var productTypeEntity = new Domain.Entities.ProductType { Id = 2, Name = "TestPT", CanBeInsured = canBeInsured };
        var productInsuranceDto = new ProductInsuranceDto { ProductId = productEntity.Id, ProductTypeHasInsurance = productTypeEntity.CanBeInsured, ProductTypeName = productTypeEntity.Name, SalesPrice = productEntity.SalesPrice };
        httpHandler.Setup(x => x.GetProduct(1)).ReturnsAsync(productEntity);
        httpHandler.Setup(x => x.GetProductType(2)).ReturnsAsync(productTypeEntity);
        productService.Setup(x => x.GetProductDto(productEntity, productTypeEntity)).Returns(productInsuranceDto);
        productService.Setup(x => x.GetProductInsurance(productInsuranceDto.SalesPrice)).Returns(productInsurance);
        productService.Setup(x => x.GetSensetiveProductsInsurance(productInsuranceDto.ProductTypeName)).Returns(sensitiveProductInsurance);
        var query = new GetProductInsuranceQuery { ProductId = 1, InsuranceValue = 0 };
        var handler = new GetProductInsuranceQueryHandler(httpHandler.Object, productService.Object);
        //Act
        var result = await handler.Handle(query, new CancellationToken());
        //Assert
        Assert.AreEqual(result?.InsuranceValue, totalInsurance);
    }



    [TestMethod]
    public async Task WhenProduct_IsNull_ReturnNewObject()
    {
        //Arrange
        float expectedInsuranceResult = 0;
        var expectedProductResult = 0;
        var httpHandler = new Mock<IHttpRequestsService>();
        var productService = new Mock<IProductService>();
        var query = new GetProductInsuranceQuery { ProductId = 1, InsuranceValue = 0 };
        var handler = new GetProductInsuranceQueryHandler(httpHandler.Object, productService.Object);
        //Act
        var result = await handler.Handle(query, new CancellationToken());
        //Assert
        Assert.AreEqual(result?.InsuranceValue, expectedInsuranceResult);
        Assert.AreEqual(result?.ProductId, expectedProductResult);
    }

    [TestMethod]
    public async Task WhenProductType_IsNull_ReturnNewObject()
    {
        //Arrange
        float expectedInsuranceResult = 0;
        var expectedProductResult = 0;
        var httpHandler = new Mock<IHttpRequestsService>();
        var productService = new Mock<IProductService>();
        var productEntity = new Domain.Entities.Product { Id = 1, Name = "Test", ProductTypeId = 2, SalesPrice = 10 };
        httpHandler.Setup(x => x.GetProduct(1)).ReturnsAsync(productEntity);
        var query = new GetProductInsuranceQuery { ProductId = 1, InsuranceValue = 0 };
        var handler = new GetProductInsuranceQueryHandler(httpHandler.Object, productService.Object);
        //Act
        var result = await handler.Handle(query, new CancellationToken());
        //Assert
        Assert.AreEqual(result?.InsuranceValue, expectedInsuranceResult);
        Assert.AreEqual(result?.ProductId, expectedProductResult);
    }
}