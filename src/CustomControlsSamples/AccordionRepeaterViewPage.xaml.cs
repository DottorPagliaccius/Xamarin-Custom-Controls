using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace CustomControlsSamples
{
    public partial class AccordionRepeaterViewPage : ContentPage
    {
        private readonly CustomSampleViewModel _viewModel;

        public AccordionRepeaterViewPage()
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
    }
}
