using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Library.Healthcare.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int PhysicianId { get; set; }
        public DateTime AppointmentTime { get; set; }

        public List<int> DiagnosisIds { get; set; } = new List<int>();

        public string Display
        {
            get
            {
                return ToString();
            }
        }
        public override string ToString()
        {
            return $"{AppointmentTime}";
        }
    }
}
