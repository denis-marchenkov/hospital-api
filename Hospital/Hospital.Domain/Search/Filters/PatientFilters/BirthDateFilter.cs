using Hospital.Domain.Contracts;
using Hospital.Domain.Entities;
using System.Linq.Expressions;

namespace Hospital.Domain.Search.Filters.PatientFilters
{
    public class BirthDateFilter : ISearchFilter<Patient>
    {
        public delegate Expression<Func<Patient, bool>> FilterExpressionDelegate(DateTime filterDateTime);

        public string Name { get; }
        public SearchOperator Operator { get; }
        public object Value { get; }

        // since approximation is at the discretion of the system lets say it is 5 days
        public static double Approximation = 5;

        public BirthDateFilter(string name, SearchOperator op, object value)
        {
            Name = name;
            Operator = op;
            Value = value;
        }

        private static readonly Dictionary<SearchOperator, FilterExpressionDelegate> Expressions = new()
        {
            { SearchOperator.Eq, filter => p => p.BirthDate.Date == filter.Date },
            { SearchOperator.Ne, filter => p => p.BirthDate.Date != filter.Date },
            { SearchOperator.Ge, filter => p => p.BirthDate >= filter },
            { SearchOperator.Le, filter => p => p.BirthDate <= filter },
            { SearchOperator.Sa, filter => p => p.BirthDate.Date > filter.Date },
            { SearchOperator.Eb, filter => p => p.BirthDate.Date < filter.Date },
            {
                SearchOperator.Gt, filter => p =>
                    (p.BirthDate.Date == filter.Date && p.BirthDate > filter)                                           ||   // same date after the specified time
                    (p.BirthDate.Date == filter.Date.AddDays(-1) && p.BirthDate.TimeOfDay >= TimeSpan.FromHours(12))    ||   // day before and time is after noon
                    (p.BirthDate.Date > filter.Date)                                                                         // any date after the specified date
            },
            {
                SearchOperator.Lt, filter => p =>
                    (p.BirthDate.Date == filter.Date && p.BirthDate.TimeOfDay <= TimeSpan.FromHours(12))                 ||  // same date before or equal to noon
                    (p.BirthDate.Date == filter.Date.AddDays(-1) && p.BirthDate.TimeOfDay < TimeSpan.FromHours(12))      ||  // day before and time before noon
                    (p.BirthDate.Date == filter.Date.AddDays(1) && p.BirthDate.TimeOfDay < filter.TimeOfDay)             ||  // next day and time before the specified time
                    (p.BirthDate.Date < filter.Date)                                                                         // any date before the specified date
            },
            {
                SearchOperator.Ap, filter => p =>
                    (p.BirthDate.Date == filter.Date)                         ||
                    (
                        (p.BirthDate >= filter.AddDays( -Approximation ) )      &&
                        (p.BirthDate <= filter.AddDays(  Approximation ) )
                    )
            }
        };

        public IQueryable<Patient> Apply(IQueryable<Patient> query)
        {
            if (Expressions.TryGetValue(Operator, out var operatorExpressions))
            {
                var dt = DateTime.Parse(Value.ToString());
                return query.Where(operatorExpressions(dt));
            }
            else
            {
                throw new NotSupportedException($"{nameof(BirthDateFilter)} with operator '{Operator}' is not supported.");
            }
        }
    }
}
