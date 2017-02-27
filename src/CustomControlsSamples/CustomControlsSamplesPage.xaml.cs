using Xamarin.Forms;

namespace CustomControlsSamples
{
    public partial class CustomControlsSamplesPage : ContentPage
    {
        public CustomControlsSamplesPage()
        {
            InitializeComponent();

            BindingContext = new CustomSampleViewModel();
        }
    }
}
