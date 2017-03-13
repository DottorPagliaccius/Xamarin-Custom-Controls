using Xamarin.Forms;

namespace CustomControlsSamples
{
    public partial class SegmentedViewPage : ContentPage
    {
        public SegmentedViewPage()
        {
            InitializeComponent();

            BindingContext = new SegmentedViewModel();
        }
    }
}
