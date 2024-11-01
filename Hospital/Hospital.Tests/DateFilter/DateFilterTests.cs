using Hospital.Domain.Entities;
using Hospital.Domain.Search.FilterExpressions;
using Hospital.Domain.Search;
using Hospital.Application.Infrastructure;

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


        //[Test]
        //[TestCase("2010-01-13T14:15:16")]
        //[TestCase("2010-01-13")]
        //public void ApplyFilter_ShouldReturnResultWithoutOne_WhenNe(string dateStr)
        //{
        //    var filter = new SearchFilter
        //    {
        //        Field = "birthdate",
        //        Operator = SearchOperator.Ne,
        //        Value = dateStr
        //    };

        //    var query = _patients.AsQueryable();


        //    var result = FilterExpressions.ApplyFilter(query, filter).ToList();

        //    Assert.That(result.Count, Is.EqualTo(_patients.Count - 1));
        //}
    }
}
