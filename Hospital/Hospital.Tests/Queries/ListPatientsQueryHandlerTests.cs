using Hospital.Application.Patients.Queries.List;
using Hospital.Domain.Contracts;
using Hospital.Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;

namespace Hospital.Tests.Queries
{
    [TestFixture]
    public class ListPatientsQueryHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly Mock<IPatientRepository> _patientRepository = new();
        private readonly Mock<ILogger<ListPatientsQueryHandler>> _loggerMock = new();

        [SetUp]
        public void Setup()
        {
            _patientRepository.Setup(x => x.Query()).Returns(Enumerable.Empty<Patient>().AsQueryable());
            _unitOfWorkMock.Setup(x => x.PatientRepository).Returns(_patientRepository.Object);
            _unitOfWorkMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()));
        }

        [Test]
        public async Task Handle_ShouldReturnId()
        {
            var query = new ListPatientsQuery();

            var handler = new ListPatientsQueryHandler(_unitOfWorkMock.Object, _loggerMock.Object);

            var result = await handler.Handle(query, default);

            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value, Is.Not.Null);
        }
    }
}
