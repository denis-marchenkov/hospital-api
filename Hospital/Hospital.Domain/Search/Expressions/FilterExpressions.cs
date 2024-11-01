using Hospital.Domain.Entities;
using Hospital.Domain.Search.FilterParsers;
using System.Linq.Expressions;

namespace Hospital.Domain.Search.FilterExpressions
{
    public static class FilterExpressions
    {
        public delegate Expression<Func<Patient, bool>> FilterExpressionDelegate(SearchFilter filter);

        private static readonly Dictionary<string, Dictionary<SearchOperator, FilterExpressionDelegate>> Expressions = new()
        {
            {
                "birthdate", new Dictionary<SearchOperator, FilterExpressionDelegate>
                {
                    { SearchOperator.Eq, filter => p => p.BirthDate.Date == DateParser.ParseDate(filter.Value.ToString()).Date },
                    { SearchOperator.Ne, filter => p => p.BirthDate.Date != DateParser.ParseDate(filter.Value.ToString()).Date },
                    //{ SearchOperator.Gt, filter => p => p.BirthDate > Convert.ToDateTime(filter.Value) },
                    //{ SearchOperator.Lt, filter => p => p.BirthDate < Convert.ToDateTime(filter.Value) },
                    //{ SearchOperator.Ge, filter => p => p.BirthDate >= Convert.ToDateTime(filter.Value) },
                    //{ SearchOperator.Le, filter => p => p.BirthDate <= Convert.ToDateTime(filter.Value) },

                    //{ SearchOperator.Sa, filter => p => p.BirthDate < Convert.ToDateTime(filter.Value) },
                    //{ SearchOperator.Eb, filter => p => p.BirthDate < Convert.ToDateTime(filter.Value) },
                    //{ SearchOperator.Ap, filter => p => p.BirthDate < Convert.ToDateTime(filter.Value) }
                }
            },
        };

        public static IQueryable<Patient> ApplyFilter(IQueryable<Patient> query, SearchFilter filter)
        {
            if (Expressions.TryGetValue(filter.Field, out var operatorExpressions) &&
                operatorExpressions.TryGetValue(filter.Operator, out var filterExpression))
            {
                return query.Where(filterExpression(filter));
            }
            else
            {
                throw new NotSupportedException($"Filter for field '{filter.Field}' with operator '{filter.Operator}' is not supported.");
            }
        }
    }
}
