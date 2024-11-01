using Hospital.Application.Infrastructure;
using Hospital.Application.Patients.Commands.Create;
using Hospital.Application.Patients.Commands.Delete;
using Hospital.Application.Patients.Commands.Update;
using Hospital.Application.Patients.Queries.GetById;
using Hospital.Application.Patients.Queries.List;
using Hospital.Domain.Contracts;
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

            app.MapGet("/patients/search", Search)
                .WithTags("Search for patient");
        }


        /// <summary>
        /// Get patient by ID
        /// </summary>
        /// <param name="id">Patient ID</param>
        /// <param name="mediator"></param>
        /// <returns></returns>
        private static async Task<IResult> GetById(Guid id, IMediator mediator)
        {
            var result = await mediator.Send(new GetPatientByIdQuery(new PatientId(id)));

            return result.IsSuccess ?
                Results.Ok(result) :
                Results.NotFound(result);
        }

        /// <summary>
        /// Get all patients
        /// </summary>
        /// <param name="mediator"></param>
        /// <returns></returns>
        private static async Task<IResult> List(IMediator mediator)
        {
            var result = await mediator.Send(new ListPatientsQuery());

            return result.IsSuccess ?
                Results.Ok(result) :
                Results.NotFound(result);
        }

        /// <summary>
        /// Create patient
        /// </summary>
        /// <param name="command"></param>
        /// <param name="mediator"></param>
        /// <returns></returns>
        private static async Task<IResult> Create(CreatePatientCommand command, IMediator mediator)
        {
            var result = await mediator.Send(command);

            var guid = result.Value.Value;

            return result.IsSuccess ?
                Results.Created($"/patient/{guid}", new { id = guid }) :
                Results.NotFound(result);
        }


        /// <summary>
        /// Remove patient from the database
        /// </summary>
        /// <param name="id">Patient ID</param>
        /// <param name="mediator"></param>
        /// <returns></returns>
        private static async Task<IResult> Delete(Guid id, IMediator mediator)
        {
            var result = await mediator.Send(new DeletePatientCommand(new PatientId(id)));

            return result.IsSuccess ?
                Results.NoContent() :
                Results.NotFound(id);
        }

        /// <summary>
        /// Update specific patient
        /// </summary>
        /// <param name="id">Patient ID</param>
        /// <param name="command"></param>
        /// <param name="mediator"></param>
        /// <returns></returns>
        private static async Task<IResult> Update(Guid id, UpdatePatientCommand command, IMediator mediator)
        {
            if (id != command.Id.Value) return Results.BadRequest("Id does not match request body Id");

            var result = await mediator.Send(command);

            return result.IsSuccess ?
                Results.NoContent() :
                Results.NotFound(id);
        }

        /// <summary>
        /// Search patient
        /// </summary>
        /// <param name="queryString">
        /// <code>Example: birthDate=gt2013-01-14T10:00</code><br/><br/><br/><br/>
        /// <code>eq - the resource value is equal to or fully contained by the parameter value;</code><br/><br/>
        /// <code>ne - the resource value is not equal to the parameter value</code><br/><br/>
        /// <code>gt - the resource value is greater than the parameter value</code><br/><br/>
        /// <code>lt - the resource value is less than the parameter value</code><br/><br/>
        /// <code>ge - the resource value is greater or equal to the parameter value</code><br/><br/>
        /// <code>le - the resource value is less or equal to the parameter value</code><br/><br/>
        /// <code>sa - the resource value starts after the parameter value</code><br/><br/>
        /// <code>eb - the resource value ends before the parameter value</code><br/><br/>
        /// <code>ap - the resource value is approximately the same to the parameter value</code><br/><br/>
        /// </param>
        /// <param name="queryStringParser"></param>
        /// <param name="searchFilterProvider"></param>
        /// <param name="mediator"></param>
        /// <returns></returns>
        public static async Task<IResult> Search(string queryString,
            IQueryStringParser queryStringParser,
            ISearchFilterProvider searchFilterProvider,
            IMediator mediator)
        {
            if (string.IsNullOrEmpty(queryString))
            {
                return Results.BadRequest(nameof(queryString));
            }

            var queryParams = queryStringParser.ParseQueryString(queryString);


            var filters = queryParams.Select(x => searchFilterProvider.GetFilter(x.Item1, x.Item2));

            var result = await mediator.Send(new ListPatientsQuery() { Filters = filters });

            return result.IsSuccess ?
                Results.Ok(result) :
                Results.NotFound(result);
        }
    }
}
