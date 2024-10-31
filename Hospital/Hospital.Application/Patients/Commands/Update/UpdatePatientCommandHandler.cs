using Hospital.Application.Infrastructure;
using Hospital.Domain.Contracts;
using Hospital.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Hospital.Application.Patients.Commands.Update
{
    public sealed class UpdatePatientCommandHandler : IRequestHandler<UpdatePatientCommand, CqrsResult<PatientId>>
    {
        private readonly ILogger<UpdatePatientCommandHandler> _logger;
        private IUnitOfWork _unitOfWork;
        public UpdatePatientCommandHandler(IUnitOfWork unitOfWork, ILogger<UpdatePatientCommandHandler> logger)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }
        public async Task<CqrsResult<PatientId>> Handle(UpdatePatientCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogDebug($"{nameof(UpdatePatientCommandHandler)}");

                var patientToUpdate = await _unitOfWork.PatientRepository.GetByIdAsync(request.Id, cancellationToken)
                    ?? throw new ArgumentException($"No patient found with ID: '{request.Id}'", nameof(request.Id));

                var patient = Patient.CreateNew(
                    use: request.Use,
                    family: request.Family,
                    given: request.GivenNames,
                    gender: request.Gender,
                    birthDate: request.BirthDate,
                    active: request.Active
                );

                patient.Update(patientToUpdate);

                _unitOfWork.PatientRepository.Update(patientToUpdate);

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return CqrsResult<PatientId>.Success(request.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(UpdatePatientCommandHandler)}");
                return CqrsResult<PatientId>.Failed($"Error while updating patient with ID: {request.Id.Value}");
            }
        }
    }
}
