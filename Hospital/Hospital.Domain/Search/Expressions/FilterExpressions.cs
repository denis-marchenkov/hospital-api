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
                    { SearchOperator.Eq, filter => p => p.BirthDate.Date == filter.DateTime.Date },
                    { SearchOperator.Ne, filter => p => p.BirthDate.Date != filter.DateTime.Date },
                    {
                        SearchOperator.Gt,
                        filter => p =>
                        (p.BirthDate.Date == filter.DateTime.Date && p.BirthDate > filter.DateTime)                               ||      // same date after the specified time
                        (p.BirthDate.Date == filter.DateTime.Date.AddDays(-1) && p.BirthDate.TimeOfDay >= TimeSpan.FromHours(12)) ||      // day before and time is after noon
                        (p.BirthDate.Date > filter.DateTime.Date)                                                                         // any date after the specified date
                    },
                    { SearchOperator.Lt,
                        filter => p =>
                        (p.BirthDate.Date == filter.DateTime.Date && p.BirthDate.TimeOfDay <= TimeSpan.FromHours(12))                   ||  // same date before or equal to noon
                        (p.BirthDate.Date == filter.DateTime.Date.AddDays(-1) && p.BirthDate.TimeOfDay < TimeSpan.FromHours(12))        ||  // day before and time before noon
                        (p.BirthDate.Date == filter.DateTime.Date.AddDays(1) && p.BirthDate.TimeOfDay < filter.DateTime.TimeOfDay)      ||  // next day and time before the specified time
                        (p.BirthDate.Date < filter.DateTime.Date)                                                                           // any date before the specified date
                    }
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
