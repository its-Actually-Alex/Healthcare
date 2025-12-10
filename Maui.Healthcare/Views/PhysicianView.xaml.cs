using Library.Healthcare.DTO;
using Library.Healthcare.Services;

namespace Maui.Healthcare.Views;

[QueryProperty(nameof(PhysicianId), "physicianId")]
public partial class PhysicianView : ContentPage
{
    public int PhysicianId { get; set; }

    public PhysicianView()
    {
        InitializeComponent();
        BindingContext = new PhysicianDTO();
    }

    private void CancelClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//MainPage");
    }

    private async void OkClicked(object sender, EventArgs e)
    {
        await PhysicianServiceProxy.Current.AddOrUpdate(BindingContext as PhysicianDTO);
        await Shell.Current.GoToAsync("//MainPage");
    }

    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        if (PhysicianId == 0)
        {
            BindingContext = new PhysicianDTO();
        }
        else
        {
            BindingContext = new PhysicianDTO(PhysicianId);
        }
    }
}