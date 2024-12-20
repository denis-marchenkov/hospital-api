﻿using Hospital.Domain.Converters;
using Hospital.Domain.Values;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Hospital.Domain.Entities
{
    public class Patient
    {
        public PatientId Id { get; private set; }
        public Name Name { get; private set; }

        [JsonConverter(typeof(GenderEnumConverter))]
        public Gender Gender { get; private set; } = Gender.Unknown;

        [Required(ErrorMessage = "Date of Birth is required")]
        public DateTime BirthDate { get; private set; }

        public bool Active { get; private set; }

        public Patient() { }

        public static Patient CreateNew(string use,
            string family,
            ICollection<string> given,
            Gender gender,
            DateTime birthDate,
            bool active,
            PatientId? id = null)
        {

            var givenNames = given.Select(GivenName.CreateNew).ToList();
            var name = Name.CreateNew(use, family, givenNames);

            return new()
            {
                Id = id != null ? id.Value : PatientId.New(),
                Name = name,
                Gender = gender,
                BirthDate = birthDate,
                Active = active
            };
        }

        public void Update(Patient patientToUpdate)
        {
            Name.Update(patientToUpdate.Name);
            patientToUpdate.Gender = Gender;
            patientToUpdate.BirthDate = BirthDate;
            patientToUpdate.Active = Active;
        }
    }

    public readonly record struct PatientId(Guid Value)
    {
        public static PatientId New() => new(Guid.NewGuid());
        public static PatientId Empty => new(Guid.Empty);
    }
}
