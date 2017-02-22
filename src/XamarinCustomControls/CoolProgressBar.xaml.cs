using Xamarin.Forms;

namespace Xamarin.CustomControls
{
    public partial class CoolProgressBar : ContentView
    {
        BoxView _coloredBoxView;

        public CoolProgressBar()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty ColorProperty = BindableProperty.Create(nameof(Color), typeof(Color), typeof(CoolProgressBar), Color.Blue);
        public static readonly BindableProperty ProgressProperty = BindableProperty.Create(nameof(Progress), typeof(double), typeof(CoolProgressBar), default(double));

        public Color Color
        {
            get { return (Color)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        public double Progress
        {
            get { return (double)GetValue(ProgressProperty); }
            set
            {
                var progress = value;

                if (value < 0)
                    progress = 0;

                if (value > 1)
                    progress = 1;

                SetValue(ProgressProperty, progress);
            }
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == ColorProperty.PropertyName && _coloredBoxView != null)
            {
                _coloredBoxView.Color = Color;
            }

            if (propertyName == ProgressProperty.PropertyName)
            {
                var percent = Progress * 100;

                Container.ColumnDefinitions = new ColumnDefinitionCollection();

                Container.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(percent, GridUnitType.Star) });

                if (percent != 100)
                    Container.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(100 - percent, GridUnitType.Star) });

                _coloredBoxView = new BoxView
                {
                    Color = Color,
                    HorizontalOptions = new LayoutOptions(LayoutAlignment.Fill, false),
                    VerticalOptions = new LayoutOptions(LayoutAlignment.Fill, false)
                };

                Container.Children.Add(_coloredBoxView, 0, 0);

                if (percent != 100)
                {
                    var otherBox = new BoxView
                    {
                        Color = Color.Transparent,
                        HorizontalOptions = new LayoutOptions(LayoutAlignment.Fill, false),
                        VerticalOptions = new LayoutOptions(LayoutAlignment.Fill, false)
                    };

                    Container.Children.Add(otherBox, 1, 0);
                }
            }
        }
    }
}