using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Xamarin.CustomControls
{
    public enum LabelPosition
    {
        Start,
        BeforeBar,
        Center,
        AfterBar,
        End
    }

    public partial class ProgressBarView : ContentView
    {
        private string _labelStringFormat = "{0} %";
        private LabelPosition _labelPosition = LabelPosition.Start;
        private bool _labelShowIntValuesOnly = true;

        private BoxView _coloredBoxView;
        private BoxView _otherBox;
        private Label _label;

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
            set { SetValue(ProgressProperty, value); }
        }
        public bool ShowLabel
        {
            get
            {
                return _label.IsVisible;
            }
            set
            {
                _label.IsVisible = value;

                if (value)
                    SetupLabel();
            }
        }

        public double LabelFontSize
        {
            get
            {
                return _label.FontSize;
            }
            set
            {
                _label.FontSize = value;
            }
        }

        public Color LabelTextColor
        {
            get
            {
                return _label.TextColor;
            }
            set
            {
                _label.TextColor = value;
            }
        }

        public string LabelStringFormat
        {
            get { return _labelStringFormat; }
            set
            {
                _labelStringFormat = value;

                if (ShowLabel)
                    SetupLabel();
            }
        }

        public bool LabelShowIntValuesOnly
        {
            get { return _labelShowIntValuesOnly; }
            set
            {
                _labelShowIntValuesOnly = value;

                if (ShowLabel)
                    SetupLabel();
            }
        }

        public LabelPosition LabelPosition
        {
            get { return _labelPosition; }
            set
            {
                _labelPosition = value;

                if (ShowLabel)
                    SetupLabel();
            }
        }

        public ProgressBarView()
        {
            InitializeComponent();

            _label = new Label { Margin = new Thickness(5, 1), VerticalOptions = LayoutOptions.Center, VerticalTextAlignment = TextAlignment.Center };

            LabelFontSize = 16;
            LabelTextColor = Color.Black;

            SetupBar();

            if (ShowLabel)
                SetupLabel();
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
                SetupBar();

                if (ShowLabel)
                    SetupLabel();
            }
        }

        private void SetupBar()
        {
            Container.ColumnDefinitions.Clear();

            var progress = NormalizeValue(Progress);

            Container.ColumnDefinitions = new ColumnDefinitionCollection();

            Container.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(progress, GridUnitType.Star) });

            if (_coloredBoxView == null)
                _coloredBoxView = new BoxView
                {
                    Color = Color,
                    HorizontalOptions = new LayoutOptions(LayoutAlignment.Fill, false),
                    VerticalOptions = new LayoutOptions(LayoutAlignment.Fill, false)
                };

            if (progress != 100)
            {
                Container.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(100 - progress, GridUnitType.Star) });

                if (_otherBox == null)
                    _otherBox = new BoxView
                    {
                        Color = Color.Transparent,
                        HorizontalOptions = new LayoutOptions(LayoutAlignment.Fill, false),
                        VerticalOptions = new LayoutOptions(LayoutAlignment.Fill, false)
                    };

                Container.Children.Add(_coloredBoxView, 0, 0);
                Container.Children.Add(_otherBox, 1, 0);
            }
            else
                Container.Children.Add(_coloredBoxView, 0, 0);
        }

        private void SetupLabel()
        {
            Container.Children.Remove(_label);

            var progress = NormalizeValue(Progress);

            _label.Text = string.Format(LabelStringFormat, LabelShowIntValuesOnly ? Convert.ToUInt32(progress) : progress);
            _label.TextColor = LabelTextColor;
            _label.FontSize = LabelFontSize;

            var labelPosition = LabelPosition;

            if (progress <= 1 && (labelPosition == LabelPosition.Start || labelPosition == LabelPosition.BeforeBar))
                labelPosition = LabelPosition.AfterBar;

            if (progress >= 99 && (labelPosition == LabelPosition.End || labelPosition == LabelPosition.AfterBar))
                labelPosition = LabelPosition.BeforeBar;

            switch (labelPosition)
            {
                case LabelPosition.Start:

                    Container.Children.Add(_label, 0, 0);

                    _label.HorizontalOptions = LayoutOptions.Start;
                    break;

                case LabelPosition.BeforeBar:

                    Container.Children.Add(_label, 0, 0);

                    _label.HorizontalOptions = LayoutOptions.End;
                    break;

                case LabelPosition.Center:

                    Container.Children.Add(_label, 0, 0);

                    if (progress != 100)
                        Grid.SetColumnSpan(_label, 2);

                    _label.HorizontalOptions = LayoutOptions.Center;
                    break;

                case LabelPosition.AfterBar:

                    if (progress != 100)
                        Container.Children.Add(_label, 1, 0);
                    else
                        Container.Children.Add(_label, 0, 0);

                    _label.HorizontalOptions = LayoutOptions.Start;
                    break;

                case LabelPosition.End:

                    if (progress != 100)
                        Container.Children.Add(_label, 1, 0);
                    else
                        Container.Children.Add(_label, 0, 0);

                    _label.HorizontalOptions = LayoutOptions.End;
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public Task<bool> ProgressTo(double value, uint length, Easing easing)
        {
            value = NormalizeValue(value);

            var tcs = new TaskCompletionSource<bool>();

            this.Animate(nameof(Progress), d => Progress = d, Progress, value, length: length, easing: easing, finished: (d, finished) => tcs.SetResult(finished));

            return tcs.Task;
        }

        public double NormalizeValue(double value)
        {
            if (value <= 0)
                value = 0.0000000001; //this to avoid "NaN is not a Number" Exception on GridLength = 0* 

            if (value > 100)
                value = 100;

            return value;
        }
    }
}