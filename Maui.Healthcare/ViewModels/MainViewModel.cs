using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Healthcare.Models;

namespace Maui.Healthcare.ViewModels
{
    public partial class MainViewModel
    {
         public List<Patient> Patients
         {
            get
            {
                return new List<Patient>
                {
                    new Patient {Name = "John Doe" },
                    new Patient {Name = "Jane Doe" }
                };
            }
         }

        public Patient? SelectedPatient { get; set; }
    }
}
