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

        private async void Entry_TextChanged(object sender, TextChangedEventArgs e)
        {
            double number;

            if (double.TryParse(e.NewTextValue, out number))
                await ProgressBar2.ProgressTo(number, 500, Easing.Linear);
        }
    }
}
