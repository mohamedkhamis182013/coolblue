namespace Insurance.Infrastructure.InsurenceChainOfResponsibilities;

public class FirstLink : Link
{
    public override float Execute(float salesPrice)
    {
        if (salesPrice < 500)
        {
            return 0;
        }

        return base.Execute(salesPrice);
    }
}
