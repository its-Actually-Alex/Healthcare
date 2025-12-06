using Api.Healthcare.Database;
using Library.Healthcare.DTO;
using Library.Healthcare.Models;

namespace Api.Healthcare.Enterprise
{
    public class AppointmentEC
    {
        public IEnumerable<AppointmentDTO> GetAppointments()
        {
            return Filebase.Current.Appointments
                .Select(a => new AppointmentDTO(a))
                .OrderByDescending(a => a.AppointmentTime)
                .Take(100);
        }

        public AppointmentDTO? GetById(int id)
        {
            var apt = Filebase.Current.Appointments.FirstOrDefault(a => a.Id == id);
            if (apt == null)
            {
                return null;
            }
            return new AppointmentDTO(apt);
        }

        public AppointmentDTO? Delete(int id)
        {
            var toRemove = Filebase.Current.Appointments.FirstOrDefault(a => a.Id == id);

            if (toRemove != null)
            {
                Filebase.Current.DeleteAppointment(id);
            }

            return toRemove != null ? new AppointmentDTO(toRemove) : null;
        }

        public AppointmentDTO? AddOrUpdate(AppointmentDTO? appointmentDTO)
        {
            if (appointmentDTO == null)
            {
                return null;
            }

            var apt = new Appointment
            {
                Id = appointmentDTO.Id,
                PatientId = appointmentDTO.PatientId,
                PhysicianId = appointmentDTO.PhysicianId,
                AppointmentTime = appointmentDTO.AppointmentTime,
                DiagnosisIds = appointmentDTO.DiagnosisIds
            };

            appointmentDTO = new AppointmentDTO(Filebase.Current.AddOrUpdate(apt));
            return appointmentDTO;
        }

        public IEnumerable<AppointmentDTO?> Search(string query)
        {
            return Filebase.Current.Appointments.Where(
                a => a.AppointmentTime.ToString().Contains(query ?? string.Empty)
            ).Select(a => new AppointmentDTO(a));
        }
    }
}