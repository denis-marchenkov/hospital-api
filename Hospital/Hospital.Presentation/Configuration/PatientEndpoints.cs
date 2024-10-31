using Hospital.Domain.Contracts;

namespace Hospital.Presentation.Configuration
{
    internal static class PatientEndpoints
    {
        public static void MapPatientEndpoints(this WebApplication app)
        {
            app.MapGet("/patients", Test)
                .WithTags("Test");

        }

        public static async Task<IResult> Test(ILogger<Program> log, IUnitOfWork unitOfWork)
        {
            var results = await unitOfWork.PatientRepository.ListAsync(new CancellationToken());
            log.LogDebug("Hello");
            return Results.Ok(results);
        }

    }
}
