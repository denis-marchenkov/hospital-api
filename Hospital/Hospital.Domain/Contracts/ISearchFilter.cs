using Hospital.Domain.Search;

namespace Hospital.Domain.Contracts
{
    public interface ISearchFilter
    {
        public string Name { get; }
        public SearchOperator Operator { get; }
        public object Value { get; }

    }

    public interface ISearchFilter<T> : ISearchFilter
    {
        IQueryable<T> Apply(IQueryable<T> query);
    }
}
