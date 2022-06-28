namespace Insurance.Infrastructure.InsurenceChainOfResponsibilities;

public class SecondLink : Link
{
    public override float Execute(float salesPrice)
    {
        if (salesPrice >= 500 && salesPrice < 2000)
        {
            return 1000;
        }

        return base.Execute(salesPrice);

    }
}

