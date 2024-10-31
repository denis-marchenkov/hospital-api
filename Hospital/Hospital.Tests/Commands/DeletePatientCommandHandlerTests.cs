using Bogus;
using Hospital.Application.Patients.Commands.Delete;
using Hospital.Domain.Contracts;
using Hospital.Domain.Entities;
using Hospital.Domain.Values;
using Microsoft.Extensions.Logging;
using Moq;

namespace Hospital.Tests.Commands
{
    [TestFixture]
    public class DeletePatientCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly Mock<IPatientRepository> _patientRepository = new();
        private readonly Mock<ILogger<DeletePatientCommandHandler>> _loggerMock = new();

        [SetUp]
        public void Setup()
        {
            _patientRepository.Setup(x => x.Delete(It.IsAny<Patient>()));
            _unitOfWorkMock.Setup(x => x.PatientRepository).Returns(_patientRepository.Object);
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()));
        }

        [Test]
        public async Task Handle_ShouldReturnId()
        {
            var patient = new Faker<Patient>()
                .RuleFor(p => p.Id, f => PatientId.New())
                .RuleFor(p => p.Gender, f => f.PickRandom<Gender>())
                .RuleFor(p => p.BirthDate, f => f.Date.Past(30))
                .RuleFor(p => p.Active, f => f.Random.Bool())
                .Generate();

            _patientRepository.Setup(x => x.GetByIdAsync(It.IsAny<PatientId>(), default)).Returns(Task.FromResult(patient));

            var command = new DeletePatientCommand(PatientId.New());

            var handler = new DeletePatientCommandHandler(_unitOfWorkMock.Object, _loggerMock.Object);

            var result = await handler.Handle(command, default);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value.Value, Is.EqualTo(patient.Id.Value));
        }
    }
}
