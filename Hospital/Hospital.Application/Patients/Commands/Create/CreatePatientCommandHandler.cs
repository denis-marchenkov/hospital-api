using Hospital.Application.Infrastructure;
using Hospital.Domain.Contracts;
using Hospital.Domain.Entities;
using Hospital.Domain.Values;
using Microsoft.Extensions.Logging;
using MediatR;


namespace Hospital.Application.Patients.Commands.Create
{
    public sealed class CreatePatientCommandHandler : IRequestHandler<CreatePatientCommand, CqrsResult<PatientId>>
    {
        private readonly ILogger<CreatePatientCommandHandler> _logger;
        private IUnitOfWork _unitOfWork;
        public CreatePatientCommandHandler(IUnitOfWork unitOfWork, ILogger<CreatePatientCommandHandler> logger)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }
        public async Task<CqrsResult<PatientId>> Handle(CreatePatientCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogDebug($"{nameof(CreatePatientCommandHandler)}");

                var givenNames = request.GivenNames.Select(GivenName.CreateNew).ToList();

                var name = Name.CreateNew(request.Use, request.Family, givenNames);

                var patient = Patient.CreateNew(
                    use: request.Use,
                    family: request.Family,
                    given: request.GivenNames,
                    gender: request.Gender,
                    birthDate: request.BirthDate,
                    active: request.Active
                );

                _unitOfWork.PatientRepository.Create(patient);

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return CqrsResult<PatientId>.Success(patient.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(CreatePatientCommandHandler)}");
                return CqrsResult<PatientId>.Failed("Error while creating patient");
            }
        }
    }
}
