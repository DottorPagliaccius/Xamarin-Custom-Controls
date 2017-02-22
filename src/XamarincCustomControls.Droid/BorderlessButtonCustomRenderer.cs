using Xamarin.CustomControls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(StateButton), typeof(ButtonRenderer))]
namespace XamarinCustomControls.Droid
{
    public class StateButtonCustomRenderer : ButtonRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.StateListAnimator = null;
            }
        }
    }
}

