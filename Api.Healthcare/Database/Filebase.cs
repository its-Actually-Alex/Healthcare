using Library.Healthcare.Models;
using Newtonsoft.Json;
using System.Reflection.Metadata;

namespace Api.Healthcare.Database
{
    public class Filebase
    {
        private string _root;
        private string _patientRoot;
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


        public bool Delete(string type, string id)
        {
            //TODO: refer to AddOrUpdate for an idea of how you can implement this.
            return true;
        }
    }



}
