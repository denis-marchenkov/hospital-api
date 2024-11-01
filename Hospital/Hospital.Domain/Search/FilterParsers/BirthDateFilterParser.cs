using Hospital.Domain.Contracts;
using Hospital.Domain.Search.Filters.PatientFilters;

namespace Hospital.Domain.Search.FilterParsers
{
    public class BirthDateFilterParser : IFilterParser
    {
        public string Name => "birthdate";

        public ISearchFilter Parse(string value)
        {
            var operators = SearchOperatorExtensions.GetAllOperatorStrings();
            var strOp = SearchOperatorExtensions.ExtractOperator(value);
            var op = SearchOperatorExtensions.FromString(strOp);

            var val = value.TrimStart(strOp.ToCharArray());
            if (!DateTime.TryParse(val, out var dateTimeValue))
            {
                throw new ArgumentException("Invalid DateTime format.");
            }

            return new BirthDateFilter(Name, op, dateTimeValue);
        }
    }
}
