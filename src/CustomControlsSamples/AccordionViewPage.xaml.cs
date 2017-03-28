using System;
using Xamarin.Forms;

namespace CustomControlsSamples
{
    public partial class AccordionViewPage : ContentPage
    {
        public AccordionViewPage()
        {
            InitializeComponent();
        }

        private void Handle_Clicked(object sender, EventArgs e)
        {
            Device.OpenUri(new Uri("https://www.nuget.org/packages/Xamarin.CustomControls.AccordionView"));
        }
    }
}
