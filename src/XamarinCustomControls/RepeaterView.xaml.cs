using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows.Input;
using Xamarin.Forms;

namespace Xamarin.CustomControls
{
    public partial class RepeaterView : ContentView
    {
        public static readonly BindableProperty ItemTemplateProperty = BindableProperty.Create(nameof(ItemTemplate), typeof(DataTemplate), typeof(RepeaterView), default(DataTemplate));
        public static readonly BindableProperty HeaderTemplateProperty = BindableProperty.Create(nameof(HeaderTemplate), typeof(DataTemplate), typeof(RepeaterView), default(DataTemplate), propertyChanged: (bindable, oldValue, newValue) => { HeaderTemplateChanged(bindable, (DataTemplate)oldValue, (DataTemplate)newValue); });
        public static readonly BindableProperty FooterTemplateProperty = BindableProperty.Create(nameof(FooterTemplate), typeof(DataTemplate), typeof(RepeaterView), default(DataTemplate), propertyChanged: (bindable, oldValue, newValue) => { FooterTemplateChanged(bindable, (DataTemplate)oldValue, (DataTemplate)newValue); });
        public static readonly BindableProperty EmptyTextTemplateProperty = BindableProperty.Create(nameof(EmptyTextTemplate), typeof(DataTemplate), typeof(RepeaterView), default(DataTemplate));

        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(ICollection), typeof(RepeaterView), new List<object>(), BindingMode.TwoWay, null, propertyChanged: (bindable, oldValue, newValue) => { ItemsChanged(bindable, (ICollection)oldValue, (ICollection)newValue); });

        public static readonly BindableProperty EmptyTextProperty = BindableProperty.Create(nameof(EmptyText), typeof(string), typeof(RepeaterView), string.Empty);

        public static readonly BindableProperty SelectedItemCommandProperty = BindableProperty.Create(nameof(SelectedItemCommand), typeof(ICommand), typeof(RepeaterView), default(ICommand));
        public static readonly BindableProperty SeparatorColorProperty = BindableProperty.Create(nameof(SeparatorColor), typeof(Color), typeof(RepeaterView), Color.Default);

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

        public DataTemplate HeaderTemplate
        {
            get { return (DataTemplate)GetValue(HeaderTemplateProperty); }
            set { SetValue(HeaderTemplateProperty, value); }
        }
        public DataTemplate FooterTemplate
        {
            get { return (DataTemplate)GetValue(FooterTemplateProperty); }
            set { SetValue(FooterTemplateProperty, value); }
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

        public bool ShowSeparator { get; set; } = true;

        public RepeaterView()
        {
            InitializeComponent();
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == SelectedItemCommandProperty.PropertyName)
            {
                foreach (var view in ItemsPanel.Children)
                {
                    BindSelectedItemCommand(view);
                }
            }
        }

        private static void HeaderTemplateChanged(BindableObject bindable, DataTemplate oldValue, DataTemplate newValue)
        {
            var repeater = (RepeaterView)bindable;

            repeater.BuildHeader();
        }

        private static void FooterTemplateChanged(BindableObject bindable, DataTemplate oldValue, DataTemplate newValue)
        {
            var repeater = (RepeaterView)bindable;

            repeater.BuildFooter();
        }

        private static void ItemsChanged(BindableObject bindable, ICollection oldValue, ICollection newValue)
        {
            var repeater = (RepeaterView)bindable;

            UpdateItems(newValue, repeater);

            var observable = repeater.ItemsSource as INotifyCollectionChanged;

            if (observable != null)
            {
                observable.CollectionChanged += (sender, e) =>
                {
                    UpdateItems(repeater.ItemsSource, repeater);
                };
            }
        }

        private static void UpdateItems(ICollection sourceItems, RepeaterView repeater)
        {
            if (sourceItems.Count == 0 && (repeater.EmptyTextTemplate != null || !string.IsNullOrEmpty(repeater.EmptyText)))
            {
                repeater.BuildEmptyText();
            }
            else
                repeater.BuildItems(sourceItems);
        }

        private void BuildHeader()
        {
            if (HeaderTemplate == null)
                return;

            var content = HeaderTemplate.CreateContent();
            if (!(content is View) && !(content is ViewCell))
            {
                throw new InvalidViewException("Templated control must be a View or a ViewCell");
            }

            var view = (content is View) ? content as View : ((ViewCell)content).View;

            HeaderPanel.Children.Clear();
            HeaderPanel.Children.Add(view);
        }

        private void BuildFooter()
        {
            if (FooterTemplate == null)
                return;

            var content = FooterTemplate.CreateContent();
            if (!(content is View) && !(content is ViewCell))
            {
                throw new InvalidViewException("Templated control must be a View or a ViewCell");
            }

            var view = (content is View) ? content as View : ((ViewCell)content).View;

            FooterPanel.Children.Clear();
            FooterPanel.Children.Add(view);
        }

        private void BuildEmptyText()
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

        private void BuildItems(ICollection sourceItems)
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

                if (SelectedItemCommand != null)
                    BindSelectedItemCommand(view);

                ItemsPanel.Children.Add(view);

                if (ShowSeparator)
                    ItemsPanel.Children.Add(new BoxView { HorizontalOptions = new LayoutOptions(LayoutAlignment.Fill, true), BackgroundColor = SeparatorColor, HeightRequest = 1.5 });
            }
        }

        private void BindSelectedItemCommand(View view)
        {
            var tapGestureRecognizer = new TapGestureRecognizer();

            tapGestureRecognizer.Tapped += (sender, e) =>
            {
                SelectedItemCommand.Execute(view.BindingContext);
            };

            view.GestureRecognizers.Clear();
            view.GestureRecognizers.Add(tapGestureRecognizer);
        }
    }
}
