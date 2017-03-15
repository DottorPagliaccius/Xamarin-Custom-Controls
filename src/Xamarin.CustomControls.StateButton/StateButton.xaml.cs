using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace Xamarin.CustomControls
{
    public enum TextPosition
    {
        Center,
        Left,
        Right
    }

    public partial class StateButton : ContentView
    {
        public event EventHandler Clicked;

        private bool _disablePropertyChangedEvent;

        private Color _inactiveBackgroundColor;

        public static readonly BindableProperty IsPressedProperty = BindableProperty.Create(nameof(IsPressed), typeof(bool), typeof(StateButton), false, propertyChanged: (bindable, oldValue, newValue) => IsPressedChanged(bindable, newValue));
        public static readonly BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(StateButton), Color.White);
        public static readonly BindableProperty ActiveBackgroundColorProperty = BindableProperty.Create(nameof(ActiveBackgroundColor), typeof(Color), typeof(StateButton), Color.White);
        public static readonly BindableProperty ActiveTextColorProperty = BindableProperty.Create(nameof(ActiveTextColor), typeof(Color), typeof(StateButton), Color.Black);
        public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(StateButton));
        public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(StateButton));

        public static readonly BindableProperty BorderColorProperty = BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(StateButton), Color.Black);
        public static readonly BindableProperty ActiveBorderColorProperty = BindableProperty.Create(nameof(ActiveBorderColor), typeof(Color), typeof(StateButton), Color.Black);

        public static readonly BindableProperty BorderProperty = BindableProperty.Create(nameof(Border), typeof(Thickness), typeof(StateButton), new Thickness(1));
        public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(StateButton), string.Empty);
        public static readonly BindableProperty TextPositionProperty = BindableProperty.Create(nameof(TextPosition), typeof(TextPosition), typeof(StateButton), TextPosition.Center);
        public static readonly BindableProperty FontAttributesProperty = BindableProperty.Create(nameof(FontAttributes), typeof(FontAttributes), typeof(StateButton), FontAttributes.None);
        public static readonly BindableProperty FontSizeProperty = BindableProperty.Create(nameof(FontSize), typeof(double), typeof(StateButton), Device.GetNamedSize(NamedSize.Default, typeof(StateButton)));
        public static readonly BindableProperty FontFamilyProperty = BindableProperty.Create(nameof(FontFamily), typeof(string), typeof(StateButton), null);

        public static readonly BindableProperty LeftImageProperty = BindableProperty.Create(nameof(LeftImage), typeof(FileImageSource), typeof(StateButton), default(FileImageSource));
        public static readonly BindableProperty RightImageProperty = BindableProperty.Create(nameof(RightImage), typeof(FileImageSource), typeof(StateButton), default(FileImageSource));
        public static readonly BindableProperty ActiveLeftImageProperty = BindableProperty.Create(nameof(ActiveLeftImage), typeof(FileImageSource), typeof(StateButton), default(FileImageSource));
        public static readonly BindableProperty ActiveRightImageProperty = BindableProperty.Create(nameof(ActiveRightImage), typeof(FileImageSource), typeof(StateButton), default(FileImageSource));

        public bool StateChangeEnabled { get; set; } = true;

        public bool IsPressed
        {
            get { return (bool)GetValue(IsPressedProperty); }
            set { SetValue(IsPressedProperty, value); }
        }

        public Color TextColor
        {
            get { return (Color)GetValue(TextColorProperty); }
            set { SetValue(TextColorProperty, value); }
        }

        public TextPosition TextPosition
        {
            get { return (TextPosition)GetValue(TextPositionProperty); }
            set { SetValue(TextPositionProperty, value); }
        }

        public Color ActiveBackgroundColor
        {
            get { return (Color)GetValue(ActiveBackgroundColorProperty); }
            set { SetValue(ActiveBackgroundColorProperty, value); }
        }

        public Color ActiveTextColor
        {
            get { return (Color)GetValue(ActiveTextColorProperty); }
            set { SetValue(ActiveTextColorProperty, value); }
        }

        public Color BorderColor
        {
            get { return (Color)GetValue(BorderColorProperty); }
            set { SetValue(BorderColorProperty, value); }
        }

        public Color ActiveBorderColor
        {
            get { return (Color)GetValue(ActiveBorderColorProperty); }
            set { SetValue(ActiveBorderColorProperty, value); }
        }

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

        public Thickness Border
        {
            get { return (Thickness)GetValue(BorderProperty); }
            set { SetValue(BorderProperty, value); }
        }

        public FontAttributes FontAttributes
        {
            get { return (FontAttributes)GetValue(FontAttributesProperty); }
            set { SetValue(FontAttributesProperty, value); }
        }

        public string FontFamily
        {
            get { return (string)GetValue(FontFamilyProperty); }
            set { SetValue(FontFamilyProperty, value); }
        }

        [TypeConverter(typeof(FontSizeConverter))]
        public double FontSize
        {
            get { return (double)GetValue(FontSizeProperty); }
            set { SetValue(FontSizeProperty, value); }
        }

        public FileImageSource LeftImage
        {
            get { return (FileImageSource)GetValue(LeftImageProperty); }
            set { SetValue(LeftImageProperty, value); }
        }

        public FileImageSource ActiveLeftImage
        {
            get { return (FileImageSource)GetValue(ActiveLeftImageProperty); }
            set { SetValue(ActiveLeftImageProperty, value); }
        }

        public FileImageSource RightImage
        {
            get { return (FileImageSource)GetValue(RightImageProperty); }
            set { SetValue(RightImageProperty, value); }
        }

        public FileImageSource ActiveRightImage
        {
            get { return (FileImageSource)GetValue(ActiveRightImageProperty); }
            set { SetValue(ActiveRightImageProperty, value); }
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public StateButton()
        {
            InitializeComponent();

            GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() =>
                {
                    IsPressed = !IsPressed;
                    Command?.Execute(CommandParameter);
                    Clicked?.Invoke(this, new EventArgs());
                })
            });

            MainGrid.BackgroundColor = _inactiveBackgroundColor = BackgroundColor;
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (!_disablePropertyChangedEvent)
            {
                if (propertyName == BackgroundColorProperty.PropertyName)
                {
                    MainGrid.BackgroundColor = _inactiveBackgroundColor = BackgroundColor;
                }

                if (propertyName == TextColorProperty.PropertyName)
                {
                    if (!IsPressed)
                        TextLabel.TextColor = TextColor;
                }

                if (propertyName == ActiveTextColorProperty.PropertyName)
                {
                    if (IsPressed)
                        TextLabel.TextColor = ActiveTextColor;
                }

                if (propertyName == TextProperty.PropertyName)
                {
                    TextLabel.Text = Text;
                }

                if (propertyName == TextPositionProperty.PropertyName)
                {
                    switch (TextPosition)
                    {
                        default:

                            TextLabel.HorizontalOptions = LayoutOptions.Center;
                            break;

                        case TextPosition.Left:

                            TextLabel.HorizontalOptions = LayoutOptions.Start;
                            break;

                        case TextPosition.Right:

                            TextLabel.HorizontalOptions = LayoutOptions.End;
                            break;
                    }
                }

                if (propertyName == FontSizeProperty.PropertyName)
                {
                    TextLabel.FontSize = FontSize;
                }

                if (propertyName == FontAttributesProperty.PropertyName)
                {
                    TextLabel.FontAttributes = FontAttributes;
                }

                if (propertyName == FontFamilyProperty.PropertyName)
                {
                    TextLabel.FontFamily = FontFamily;
                }

                if (propertyName == BorderColorProperty.PropertyName)
                {
                    if (!IsPressed)
                        MainPanel.BackgroundColor = BorderColor;
                }

                if (propertyName == BorderColorProperty.PropertyName)
                {
                    if (!IsPressed)
                        MainPanel.BackgroundColor = BorderColor;
                }

                if (propertyName == BorderProperty.PropertyName)
                {
                    MainPanel.Padding = Border;
                }

                if (propertyName == ActiveBorderColorProperty.PropertyName)
                {
                    if (IsPressed)
                        MainPanel.BackgroundColor = ActiveBorderColor;
                }

                if (propertyName == ActiveBorderColorProperty.PropertyName)
                {
                    if (IsPressed)
                        MainPanel.BackgroundColor = ActiveBorderColor;
                }

                if (propertyName == LeftImageProperty.PropertyName)
                {
                    if (!IsPressed)
                        LeftImageControl.Source = LeftImage;

                    LeftImageControlContainer.IsVisible = true;
                }

                if (propertyName == RightImageProperty.PropertyName)
                {
                    if (!IsPressed)
                        RightImageControl.Source = RightImage;

                    RightImageControlContainer.IsVisible = true;
                }

                if (propertyName == ActiveLeftImageProperty.PropertyName)
                {
                    if (IsPressed)
                        LeftImageControl.Source = ActiveLeftImage;

                    LeftImageControlContainer.IsVisible = true;
                }

                if (propertyName == ActiveRightImageProperty.PropertyName)
                {
                    if (IsPressed)
                        RightImageControl.Source = ActiveRightImage;

                    RightImageControlContainer.IsVisible = true;
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
                    button.TextLabel.TextColor = button.ActiveTextColor;
                    button.MainGrid.BackgroundColor = button.ActiveBackgroundColor;

                    if (button.ActiveLeftImage != null)
                    {
                        button.LeftImageControl.Source = button.ActiveLeftImage;
                    }

                    if (button.ActiveRightImage != null)
                        button.RightImageControl.Source = button.ActiveRightImage;

                    button.LeftImageControlContainer.IsVisible = button.ActiveLeftImage != null;
                    button.RightImageControlContainer.IsVisible = button.ActiveRightImage != null;

                    button.MainPanel.BackgroundColor = button.ActiveBorderColor;
                }
                else
                {
                    button.TextLabel.TextColor = button.TextColor;
                    button.MainGrid.BackgroundColor = button._inactiveBackgroundColor;

                    if (button.LeftImage != null)
                        button.LeftImageControl.Source = button.LeftImage;

                    if (button.RightImage != null)
                        button.RightImageControl.Source = button.RightImage;

                    button.LeftImageControlContainer.IsVisible = button.LeftImage != null;
                    button.RightImageControlContainer.IsVisible = button.RightImage != null;

                    button.MainPanel.BackgroundColor = button.BorderColor;
                }
            }
            finally
            {
                button._disablePropertyChangedEvent = false;
            }
        }
    }
}
