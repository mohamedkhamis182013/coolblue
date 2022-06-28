using Insurance.Infrastructure.Services;
using Moq;
using Moq.Contrib.HttpClient;
using Newtonsoft.Json;

namespace Insurance.Application.Tests.Services;
[TestClass]
public class HttpRequestsServiceTests
{
    private Mock<HttpMessageHandler> handlerMock = new Mock<HttpMessageHandler>();
    [TestInitialize]
    public void Setup()
    {
        handlerMock = new Mock<HttpMessageHandler>();
    }

    [TestMethod]
    public async Task WhenGetProduct_GetAsync_shouldGetTheData()
    {
        //Arrange
        var product = new Domain.Entities.Product
        {
            Id = 572770,
            Name = "Samsung WW80J6400CW EcoBubble",
            SalesPrice = 475,
            ProductTypeId = 124
        };
        var httpclient = handlerMock.CreateClient();
        httpclient.BaseAddress = new Uri("http://localhost:5002");
        handlerMock.SetupRequest(HttpMethod.Get, "http://localhost:5002/products/572770")
 .ReturnsResponse(JsonConvert.SerializeObject(product), "application/json");

        var subject = new HttpRequestsService(httpclient);
        //Act 
        var result = await subject.GetProduct(572770);
        //Assert
        Assert.AreEqual(result.ProductTypeId, product.ProductTypeId);
        Assert.AreEqual(result.Id, product.Id);
        Assert.AreEqual(result.Name, product.Name);
        Assert.AreEqual(result.SalesPrice, product.SalesPrice);
    }

    [TestMethod]
    public async Task WhenGetProductType_GetAsync_shouldGetTheData()
    {
        //Arrange
        var productType = new Domain.Entities.ProductType
        {
            Id = 572,
            Name = "Samsung WW80J6400CW EcoBubble",
            CanBeInsured = true
        };
        var httpclient = handlerMock.CreateClient();
        httpclient.BaseAddress = new Uri("http://localhost:5002");
        handlerMock.SetupRequest(HttpMethod.Get, "http://localhost:5002/product_types/572")
 .ReturnsResponse(JsonConvert.SerializeObject(productType), "application/json");
        var subject = new HttpRequestsService(httpclient);

        //Act 
        var result = await subject.GetProductType(572);

        //Assert
        Assert.AreEqual(result.Id, productType.Id);
        Assert.AreEqual(result.Name, productType.Name);
        Assert.AreEqual(result.CanBeInsured, productType.CanBeInsured);
    }

    [TestMethod]
    public async Task WhenGetProducts_GetAsync_shouldGetTheData()
    {
        //Arrange
        var products = new List<Domain.Entities.Product>
        {
            new Domain.Entities.Product
            {
            Id = 572770,
            Name = "Samsung WW80J6400CW EcoBubble",
            SalesPrice = 475,
            ProductTypeId = 124
            },
            new Domain.Entities.Product
            {
            Id = 572775,
            Name = "Mobile",
            SalesPrice = 250,
            ProductTypeId = 125
            },

        };
        var httpclient = handlerMock.CreateClient();
        httpclient.BaseAddress = new Uri("http://localhost:5002");
        handlerMock.SetupRequest(HttpMethod.Get, "http://localhost:5002/products")
 .ReturnsResponse(JsonConvert.SerializeObject(products), "application/json");

        var subject = new HttpRequestsService(httpclient);
        //act 
        var result = await subject.GetProducts();
        //Assert
        Assert.AreEqual(result.Count, products.Count);
        Assert.AreEqual(result[0].ProductTypeId, products[0].ProductTypeId);
        Assert.AreEqual(result[0].Id, products[0].Id);
        Assert.AreEqual(result[0].Name, products[0].Name);
        Assert.AreEqual(result[0].SalesPrice, products[0].SalesPrice);
        Assert.AreEqual(result[1].ProductTypeId, products[1].ProductTypeId);
        Assert.AreEqual(result[1].Id, products[1].Id);
        Assert.AreEqual(result[1].Name, products[1].Name);
        Assert.AreEqual(result[1].SalesPrice, products[1].SalesPrice);
    }

    [TestMethod]
    public async Task WhenGetProductTypes_GetAsync_shouldGetTheData()
    {
        //Arrange
        var productTypes = new List<Domain.Entities.ProductType>
        {
            new Domain.Entities.ProductType
        {
            Id = 572,
            Name = "Samsung WW80J6400CW EcoBubble",
            CanBeInsured = true
        },
             new Domain.Entities.ProductType
        {
            Id = 570,
            Name = "Samsung ",
            CanBeInsured = true
        },
    };
        var httpclient = handlerMock.CreateClient();
        httpclient.BaseAddress = new Uri("http://localhost:5002");
        handlerMock.SetupRequest(HttpMethod.Get, "http://localhost:5002/product_types")
     .ReturnsResponse(JsonConvert.SerializeObject(productTypes), "application/json");
        var subject = new HttpRequestsService(httpclient);

        //Act 
        var result = await subject.GetProductsTypes();

        //Assert
        Assert.AreEqual(result.Count, productTypes.Count);
        Assert.AreEqual(result[0].Id, productTypes[0].Id);
        Assert.AreEqual(result[0].Name, productTypes[0].Name);
        Assert.AreEqual(result[0].CanBeInsured, productTypes[0].CanBeInsured);
        Assert.AreEqual(result[1].Id, productTypes[1].Id);
        Assert.AreEqual(result[1].Name, productTypes[1].Name);
        Assert.AreEqual(result[1].CanBeInsured, productTypes[1].CanBeInsured);
    }
}
