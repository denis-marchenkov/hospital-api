namespace Hospital.Application.Infrastructure
{
    public class QueryStringParser : IQueryStringParser
    {
        public List<(string, string)> ParseQueryString(string queryString)
        {
            var queryParams = new List<(string, string)>();

            queryString = queryString.TrimStart('?').ToLower();

            var pairs = queryString.Split('&', StringSplitOptions.RemoveEmptyEntries);

            foreach (var pair in pairs)
            {
                var keyValue = pair.Split('=');
                if (keyValue.Length == 2)
                {
                    queryParams.Add((keyValue[0], keyValue[1]));
                }
            }

            return queryParams;
        }
    }
}
