using System;
using System.Collections.Generic;
using System.Text;

namespace PDR.PatientBooking.Service.BookingServices.Responses
{
    public class GetPatientBookingResponse
    {
        public Guid Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public long DoctorId { get; set; }
    }
}
