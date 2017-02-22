using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows.Input;
using Xamarin.Forms;

namespace Xamarin.CustomControls
{
    public partial class RepeaterView : ContentView
    {
        public static readonly BindableProperty ItemTemplateProperty = BindableProperty.Create(nameof(ItemTemplate), typeof(DataTemplate), typeof(RefreshableRepeaterView), default(DataTemplate));
        public static readonly BindableProperty SeparatorTemplateProperty = BindableProperty.Create(nameof(SeparatorTemplate), typeof(DataTemplate), typeof(RefreshableRepeaterView), default(DataTemplate), propertyChanged: (bindable, oldValue, newValue) => { SeparatorTemplateChanged(bindable); });
        public static readonly BindableProperty EmptyTextTemplateProperty = BindableProperty.Create(nameof(EmptyTextTemplate), typeof(DataTemplate), typeof(RefreshableRepeaterView), default(DataTemplate));

        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(ICollection), typeof(RefreshableRepeaterView), new List<object>(), BindingMode.TwoWay, null, propertyChanged: (bindable, oldValue, newValue) => { ItemsChanged(bindable, (ICollection)newValue); });

        public static readonly BindableProperty EmptyTextProperty = BindableProperty.Create(nameof(EmptyText), typeof(string), typeof(RefreshableRepeaterView), string.Empty);

        public static readonly BindableProperty SelectedItemCommandProperty = BindableProperty.Create(nameof(SelectedItemCommand), typeof(ICommand), typeof(RefreshableRepeaterView), default(ICommand));
        public static readonly BindableProperty SeparatorColorProperty = BindableProperty.Create(nameof(SeparatorColor), typeof(Color), typeof(RefreshableRepeaterView), Color.Default);
        public static readonly BindableProperty SeparatorHeightProperty = BindableProperty.Create(nameof(SeparatorHeight), typeof(double), typeof(RefreshableRepeaterView), 1.5d);

        public ICollection ItemsSource
        {
            get { return (ICollection)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); OnPropertyChanged(); }
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

        public bool ShowSeparator { get; set; } = true;


        public RepeaterView()
        {
            InitializeComponent();

            ItemsPanel.Spacing = 0;
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == SelectedItemCommandProperty.PropertyName)
            {
                if (SelectedItemCommand == null)
                    return;

                foreach (var view in ItemsPanel.Children)
                {
                    BindSelectedItemCommand(view);
                }
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

        protected void UpdateItems()
        {
            if (ItemsSource.Count == 0 && (EmptyTextTemplate != null || !string.IsNullOrEmpty(EmptyText)))
            {
                BuildEmptyText();
            }
            else
                BuildItems(ItemsSource);
        }

        protected View BuildSeparator()
        {
            if (SeparatorTemplate != null)
            {
                var content = SeparatorTemplate.CreateContent();
                if (!(content is View) && !(content is ViewCell))
                {
                    throw new InvalidViewException("Templated control must be a View or a ViewCell");
                }

                return (content is View) ? content as View : ((ViewCell)content).View;
            }
            else
            {
                return new BoxView { HorizontalOptions = new LayoutOptions(LayoutAlignment.Fill, true), BackgroundColor = SeparatorColor, HeightRequest = SeparatorHeight };
            }
        }

        protected void BuildEmptyText()
        {
            ItemsPanel.Children.Clear();

            if (EmptyTextTemplate == null)
                ItemsPanel.Children.Add(new Label { Text = EmptyText });
            else
            {
                var content = EmptyTextTemplate.CreateContent();
                if (!(content is View) && !(content is ViewCell))
                {
                    throw new InvalidViewException("Templated control must be a View or a ViewCell");
                }

                var view = (content is View) ? content as View : ((ViewCell)content).View;

                ItemsPanel.Children.Add(view);
            }
        }

        protected virtual void BuildItems(ICollection sourceItems)
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
        }

        protected void BindSelectedItemCommand(View view)
        {
            if (!SelectedItemCommand.CanExecute(view.BindingContext))
                return;

            var tapGestureRecognizer = new TapGestureRecognizer { Command = SelectedItemCommand, CommandParameter = view.BindingContext };

            view.GestureRecognizers.Clear();
            view.GestureRecognizers.Add(tapGestureRecognizer);
        }
    }
}
