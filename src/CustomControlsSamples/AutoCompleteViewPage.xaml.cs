using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CustomControlsSamples
{
    public partial class AutoCompleteViewPage : ContentPage
    {
        private readonly CustomSampleViewModel _viewModel;

        public AutoCompleteViewPage()
        {
            InitializeComponent();

            _viewModel = new CustomSampleViewModel();

            BindingContext = _viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await _viewModel.LoadData();
        }

        private async void Handle_OnSuggestionOpen(object sender, EventArgs e)
        {
            await Task.Delay(1000);

            await MainScroll.ScrollToAsync((Element)sender, ScrollToPosition.Start, true);
        }
    }
}
