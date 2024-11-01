namespace Hospital.Domain.Contracts
{
    public interface ISearchFilterProvider
    {
        ISearchFilter GetFilter(string queryKey, string queryValue);
    }
}
