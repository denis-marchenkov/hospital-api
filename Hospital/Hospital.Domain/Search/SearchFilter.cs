namespace Hospital.Domain.Search
{
    public class SearchFilter
    {
        public string Field { get; set; }
        public SearchOperator Operator { get; set; }
        public object Value { get; set; }

        public DateTime DateTime
        {
            get
            {
                var dateStr = Value as string;
                if (DateTime.TryParse(dateStr, out var dateTime))
                {
                    return dateTime;
                }

                throw new FormatException($"Invalid date format: {dateStr}");
            }
        }
    }
}
