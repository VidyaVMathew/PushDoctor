
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using PDR.PatientBooking.Service.BookingServices;
using PDR.PatientBooking.Service.BookingServices.Requests;
using PDR.PatientBooking.Service.BookingServices.Responses;

namespace PDR.PatientBookingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        //private readonly PatientBookingContext _context;
        private readonly IBookingService _bookingService;

        //public BookingController(PatientBookingContext context)
        //{
        //    _context = context;
        //} 

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpGet("patient/{identificationNumber}/next")]
        public IActionResult GetPatientNextAppointment(long identificationNumber)
        {

            try
            {
                GetPatientBookingResponse booking = _bookingService.GetNextAppointment(identificationNumber);
                if(booking == null) 
                {
                    return StatusCode(502);
                }
                return Ok(booking);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }

            //var bockings = _context.Order.OrderBy(x => x.StartTime).ToList();

            //if (bockings.Where(x => x.Patient.Id == identificationNumber).Count() == 0)
            //{
            //    return StatusCode(502);
            //}
            //else
            //{
            //    var bookings2 = bockings.Where(x => x.PatientId == identificationNumber);
            //    if (bookings2.Where(x => x.StartTime > DateTime.Now).Count() == 0)
            //    {
            //        return StatusCode(502);
            //    }
            //    else
            //    {
            //        var bookings3 = bookings2.Where(x => x.StartTime > DateTime.Now);
            //        return Ok(new
            //        {
            //            bookings3.First().Id,
            //            bookings3.First().DoctorId,
            //            bookings3.First().StartTime,
            //            bookings3.First().EndTime
            //        });
            //    }
            //}
        }

        [HttpPost()]
        public IActionResult AddBooking(BookingRequest request)
        {
            try
            {
                var newBooking = _bookingService.AddBooking(request);
                return Ok(newBooking);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }

            //var bookingId = new Guid();
            //var bookingStartTime = newBooking.StartTime;
            //var bookingEndTime = newBooking.EndTime;
            //var bookingPatientId = newBooking.PatientId;
            //var bookingPatient = _context.Patient.FirstOrDefault(x => x.Id == newBooking.PatientId);
            //var bookingDoctorId = newBooking.DoctorId;
            //var bookingDoctor = _context.Doctor.FirstOrDefault(x => x.Id == newBooking.DoctorId);
            //var bookingSurgeryType = _context.Patient.FirstOrDefault(x => x.Id == bookingPatientId).Clinic.SurgeryType;

            //var myBooking = new Order
            //{
            //    Id = bookingId,
            //    StartTime = bookingStartTime,
            //    EndTime = bookingEndTime,
            //    PatientId = bookingPatientId,
            //    DoctorId = bookingDoctorId,
            //    Patient = bookingPatient,
            //    Doctor = bookingDoctor,
            //    SurgeryType = (int)bookingSurgeryType
            //};

            //_context.Order.AddRange(new List<Order> { myBooking });
            //_context.SaveChanges();

            //return StatusCode(200);
        }

        [HttpDelete()]
        public IActionResult CancelBooking(string bookingId)
        {
            try
            {
                var cancelBooking = _bookingService.CancelBooking(bookingId);
                return Ok(cancelBooking);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        //public class NewBooking
        //{
        //    public Guid Id { get; set; }
        //    public DateTime StartTime { get; set; }
        //    public DateTime EndTime { get; set; }
        //    public long PatientId { get; set; }
        //    public long DoctorId { get; set; }
        //}

        //private static MyOrderResult UpdateLatestBooking(List<Order> bookings2, int i)
        //{
        //    MyOrderResult latestBooking;
        //    latestBooking = new MyOrderResult();
        //    latestBooking.Id = bookings2[i].Id;
        //    latestBooking.DoctorId = bookings2[i].DoctorId;
        //    latestBooking.StartTime = bookings2[i].StartTime;
        //    latestBooking.EndTime = bookings2[i].EndTime;
        //    latestBooking.PatientId = bookings2[i].PatientId;
        //    latestBooking.SurgeryType = (int)bookings2[i].GetSurgeryType();

        //    return latestBooking;
        //}

        //private class MyOrderResult
        //{
        //    public Guid Id { get; set; }
        //    public DateTime StartTime { get; set; }
        //    public DateTime EndTime { get; set; }
        //    public long PatientId { get; set; }
        //    public long DoctorId { get; set; }
        //    public int SurgeryType { get; set; }
        //}
    }
}