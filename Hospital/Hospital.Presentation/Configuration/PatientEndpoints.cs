using Hospital.Application.Patients.Commands.Create;
using Hospital.Application.Patients.Commands.Delete;
using Hospital.Application.Patients.Commands.Update;
using Hospital.Application.Patients.Queries.GetById;
using Hospital.Application.Patients.Queries.List;
using Hospital.Domain.Entities;
using MediatR;

namespace Hospital.Presentation.Configuration
{
    internal static class PatientEndpoints
    {
        public static void MapPatientEndpoints(this WebApplication app)
        {
            app.MapGet("/patients/{id:guid}", GetById)
                            .WithTags("Get Patient By Id");

            app.MapGet("/patients", List)
                .WithTags("List Patients");

            app.MapPost("/patients", Create)
                .WithTags("Create Patient");

            app.MapDelete("/patients/{id:guid}", Delete)
                .WithTags("Delete Patient");

            app.MapPut("/patients/{id:guid}", Update)
                .WithTags("Update Patient");
        }

        private static async Task<IResult> GetById(Guid id, IMediator mediator)
        {
            var result = await mediator.Send(new GetPatientByIdQuery(new PatientId(id)));

            return result.IsSuccess ?
                Results.Ok(result) :
                Results.NotFound(result);
        }

        private static async Task<IResult> List(IMediator mediator)
        {
            var result = await mediator.Send(new ListPatientsQuery());

            return result.IsSuccess ?
                Results.Ok(result) :
                Results.NotFound(result);
        }

        private static async Task<IResult> Create(CreatePatientCommand command, IMediator mediator)
        {
            var result = await mediator.Send(command);

            var guid = result.Value.Value;

            return result.IsSuccess ?
                Results.Created($"/patient/{guid}", new { id = guid }) :
                Results.NotFound(result);
        }

        private static async Task<IResult> Delete(Guid id, IMediator mediator)
        {
            var result = await mediator.Send(new DeletePatientCommand(new PatientId(id)));

            return result.IsSuccess ?
                Results.NoContent() :
                Results.NotFound(id);
        }

        private static async Task<IResult> Update(Guid id, UpdatePatientCommand command, IMediator mediator)
        {
            if (id != command.Id.Value) return Results.BadRequest("Id does not match request body Id");

            var result = await mediator.Send(command);

            return result.IsSuccess ?
                Results.NoContent() :
                Results.NotFound(id);
        }
    }
}
