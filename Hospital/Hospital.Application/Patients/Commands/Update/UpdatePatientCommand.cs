﻿using Hospital.Application.Infrastructure;
using Hospital.Domain.Entities;
using Hospital.Domain.Values;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Hospital.Application.Patients.Commands.Update
{
    public sealed record UpdatePatientCommand : IRequest<CqrsResult<PatientId>>
    {
        public PatientId Id { get; init; }
        public string Use { get; init; }

        [Required(ErrorMessage = "Family name is required")]
        public string Family { get; init; }
        public ICollection<string> GivenNames { get; init; }
        public Gender Gender { get; init; }
        public DateTime BirthDate { get; init; }
        public bool Active { get; init; } = true;
    }
}