namespace Insurance.Domain.Entities;

public class Order
{
    public int Id { get; set; }
    public float Insurance { get; set; }
    public float AdditionalInsurance { get; set; }
    public IList<OrderProduct> OrderOrderProducts { get; private set; } = new List<OrderProduct>();

}
