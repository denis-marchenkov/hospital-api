using Hospital.Domain.Entities;
using Hospital.Domain.Search.FilterExpressions;
using Hospital.Domain.Search;


// Test cases: https://www.hl7.org/fhir/search.html#date
namespace Hospital.Tests.DateFilter
{
    [TestFixture]
    public class DateFilterTests
    {
        private IQueryable<Patient> GeneratePatients(string[] dates)
        {
            var result = new List<Patient>();

            foreach (var d in dates)
            {
                var date = DateTime.Parse(d);
                result.Add(Patient.CreateNew("use", "family", new List<string>(), default, date, default));
            }

            return result.AsQueryable();
        }

        [Test]
        [TestCase("2013-01-14")]
        [TestCase("2013-01-14T10:00")]
        public void ApplyFilter_ShouldReturnResult_WhenEq(string dateStr)
        {
            var filter = new SearchFilter
            {
                Field = "birthdate",
                Operator = SearchOperator.Eq,
                Value = dateStr
            };

            var dates = new[]
            {
                // match
                "2013-01-14",
                "2013-01-14T00:00",
                "2013-01-14T10:00",

                // not match
                "2013-01-15T00:00"
            };

            var query = GeneratePatients(dates).AsQueryable();


            var result = FilterExpressions.ApplyFilter(query, filter).ToList();

            Assert.That(result.Count, Is.EqualTo(3));
            Assert.That(result.First().BirthDate, Is.Not.EqualTo(DateTime.Parse("2013-01-15T00:00")));
        }


        [Test]
        [TestCase("2013-01-14T00:00:00")]
        [TestCase("2013-01-14")]
        public void ApplyFilter_ShouldReturnResult_WhenNe(string dateStr)
        {
            var filter = new SearchFilter
            {
                Field = "birthdate",
                Operator = SearchOperator.Ne,
                Value = dateStr
            };

            var dates = new[]
            {
                // match
                "2013-01-15T00:00",

                // not match
                "2013-01-14T00:00",
                "2013-01-14T10:00"
            };

            var query = GeneratePatients(dates).AsQueryable();


            var result = FilterExpressions.ApplyFilter(query, filter).ToList();

            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result.First().BirthDate, Is.EqualTo(DateTime.Parse("2013-01-15T00:00")));
        }

        [Test]
        [TestCase("2013-01-14T10:00")]
        public void ApplyFilter_ShouldReturnResult_WhenTimeSpecified_And_Lt(string dateStr)
        {
            var filter = new SearchFilter
            {
                Field = "birthdate",
                Operator = SearchOperator.Lt,
                Value = dateStr
            };

            var dates = new[]
            {
                // match
                "2013-01-14",
                "2013-01-13T12:00",
                "2013-01-14T12:00",
                "2013-01-14T08:00",
                "2013-01-15T08:00",

                // not match
                "2013-01-15T12:00",
                "2013-01-16",
            };

            var query = GeneratePatients(dates).AsQueryable();


            var result = FilterExpressions.ApplyFilter(query, filter).ToList();

            Assert.That(result.Count, Is.EqualTo(5));

            foreach (var d in dates.Take(5))
            {
                Assert.That(result.Any(x => x.BirthDate == DateTime.Parse(d)), Is.True);
            }
        }

        [Test]
        [TestCase("2013-01-14T10:00")]
        public void ApplyFilter_ShouldReturnResult_WhenTimeSpecified_And_Gt(string dateStr)
        {
            var filter = new SearchFilter
            {
                Field = "birthdate",
                Operator = SearchOperator.Gt,
                Value = dateStr
            };

            var dates = new[]
            {
                // match
                "2013-01-13T12:00", // "2013-01-13T12:00" matches, because it includes the part of 14-Jan 2013 until noon
                "2013-01-14T12:00",
                "2013-01-14T12:00",
                "2013-01-15T12:00",

                // not match
                "2013-01-11T00:00",
                "2013-01-14",
            };

            var query = GeneratePatients(dates).AsQueryable();


            var result = FilterExpressions.ApplyFilter(query, filter).ToList();

            Assert.That(result.Count, Is.EqualTo(4));

            foreach (var d in dates.Take(4))
            {
                Assert.That(result.Any(x => x.BirthDate == DateTime.Parse(d)), Is.True);
            }
        }

    }
}
