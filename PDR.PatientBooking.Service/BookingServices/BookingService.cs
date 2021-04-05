
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PDR.PatientBooking.Data;
using PDR.PatientBooking.Data.Models;
using PDR.PatientBooking.Service.BookingServices.Requests;
using PDR.PatientBooking.Service.BookingServices.Responses;
using PDR.PatientBooking.Service.BookingServices.Validation;

namespace PDR.PatientBooking.Service.BookingServices
{
    public class BookingService : IBookingService
    {
        private readonly PatientBookingContext _context;
        private readonly IAddBookingRequestValidator _addBookingValidator;
        private readonly ICancelBookingRequestValidator _cancelBookingValidator;

        public BookingService(PatientBookingContext context, IAddBookingRequestValidator addBookingValidator, ICancelBookingRequestValidator cancelBookingValidator)
        {
            _context = context;
            _addBookingValidator = addBookingValidator;
            _cancelBookingValidator = cancelBookingValidator;
        }

        public GetPatientBookingResponse GetNextAppointment(long identificationNumber)
        {
            var bookings = _context.Order.OrderBy(x => x.StartTime).ToList();

            if (bookings.Where(x => x.Patient.Id == identificationNumber).Count() == 0)
            {
                return null;
            }
            else
            {
                var bookings2 = bookings.Where(x => x.PatientId == identificationNumber);
                if (bookings2.Where(x => x.StartTime > DateTime.Now).Count() == 0)
                {
                    return null;
                }
                else
                {
                    var bookings3 = bookings2.Where(x => x.StartTime > DateTime.Now);
                    return new GetPatientBookingResponse
                    {
                        Id = bookings3.First().Id,
                        DoctorId = bookings3.First().DoctorId,
                        StartTime = bookings3.First().StartTime,
                        EndTime = bookings3.First().EndTime
                    };
                }
            }
        }

        public GetBookingOrderResponse AddBooking(BookingRequest request)
        {
            var validationResult = _addBookingValidator.ValidateRequest(request);

            if (!validationResult.PassedValidation)
            {
                throw new ArgumentException(validationResult.Errors.First());
            }

            var bookingId = new Guid();
            var bookingStartTime = request.StartTime;
            var bookingEndTime = request.EndTime;
            var bookingPatientId = request.PatientId;
            var bookingPatient = _context.Patient.FirstOrDefault(x => x.Id == request.PatientId);
            var bookingDoctorId = request.DoctorId;
            var bookingDoctor = _context.Doctor.FirstOrDefault(x => x.Id == request.DoctorId);
            var bookingSurgeryType = _context.Patient.FirstOrDefault(x => x.Id == bookingPatientId).Clinic.SurgeryType;

            Order newBooking = new Order
            {
                Id = bookingId,
                StartTime = bookingStartTime,
                EndTime = bookingEndTime,
                PatientId = bookingPatientId,
                DoctorId = bookingDoctorId,
                Patient = bookingPatient,
                Doctor = bookingDoctor,
                SurgeryType = (int)bookingSurgeryType
            };

            _context.Order.AddRange(new List<Order> { newBooking });
            _context.SaveChanges();

            return new GetBookingOrderResponse 
            {
                Id = newBooking.Id,
                StartTime = newBooking.StartTime,
                EndTime = newBooking.EndTime,
                PatientId = newBooking.PatientId,
                DoctorId = newBooking.DoctorId,
                SurgeryType = newBooking.SurgeryType
            };
        }

        public GetBookingOrderResponse CancelBooking(string bookingId)
        {
            var validationResult = _cancelBookingValidator.ValidateRequest(bookingId);

            if (!validationResult.PassedValidation)
            {
                throw new ArgumentException(validationResult.Errors.First());
            }

            var result = _context.Order.FirstOrDefault(x => x.Id == new Guid(bookingId));
            if (result != null)
            {
                _context.Order.Remove(result);
                _context.SaveChanges();
                return new GetBookingOrderResponse
                {
                    Id = result.Id,
                    StartTime = result.StartTime,
                    EndTime = result.EndTime,
                    PatientId = result.PatientId,
                    DoctorId = result.DoctorId,
                    SurgeryType = result.SurgeryType
                };
            }

            return null;
        }

    }
}
