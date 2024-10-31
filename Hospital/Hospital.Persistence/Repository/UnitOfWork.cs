using Hospital.Domain.Contracts;

namespace Hospital.Persistence.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dbContext;

        private IPatientRepository _patientRepository;

        public IPatientRepository PatientRepository
        {
            get
            {
                _patientRepository ??= new PatientRepository(_dbContext);

                return _patientRepository;
            }
        }

        public UnitOfWork(AppDbContext dbContext)
        {
            _dbContext = dbContext;

        }
        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
