using Hospital.Domain.Search;

namespace Hospital.Domain.Contracts
{
    public interface IFilterParser
    {
        string Name { get; }
        SearchFilter Parse(string value);
    }
}
