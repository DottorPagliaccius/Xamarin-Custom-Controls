using System;
using System.Threading.Tasks;
using Xamarin.CustomControls.LottieLoader;
using Xamarin.Forms;

namespace CustomControlsSamples
{
    public partial class LottieLoaderPage : ContentPage
    {
        public LottieLoaderPage()
        {
            InitializeComponent();
        }

        private async void Handle_Clicked(object sender, EventArgs e)
        {
            using (var loader = new LottieLoader(Navigation, "party_penguin.json"))
            {
                await Task.Delay(7000);
            }
        }
    }
}
