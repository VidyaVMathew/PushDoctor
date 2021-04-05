
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PDR.PatientBooking.Data;
using PDR.PatientBooking.Service.BookingServices.Requests;
using PDR.PatientBooking.Service.Validation;

namespace PDR.PatientBooking.Service.BookingServices.Validation
{
    public class AddBookingRequestValidator : IAddBookingRequestValidator
    {
        private readonly PatientBookingContext _context;

        public AddBookingRequestValidator(PatientBookingContext context)
        {
            _context = context;
        }

        public PdrValidationResult ValidateRequest(BookingRequest request)
        {
            var result = new PdrValidationResult(true);

            if (IsAnAppointmentInPast(request, ref result))
                return result;

            if (IsSlotAlreadyBooked(request, ref result))
                return result;

            return result;
        }

        private bool IsAnAppointmentInPast(BookingRequest request, ref PdrValidationResult result)
        {
            if (!_context.Order.Any()) return false;

            if(request.StartTime <= DateTime.UtcNow.AddHours(1))
            {
                result.PassedValidation = false;
                result.Errors.Add("Appointment cannot be scheduled earlier than than current time plus 1 hour");
                return true;
            }

            return false;
        }

        private bool IsSlotAlreadyBooked(BookingRequest request, ref PdrValidationResult result)
        {
            if (!_context.Order.Any()) return false;


            if (_context.Order.Any(x => 
                                       (x.PatientId == request.PatientId) &&
                                       ((x.StartTime <= request.StartTime && x.EndTime >= request.StartTime) || (x.StartTime <= request.EndTime && x.EndTime >= request.EndTime))
                                   )
                )
            {
                result.PassedValidation = false;
                result.Errors.Add("There is already an appointment scheduled for the patient at the selected time");
                return true;
            }

            if (_context.Order.Any(x =>
                                       (x.DoctorId == request.DoctorId) &&
                                       ((x.StartTime <= request.StartTime && x.EndTime >= request.StartTime) || (x.StartTime <= request.EndTime && x.EndTime >= request.EndTime))
                                   )
                )
            {
                result.PassedValidation = false;
                result.Errors.Add("There is already an appointment scheduled for the doctor at the selected time");
                return true;
            }

            return false;
        }
    }
}
