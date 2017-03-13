using System.Collections;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;

namespace Xamarin.CustomControls
{
    public partial class RefreshableRepeaterView : ContentView
    {
        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(ICollection), typeof(RepeaterView), new List<object>(), BindingMode.TwoWay, null);
        public static readonly BindableProperty ItemTemplateProperty = BindableProperty.Create(nameof(ItemTemplate), typeof(DataTemplate), typeof(RepeaterView), default(DataTemplate));
        public static readonly BindableProperty SeparatorTemplateProperty = BindableProperty.Create(nameof(SeparatorTemplate), typeof(DataTemplate), typeof(RepeaterView), default(DataTemplate));
        public static readonly BindableProperty EmptyTextTemplateProperty = BindableProperty.Create(nameof(EmptyTextTemplate), typeof(DataTemplate), typeof(RepeaterView), default(DataTemplate));
        public static readonly BindableProperty EmptyTextProperty = BindableProperty.Create(nameof(EmptyText), typeof(string), typeof(RepeaterView), string.Empty);
        public static readonly BindableProperty SelectedItemCommandProperty = BindableProperty.Create(nameof(SelectedItemCommand), typeof(ICommand), typeof(RepeaterView), default(ICommand));
        public static readonly BindableProperty SeparatorColorProperty = BindableProperty.Create(nameof(SeparatorColor), typeof(Color), typeof(RepeaterView), Color.Default);
        public static readonly BindableProperty SeparatorHeightProperty = BindableProperty.Create(nameof(SeparatorHeight), typeof(double), typeof(RepeaterView), 1.5d);

        public static readonly BindableProperty IsRefreshingProperty = BindableProperty.Create(nameof(IsRefreshing), typeof(bool), typeof(RefreshableRepeaterView), false);
        public static readonly BindableProperty RefreshBackgroundColorProperty = BindableProperty.Create(nameof(RefreshBackgroundColor), typeof(Color), typeof(RefreshableRepeaterView), Color.Default);
        public static readonly BindableProperty IsPullToRefreshEnabledProperty = BindableProperty.Create(nameof(IsPullToRefreshEnabled), typeof(bool), typeof(RefreshableRepeaterView), false);
        public static readonly BindableProperty RefreshCommandProperty = BindableProperty.Create(nameof(RefreshCommand), typeof(ICommand), typeof(RefreshableRepeaterView));
        public static readonly BindableProperty RefreshColorProperty = BindableProperty.Create(nameof(RefreshColor), typeof(Color), typeof(RefreshableRepeaterView), Color.Default);

        public ICollection ItemsSource
        {
            get { return (ICollection)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        public DataTemplate SeparatorTemplate
        {
            get { return (DataTemplate)GetValue(SeparatorTemplateProperty); }
            set { SetValue(SeparatorTemplateProperty, value); }
        }

        public DataTemplate EmptyTextTemplate
        {
            get { return (DataTemplate)GetValue(EmptyTextTemplateProperty); }
            set { SetValue(EmptyTextTemplateProperty, value); }
        }

        public string EmptyText
        {
            get { return (string)GetValue(EmptyTextProperty); }
            set { SetValue(EmptyTextProperty, value); }
        }

        public ICommand SelectedItemCommand
        {
            get { return (ICommand)GetValue(SelectedItemCommandProperty); }
            set { SetValue(SelectedItemCommandProperty, value); }
        }

        public Color SeparatorColor
        {
            get { return (Color)GetValue(SeparatorColorProperty); }
            set { SetValue(SeparatorColorProperty, value); }
        }

        public double SeparatorHeight
        {
            get { return (double)GetValue(SeparatorHeightProperty); }
            set { SetValue(SeparatorHeightProperty, value); }
        }

        public bool IsRefreshing
        {
            get { return PullToRefreshPanel.IsRefreshing; }
            set
            {
                if (PullToRefreshPanel.IsRefreshing != value)
                    OnPropertyChanged(nameof(IsRefreshing));

                PullToRefreshPanel.IsRefreshing = value;
            }
        }

        public bool IsPullToRefreshEnabled
        {
            get { return (bool)GetValue(IsPullToRefreshEnabledProperty); }
            set { SetValue(IsPullToRefreshEnabledProperty, value); }
        }

        public ICommand RefreshCommand
        {
            get { return (ICommand)GetValue(RefreshCommandProperty); }
            set { SetValue(RefreshCommandProperty, value); }
        }

        public Color RefreshColor
        {
            get { return (Color)GetValue(RefreshColorProperty); }
            set { SetValue(RefreshColorProperty, value); }
        }

        public Color RefreshBackgroundColor
        {
            get { return (Color)GetValue(RefreshBackgroundColorProperty); }
            set { SetValue(RefreshBackgroundColorProperty, value); }
        }

        public bool ShowSeparator
        {
            get { return MainRepeater.ShowSeparator; }
            set { MainRepeater.ShowSeparator = value; }
        }

        public RefreshableRepeaterView()
        {
            InitializeComponent();

            MainRepeater.OnDataUpdate += (sender, e) =>
            {
                //this because IsRefreshing = false doesn't hide the spinner if set right after the execution of RefreshCommand
                //https://github.com/jamesmontemagno/Xamarin.Forms-PullToRefreshLayout/issues/3
                PullToRefreshPanel.IsRefreshing = false;
            };
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == ItemsSourceProperty.PropertyName)
            {
                MainRepeater.ItemsSource = ItemsSource;
            }

            if (propertyName == ItemTemplateProperty.PropertyName)
            {
                MainRepeater.ItemTemplate = ItemTemplate;
            }

            if (propertyName == SeparatorTemplateProperty.PropertyName)
            {
                MainRepeater.SeparatorTemplate = SeparatorTemplate;
            }

            if (propertyName == EmptyTextTemplateProperty.PropertyName)
            {
                MainRepeater.EmptyTextTemplate = EmptyTextTemplate;
            }

            if (propertyName == EmptyTextProperty.PropertyName)
            {
                MainRepeater.EmptyText = EmptyText;
            }

            if (propertyName == SelectedItemCommandProperty.PropertyName)
            {
                MainRepeater.SelectedItemCommand = SelectedItemCommand;
            }

            if (propertyName == SeparatorColorProperty.PropertyName)
            {
                MainRepeater.SeparatorColor = SeparatorColor;
            }

            if (propertyName == SeparatorHeightProperty.PropertyName)
            {
                MainRepeater.SeparatorHeight = SeparatorHeight;
            }

            if (propertyName == IsPullToRefreshEnabledProperty.PropertyName)
            {
                PullToRefreshPanel.IsPullToRefreshEnabled = IsPullToRefreshEnabled;
            }

            if (propertyName == RefreshCommandProperty.PropertyName)
            {
                PullToRefreshPanel.RefreshCommand = RefreshCommand;
            }

            if (propertyName == RefreshColorProperty.PropertyName)
            {
                PullToRefreshPanel.RefreshColor = RefreshColor;
            }

            if (propertyName == RefreshBackgroundColorProperty.PropertyName)
            {
                PullToRefreshPanel.RefreshBackgroundColor = RefreshBackgroundColor;
            }
        }
    }
}
