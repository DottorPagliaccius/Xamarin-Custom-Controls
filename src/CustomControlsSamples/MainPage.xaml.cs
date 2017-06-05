using System;

using Xamarin.Forms;

namespace CustomControlsSamples
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void RepeaterViewClick(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RepeaterViewPage());
        }

        private async void RefreshableRepeaterViewClick(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RefreshableRepeaterViewPage());
        }

        private async void AutoCompleteViewClick(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AutoCompleteViewPage());
        }

        private async void ProgressBarViewClick(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ProgressBarViewPage());
        }

        private async void WrapRepeaterViewClick(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new WrapRepeaterViewPage());
        }

        private async void StateButtonClick(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new StateButtonPage());
        }

        private async void SegmentedViewClick(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SegmentedViewPage());
        }

        private async void AccordionViewClick(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AccordionViewPage());
        }
    }
}
