using Library.Healthcare.Models;

namespace Api.Healthcare.Nonsense
{
    public static class FakeDatabase
    {
        public static List<Patient> Patients = new List<Patient>
        {
            new Patient{Name = "John Doe", Birthdate = "01/01/2000", Id = 1},
            new Patient{Name = "Jane Doe", Birthdate = "02/15/1967", Id = 2},
            new Patient{Name = "Jim Doe", Birthdate = "06/07/1941", Id = 3}
        };
    }
}
