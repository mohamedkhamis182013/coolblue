namespace Insurance.Application.UseCases.OrderInsurance.Commands.CalculateOrderInsuranceCommand;

public class OrderInsuranceVm
{
    public int orderId { get; set; }
    public float productsInsurance { get; set; }
    public float AdditionalInsurance { get; set; }
    public float totalInsurance { get; set; }
}
