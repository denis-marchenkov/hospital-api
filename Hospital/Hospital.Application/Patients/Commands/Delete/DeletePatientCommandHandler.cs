using Hospital.Application.Infrastructure;
using Hospital.Domain.Contracts;
using Hospital.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Hospital.Application.Patients.Commands.Delete
{
    public sealed class DeletePatientCommandHandler : IRequestHandler<DeletePatientCommand, CqrsResult<PatientId>>
    {
        private readonly ILogger<DeletePatientCommandHandler> _logger;
        private IUnitOfWork _unitOfWork;

        public DeletePatientCommandHandler(IUnitOfWork unitOfWork, ILogger<DeletePatientCommandHandler> logger)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }
        public async Task<CqrsResult<PatientId>> Handle(DeletePatientCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogDebug($"{nameof(DeletePatientCommandHandler)}");

                var patient = await _unitOfWork.PatientRepository.GetByIdAsync(request.Id, cancellationToken)
                    ?? throw new ArgumentException($"No patient found with ID: '{request.Id}'", nameof(request.Id));

                _unitOfWork.PatientRepository.Delete(patient);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return CqrsResult<PatientId>.Success(patient.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(DeletePatientCommandHandler)}");
                return CqrsResult<PatientId>.Failed($"Error while removing patient with ID: {request.Id.Value}");
            }
        }
    }
}
