using Library.Healthcare.Models;

namespace Library.Healthcare.Models
{
    public class Patient
    {
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? Birthdate { get; set; }
        public char? Gender { get; set; }
        public int? Age { get; set; }
        public int Id { get; set; }

        public string Display
        {
            get
            {
                return ToString();
            }
        }

        public List<Diagnosis> Diagnoses { get; set; } = new List<Diagnosis> { };

        public override string ToString()
        {
            return $"{Name}";
        }

        public void DetailedView()
        {
            var diagnosisText = Diagnoses.Any()
            ? string.Join("\n ", Diagnoses)
            : "No recent diagnoses";
            Console.WriteLine($"ID: {Id}\nName: {Name}\nAddress: {Address}\nAge: {Age}\n" +
                $"Birthdate: {Birthdate}\nGender: {Gender}\nRecent Diagnoses:\n{diagnosisText}\n");
        }
    }
}
