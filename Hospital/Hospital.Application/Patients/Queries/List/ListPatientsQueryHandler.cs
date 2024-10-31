using Hospital.Application.Infrastructure;
using Hospital.Application.Patients.Commands.Create;
using Hospital.Domain.Contracts;
using Hospital.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Hospital.Application.Patients.Queries.List
{
    public sealed class ListPatientsQueryHandler : IRequestHandler<ListPatientsQuery, CqrsResult<IEnumerable<Patient>>>
    {
        private readonly ILogger<ListPatientsQueryHandler> _logger;
        private IUnitOfWork _unitOfWork;
        public ListPatientsQueryHandler(IUnitOfWork unitOfWork, ILogger<ListPatientsQueryHandler> logger)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }
        public async Task<CqrsResult<IEnumerable<Patient>>> Handle(ListPatientsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogDebug($"{nameof(ListPatientsQueryHandler)}");

                var result = await _unitOfWork.PatientRepository.ListAsync(cancellationToken: cancellationToken);
                return CqrsResult<IEnumerable<Patient>>.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(ListPatientsQueryHandler)}");
                return CqrsResult<IEnumerable<Patient>>.Failed($"Error while listing patients");
            }
        }
    }
}
