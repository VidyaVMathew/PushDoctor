using Microsoft.EntityFrameworkCore;
using PDR.PatientBooking.Data;
using PDR.PatientBooking.Data.Models;
using PDR.PatientBooking.Service.DoctorServices.Requests;
using PDR.PatientBooking.Service.DoctorServices.Responses;
using PDR.PatientBooking.Service.DoctorServices.Validation;
using PDR.PatientBooking.Service.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PDR.PatientBooking.Service.DoctorServices
{
    public class DoctorService : IDoctorService
    {
        private readonly PatientBookingContext _context;
        private readonly IAddDoctorRequestValidator _validator;

        public DoctorService(PatientBookingContext context, IAddDoctorRequestValidator validator)
        {
            _context = context;
            _validator = validator;
        }

        public long AddDoctor(AddDoctorRequest request)
        {
            var validationResult = _validator.ValidateRequest(request);

            if (!validationResult.PassedValidation)
            {
                throw new ArgumentException(validationResult.Errors.First());
            }

            Doctor doctor = new Doctor
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Gender = (int)request.Gender,
                Email = request.Email,
                DateOfBirth = request.DateOfBirth,
                Orders = new List<Order>(),
                Created = request.Created
            };

            _context.Doctor.Add(doctor);

            _context.SaveChanges();

            return doctor.Id;
        }

        public GetAllDoctorsResponse GetAllDoctors()
        {
            var doctors = _context
                .Doctor
                .Select(x => new GetAllDoctorsResponse.Doctor
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Email = x.Email,
                    DateOfBirth = x.DateOfBirth,
                    Gender = (Gender)x.Gender
                })
                .AsNoTracking()
                .ToList();

            return new GetAllDoctorsResponse
            {
                Doctors = doctors
            };
        }
    }
}
