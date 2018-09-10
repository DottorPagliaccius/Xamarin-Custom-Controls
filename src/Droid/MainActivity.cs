using Android.App;
using Android.Content.PM;
using Android.OS;
using Refractored.XamForms.PullToRefresh.Droid;

namespace CustomControlsSamples.Droid
{
    [Activity(Label = "CustomControlsSamples.Droid", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Rg.Plugins.Popup.Popup.Init(this, savedInstanceState);

            Xamarin.Forms.Forms.Init(this, savedInstanceState);

            PullToRefreshLayoutRenderer.Init();

            LoadApplication(new App());
        }
    }
}
