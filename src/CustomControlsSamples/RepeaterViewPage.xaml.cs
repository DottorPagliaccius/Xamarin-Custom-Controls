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

            await _viewModel.LoadNullData();
        }
    }
}
