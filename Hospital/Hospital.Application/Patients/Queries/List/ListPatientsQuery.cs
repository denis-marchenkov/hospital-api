using Hospital.Application.Infrastructure;
using Hospital.Domain.Entities;
using Hospital.Domain.Search;
using MediatR;

namespace Hospital.Application.Patients.Queries.List
{
    public sealed record ListPatientsQuery : IRequest<CqrsResult<List<Patient>>>
    {
        public IEnumerable<SearchFilter> Filters { get; set; } = Enumerable.Empty<SearchFilter>();
    }
}
