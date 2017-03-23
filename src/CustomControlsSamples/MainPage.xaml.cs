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
            await Navigation.PushAsync(new NavigationPage(new RepeaterViewPage()));
        }

        private async void RefreshableRepeaterViewClick(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NavigationPage(new RefreshableRepeaterViewPage()));
        }

        private async void AutoCompleteViewClick(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NavigationPage(new AutoCompleteViewPage()));
        }

        private async void ProgressBarViewClick(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NavigationPage(new ProgressBarViewPage()));
        }

        private async void WrapRepeaterViewClick(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NavigationPage(new WrapRepeaterViewPage()));
        }

        private async void StateButtonClick(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NavigationPage(new StateButtonPage()));
        }

        private async void SegmentedViewClick(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NavigationPage(new SegmentedViewPage()));
        }

        private async void AccordionViewClick(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NavigationPage(new AccordionViewPage()));
        }
    }
}
