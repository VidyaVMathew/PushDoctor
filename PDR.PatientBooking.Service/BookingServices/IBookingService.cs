using System;
using System.Collections.Generic;
using System.Text;
using PDR.PatientBooking.Service.BookingServices.Requests;
using PDR.PatientBooking.Service.BookingServices.Responses;

namespace PDR.PatientBooking.Service.BookingServices
{
    public interface IBookingService
    {
        GetBookingOrderResponse AddBooking(BookingRequest request);
        GetPatientBookingResponse GetNextAppointment(long identificationNumber);
        GetBookingOrderResponse CancelBooking(string bookingId);
    }
}
