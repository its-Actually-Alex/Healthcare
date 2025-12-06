using Library.Healthcare.DTO;
using Library.Healthcare.Services;
using System.Collections.ObjectModel;
using Microsoft.Maui.Controls;

namespace Maui.Healthcare.Views;

[QueryProperty(nameof(AppointmentId), "appointmentId")]
public partial class AppointmentView : ContentPage
{
    public int AppointmentId { get; set; }

    // Properties for binding
    private DateTime appointmentDate = DateTime.Today;
    public DateTime AppointmentDate
    {
        get => appointmentDate;
        set => appointmentDate = value;
    }

    private TimeSpan appointmentTime = DateTime.Now.TimeOfDay;
    public TimeSpan AppointmentTime
    {
        get => appointmentTime;
        set => appointmentTime = value;
    }

    public AppointmentView()
    {
        InitializeComponent();
        BindingContext = new AppointmentDTO();
    }

    private void CancelClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//MainPage");
    }

    private async void OkClicked(object sender, EventArgs e)
    {
        var dto = BindingContext as AppointmentDTO;
        if (dto != null)
        {
            // Combine date and time into AppointmentTime
            dto.AppointmentTime = AppointmentDate.Date + AppointmentTime;
            await AppointmentServiceProxy.Current.AddOrUpdate(dto);
        }
        await Shell.Current.GoToAsync("//MainPage");
    }

    private void AddDiagnosisClicked(object sender, EventArgs e)
    {
        if (int.TryParse(NewDiagnosisEntry.Text, out int diagnosisId))
        {
            var dto = BindingContext as AppointmentDTO;
            if (dto != null)
            {
                if (dto.DiagnosisIds == null)
                {
                    dto.DiagnosisIds = new List<int>();
                }
                dto.DiagnosisIds.Add(diagnosisId);

                // Refresh the binding
                var temp = BindingContext;
                BindingContext = null;
                BindingContext = temp;

                // Clear the entry
                NewDiagnosisEntry.Text = string.Empty;
            }
        }
        else
        {
            DisplayAlert("Error", "Please enter a valid diagnosis ID", "OK");
        }
    }

    private void RemoveDiagnosisClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        if (button?.CommandParameter is int diagnosisId)
        {
            var dto = BindingContext as AppointmentDTO;
            if (dto != null && dto.DiagnosisIds != null)
            {
                dto.DiagnosisIds.Remove(diagnosisId);

                // Refresh the binding
                var temp = BindingContext;
                BindingContext = null;
                BindingContext = temp;
            }
        }
    }

    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        if (AppointmentId == 0)
        {
            var newApt = new AppointmentDTO();
            AppointmentDate = DateTime.Today;
            AppointmentTime = DateTime.Now.TimeOfDay;
            BindingContext = newApt;
        }
        else
        {
            var apt = new AppointmentDTO(AppointmentId);
            AppointmentDate = apt.AppointmentTime.Date;
            AppointmentTime = apt.AppointmentTime.TimeOfDay;
            BindingContext = apt;
        }
    }
}