using Hospital.Domain.Values;
using System.ComponentModel.DataAnnotations;

namespace Hospital.Domain.Entities
{
    public class Name
    {
        public Guid Id { get; private set; }
        public PatientId PatientId { get; private set; }
        public string Use { get; private set; }

        [Required(ErrorMessage = "Family name is required")]
        public string Family { get; private set; }

        public ICollection<GivenName> Given { get; private set; }

        public Name() { }

        public static Name CreateNew(string use, string family, IEnumerable<GivenName> given)
        {
            if (string.IsNullOrWhiteSpace(family))
                throw new ArgumentException("Required", nameof(family));

            return new()
            {
                Id = Guid.NewGuid(),
                Use = use,
                Family = family,
                Given = given.ToList()
            };
        }

        public void Update(Name nameToUpdate)
        {
            nameToUpdate.Use = Use;
            nameToUpdate.Family = Family;

            foreach (var givenName in Given)
            {
                nameToUpdate.Given.Add(new GivenName { Value = givenName.Value });
            }
        }
    }
}
