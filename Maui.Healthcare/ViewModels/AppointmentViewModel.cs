using Library.Healthcare.DTO;
using Library.Healthcare.Services;
using System.Windows.Input;

namespace Maui.Healthcare.ViewModels
{
    public class AppointmentViewModel
    {
        public AppointmentViewModel()
        {
            Model = new AppointmentDTO();
            SetUpCommands();
        }

        public AppointmentViewModel(AppointmentDTO? model)
        {
            Model = model;
            SetUpCommands();
        }

        private void SetUpCommands()
        {
            DeleteCommand = new Command(DoDelete);
            EditCommand = new Command((a) => DoEdit(a as AppointmentViewModel));
        }

        private void DoDelete()
        {
            if (Model?.Id > 0)
            {
                AppointmentServiceProxy.Current.Delete(Model.Id);
                Shell.Current.GoToAsync("//MainPage");
            }
        }

        private void DoEdit(AppointmentViewModel? av)
        {
            if (av == null)
            {
                return;
            }
            var selectedId = av?.Model?.Id ?? 0;
            Shell.Current.GoToAsync($"//Appointment?appointmentId={selectedId}");
        }

        public AppointmentDTO? Model { get; set; }
        public ICommand? DeleteCommand { get; set; }
        public ICommand EditCommand { get; set; }
    }
}