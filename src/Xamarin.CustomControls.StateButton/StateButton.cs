using Xamarin.Forms;

namespace Xamarin.CustomControls
{
    public class StateButton : Button
    {
        private bool _disablePropertyChangedEvent;

        private Color _unpressedBackgroundColor;
        private Color _unpressedTextColor;

        public static readonly BindableProperty IsPressedProperty = BindableProperty.Create(nameof(IsPressed), typeof(bool), typeof(StateButton), false, propertyChanged: (bindable, oldValue, newValue) => IsPressedChanged(bindable, newValue));
        public static readonly BindableProperty PressedBackgroundColorProperty = BindableProperty.Create(nameof(PressedBackgroundColor), typeof(Color), typeof(StateButton), Color.White);
        public static readonly BindableProperty PressedTextColorProperty = BindableProperty.Create(nameof(PressedTextColor), typeof(Color), typeof(StateButton), Color.Black);

        public StateButton()
        {
            Clicked += (s, e) => { IsPressed = !IsPressed; };

            _unpressedBackgroundColor = BackgroundColor;
            _unpressedTextColor = TextColor;
        }

        public bool StateChangeEnabled { get; set; } = true;

        public bool IsPressed
        {
            get { return (bool)GetValue(IsPressedProperty); }
            set { SetValue(IsPressedProperty, value); }
        }

        public Color PressedBackgroundColor
        {
            get { return (Color)GetValue(PressedBackgroundColorProperty); }
            set { SetValue(PressedBackgroundColorProperty, value); }
        }

        public Color PressedTextColor
        {
            get { return (Color)GetValue(PressedTextColorProperty); }
            set { SetValue(PressedTextColorProperty, value); }
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (!_disablePropertyChangedEvent)
            {
                if (propertyName == BackgroundColorProperty.PropertyName)
                {
                    _unpressedBackgroundColor = BackgroundColor;
                }

                if (propertyName == TextColorProperty.PropertyName)
                {
                    _unpressedTextColor = TextColor;
                }
            }
        }

        private static void IsPressedChanged(BindableObject bindable, object newValue)
        {
            var button = (StateButton)bindable;
            var value = (bool)newValue;

            if (!button.StateChangeEnabled)
                return;

            try
            {
                button._disablePropertyChangedEvent = true;

                if (value)
                {
                    button.TextColor = button.PressedTextColor;
                    button.BackgroundColor = button.PressedBackgroundColor;
                }
                else
                {
                    button.TextColor = button._unpressedTextColor;
                    button.BackgroundColor = button._unpressedBackgroundColor;
                }
            }
            finally
            {
                button._disablePropertyChangedEvent = false;
            }
        }
    }
}
