using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace Xamarin.CustomControls
{
    public delegate void DataUpdateEventHandler(object sender, EventArgs e);

    public partial class RepeaterView : StackLayout
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

        public event DataUpdateEventHandler OnDataUpdate;

        public static readonly BindableProperty ItemTemplateProperty = BindableProperty.Create(nameof(ItemTemplate), typeof(DataTemplate), typeof(RepeaterView), default(DataTemplate));
        public static readonly BindableProperty PanLeftItemTemplateProperty = BindableProperty.Create(nameof(PanLeftItemTemplate), typeof(DataTemplate), typeof(RepeaterView), default(DataTemplate));
        public static readonly BindableProperty SeparatorTemplateProperty = BindableProperty.Create(nameof(SeparatorTemplate), typeof(DataTemplate), typeof(RepeaterView), default(DataTemplate));
        public static readonly BindableProperty EmptyTextTemplateProperty = BindableProperty.Create(nameof(EmptyTextTemplate), typeof(DataTemplate), typeof(RepeaterView), default(DataTemplate));
        public static readonly BindableProperty EmptyTextProperty = BindableProperty.Create(nameof(EmptyText), typeof(string), typeof(RepeaterView), string.Empty);

        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(ICollection), typeof(RepeaterView), new List<object>(), BindingMode.TwoWay, null, propertyChanged: (bindable, oldValue, newValue) => { ItemsChanged(bindable, (ICollection)oldValue, (ICollection)newValue); });

        public static readonly BindableProperty SelectedItemCommandProperty = BindableProperty.Create(nameof(SelectedItemCommand), typeof(ICommand), typeof(RepeaterView), default(ICommand));

        public static readonly BindableProperty SeparatorColorProperty = BindableProperty.Create(nameof(SeparatorColor), typeof(Color), typeof(RepeaterView), Color.Default);
        public static readonly BindableProperty SeparatorHeightProperty = BindableProperty.Create(nameof(SeparatorHeight), typeof(double), typeof(RepeaterView), 1.5d);

        public ICollection ItemsSource
        {
            get => (ICollection)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public DataTemplate ItemTemplate
        {
            get => (DataTemplate)GetValue(ItemTemplateProperty);
            set => SetValue(ItemTemplateProperty, value);
        }

        public DataTemplate PanLeftItemTemplate
        {
            get => (DataTemplate)GetValue(PanLeftItemTemplateProperty);
            set => SetValue(PanLeftItemTemplateProperty, value);
        }

        public DataTemplate SeparatorTemplate
        {
            get => (DataTemplate)GetValue(SeparatorTemplateProperty);
            set => SetValue(SeparatorTemplateProperty, value);
        }

        public DataTemplate EmptyTextTemplate
        {
            get => (DataTemplate)GetValue(EmptyTextTemplateProperty);
            set => SetValue(EmptyTextTemplateProperty, value);
        }

        public string EmptyText
        {
            get => (string)GetValue(EmptyTextProperty);
            set => SetValue(EmptyTextProperty, value);
        }

        public ICommand SelectedItemCommand
        {
            get => (ICommand)GetValue(SelectedItemCommandProperty);
            set => SetValue(SelectedItemCommandProperty, value);
        }

        public Color SeparatorColor
        {
            get => (Color)GetValue(SeparatorColorProperty);
            set => SetValue(SeparatorColorProperty, value);
        }

        public double SeparatorHeight
        {
            get => (double)GetValue(SeparatorHeightProperty);
            set => SetValue(SeparatorHeightProperty, value);
        }

        public bool ShowSeparator { get; set; }

        public bool IsEmpty => ItemsSource.Count == 0 && (EmptyTextTemplate != null || !string.IsNullOrEmpty(EmptyText));

        public RepeaterView()
        {
            Spacing = 0;
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == SelectedItemCommandProperty.PropertyName)
            {
                if (SelectedItemCommand == null)
                    return;

                foreach (var view in Children)
                {
                    if (view.GestureRecognizers.Any())
                        view.GestureRecognizers.Clear();

                    BindSelectedItemCommand(view);
                    BindLeftPanel(view);
                }
            }

            if (propertyName == SeparatorTemplateProperty.PropertyName)
            {
                UpdateItems();
            }
        }

        private static void ItemsChanged(BindableObject bindable, ICollection oldValue, ICollection newValue)
        {
            var repeater = (RepeaterView)bindable;

            if (oldValue != null)
            {
                if (oldValue is INotifyCollectionChanged observable)
                {
                    observable.CollectionChanged -= repeater.CollectionChanged;
                }
            }

            if (newValue != null)
            {
                repeater.UpdateItems();

                if (repeater.ItemsSource is INotifyCollectionChanged observable)
                {
                    observable.CollectionChanged += repeater.CollectionChanged;
                }
            }
        }

        private void UpdateItems()
        {
            if (IsEmpty)
            {
                BuildEmptyText();
            }
            else
                BuildItems();

            OnDataUpdate?.Invoke(this, new EventArgs());
        }

        private View BuildSeparator()
        {
            if (SeparatorTemplate != null)
            {
                var content = SeparatorTemplate.CreateContent();
                if (!(content is View) && !(content is ViewCell))
                {
                    throw new InvalidViewException($"Templated control must be a View or a ViewCell ({nameof(SeparatorTemplate)})");
                }

                return content as View ?? ((ViewCell)content).View;
            }
            else
            {
                return new BoxView { HorizontalOptions = new LayoutOptions(LayoutAlignment.Fill, true), BackgroundColor = SeparatorColor, HeightRequest = SeparatorHeight };
            }
        }

        private void BuildEmptyText()
        {
            Children.Clear();

            if (EmptyTextTemplate == null)
                Children.Add(new Label { Text = EmptyText, Margin = new Thickness(4) });
            else
            {
                var content = EmptyTextTemplate.CreateContent();
                if (!(content is View) && !(content is ViewCell))
                {
                    throw new InvalidViewException($"Templated control must be a View or a ViewCell ({nameof(EmptyTextTemplate)})");
                }

                var view = content as View ?? ((ViewCell)content).View;

                Children.Add(view);
            }
        }

        public void BuildItems()
        {
            Children.Clear();

            foreach (object item in ItemsSource)
            {
                Children.Add(GetItemView(item));
            }
        }

        private View GetItemView(object item)
        {
            var content = CreateItemContent(ItemTemplate, item);
            if (!(content is View) && !(content is ViewCell))
            {
                throw new InvalidViewException($"Templated control must be a View or a ViewCell ({nameof(ItemTemplate)})");
            }

            var view = content as View ?? ((ViewCell)content).View;

            view.BindingContext = item;

            if (view.GestureRecognizers.Any())
                view.GestureRecognizers.Clear();

            if (SelectedItemCommand != null && SelectedItemCommand.CanExecute(item))
            {
                BindSelectedItemCommand(view);
            }

            View finalView;
            if (PanLeftItemTemplate != null)
            {
                var leftView = BuildLeftPanelItems();
                leftView.WidthRequest = 0;
                leftView.IsVisible = false;
                leftView.BindingContext = item;

                finalView = new StackLayout { Spacing = 0, Orientation = StackOrientation.Horizontal };

                ((StackLayout)finalView).Children.Add(leftView);
                ((StackLayout)finalView).Children.Add(view);

                BindLeftPanel(finalView);
            }
            else
                finalView = view;

            if (ShowSeparator && ItemsSource.Cast<object>().Last() != item)
            {
                var container = new StackLayout { Spacing = 0 };

                container.Children.Add(finalView);
                container.Children.Add(BuildSeparator());

                return container;
            }

            return finalView;
        }

        private void BindSelectedItemCommand(View view)
        {
            var tapGestureRecognizer = new TapGestureRecognizer { Command = SelectedItemCommand, CommandParameter = view.BindingContext };

            view.GestureRecognizers.Add(tapGestureRecognizer);
        }

        private double _startingWidth = 0;

        private void BindLeftPanel(View view)
        {
            var panGestureRecognizer = new PanGestureRecognizer { };

            panGestureRecognizer.PanUpdated += (object sender, PanUpdatedEventArgs e) =>
            {
                var container = (StackLayout)sender;
                var currentView = container.Children[0];

                switch (e.StatusType)
                {
                    case GestureStatus.Started:

                        currentView.IsVisible = true;

                        _startingWidth = currentView.WidthRequest;
                        break;

                    case GestureStatus.Running:

                        currentView.WidthRequest = _startingWidth + e.TotalX;

                        System.Diagnostics.Debug.WriteLine($"TotalX: {e.TotalX} - LeftPanelWidth: {currentView.Width}");
                        break;

                    case GestureStatus.Completed:

                        if (currentView.WidthRequest > 0)
                        {
                            currentView.Animate("open", (arg) => currentView.WidthRequest = arg, currentView.WidthRequest, container.Width / 4, length: 750, easing: Easing.CubicInOut);
                        }
                        else
                        {
                            currentView.Animate("open", (arg) => currentView.WidthRequest = arg, currentView.WidthRequest, 0, length: 750, easing: Easing.CubicInOut);
                        }

                        _startingWidth = 0;

                        System.Diagnostics.Debug.WriteLine(string.Empty);
                        System.Diagnostics.Debug.WriteLine($"COMPLETED TotalX: {e.TotalX} - LeftPanelWidth: {currentView.Width} - panelWidth: {container.Children[1].Width}");
                        System.Diagnostics.Debug.WriteLine(string.Empty);
                        break;

                    case GestureStatus.Canceled:

                        currentView.WidthRequest = 0;
                        currentView.IsVisible = false;
                        break;
                }
            };

            view.GestureRecognizers.Add(panGestureRecognizer);
        }

        private View BuildLeftPanelItems()
        {
            var content = PanLeftItemTemplate.CreateContent();
            if (!(content is View) && !(content is ViewCell))
            {
                throw new InvalidViewException($"Templated control must be a View or a ViewCell ({nameof(PanLeftItemTemplate)})");
            }

            return content as View ?? ((ViewCell)content).View;
        }

        private void CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var items = ItemsSource.Cast<object>().ToList();

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:

                    var index = e.NewStartingIndex;

                    foreach (var newItem in e.NewItems)
                    {
                        Children.Insert(index++, GetItemView(newItem));
                    }
                    break;

                case NotifyCollectionChangedAction.Move:

                    var moveItem = items[e.OldStartingIndex];

                    Children.RemoveAt(e.OldStartingIndex);
                    Children.Insert(e.NewStartingIndex, GetItemView(moveItem));
                    break;

                case NotifyCollectionChangedAction.Remove:

                    Children.RemoveAt(e.OldStartingIndex);
                    break;

                case NotifyCollectionChangedAction.Replace:

                    Children.RemoveAt(e.OldStartingIndex);
                    Children.Insert(e.NewStartingIndex, GetItemView(items[e.NewStartingIndex]));
                    break;

                case NotifyCollectionChangedAction.Reset:

                    UpdateItems();
                    break;
            }

            OnDataUpdate?.Invoke(this, new EventArgs());
        }

        private static object CreateItemContent(DataTemplate dataTemplate, object item)
        {
            if (dataTemplate is DataTemplateSelector dts)
            {
                var template = dts.SelectTemplate(item, null);
                template.SetValue(BindingContextProperty, item);
                return template.CreateContent();
            }
            
            dataTemplate.SetValue(BindingContextProperty, item);
            return dataTemplate.CreateContent();
        }
    }
}
