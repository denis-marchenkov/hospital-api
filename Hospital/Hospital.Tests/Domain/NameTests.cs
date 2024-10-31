using Hospital.Domain.Entities;
using Hospital.Domain.Values;

namespace Hospital.Tests.Domain
{
    [TestFixture]
    public class NameTests
    {
        [Test]
        public void Create_ShouldCreateName_Valid()
        {
            var use = "use";
            var family = "family";
            var givenNames = new List<GivenName>
            {
                new() { Value = "given1" },
                new() { Value = "given2" }
            };

            var name = Name.CreateNew(use, family, givenNames);

            Assert.Multiple(() =>
            {
                Assert.That(name, Is.Not.Null);
                Assert.That(name.Use, Is.EqualTo(use));
                Assert.That(name.Family, Is.EqualTo(family));
                Assert.That(name.Given.Count, Is.EqualTo(givenNames.Count));
            });
        }

        [Test]
        public void Create_ShouldThrow_WhenFamilyIsNotProvided()
        {
            var use = "use";
            var family = "";

            var ex = Assert.Throws<ArgumentException>(() => Name.CreateNew(use, family, new List<GivenName>()));
            Assert.That(ex.Message, Is.EqualTo("Required (Parameter 'family')"));
        }

        [Test]
        public void Update_ShouldUpdateNameProperties()
        {
            var originalName = Name.CreateNew("use_new", "family_new", new List<GivenName> { new() { Value = "given_new" } });
            var nameToUpdate = Name.CreateNew("use", "family", new List<GivenName> { new() { Id = Guid.NewGuid(), Value = "given" } });

            originalName.Update(nameToUpdate);

            Assert.Multiple(() =>
            {
                Assert.That(nameToUpdate.Use, Is.EqualTo(originalName.Use));
                Assert.That(nameToUpdate.Family, Is.EqualTo(originalName.Family));
                Assert.That(nameToUpdate.Given.Count(x => x.Id == Guid.Empty), Is.EqualTo(originalName.Given.Count));
            });
        }
    }
}
