namespace Insurance.Infrastructure.InsurenceChainOfResponsibilities;

public abstract class Link
{
    private Link? nextLink;

    public void SetSuccessor(Link next)
    {
        nextLink = next;
    }

    public virtual float Execute(float salesPrice)
    {
        if (nextLink != null)
        {
            return nextLink.Execute(salesPrice);
        }
        return 0;
    }
}
