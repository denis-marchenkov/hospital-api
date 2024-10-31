using Hospital.Domain.Entities;
using Hospital.Domain.Values;

namespace Hospital.Tests.Domain
{
    [TestFixture]
    public class PatientTests
    {
        [Test]
        public void Create_ShouldCreatePatient_Valid()
        {
            var use = "use";
            var family = "family";
            var given = new List<string> { "given1", "given2" };
            var gender = Gender.Male;
            var birthDate = new DateTime(1986, 3, 23);
            var active = true;

            var patient = Patient.CreateNew(use, family, given, gender, birthDate, active);

            Assert.Multiple(() =>
            {
                Assert.That(patient, Is.Not.Null);
                Assert.That(patient.Name.Use, Is.EqualTo(use));
                Assert.That(patient.Name.Family, Is.EqualTo(family));
                Assert.That(patient.Gender, Is.EqualTo(gender));
                Assert.That(patient.BirthDate, Is.EqualTo(birthDate));
                Assert.That(patient.Active, Is.EqualTo(active));
            });
        }

        [Test]
        public void Update_ShouldUpdatePatient()
        {
            var originalPatient = Patient.CreateNew("use_new", "family_new", new List<string> { "given_new" }, Gender.Male, new DateTime(1986, 3, 23), true);
            var patientToUpdate = Patient.CreateNew("use", "family", new List<string> { "given" }, Gender.Female, new DateTime(1990, 1, 1), false);

            originalPatient.Update(patientToUpdate);

            Assert.Multiple(() =>
            {
                Assert.That(patientToUpdate.Gender, Is.EqualTo(originalPatient.Gender));
                Assert.That(patientToUpdate.BirthDate, Is.EqualTo(originalPatient.BirthDate));
                Assert.That(patientToUpdate.Active, Is.EqualTo(originalPatient.Active));
            });
        }
    }
}
