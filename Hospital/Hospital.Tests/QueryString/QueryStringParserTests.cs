using Hospital.Application.Infrastructure;

namespace Hospital.Tests.QueryString
{
    [TestFixture]
    public class QueryStringParserTests
    {
        private IQueryStringParser _parser;

        [SetUp]
        public void Setup()
        {
            _parser = new QueryStringParser();
        }

        [Test]
        public void ParseQueryString_EmptyQueryString_ReturnsEmpty()
        {
            var result = _parser.ParseQueryString(string.Empty);

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void ParseQueryString_IncorrectQueryString_ReturnsEmpty()
        {
            var result = _parser.ParseQueryString("fail");

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void ParseQueryString_ValidQueryStringWithQuestionMark_ReturnsValid()
        {
            var result = _parser.ParseQueryString("?key1=value1&key2=value2");

            Assert.That(result.Count, Is.EqualTo(2));
            Assert.Multiple(() =>
            {
                Assert.That(result.First(x => x.Item1 == "key1").Item2, Is.EqualTo("value1"));
                Assert.That(result.First(x => x.Item1 == "key2").Item2, Is.EqualTo("value2"));
            });
        }

        [Test]
        public void ParseQueryString_ValidQueryStringWithoutQuestionMark_ReturnsValid()
        {
            var result = _parser.ParseQueryString("key1=value1&key2=value2");

            Assert.That(result.Count, Is.EqualTo(2));
            Assert.Multiple(() =>
            {
                Assert.That(result.First(x => x.Item1 == "key1").Item2, Is.EqualTo("value1"));
                Assert.That(result.First(x => x.Item1 == "key2").Item2, Is.EqualTo("value2"));
            });
        }

        [Test]
        public void ParseQueryString_ValueMissing_ReturnsOnlyKey()
        {
            var result = _parser.ParseQueryString("?key1=&key2=value2");

            Assert.That(result.Count, Is.EqualTo(2));
            Assert.Multiple(() =>
            {
                Assert.That(result.First(x => x.Item1 == "key1").Item2, Is.Empty);
                Assert.That(result.First(x => x.Item1 == "key2").Item2, Is.EqualTo("value2"));
            });
        }

        [Test]
        public void ParseQueryString_QueryStringParamBroken_ReturnsOnlyValid()
        {
            var result = _parser.ParseQueryString("?key1=value1&key2");

            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result.First(x => x.Item1 == "key1").Item2, Is.EqualTo("value1"));
        }

        [Test]
        public void ParseQueryString_QueryStringCamelCase_ReturnsDictionary()
        {
            var result = _parser.ParseQueryString("?Key1=Value1&key2=value2");

            Assert.That(result.Count, Is.EqualTo(2));
            Assert.Multiple(() =>
            {
                Assert.That(result.First(x => x.Item1 == "key1").Item2, Is.EqualTo("value1"));
                Assert.That(result.First(x => x.Item1 == "key2").Item2, Is.EqualTo("value2"));
            });
        }
    }
}
