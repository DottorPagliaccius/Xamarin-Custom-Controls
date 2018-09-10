using Foundation;
using Refractored.XamForms.PullToRefresh.iOS;
using UIKit;

namespace CustomControlsSamples.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication uiApplication, NSDictionary launchOptions)
        {
            Rg.Plugins.Popup.Popup.Init();

            Xamarin.Forms.Forms.Init();

            LoadApplication(new App());

            PullToRefreshLayoutRenderer.Init();

            return base.FinishedLaunching(uiApplication, launchOptions);
        }
    }
}
