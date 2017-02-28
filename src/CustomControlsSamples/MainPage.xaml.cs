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
    }
}
