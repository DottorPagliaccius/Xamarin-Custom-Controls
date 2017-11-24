using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace Xamarin.CustomControls
{
    public partial class BadgeButton : ContentView
    {
        private bool _hideBadgeIfTextIsZero = true;

        public event EventHandler Clicked;

        public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(BadgeButton));
        public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(BadgeButton));

        public static readonly BindableProperty ImageSourceProperty = BindableProperty.Create(nameof(ImageSource), typeof(ImageSource), typeof(BadgeButton), default(FileImageSource));
        public static readonly BindableProperty BadgeTextColorProperty = BindableProperty.Create(nameof(BadgeTextColor), typeof(Color), typeof(BadgeButton), Color.White);
        public static readonly BindableProperty BadgeBackgroundColorProperty = BindableProperty.Create(nameof(BadgeBackgroundColor), typeof(Color), typeof(BadgeButton), Color.Red);

        public static readonly BindableProperty BadgeTextProperty = BindableProperty.Create(nameof(BadgeText), typeof(string), typeof(BadgeButton), string.Empty);
        public static readonly BindableProperty BadgeFontSizeProperty = BindableProperty.Create(nameof(BadgeFontSize), typeof(double), typeof(BadgeButton), Device.GetNamedSize(NamedSize.Default, typeof(BadgeButton)));

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public object CommandParameter
        {
            get { return GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        public Color BadgeTextColor
        {
            get { return (Color)GetValue(BadgeTextColorProperty); }
            set { SetValue(BadgeTextColorProperty, value); }
        }

        public Color BadgeBackgroundColor
        {
            get { return (Color)GetValue(BadgeBackgroundColorProperty); }
            set { SetValue(BadgeBackgroundColorProperty, value); }
        }

        public ImageSource ImageSource
        {
            get { return (ImageSource)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

        public string BadgeText
        {
            get { return (string)GetValue(BadgeTextProperty); }
            set { SetValue(BadgeTextProperty, value); }
        }

        [TypeConverter(typeof(FontSizeConverter))]
        public double BadgeFontSize
        {
            get { return (double)GetValue(BadgeFontSizeProperty); }
            set { SetValue(BadgeFontSizeProperty, value); }
        }

        public bool HideBadgeIfTextIsZero
        {
            get
            {
                return _hideBadgeIfTextIsZero;
            }
            set
            {
                _hideBadgeIfTextIsZero = value;

                if (int.TryParse(BadgeText, out int parsedValue))
                {
                    BadgeFrame.IsVisible = parsedValue != 0;
                }
                else
                    BadgeFrame.IsVisible = true;
            }
        }

        public BadgeButton()
        {
            InitializeComponent();

            GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() =>
                {
                    Command?.Execute(CommandParameter);
                    Clicked?.Invoke(this, new EventArgs());
                })
            });

            BadgeFrame.BackgroundColor = BadgeBackgroundColor;
            BadgeTextLabel.Text = BadgeText;
            BadgeTextLabel.TextColor = BadgeTextColor;
            BadgeTextLabel.FontSize = BadgeFontSize;
            BadgeIconImage.Source = ImageSource;
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (BadgeFrame != null && propertyName == BadgeBackgroundColorProperty.PropertyName)
            {
                BadgeFrame.BackgroundColor = BadgeBackgroundColor;
            }

            if (BadgeTextLabel != null && propertyName == BadgeTextProperty.PropertyName)
            {
                BadgeTextLabel.Text = BadgeText;

                if (HideBadgeIfTextIsZero && int.TryParse(BadgeText, out int parsedValue))
                {
                    BadgeFrame.IsVisible = parsedValue != 0;
                }
                else
                    BadgeFrame.IsVisible = !string.IsNullOrEmpty(BadgeText);
            }

            if (BadgeTextLabel != null && propertyName == BadgeTextColorProperty.PropertyName)
            {
                BadgeTextLabel.TextColor = BadgeTextColor;
            }

            if (BadgeTextLabel != null && propertyName == BadgeFontSizeProperty.PropertyName)
            {
                BadgeTextLabel.FontSize = BadgeFontSize;
            }

            if (BadgeIconImage != null && propertyName == ImageSourceProperty.PropertyName)
            {
                BadgeIconImage.Source = ImageSource;
            }
        }
    }
}
