using Library.Healthcare.Models;
using Library.Healthcare.Services;
using System.IO;
using System.Reflection.Metadata;

namespace Library.Healthcare.DTO
{
    public class PatientDTO
    {
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? Birthdate { get; set; }
        public char? Gender { get; set; }
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
            return $"{Id}. {Name}";
        }

        public PatientDTO(Patient pat)
        {
            Name = pat.Name;
            Address = pat.Address;
            Birthdate = pat.Birthdate;
            Gender = pat.Gender;
            Id = pat.Id;
        }

        public PatientDTO()
        {

        }

        public PatientDTO(int id)
        {
            var patCopy = PatientServiceProxy.Current.Patients.FirstOrDefault(b => (b?.Id ?? 0) == id);

            if (patCopy != null)
            {
                Name = patCopy.Name;
                Address = patCopy.Address;
                Birthdate = patCopy.Birthdate;
                Gender = patCopy.Gender;
                Id = patCopy.Id;
            }

        }
    }
}