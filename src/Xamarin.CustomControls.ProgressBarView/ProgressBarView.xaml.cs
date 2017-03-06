using Xamarin.Forms;

namespace Xamarin.CustomControls
{
    public partial class ProgressBarView : ContentView
    {
        private BoxView _coloredBoxView;

        public ProgressBarView()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty ColorProperty = BindableProperty.Create(nameof(Color), typeof(Color), typeof(ProgressBarView), Color.Blue);
        public static readonly BindableProperty ProgressProperty = BindableProperty.Create(nameof(Progress), typeof(double), typeof(ProgressBarView), default(double));

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
                SetValue(ProgressProperty, value);
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
                var progress = Progress;

                if (progress < 0)
                    progress = 0;

                if (progress > 100)
                    progress = 100;

                Container.ColumnDefinitions = new ColumnDefinitionCollection();

                Container.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(progress, GridUnitType.Star) });

                if (progress != 100)
                    Container.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(100 - progress, GridUnitType.Star) });

                _coloredBoxView = new BoxView
                {
                    Color = Color,
                    HorizontalOptions = new LayoutOptions(LayoutAlignment.Fill, false),
                    VerticalOptions = new LayoutOptions(LayoutAlignment.Fill, false)
                };

                Container.Children.Add(_coloredBoxView, 0, 0);

                if (progress != 100)
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