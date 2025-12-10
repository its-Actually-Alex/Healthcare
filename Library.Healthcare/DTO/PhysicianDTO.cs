using Library.Healthcare.Models;
using Library.Healthcare.Services;
using System.Net;
using System.Reflection;

namespace Library.Healthcare.DTO
{
    public class PhysicianDTO
    {
        public string? Name { get; set; }
        public int License { get; set; }
        public string? GraduationDate { get; set; }
        public string? Specialization { get; set; }
        public int Id { get; set; }

        public string Display
        {
            get
            {
                return ToString();
            }
        }

        public override string ToString()
        {
            //Display physicians as their ID and name
            return $"{Id}. {Name}";
        }

        public PhysicianDTO(Physician phy)
        {
            //convert from physician object
            Name = phy.Name;
            License = phy.License;
            GraduationDate = phy.GraduationDate;
            Specialization = phy.Specialization;
            Id = phy.Id;
        }

        public PhysicianDTO()
        {

        }

        public PhysicianDTO(int id)
        {
            //Convert from ID
            var phyCopy = PhysicianServiceProxy.Current.Physicians.FirstOrDefault(b => (b?.Id ?? 0) == id);

            if (phyCopy != null)
            {
                Name = phyCopy.Name;
                License = phyCopy.License;
                GraduationDate = phyCopy.GraduationDate;
                Specialization = phyCopy.Specialization;
                Id = phyCopy.Id;
            }
        }
    }
}
