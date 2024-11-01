using Hospital.Application.Infrastructure;
using Hospital.Domain.Contracts;
using Hospital.Domain.Entities;
using MediatR;

namespace Hospital.Application.Patients.Queries.List
{
    public sealed record ListPatientsQuery : IRequest<CqrsResult<List<Patient>>>
    {
        public IEnumerable<ISearchFilter<Patient>> Filters { get; set; } = Enumerable.Empty<ISearchFilter<Patient>>();
    }
}
