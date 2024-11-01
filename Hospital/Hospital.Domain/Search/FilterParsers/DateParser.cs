namespace Hospital.Domain.Search.FilterParsers
{
    public class DateParser
    {
        public static DateTime ParseDate(string? dateStr)
        {
            if (DateTime.TryParse(dateStr, out var dateTime))
            {
                return dateTime;
            }

            throw new FormatException($"Invalid date format: {dateStr}");
        }
    }
}
