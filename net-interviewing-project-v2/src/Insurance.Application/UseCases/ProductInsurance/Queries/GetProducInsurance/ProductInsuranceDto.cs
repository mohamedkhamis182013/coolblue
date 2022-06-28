namespace Insurance.Application.UseCases.ProductInsurance.Queries.GetProducInsurance;

public class ProductInsuranceDto
{
    public int ProductId { get; set; }
    public string? ProductTypeName { get; set; }
    public bool ProductTypeHasInsurance { get; set; }
    public float SalesPrice { get; set; }
}
