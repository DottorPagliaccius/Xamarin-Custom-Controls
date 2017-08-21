using System;
using Xamarin.Forms;

namespace Xamarin.CustomControls
{
    public class AccordionItemClickEventArgs
    {
        public AccordionItemClickEventArgs(AccordionItemView item)
        {
            Item = item;
        }

        public AccordionItemView Item { get; private set; }
    }

    public delegate void ClickEventHandler(object sender, AccordionItemClickEventArgs e);

    public partial class AccordionItemView : ContentView
    {
        public class InvalidViewException : Exception
        {
            public InvalidViewException()
            {
            }

            public InvalidViewException(string message) : base(message)
            {
            }

            public InvalidViewException(string message, Exception innerException) : base(message, innerException)
            {
            }
        }

        public event ClickEventHandler OnClick;

        public static readonly BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(AccordionItemView), Color.Black);
        public static readonly BindableProperty ButtonBackgroundColorProperty = BindableProperty.Create(nameof(ButtonBackgroundColor), typeof(Color), typeof(AccordionItemView), Color.White);
        public static readonly BindableProperty ButtonActiveBackgroundColorProperty = BindableProperty.Create(nameof(ButtonActiveBackgroundColor), typeof(Color), typeof(AccordionItemView), Color.White);
        public static readonly BindableProperty ActiveTextColorProperty = BindableProperty.Create(nameof(ActiveTextColor), typeof(Color), typeof(AccordionItemView), Color.Black);

