using Library.Healthcare.Models;
using Library.Healthcare.Services;

namespace Library.Healthcare.DTO
{
    public class AppointmentDTO
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int PhysicianId { get; set; }
        public DateTime AppointmentTime { get; set; }  // Changed from AppointmentDate
        public List<int> DiagnosisIds { get; set; }

        public string Display
        {
            get
            {
                return ToString();
            }
        }

        public override string ToString()
        {
            return $"{Id}. Appointment on {AppointmentTime:MM/dd/yyyy hh:mm tt}";
        }
        public AppointmentDTO(Appointment apt)
        {
            Id = apt.Id;
            PatientId = apt.PatientId;
            PhysicianId = apt.PhysicianId;
            AppointmentTime = apt.AppointmentTime;
            DiagnosisIds = apt.DiagnosisIds ?? new List<int>();
        }

        public AppointmentDTO()
        {
            DiagnosisIds = new List<int>();
        }

        public AppointmentDTO(int id)
        {
            var aptCopy = AppointmentServiceProxy.Current.Appointments.FirstOrDefault(a => (a?.Id ?? 0) == id);
            if (aptCopy != null)
            {
                Id = aptCopy.Id;
                PatientId = aptCopy.PatientId;
                PhysicianId = aptCopy.PhysicianId;
                AppointmentTime = aptCopy.AppointmentTime;
                DiagnosisIds = aptCopy.DiagnosisIds ?? new List<int>();
            }
            else
            {
                DiagnosisIds = new List<int>();
            }
        }
    }
}