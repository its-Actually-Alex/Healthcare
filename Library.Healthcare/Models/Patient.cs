using Library.Healthcare.Models;
using Library.Healthcare.Services;
using Library.Healthcare.DTO;

namespace Library.Healthcare.Models
{
    public class Patient
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

        public List<int> AppointmentIds { get; set; } = new List<int> { };
        public List<int> DiagnosisIds { get; set; } = new List<int> { };

        public override string ToString()
        {
            return $"{Name}";
        }

        public void DetailedView()
        {
            var diagnosisText = DiagnosisIds.Any()
            ? string.Join("\n ", DiagnosisIds)
            : "No recent diagnoses";
            Console.WriteLine($"ID: {Id}\nName: {Name}\nAddress: {Address}\n" +
                $"Birthdate: {Birthdate}\nGender: {Gender}\nRecent Diagnoses:\n{diagnosisText}\n");
        }

        public Patient()
        {

        }

        public Patient(PatientDTO patientDto)
        {
            Name = patientDto.Name;
            Address = patientDto.Address;
            Birthdate = patientDto.Birthdate;
            Gender = patientDto.Gender;
            Id = patientDto.Id;
        }

        public Patient(int id)
        {
            var patCopy = PatientServiceProxy.Current.Patients.FirstOrDefault(b => (b?.Id ?? 0) == id);

            if(patCopy != null)
            {
                Id = patCopy.Id;
                Name = patCopy.Name;
                Address = patCopy.Address;
                Birthdate = patCopy.Birthdate;
                Gender = patCopy.Gender;
            }
        }
    }
}
