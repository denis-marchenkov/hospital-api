using Hospital.Domain.Search;

namespace Hospital.Domain.Contracts
{
    public interface ISearchFilter
    {
        public string Field { get; set; }
        public SearchOperator Operator { get; set; }
        public object Value { get; set; }

    }

    public interface ISearchFilter<T> : ISearchFilter
    {
        IQueryable<T> Apply(IQueryable<T> query);
    }
}
