namespace Hospital.Domain.Search
{
    // https://www.hl7.org/fhir/search.html#prefix
    public enum SearchOperator
    {
        Eq,     // the resource value is equal to or fully contained by the parameter value
        Ne,     // the resource value is not equal to the parameter value
        Gt,     // the resource value is greater than the parameter value
        Lt,     // the resource value is less than the parameter value
        Ge,     // the resource value is greater or equal to the parameter value
        Le,     // the resource value is less or equal to the parameter value
        Sa,     // the resource value starts after the parameter value
        Eb,     // the resource value ends before the parameter value

        Ap      // the resource value is approximately the same to the parameter value
                // recommended value for the approximation is 10% of the stated value
                // (or for a date, 10% of the gap between now and the date)
    }

    public static class SearchOperatorExtensions
    {
        public static string GetOperatorString(this SearchOperator op)
        {
            return op switch
            {
                SearchOperator.Eq => "eq",
                SearchOperator.Ne => "ne",
                SearchOperator.Gt => "gt",
                SearchOperator.Lt => "lt",
                SearchOperator.Ge => "ge",
                SearchOperator.Le => "le",
                SearchOperator.Sa => "sa",
                SearchOperator.Eb => "eb",
                SearchOperator.Ap => "ap",

                _ => throw new NotImplementedException()
            };
        }

        public static IEnumerable<string> GetAllOperatorStrings()
        {
            return Enum.GetValues(typeof(SearchOperator))
                       .Cast<SearchOperator>()
                       .Select(op => op.GetOperatorString());
        }

        public static string ExtractOperator(string input)
        {
            var operators = GetAllOperatorStrings();

            foreach (var op in operators)
            {
                if (input.StartsWith(op, StringComparison.OrdinalIgnoreCase))
                {
                    return op;
                }
            }

            throw new ArgumentException($"'{input}'does not contain a valid operator.");
        }

        public static SearchOperator FromString(string operatorString)
        {
            if (string.IsNullOrEmpty(operatorString)) throw new ArgumentNullException(operatorString);

            if (!Enum.TryParse(operatorString, true, out SearchOperator searchOperator))
            {
                throw new ArgumentException($"'{operatorString}' is not a valid operator.");
            }

            return searchOperator;
        }
    }
}
