using Hospital.Application.Infrastructure;
using Hospital.Domain.Contracts;
using Hospital.Domain.Entities;
using Hospital.Domain.Search.FilterExpressions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Hospital.Application.Patients.Queries.List
{
    public sealed class ListPatientsQueryHandler : IRequestHandler<ListPatientsQuery, CqrsResult<List<Patient>>>
    {
        private readonly ILogger<ListPatientsQueryHandler> _logger;
        private IUnitOfWork _unitOfWork;
        public ListPatientsQueryHandler(IUnitOfWork unitOfWork, ILogger<ListPatientsQueryHandler> logger)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }
        public Task<CqrsResult<List<Patient>>> Handle(ListPatientsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogDebug($"{nameof(ListPatientsQueryHandler)}");

                var result = _unitOfWork.PatientRepository.Query();

                if (request.Filters.Any())
                {
                    foreach (var filter in request.Filters)
                    {
                        result = FilterExpressions.ApplyFilter(result, filter);
                    }
                }

                return Task.FromResult(CqrsResult<List<Patient>>.Success(result.ToList()));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(ListPatientsQueryHandler)}");
                return Task.FromResult(CqrsResult<List<Patient>>.Failed($"Error while listing patients"));
            }
        }
    }
}
