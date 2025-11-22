namespace Maui.Healthcare.Views;
using Library.Healthcare.Models;
using Library.Healthcare.Services;
using Library.Healthcare.DTO;

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
        PatientServiceProxy.Current.AddOrUpdate(BindingContext as PatientDTO);

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
            BindingContext = new PatientDTO();
        }
        else
        {
            BindingContext = new PatientDTO(PatientId);
        }
    }
}