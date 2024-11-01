using Hospital.Domain.Entities;
using Hospital.Domain.Search;
using Hospital.Domain.Search.Filters.PatientFilters;


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
            var filter = new BirthDateFilter("birthdate", SearchOperator.Eq, dateStr);

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


            var result = filter.Apply(query).ToList();

            Assert.That(result.Count, Is.EqualTo(3));
            Assert.That(result.First().BirthDate, Is.Not.EqualTo(DateTime.Parse("2013-01-15T00:00")));
        }


        [Test]
        [TestCase("2013-01-14T00:00:00")]
        [TestCase("2013-01-14")]
        public void ApplyFilter_ShouldReturnResult_WhenNe(string dateStr)
        {
            var filter = new BirthDateFilter("birthdate", SearchOperator.Ne, dateStr);

            var dates = new[]
            {
                // match
                "2013-01-15T00:00",

                // not match
                "2013-01-14T00:00",
                "2013-01-14T10:00"
            };

            var query = GeneratePatients(dates).AsQueryable();


            var result = filter.Apply(query).ToList();

            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result.First().BirthDate, Is.EqualTo(DateTime.Parse("2013-01-15T00:00")));
        }


        [Test]
        [TestCase("2013-01-14T10:00")]
        public void ApplyFilter_ShouldReturnResult_WhenTimeSpecified_And_Lt(string dateStr)
        {
            var filter = new BirthDateFilter("birthdate", SearchOperator.Lt, dateStr);

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
                "2013-01-16"
            };

            var query = GeneratePatients(dates).AsQueryable();


            var result = filter.Apply(query).ToList();

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
            var filter = new BirthDateFilter("birthdate", SearchOperator.Gt, dateStr);

            var dates = new[]
            {
                // match
                "2013-01-13T12:00", // "2013-01-13T12:00" matches, because it includes the part of 14-Jan 2013 until noon
                "2013-01-14T12:00",
                "2013-01-14T12:00",
                "2013-01-15T12:00",

                // not match
                "2013-01-11T00:00",
                "2013-01-14"
            };

            var query = GeneratePatients(dates).AsQueryable();


            var result = filter.Apply(query).ToList();

            Assert.That(result.Count, Is.EqualTo(4));

            foreach (var d in dates.Take(4))
            {
                Assert.That(result.Any(x => x.BirthDate == DateTime.Parse(d)), Is.True);
            }
        }


        [Test]
        [TestCase("2013-03-14T10:00")]
        public void ApplyFilter_ShouldReturnResult_WhenTimeSpecified_And_Ge(string dateStr)
        {
            var filter = new BirthDateFilter("birthdate", SearchOperator.Ge, dateStr);

            // todo: is there a typo in specification? "from 21-Jan 2013 onwards" - how can it come AFTER 14-Mar 2013
            var dates = new[]
            {
                // match
                "2013-03-14T10:00",
                "2013-03-15",
                "2013-03-15T00:00",

                // not match
                "2013-03-14",
                "2013-03-14T08:00"
            };

            var query = GeneratePatients(dates).AsQueryable();


            var result = filter.Apply(query).ToList();

            Assert.That(result.Count, Is.EqualTo(3));

            foreach (var d in dates.Take(3))
            {
                Assert.That(result.Any(x => x.BirthDate == DateTime.Parse(d)), Is.True);
            }
        }


        [Test]
        [TestCase("2013-03-14T10:00")]
        public void ApplyFilter_ShouldReturnResult_WhenTimeSpecified_And_Le(string dateStr)
        {
            var filter = new BirthDateFilter("birthdate", SearchOperator.Le, dateStr);

            var dates = new[]
            {
                // match
                "2013-03-14T10:00",
                "2013-03-13",
                "2013-01-21",

                // not match
                "2013-03-14T11:00",
                "2013-03-15"
            };

            var query = GeneratePatients(dates).AsQueryable();


            var result = filter.Apply(query).ToList();

            Assert.That(result.Count, Is.EqualTo(3));

            foreach (var d in dates.Take(3))
            {
                Assert.That(result.Any(x => x.BirthDate == DateTime.Parse(d)), Is.True);
            }
        }


        [Test]
        [TestCase("2013-03-14T10:00")]
        public void ApplyFilter_ShouldReturnResult_WhenTimeSpecified_And_Sa(string dateStr)
        {
            var filter = new BirthDateFilter("birthdate", SearchOperator.Sa, dateStr);

            var dates = new[]
            {
                // match
                "2013-03-15",

                // not match
                "2013-03-14T10:00",
                "2013-03-14T11:00",
                "2013-03-13",

            };

            var query = GeneratePatients(dates).AsQueryable();


            var result = filter.Apply(query).ToList();

            Assert.That(result.Count, Is.EqualTo(1));

            foreach (var d in dates.Take(1))
            {
                Assert.That(result.Any(x => x.BirthDate == DateTime.Parse(d)), Is.True);
            }
        }


        [Test]
        [TestCase("2013-03-14T10:00")]
        public void ApplyFilter_ShouldReturnResult_WhenTimeSpecified_And_Eb(string dateStr)
        {
            var filter = new BirthDateFilter("birthdate", SearchOperator.Eb, dateStr);

            var dates = new[]
            {
                // match
                "2013-03-13",
                "2013-03-13T12:00",

                // not match
                "2013-03-14T09:00",
                "2013-03-14"

            };

            var query = GeneratePatients(dates).AsQueryable();


            var result = filter.Apply(query).ToList();

            Assert.That(result.Count, Is.EqualTo(2));

            foreach (var d in dates.Take(2))
            {
                Assert.That(result.Any(x => x.BirthDate == DateTime.Parse(d)), Is.True);
            }
        }


        [Test]
        [TestCase("2013-03-14")]
        public void ApplyFilter_ShouldReturnResult_WhenTimeSpecified_And_Ap(string dateStr)
        {
            var filter = new BirthDateFilter("birthdate", SearchOperator.Ap, dateStr);

            BirthDateFilter.Now = DateTime.Parse("2013-05-20");

            var dates = new[]
            {
                // match
                "2013-03-14",
                "2013-03-08",
                "2013-03-20",

                // not match
                "2013-01-21",
                "2013-05-14"

            };

            var query = GeneratePatients(dates).AsQueryable();

            var result = filter.Apply(query).ToList();

            Assert.That(result.Count, Is.EqualTo(3));

            foreach (var d in dates.Take(3))
            {
                Assert.That(result.Any(x => x.BirthDate == DateTime.Parse(d)), Is.True);
            }
        }
    }
}
