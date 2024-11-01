using Hospital.Domain.Search;

namespace Hospital.Domain.Contracts
{
    public interface ISearchFilterProvider
    {
        SearchFilter GetFilter(string queryKey, string queryValue);
    }
}
