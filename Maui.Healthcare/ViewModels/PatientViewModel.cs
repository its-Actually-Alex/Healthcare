using Library.Healthcare.Models;
using Library.Healthcare.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Maui.Healthcare.ViewModels
{
    public class PatientViewModel
    {
        public PatientViewModel()
        {
            Model = new Patient();
            SetUpCommands();
        }

        public PatientViewModel(Patient? model)
        {
            Model = model;
            SetUpCommands();
        }
        
        private void SetUpCommands()
        {
            DeleteCommand = new Command(DoDelete);
            EditCommand = new Command((p) => DoEdit(p as PatientViewModel));
        }

        private void DoDelete()
        {
            if(Model?.Id > 0)
            {
                PatientServiceProxy.Current.Delete(Model.Id);
                Shell.Current.GoToAsync("//MainPage");
            }
        }

        private void DoEdit(PatientViewModel? pv)
        {
            if(pv == null)
            {
                return;
            }
            var selectedId = pv?.Model?.Id ?? 0;
            Shell.Current.GoToAsync($"//Patient?patientId={selectedId}");
        }

        public Patient? Model { get; set; }

        public ICommand? DeleteCommand { get; set; }
        public ICommand EditCommand { get; set; }
    }
}
