using Hospital.Domain.Contracts;
using Hospital.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hospital.Persistence.Repository
{
    public class PatientRepository : IPatientRepository
    {
        protected AppDbContext _dbContext;
        public PatientRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Patient?> GetByIdAsync(PatientId id, CancellationToken cancellationToken) =>
            await _dbContext.Patients
                    .AsNoTracking()
                    .Include(patient => patient.Name)
                    .ThenInclude(name => name.Given)
                    .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);


        public void Create(Patient patient) => _dbContext.Patients.AddAsync(patient);

        public void Update(Patient patient)
        {

            foreach (var givenName in patient.Name.Given.Where(given => given.Id != Guid.Empty))
            {
                _dbContext.Entry(givenName).State = EntityState.Deleted;
            }

            _dbContext.Patients.Update(patient);

        }

        public void Delete(Patient patient) => _dbContext.Patients.Remove(patient);

        public IQueryable<Patient> Query() =>
            _dbContext.Patients
                .AsNoTracking()
                .Include(patient => patient.Name)
                .ThenInclude(name => name.Given);
    }
}
