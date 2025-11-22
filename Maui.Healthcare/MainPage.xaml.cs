using Maui.Healthcare.ViewModels;
using System.ComponentModel;

namespace Maui.Healthcare
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainViewModel();
        }

        private void PatientAddClicked(object sender, EventArgs e)
        {
            Shell.Current.GoToAsync("//Patient?patientId=0");
        }

        private void PhysicianAddClicked(object sender, EventArgs e)
        {
            Shell.Current.GoToAsync("//Physician?physicianId=0");
        }

        private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
        {
            (BindingContext as MainViewModel)?.Refresh();
        }

        private void DeletePatientClicked(object sender, EventArgs e)
        {
            (BindingContext as MainViewModel)?.Delete();
        }

        private void PatientEditClicked(object sender, EventArgs e)
        {
            var selectedId = (BindingContext as MainViewModel)?.SelectedPatient?.Model?.Id ?? 0;
            Shell.Current.GoToAsync($"//Patient?patientId={selectedId}");
        }

        /*private void PhysicianEditClicked(object sender, EventArgs e)
        {
            var selectedId = (BindingContext as MainViewModel)?.SelectedPhysician?.Model?.Id ?? 0;
            Shell.Current.GoToAsync($"//Physician?physicianId={selectedId}");
        }*/

        private void InlineEditClicked(object sender, EventArgs e)
        {
            (BindingContext as MainViewModel)?.Refresh();
        }

        private void SearchClicked(object sender, EventArgs e)
        {
            (BindingContext as MainViewModel)?.Refresh();
        }

        private void ExpandCardClicked(object sender, EventArgs e)
        {
            (BindingContext as MainViewModel)?.ExpandCard();
        }
    }

}
