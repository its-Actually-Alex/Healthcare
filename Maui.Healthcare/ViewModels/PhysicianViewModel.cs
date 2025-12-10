using Library.Healthcare.DTO;
using Library.Healthcare.Services;
using System.Windows.Input;

namespace Maui.Healthcare.ViewModels
{
    public class PhysicianViewModel
    {
        public PhysicianViewModel()
        {
            Model = new PhysicianDTO();
            SetUpCommands();
        }

        public PhysicianViewModel(PhysicianDTO? model)
        {
            Model = model;
            SetUpCommands();
        }

        private void SetUpCommands()
        {
            DeleteCommand = new Command(DoDelete);
            EditCommand = new Command((p) => DoEdit(p as PhysicianViewModel));
        }

        private void DoDelete()
        {
            if (Model?.Id > 0)
            {
                PhysicianServiceProxy.Current.Delete(Model.Id);
                Shell.Current.GoToAsync("//MainPage");
            }
        }

        private void DoEdit(PhysicianViewModel? pv)
        {
            if (pv == null)
            {
                return;
            }
            var selectedId = pv?.Model?.Id ?? 0;
            Shell.Current.GoToAsync($"//Physician?physicianId={selectedId}");
        }

        public PhysicianDTO? Model { get; set; }
        public ICommand? DeleteCommand { get; set; }
        public ICommand EditCommand { get; set; }
    }
}