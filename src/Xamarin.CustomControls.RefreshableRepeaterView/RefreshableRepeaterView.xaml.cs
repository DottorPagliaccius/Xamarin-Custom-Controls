using System.Collections;
using System.Collections.Specialized;
using System.Windows.Input;
using Xamarin.Forms;

namespace Xamarin.CustomControls
{
    public partial class RefreshableRepeaterView : RepeaterView
    {
        public static readonly BindableProperty IsRefreshingProperty = BindableProperty.Create(nameof(IsRefreshing), typeof(bool), typeof(RefreshableRepeaterView), false);
        public static readonly BindableProperty RefreshBackgroundColorProperty = BindableProperty.Create(nameof(RefreshBackgroundColor), typeof(Color), typeof(RefreshableRepeaterView), Color.Default);
        public static readonly BindableProperty IsPullToRefreshEnabledProperty = BindableProperty.Create(nameof(IsPullToRefreshEnabled), typeof(bool), typeof(RefreshableRepeaterView), false);
        public static readonly BindableProperty RefreshCommandProperty = BindableProperty.Create(nameof(RefreshCommand), typeof(ICommand), typeof(RefreshableRepeaterView));
        public static readonly BindableProperty RefreshColorProperty = BindableProperty.Create(nameof(RefreshColor), typeof(Color), typeof(RefreshableRepeaterView), Color.Default);

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

        public RefreshableRepeaterView()
        {
            InitializeComponent();
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

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

        private static void SeparatorTemplateChanged(BindableObject bindable)
        {
            var repeater = (RefreshableRepeaterView)bindable;

            repeater.UpdateItems();
        }

        private static void ItemsChanged(BindableObject bindable, ICollection newValue)
        {
            if (newValue == null)
                return;

            var repeater = (RefreshableRepeaterView)bindable;

            repeater.UpdateItems();

            var observable = repeater.ItemsSource as INotifyCollectionChanged;

            if (observable != null)
            {
                observable.CollectionChanged += (sender, e) =>
                {
                    repeater.UpdateItems();
                };
            }
        }

        protected override void BuildItems(ICollection sourceItems)
        {
            ItemsPanel.Children.Clear();

            foreach (object item in sourceItems)
            {
                var content = ItemTemplate.CreateContent();
                if (!(content is View) && !(content is ViewCell))
                {
                    throw new InvalidViewException("Templated control must be a View or a ViewCell");
                }

                var view = (content is View) ? content as View : ((ViewCell)content).View;

                view.BindingContext = item;

                if (SelectedItemCommand != null && SelectedItemCommand.CanExecute(item))
                    BindSelectedItemCommand(view);

                ItemsPanel.Children.Add(view);

                if (ShowSeparator)
                    ItemsPanel.Children.Add(BuildSeparator());
            }

            //this because IsRefreshing = false doesn't hide the spinner if set right after the execution of RefreshCommand
            //https://github.com/jamesmontemagno/Xamarin.Forms-PullToRefreshLayout/issues/3
            PullToRefreshPanel.IsRefreshing = false;
        }
    }
}
