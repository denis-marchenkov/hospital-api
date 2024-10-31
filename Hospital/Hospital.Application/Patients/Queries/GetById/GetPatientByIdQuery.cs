using Hospital.Application.Infrastructure;
using Hospital.Domain.Entities;
using MediatR;

namespace Hospital.Application.Patients.Queries.GetById
{
    public sealed record GetPatientByIdQuery(PatientId Id) : IRequest<CqrsResult<Patient?>>;
}
