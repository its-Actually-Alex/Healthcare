namespace Maui.Healthcare.Views;
using Library.Healthcare.Models;
using Library.Healthcare.Services;

[QueryProperty(nameof(PatientId), "patientId")]
public partial class PatientView : ContentPage
{
    public int PatientId { get; set; }
	public PatientView()
	{
		InitializeComponent();
        BindingContext = new Patient();
	}

    private void CancelClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//MainPage");
    }

    private void OkClicked(object sender, EventArgs e)
    {
        PatientServiceProxy.Current.AddOrUpdate(BindingContext as Patient);

        Shell.Current.GoToAsync("//MainPage");
    }

    private void RemoveDiagnosisClicked(object sender, EventArgs e)
    {

    }

    private void AddDiagnosisClicked(object sender, EventArgs e)
    {

    }

    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        if (PatientId == 0)
        {
            BindingContext = new Patient();
        }
        else
        {
            BindingContext = new Patient(PatientId);
        }
    }
}