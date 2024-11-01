using Hospital.Domain.Contracts;

namespace Hospital.Domain.Search.FilterParsers
{
    public class BirthDateFilterParser : IFilterParser
    {
        public string Name => "birthdate";

        public SearchFilter Parse(string value)
        {
            var operators = SearchOperatorExtensions.GetAllOperatorStrings();
            var strOp = SearchOperatorExtensions.ExtractOperator(value);
            var op = SearchOperatorExtensions.FromString(strOp);

            var val = value.TrimStart(strOp.ToCharArray());
            if (!DateTime.TryParse(val, out var dateTimeValue))
            {
                throw new ArgumentException("Invalid DateTime format.");
            }

            return new SearchFilter { Field = Name, Operator = op, Value = dateTimeValue };
        }
    }
}
