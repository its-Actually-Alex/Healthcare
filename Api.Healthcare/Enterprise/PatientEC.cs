using Api.Healthcare.Database;
using Library.Healthcare.DTO;
using Library.Healthcare.Models;
using System.Reflection.Metadata;

namespace Api.Healthcare.Enterprise
{
    public class PatientEC
    {
        public IEnumerable<PatientDTO> GetPatients()
        {
            return Filebase.Current.Patients
                .Select(b => new PatientDTO(b))
                .OrderByDescending(b => b.Id)
                .Take(100);
        }
        public PatientDTO? GetById(int id)
        {
            var pat = Filebase.Current.Patients.FirstOrDefault(b => b.Id == id);
            return new PatientDTO(pat);
        }

        public PatientDTO? Delete(int id)
        {
            var toRemove = Filebase.Current.Patients.FirstOrDefault(b => b.Id == id);
            if (toRemove != null)
            {
                Filebase.Current.Patients.Remove(toRemove);
            }
            return new PatientDTO(toRemove);
        }

        public PatientDTO? AddOrUpdate(PatientDTO? patientDTO)
        {
            if (patientDTO == null)
            {
                return null;
            }
            var pat = new Patient(patientDTO);
            patientDTO = new PatientDTO(Filebase.Current.AddOrUpdate(pat));
            return patientDTO;
        }

        public IEnumerable<PatientDTO?> Search(string query)
        {
            return Filebase.Current.Patients.Where(
                        b => (b?.Name?.ToUpper()?.Contains(query?.ToUpper() ?? string.Empty) ?? false)
                    ).Select(b => new PatientDTO(b));
        }
    }
}
