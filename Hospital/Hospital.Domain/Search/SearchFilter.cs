namespace Hospital.Domain.Search
{
    public class SearchFilter
    {
        public string Field { get; set; }
        public SearchOperator Operator { get; set; }
        public object Value { get; set; }
    }
}
