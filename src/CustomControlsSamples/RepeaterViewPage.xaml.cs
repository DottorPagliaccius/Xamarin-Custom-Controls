using System;
using System.Linq;
using Xamarin.Forms;

namespace CustomControlsSamples
{
    public partial class RepeaterViewPage : ContentPage
    {
        private readonly CustomSampleViewModel _viewModel;

        public RepeaterViewPage()
        {
            InitializeComponent();

            _viewModel = new CustomSampleViewModel();

            BindingContext = _viewModel;

            MainRepeater.OnDataUpdate += (sender, e) =>
            {

            };
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await _viewModel.LoadData();
        }

        private void Handle_Tapped(object sender, EventArgs e)
        {
            var control = (Image)sender;

            _viewModel.LeftPanelSelectCommand.Execute(((TapGestureRecognizer)control.GestureRecognizers.Single()).CommandParameter);
        }
    }
}
