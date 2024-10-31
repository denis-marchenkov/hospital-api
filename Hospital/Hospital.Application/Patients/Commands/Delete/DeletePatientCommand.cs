using Hospital.Application.Infrastructure;
using Hospital.Domain.Entities;
using MediatR;

namespace Hospital.Application.Patients.Commands.Delete
{
    public sealed record DeletePatientCommand(PatientId Id) : IRequest<CqrsResult<PatientId>>;
}
