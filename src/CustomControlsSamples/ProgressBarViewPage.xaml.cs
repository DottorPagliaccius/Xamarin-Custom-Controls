using Xamarin.Forms;

namespace CustomControlsSamples
{
    public partial class ProgressBarViewPage : ContentPage
    {
        public ProgressBarViewPage()
        {
            InitializeComponent();
        }

        private void Slider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            ProgressBar.Progress = e.NewValue;
        }
    }
}
