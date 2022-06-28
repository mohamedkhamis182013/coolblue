namespace Insurance.Infrastructure.InsurenceChainOfResponsibilities;

public class ThirdLink : Link
{
    public override float Execute(float salesPrice)
    {
        if (salesPrice >= 2000)
        {
            return 2000;
        }

        return base.Execute(salesPrice);

    }

}
