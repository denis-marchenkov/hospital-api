using Hospital.Domain.Contracts;
using System.Collections.Immutable;
using System.Reflection;

namespace Hospital.Domain.Search
{
    public class SearchFilterProvider : ISearchFilterProvider
    {
        private readonly IReadOnlyDictionary<string, IFilterParser> _parsers;

        public SearchFilterProvider()
        {
            var parserType = typeof(IFilterParser);

            _parsers = Assembly.GetExecutingAssembly()
                .DefinedTypes
                .Where(
                    type => parserType.IsAssignableFrom(type) &&
                    !type.IsAbstract &&
                    !type.IsInterface
                    )
                .Select(Activator.CreateInstance)
                .Cast<IFilterParser>()
                .ToImmutableDictionary(parser => parser.Name, parser => parser);
        }

        public SearchFilterProvider(IReadOnlyDictionary<string, IFilterParser> parsers)
        {
            _parsers = parsers;
        }

        public ISearchFilter GetFilter(string queryKey, string queryValue)
        {
            if (_parsers == null || !_parsers.Any())
            {
                throw new Exception($"No query filter parsers assignable from {typeof(IFilterParser)} found in {typeof(SearchFilterProvider).Assembly.FullName}");
            }

            if (!_parsers.TryGetValue(queryKey.ToLower(), out var parser))
            {
                throw new Exception($"No query filter parser for key: {queryKey}");
            }

            return parser.Parse(queryValue);
        }
    }
}
