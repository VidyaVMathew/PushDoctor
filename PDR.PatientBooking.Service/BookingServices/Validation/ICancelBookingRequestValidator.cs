
using System;
using System.Collections.Generic;
using System.Text;
using PDR.PatientBooking.Service.Validation;

namespace PDR.PatientBooking.Service.BookingServices.Validation
{
    public interface ICancelBookingRequestValidator
    {
        PdrValidationResult ValidateRequest(string bookingId);
    }
}
