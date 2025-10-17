using Library.Healthcare.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Library.Healthcare.Models
{
    public class Physician
    {
        public string? Name { get; set; }
        public int Id { get; set; }
        public int License { get; set; }
        public string? GraduationDate { get; set; }
        public string? Specialization {  get; set; }
        public List<Appointment> Appointments = new List<Appointment>();

        public string Display
        {
            get
            {
                return ToString();
            }
        }

        public override string ToString()
        {
            return $"{Name}";
        }

        public Physician()
        {

        }

        public Physician(int id)
        {
            var physCopy = PhysicianServiceProxy.Current.Physicians.FirstOrDefault(b => (b?.Id ?? 0) == id);

            if (physCopy != null)
            {
                Id = physCopy.Id;
                Name = physCopy.Name;
                GraduationDate = physCopy.GraduationDate;
                Specialization = physCopy.Specialization;
                Appointments = physCopy.Appointments;
            }
        }
    }
}
