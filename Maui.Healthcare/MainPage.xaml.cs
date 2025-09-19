using Maui.Healthcare.ViewModels;

namespace Maui.Healthcare
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainViewModel();
        }

        private void AddClicked(object sender, EventArgs e)
        {

        }
    }
}
