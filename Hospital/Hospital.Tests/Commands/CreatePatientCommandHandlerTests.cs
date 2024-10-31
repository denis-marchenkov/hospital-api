using Bogus;
using Hospital.Application.Patients.Commands.Create;
using Hospital.Domain.Contracts;
using Hospital.Domain.Entities;
using Hospital.Domain.Values;
using Microsoft.Extensions.Logging;
using Moq;

namespace Hospital.Tests.Commands
{
    [TestFixture]
    public class CreatePatientCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly Mock<IPatientRepository> _patientRepository = new();
        private readonly Mock<ILogger<CreatePatientCommandHandler>> _loggerMock = new();

        [SetUp]
        public void Setup()
        {
            _patientRepository.Setup(x => x.Create(It.IsAny<Patient>()));
            _unitOfWorkMock.Setup(x => x.PatientRepository).Returns(_patientRepository.Object);
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()));
        }

        [Test]
        public async Task Handle_ShouldReturnId()
        {
            var command = new Faker<CreatePatientCommand>()
                            .RuleFor(p => p.Gender, f => f.PickRandom<Gender>())
                            .RuleFor(p => p.BirthDate, f => f.Date.Past(30))
                            .RuleFor(p => p.Active, f => f.Random.Bool())
                            .RuleFor(n => n.Use, f => f.Name.Prefix())
                            .RuleFor(n => n.Family, f => f.Name.LastName())
                            .RuleFor(n => n.GivenNames, f => f.Lorem.Words(f.Random.Int(1, 3)))
                            .Generate();

            var handler = new CreatePatientCommandHandler(_unitOfWorkMock.Object, _loggerMock.Object);

            var result = await handler.Handle(command, default);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsSuccess, Is.True);
            _unitOfWorkMock.Verify(x => x.PatientRepository.Create(It.Is<Patient>(p => p.Id == result.Value)));
        }
    }
}
