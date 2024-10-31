using Hospital.Application.Infrastructure;
using Hospital.Application.Patients.Commands.Create;
using Hospital.Domain.Contracts;
using Hospital.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;


namespace Hospital.Application.Patients.Queries.GetById
{
    public sealed class GetPatientByIdQueryHandler : IRequestHandler<GetPatientByIdQuery, CqrsResult<Patient?>>
    {
        private readonly ILogger<GetPatientByIdQueryHandler> _logger;
        private IUnitOfWork _unitOfWork;
        public GetPatientByIdQueryHandler(IUnitOfWork unitOfWork, ILogger<GetPatientByIdQueryHandler> logger)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }
        public async Task<CqrsResult<Patient?>> Handle(GetPatientByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogDebug($"{nameof(GetPatientByIdQueryHandler)}");

                var result = await _unitOfWork.PatientRepository.GetByIdAsync(request.Id, cancellationToken: cancellationToken);

                return CqrsResult<Patient?>.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(GetPatientByIdQueryHandler)}");
                return CqrsResult<Patient?>.Failed($"Error while retrieving patient with ID: {request.Id.Value}");
            }
        }
    }
}
