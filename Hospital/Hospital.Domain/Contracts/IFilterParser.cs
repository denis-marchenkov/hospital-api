namespace Hospital.Domain.Contracts
{
    public interface IFilterParser
    {
        string Name { get; }
        ISearchFilter Parse(string value);
    }
}
