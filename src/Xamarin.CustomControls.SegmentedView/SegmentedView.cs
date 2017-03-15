//using System.Collections.Generic;
//using System.Linq;
//using System.Windows.Input;
//using Xamarin.Forms;

//namespace Xamarin.CustomControls
//{
//    public class SegmentedViewItem : View
//    {
//        public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(SegmentedViewItem), string.Empty);

//        public string Text
//        {
//            get { return (string)GetValue(TextProperty); }
//            set { SetValue(TextProperty, value); }
//        }
//    }

//    public partial class SegmentedView : StackLayout, IViewContainer<SegmentedViewItem>
//    {
//        IList<SegmentedViewItem> IViewContainer<SegmentedViewItem>.Children { get; } = new List<SegmentedViewItem>();

//        public static readonly BindableProperty SelectedCommandProperty = BindableProperty.Create(nameof(SelectedCommand), typeof(ICommand), typeof(SegmentedView));
//        public static readonly BindableProperty ActiveBackgroundColorProperty = BindableProperty.Create(nameof(ActiveBackgroundColor), typeof(Color), typeof(SegmentedView), Color.White);
//        public static readonly BindableProperty ActiveTextColorProperty = BindableProperty.Create(nameof(ActiveTextColor), typeof(Color), typeof(SegmentedView), Color.White);
//        public static readonly BindableProperty InactiveBackgroundColorProperty = BindableProperty.Create(nameof(InactiveBackgroundColor), typeof(Color), typeof(SegmentedView), Color.Gray);
//        public static readonly BindableProperty InactiveTextColorProperty = BindableProperty.Create(nameof(InactiveTextColor), typeof(Color), typeof(SegmentedView), Color.Gray);
//        public static readonly BindableProperty HeaderHeightProperty = BindableProperty.Create(nameof(HeaderHeight), typeof(int), typeof(SegmentedView), 50);

//        public ICommand SelectedCommand
//        {
//            get { return (ICommand)GetValue(SelectedCommandProperty); }
//            set { SetValue(SelectedCommandProperty, value); }
//        }

//        public Color ActiveBackgroundColor
//        {
//            get { return (Color)GetValue(ActiveBackgroundColorProperty); }
//            set { SetValue(ActiveBackgroundColorProperty, value); }
//        }

//        public Color InactiveBackgroundColor
//        {
//            get { return (Color)GetValue(InactiveBackgroundColorProperty); }
//            set { SetValue(InactiveBackgroundColorProperty, value); }
//        }

//        public Color ActiveTextColor
//        {
//            get { return (Color)GetValue(ActiveTextColorProperty); }
//            set { SetValue(ActiveTextColorProperty, value); }
//        }

//        public Color InactiveTextColor
//        {
//            get { return (Color)GetValue(InactiveTextColorProperty); }
//            set { SetValue(InactiveTextColorProperty, value); }
//        }

//        public int HeaderHeight
//        {
//            get { return (int)GetValue(HeaderHeightProperty); }
//            set { SetValue(HeaderHeightProperty, value); }
//        }

//        public SegmentedView()
//        {
//            Spacing = 0;
//        }

//        protected override void OnPropertyChanged(string propertyName = null)
//        {
//            base.OnPropertyChanged(propertyName);

//            if (propertyName == ItemsSourceProperty.PropertyName)
//            {
//                Children.Clear();

//                var items = ItemsSource;
//                var buttons = new List<StateButton>(items.Count());

//                var thereIsOnlyOneButton = items.Count() == 1;

//                foreach (var item in items)
//                {
//                    var stateButton = new StateButton
//                    {
//                        PressedTextColor = ActiveTextColor,
//                        PressedBackgroundColor = ActiveBackgroundColor,
//                        BackgroundColor = InactiveBackgroundColor,
//                        TextColor = InactiveTextColor,
//                        Command = SelectedCommand,
//                        CommandParameter = item.Key,
//                        Text = item.Text,
//                        HorizontalOptions = new LayoutOptions(LayoutAlignment.Fill, true),
//                        HeightRequest = HeaderHeight,
//                        BorderRadius = 0,
//                        FontSize = 16,
//                        Margin = 0
//                    };

//                    if (!thereIsOnlyOneButton)
//                    {
//                        stateButton.Clicked += (s, e) =>
//                         {
//                             foreach (var button in buttons)
//                             {
//                                 button.IsPressed = false;
//                             }

//                             var clickedButton = (StateButton)s;

//                             clickedButton.IsPressed = true;

//                             clickedButton.Command?.Execute(clickedButton.CommandParameter);
//                         };
//                    }

//                    buttons.Add(stateButton);

//                    Children.Add(stateButton);
//                }

//                if (!thereIsOnlyOneButton)
//                {
//                    foreach (var button in buttons)
//                    {
//                        button.IsPressed = false;
//                    }
//                }

//                if (buttons.Any())
//                {
//                    var firstButton = buttons.First();

//                    firstButton.IsPressed = true;

//                    if (thereIsOnlyOneButton)
//                    {
//                        firstButton.StateChangeEnabled = false;

//                        SelectedCommand.Execute(firstButton.CommandParameter);
//                    }
//                }
//            }

//            if (propertyName == SelectedCommandProperty.PropertyName)
//            {
//                foreach (var child in Children)
//                {
//                    var stateButton = (StateButton)child;

//                    stateButton.Command = SelectedCommand;
//                }
//            }
//        }
//    }
//}