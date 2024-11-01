namespace Hospital.Application.Infrastructure
{
    public interface IQueryStringParser
    {
        public List<(string, string)> ParseQueryString(string queryString);
    }
}
