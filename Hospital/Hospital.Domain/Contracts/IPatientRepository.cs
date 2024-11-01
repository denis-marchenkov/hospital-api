using Hospital.Domain.Entities;

namespace Hospital.Domain.Contracts
{
    public interface IPatientRepository
    {
        public Task<Patient?> GetByIdAsync(PatientId id, CancellationToken cancellationToken);

        public void Create(Patient patient);
        public void Update(Patient patient);
        public void Delete(Patient patient);

        public IQueryable<Patient> Query();
    }
}
