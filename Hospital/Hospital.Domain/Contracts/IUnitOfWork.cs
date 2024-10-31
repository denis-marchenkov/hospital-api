namespace Hospital.Domain.Contracts
{
    public interface IUnitOfWork
    {
        public IPatientRepository PatientRepository { get; }
        public Task SaveChangesAsync(CancellationToken cancellationToken);
    }
}
