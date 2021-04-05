using System;
using System.Linq;
using PDR.PatientBooking.Data;
using PDR.PatientBooking.Service.Validation;

namespace PDR.PatientBooking.Service.BookingServices.Validation
{
    public class CancelBookingRequestValidator : ICancelBookingRequestValidator
    {
        private readonly PatientBookingContext _context;

        public CancelBookingRequestValidator(PatientBookingContext context)
        {
            _context = context;
        }

        public PdrValidationResult ValidateRequest(string bookingId)
        {
            var result = new PdrValidationResult(true);


            Guid guid = Guid.Empty;
            if (IsNotAValidGuid(bookingId, ref result, out guid))
                return result;

            if (IsNotValidAppoinment(guid, ref result))
                return result;

            return result;

        }

        private bool IsNotAValidGuid(string bookingId, ref PdrValidationResult result, out Guid guid)
        {

            if (!Guid.TryParse(bookingId, out guid))
            {
                result.PassedValidation = false;
                result.Errors.Add("BookingId is not valid");
                return true;
            }
            return false;
        }

        private bool IsNotValidAppoinment(Guid guid, ref PdrValidationResult result)
        {
            if (!_context.Order.Any())
            {
                result.PassedValidation = false;
                result.Errors.Add("No appointments were found in the system for the given bookingId");
                return true;
            }

            var order = _context.Order.FirstOrDefault(x => x.Id == guid);
            if (order == null)
            {
                result.PassedValidation = false;
                result.Errors.Add("No appointments were found in the system for the given bookingId");
                return true;
            }

            if (DateTime.UtcNow >= order.StartTime.AddHours(-1))
            {
                result.PassedValidation = false;
                result.Errors.Add("Cannot cancel an appoinment in the past or less than 1 hour from the start time");
                return true;
            }

            return false;
        }
    }
}