        public static readonly BindableProperty BorderColorProperty = BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(AccordionItemView), Color.Black);
        public static readonly BindableProperty ActiveBorderColorProperty = BindableProperty.Create(nameof(ActiveBorderColor), typeof(Color), typeof(AccordionItemView), Color.Black);

        public static readonly BindableProperty BorderProperty = BindableProperty.Create(nameof(Border), typeof(Thickness), typeof(AccordionItemView), new Thickness(1));
        public static readonly BindableProperty InnerPaddingProperty = BindableProperty.Create(nameof(InnerPadding), typeof(Thickness), typeof(StateButton), new Thickness(0));
        public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(AccordionItemView), string.Empty);
        public static readonly BindableProperty TextPositionProperty = BindableProperty.Create(nameof(TextPosition), typeof(TextPosition), typeof(AccordionItemView), TextPosition.Center);
        public static readonly BindableProperty FontAttributesProperty = BindableProperty.Create(nameof(FontAttributes), typeof(FontAttributes), typeof(AccordionItemView), FontAttributes.None);
        public static readonly BindableProperty FontSizeProperty = BindableProperty.Create(nameof(FontSize), typeof(double), typeof(AccordionItemView), Device.GetNamedSize(NamedSize.Default, typeof(AccordionItemView)));
        public static readonly BindableProperty FontFamilyProperty = BindableProperty.Create(nameof(FontFamily), typeof(string), typeof(AccordionItemView), null);

        public static readonly BindableProperty LeftImageProperty = BindableProperty.Create(nameof(LeftImage), typeof(FileImageSource), typeof(AccordionItemView), default(FileImageSource));
        public static readonly BindableProperty RightImageProperty = BindableProperty.Create(nameof(RightImage), typeof(FileImageSource), typeof(AccordionItemView), default(FileImageSource));
        public static readonly BindableProperty ActiveLeftImageProperty = BindableProperty.Create(nameof(ActiveLeftImage), typeof(FileImageSource), typeof(AccordionItemView), default(FileImageSource));
        public static readonly BindableProperty ActiveRightImageProperty = BindableProperty.Create(nameof(ActiveRightImage), typeof(FileImageSource), typeof(AccordionItemView), default(FileImageSource));

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

        public Color ButtonBackgroundColor
        {
            get { return (Color)GetValue(ButtonBackgroundColorProperty); }
            set { SetValue(ButtonBackgroundColorProperty, value); }
        }

        public Color ButtonActiveBackgroundColor
        {
            get { return (Color)GetValue(ButtonActiveBackgroundColorProperty); }
            set { SetValue(ButtonActiveBackgroundColorProperty, value); }
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

        public Thickness InnerPadding
        {
            get { return (Thickness)GetValue(InnerPaddingProperty); }
            set { SetValue(InnerPaddingProperty, value); }
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

        public bool IsOpen
        {
            get { return AccordionItemButton.IsPressed; }
            set { AccordionItemButton.IsPressed = value; if (value) OpenPanel(); else ClosePanel(); }
        }

        public View ItemContent
        {
            get
            {
                return ContentPanel.Content;
            }
            set
            {
                ContentPanel.Content = value;
            }
        }

        public bool RotateImages
        {
            get
            {
                return AccordionItemButton.RotateImages;
            }
            set
            {
                AccordionItemButton.RotateImages = value;
            }
        }

        public AccordionItemView()
        {
            InitializeComponent();

            Padding = 0;

            AccordionItemButton.Command = new Command(() =>
            {
                if (AccordionItemButton.IsPressed)
                    OpenPanel();
                else
                    ClosePanel();

                OnClick?.Invoke(this, new AccordionItemClickEventArgs(this));
            });

            AccordionItemButton.BorderColor = BorderColor;
            AccordionItemButton.TextColor = TextColor;
            AccordionItemButton.BackgroundColor = ButtonBackgroundColor;
        }

        public void OpenPanel()
        {
            AccordionItemButton.IsPressed = true;

            ContentPanel.IsVisible = true;
            ContentPanel.Animate("ClosePanel", o => ContentPanel.Opacity = o, 0, 1, length: 250, easing: Easing.CubicIn);
        }

        public void ClosePanel()
        {
            AccordionItemButton.IsPressed = false;

            ContentPanel.IsVisible = false;
            ContentPanel.Opacity = 0;

        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == ButtonBackgroundColorProperty.PropertyName)
            {
                AccordionItemButton.BackgroundColor = ButtonBackgroundColor;
            }

            if (propertyName == ButtonActiveBackgroundColorProperty.PropertyName)
            {
                AccordionItemButton.ActiveBackgroundColor = ButtonActiveBackgroundColor;
            }

            if (propertyName == TextColorProperty.PropertyName)
            {
                AccordionItemButton.TextColor = TextColor;
            }

            if (propertyName == ActiveTextColorProperty.PropertyName)
            {
                AccordionItemButton.ActiveTextColor = ActiveTextColor;
            }

            if (propertyName == TextProperty.PropertyName)
            {
                AccordionItemButton.Text = Text;
            }

            if (propertyName == TextPositionProperty.PropertyName)
            {
                AccordionItemButton.TextPosition = TextPosition;
            }

            if (propertyName == FontSizeProperty.PropertyName)
            {
                AccordionItemButton.FontSize = FontSize;
            }

            if (propertyName == FontAttributesProperty.PropertyName)
            {
                AccordionItemButton.FontAttributes = FontAttributes;
            }

            if (propertyName == FontFamilyProperty.PropertyName)
            {
                AccordionItemButton.FontFamily = FontFamily;
            }

            if (propertyName == BorderColorProperty.PropertyName)
            {
                AccordionItemButton.BorderColor = BorderColor;
            }

            if (propertyName == BorderProperty.PropertyName)
            {
                AccordionItemButton.Border = Border;
            }

            if (propertyName == ActiveBorderColorProperty.PropertyName)
            {
                AccordionItemButton.ActiveBorderColor = ActiveBorderColor;
            }

            if (propertyName == LeftImageProperty.PropertyName)
            {
                AccordionItemButton.LeftImage = LeftImage;
            }

            if (propertyName == RightImageProperty.PropertyName)
            {
                AccordionItemButton.RightImage = RightImage;
            }

            if (propertyName == ActiveLeftImageProperty.PropertyName)
            {
                AccordionItemButton.ActiveLeftImage = ActiveLeftImage;
            }

            if (propertyName == ActiveRightImageProperty.PropertyName)
            {
                AccordionItemButton.ActiveRightImage = ActiveRightImage;
            }

            if (propertyName == InnerPaddingProperty.PropertyName)
            {
                AccordionItemButton.InnerPadding = InnerPadding;
            }
        }
    }
}
