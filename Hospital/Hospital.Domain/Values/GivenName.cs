namespace Hospital.Domain.Values
{
    public class GivenName
    {
        public Guid Id { get; set; }
        public Guid NameId { get; set; }
        public string Value { get; set; }

        public GivenName() { }

        public static GivenName CreateNew(string value) => new()
        {
            Id = Guid.NewGuid(),
            Value = value
        };
    }
}
