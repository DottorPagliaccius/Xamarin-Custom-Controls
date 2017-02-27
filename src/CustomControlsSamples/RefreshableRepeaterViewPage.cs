using Xamarin.Forms;

namespace CustomControlsSamples
{
    public partial class RefreshableRepeaterViewPage : ContentPage
    {
        private readonly CustomSampleViewModel _viewModel;

        public RefreshableRepeaterViewPage()
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
