namespace Hospital.Domain.Contracts
{
    public interface IFilterParser
    {
        string Name { get; } // query string param name
        ISearchFilter Parse(string value);
    }
}
