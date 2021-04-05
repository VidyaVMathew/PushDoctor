using PDR.PatientBooking.Service.PatientServices.Requests;
using PDR.PatientBooking.Service.PatientServices.Responses;

namespace PDR.PatientBooking.Service.PatientServices
{
    public interface IPatientService
    {
        long AddPatient(AddPatientRequest request);
        GetAllPatientsResponse GetAllPatients();
    }
}