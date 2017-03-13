using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace Xamarin.CustomControls
{
    public interface ISegmentedViewItem
    {
        object Key { get; set; }
        string Text { get; set; }
    }

    public enum DisplayMode
    {
        Tab,
        Accordion
    }

    public partial class SegmentedView : ContentView
    {
        private DisplayMode _displayMode = DisplayMode.Tab;

        public SegmentedView()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable<ISegmentedViewItem>), typeof(SegmentedView), Enumerable.Empty<ISegmentedViewItem>(), BindingMode.OneWay, null);

        public static readonly BindableProperty SelectedCommandProperty = BindableProperty.Create(nameof(SelectedCommand), typeof(ICommand), typeof(SegmentedView));

        public static readonly BindableProperty ActiveBackgroundColorProperty = BindableProperty.Create(nameof(ActiveBackgroundColor), typeof(Color), typeof(SegmentedView), Color.White);
        public static readonly BindableProperty ActiveTextColorProperty = BindableProperty.Create(nameof(ActiveTextColor), typeof(Color), typeof(SegmentedView), Color.White);
        public static readonly BindableProperty InactiveBackgroundColorProperty = BindableProperty.Create(nameof(InactiveBackgroundColor), typeof(Color), typeof(SegmentedView), Color.Gray);
        public static readonly BindableProperty InactiveTextColorProperty = BindableProperty.Create(nameof(InactiveTextColor), typeof(Color), typeof(SegmentedView), Color.Gray);

        public static readonly BindableProperty HeaderHeightProperty = BindableProperty.Create(nameof(HeaderHeight), typeof(int), typeof(SegmentedView), 50);

        public DisplayMode DisplayMode
        {
            get { return _displayMode; }
            set
            {
                _displayMode = value;

                MainPanel.Orientation = value == DisplayMode.Accordion ? StackOrientation.Vertical : StackOrientation.Horizontal;
            }
        }

        public IEnumerable<ISegmentedViewItem> ItemsSource
        {
            get { return (IEnumerable<ISegmentedViewItem>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public ICommand SelectedCommand
        {
            get { return (ICommand)GetValue(SelectedCommandProperty); }
            set { SetValue(SelectedCommandProperty, value); }
        }

        public Color ActiveBackgroundColor
        {
            get { return (Color)GetValue(ActiveBackgroundColorProperty); }
            set { SetValue(ActiveBackgroundColorProperty, value); }
        }

        public Color InactiveBackgroundColor
        {
            get { return (Color)GetValue(InactiveBackgroundColorProperty); }
            set { SetValue(InactiveBackgroundColorProperty, value); }
        }

        public Color ActiveTextColor
        {
            get { return (Color)GetValue(ActiveTextColorProperty); }
            set { SetValue(ActiveTextColorProperty, value); }
        }

        public Color InactiveTextColor
        {
            get { return (Color)GetValue(InactiveTextColorProperty); }
            set { SetValue(InactiveTextColorProperty, value); }
        }

        public int HeaderHeight
        {
            get { return (int)GetValue(HeaderHeightProperty); }
            set { SetValue(HeaderHeightProperty, value); }
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == ItemsSourceProperty.PropertyName)
            {
                MainPanel.Children.Clear();

                var items = ItemsSource;
                var buttons = new List<StateButton>(items.Count());

                var thereIsOnlyOneButton = items.Count() == 1;

                foreach (var item in items)
                {
                    var stateButton = new StateButton
                    {
                        PressedTextColor = ActiveTextColor,
                        PressedBackgroundColor = ActiveBackgroundColor,
                        BackgroundColor = InactiveBackgroundColor,
                        TextColor = InactiveTextColor,
                        Command = SelectedCommand,
                        CommandParameter = item.Key,
                        Text = item.Text,
                        HorizontalOptions = new LayoutOptions(LayoutAlignment.Fill, true),
                        HeightRequest = HeaderHeight,
                        BorderRadius = 0,
                        FontSize = 16,
                        Margin = 0
                    };

                    if (!thereIsOnlyOneButton && DisplayMode != DisplayMode.Accordion)
                    {
                        stateButton.Clicked += (s, e) =>
                         {
                             foreach (var button in buttons)
                             {
                                 button.IsPressed = false;
                             }

                             var clickedButton = (StateButton)s;

                             clickedButton.IsPressed = true;

                             clickedButton.Command?.Execute(clickedButton.CommandParameter);
                         };
                    }

                    buttons.Add(stateButton);

                    MainPanel.Children.Add(stateButton);
                }

                if (!thereIsOnlyOneButton)
                {
                    foreach (var button in buttons)
                    {
                        button.IsPressed = false;
                    }
                }

                if (DisplayMode == DisplayMode.Tab && buttons.Any())
                {
                    var firstButton = buttons.First();

                    firstButton.IsPressed = true;

                    if (thereIsOnlyOneButton)
                    {
                        firstButton.StateChangeEnabled = false;

                        SelectedCommand.Execute(firstButton.CommandParameter);
                    }
                }
            }

            if (propertyName == SelectedCommandProperty.PropertyName)
            {
                foreach (var child in MainPanel.Children)
                {
                    var stateButton = (StateButton)child;

                    stateButton.Command = SelectedCommand;
                }
            }
        }
    }
}