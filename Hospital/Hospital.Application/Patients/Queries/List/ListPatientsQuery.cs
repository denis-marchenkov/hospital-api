using Hospital.Application.Infrastructure;
using Hospital.Domain.Entities;
using MediatR;

namespace Hospital.Application.Patients.Queries.List
{
    public sealed record ListPatientsQuery : IRequest<CqrsResult<IEnumerable<Patient>>>;
}
