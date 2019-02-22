using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.CustomControls;

namespace CustomControlsSamples
{
    public partial class GifLoaderPage : ContentPage
    {
        public GifLoaderPage()
        {
            InitializeComponent();
        }

        private async void Handle_Clicked(object sender, EventArgs e)
        {
            using (var loader = new GifLoader(Navigation, "loading.gif") { BackgroundColor = Color.Transparent })
            {
                await Task.Delay(7000);
            }
        }
    }
}
