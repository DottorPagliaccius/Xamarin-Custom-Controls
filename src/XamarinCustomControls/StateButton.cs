using Xamarin.Forms;

namespace Xamarin.CustomControls
{
    public class StateButton : Button
    {
        public static readonly BindableProperty IsPressedProperty = BindableProperty.Create(nameof(IsPressed), typeof(bool), typeof(StateButton), false);
        public static readonly BindableProperty PressedBackgroundColorProperty = BindableProperty.Create(nameof(PressedBackgroundColor), typeof(Color), typeof(StateButton), Color.White);
        public static readonly BindableProperty UnpressedBackgroundColorProperty = BindableProperty.Create(nameof(UnpressedBackgroundColor), typeof(Color), typeof(StateButton), Color.Gray);
        public static readonly BindableProperty PressedTextColorProperty = BindableProperty.Create(nameof(PressedTextColor), typeof(Color), typeof(StateButton), Color.White);
        public static readonly BindableProperty UnpressedTextColorProperty = BindableProperty.Create(nameof(UnpressedTextColor), typeof(Color), typeof(StateButton), Color.Gray);

        public StateButton()
        {
            Clicked += (s, e) => { IsPressed = !IsPressed; };
        }

        public bool StateChangeEnabled { get; set; } = true;

        public bool IsPressed
        {
            get
            {
                return (bool)GetValue(IsPressedProperty);
            }
            set
            {
                SetValue(IsPressedProperty, value);

                if (!StateChangeEnabled)
                    return;

                if (value)
                {
                    SetValue(TextColorProperty, PressedTextColor);
                    SetValue(BackgroundColorProperty, PressedBackgroundColor);
                }
                else
                {
                    SetValue(TextColorProperty, UnpressedTextColor);
                    SetValue(BackgroundColorProperty, UnpressedBackgroundColor);
                }
            }
        }

        public Color PressedBackgroundColor
        {
            get
            {
                return (Color)GetValue(PressedBackgroundColorProperty);
            }
            set
            {
                SetValue(PressedBackgroundColorProperty, value);
            }
        }

        public Color UnpressedBackgroundColor
        {
            get
            {
                return (Color)GetValue(UnpressedBackgroundColorProperty);
            }
            set
            {
                SetValue(UnpressedBackgroundColorProperty, value);
            }
        }

        public Color PressedTextColor
        {
            get
            {
                return (Color)GetValue(PressedTextColorProperty);
            }
            set
            {
                SetValue(PressedTextColorProperty, value);
            }
        }

        public Color UnpressedTextColor
        {
            get
            {
                return (Color)GetValue(UnpressedTextColorProperty);
            }
            set
            {
                SetValue(UnpressedTextColorProperty, value);
            }
        }
    }
}
