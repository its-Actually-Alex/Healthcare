using Library.Healthcare.Models;
using Newtonsoft.Json;
using System.Reflection.Metadata;

namespace Api.Healthcare.Database
{
    public class Filebase
    {
        private string _root;
        private string _patientRoot;
        private string _appointmentRoot;
        private static Filebase _instance;


        public static Filebase Current
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Filebase();
                }

                return _instance;
            }
        }

        private Filebase()
        {
            _root = @"C:\temp";
            _patientRoot = $"{_root}\\Patients";
            _appointmentRoot = $"{_root}\\Appointments";

            // Create directories if they don't exist
            if (!Directory.Exists(_root))
            {
                Directory.CreateDirectory(_root);
            }
            if (!Directory.Exists(_patientRoot))
            {
                Directory.CreateDirectory(_patientRoot);
            }
            if (!Directory.Exists(_appointmentRoot))
            {
                Directory.CreateDirectory(_appointmentRoot);
            }
        }

        public int LastPatientKey
        {
            get
            {
                if (Patients.Any())
                {
                    return Patients.Select(x => x.Id).Max();
                }
                return 0;
            }
        }

        public Patient AddOrUpdate(Patient pat)
        {
            //set up a new Id if one doesn't already exist
            if (pat.Id <= 0)
            {
                pat.Id = LastPatientKey + 1;
            }

            //go to the right place
            string path = $"{_patientRoot}\\{pat.Id}.json";


            //if the item has been previously persisted
            if (File.Exists(path))
            {
                //blow it up
                File.Delete(path);
            }

            //write the file
            File.WriteAllText(path, JsonConvert.SerializeObject(pat));

            //return the item, which now has an id
            return pat;
        }

        public List<Patient> Patients
        {
            get
            {
                var root = new DirectoryInfo(_patientRoot);
                var _pats = new List<Patient>();
                foreach (var patientFile in root.GetFiles())
                {
                    var patient = JsonConvert
                        .DeserializeObject<Patient>
                        (File.ReadAllText(patientFile.FullName));
                    if (patient != null)
                    {
                        _pats.Add(patient);
                    }

                }
                return _pats;
            }
        }
        public bool Delete(int id)
        {
            string path = $"{_patientRoot}\\{id}.json";

            if (File.Exists(path))
            {
                File.Delete(path);
                return true;
            }

            return false;
        }
    

    public int LastAppointmentKey
        {
            get
            {
                if (Appointments.Any())
                {
                    return Appointments.Select(x => x.Id).Max();
                }
                return 0;
            }
        }

        public Appointment AddOrUpdate(Appointment apt)
        {
            //set up a new Id if one doesn't already exist
            if (apt.Id <= 0)
            {
                apt.Id = LastAppointmentKey + 1;
            }
            //go to the right place
            string path = $"{_appointmentRoot}\\{apt.Id}.json";
            //if the item has been previously persisted
            if (File.Exists(path))
            {
                //blow it up
                File.Delete(path);
            }
            //write the file
            File.WriteAllText(path, JsonConvert.SerializeObject(apt));
            //return the item, which now has an id
            return apt;
        }

        public List<Appointment> Appointments
        {
            get
            {
                var root = new DirectoryInfo(_appointmentRoot);
                var _apts = new List<Appointment>();
                foreach (var appointmentFile in root.GetFiles())
                {
                    var appointment = JsonConvert
                        .DeserializeObject<Appointment>
                        (File.ReadAllText(appointmentFile.FullName));
                    if (appointment != null)
                    {
                        _apts.Add(appointment);
                    }
                }
                return _apts;
            }
        }

        public bool DeleteAppointment(int id)
        {
            string path = $"{_appointmentRoot}\\{id}.json";
            if (File.Exists(path))
            {
                File.Delete(path);
                return true;
            }
            return false;
        }

    }
}
