namespace Maui.Healthcare.Views;
using Library.Healthcare.Models;
using Library.Healthcare.Services;

[QueryProperty(nameof(PhysicianId), "physicianId")]
public partial class PhysicianView : ContentPage
{
    public int PhysicianId { get; set; }
    public PhysicianView()
	{
		InitializeComponent();
		BindingContext = new Physician();
	}

    private void CancelClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//MainPage");
    }

    private void OkClicked(object sender, EventArgs e)
    {
        PhysicianServiceProxy.Current.AddOrUpdate(BindingContext as Physician);

        Shell.Current.GoToAsync("//MainPage");
    }

    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        if (PhysicianId == 0)
        {
            BindingContext = new Physician();
        }
        else
        {
            BindingContext = new Physician(PhysicianId);
        }
    }
}