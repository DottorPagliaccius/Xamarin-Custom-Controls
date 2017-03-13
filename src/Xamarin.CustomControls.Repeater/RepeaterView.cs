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
        public static readonly BindableProperty SeparatorTemplateProperty = BindableProperty.Create(nameof(SeparatorTemplate), typeof(DataTemplate), typeof(RepeaterView), default(DataTemplate));
        public static readonly BindableProperty EmptyTextTemplateProperty = BindableProperty.Create(nameof(EmptyTextTemplate), typeof(DataTemplate), typeof(RepeaterView), default(DataTemplate));
        public static readonly BindableProperty EmptyTextProperty = BindableProperty.Create(nameof(EmptyText), typeof(string), typeof(RepeaterView), string.Empty);

        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(ICollection), typeof(RepeaterView), new List<object>(), BindingMode.TwoWay, null, propertyChanged: (bindable, oldValue, newValue) => { ItemsChanged(bindable, (ICollection)oldValue, (ICollection)newValue); });

        public static readonly BindableProperty SelectedItemCommandProperty = BindableProperty.Create(nameof(SelectedItemCommand), typeof(ICommand), typeof(RepeaterView), default(ICommand));
        public static readonly BindableProperty SeparatorColorProperty = BindableProperty.Create(nameof(SeparatorColor), typeof(Color), typeof(RepeaterView), Color.Default);
        public static readonly BindableProperty SeparatorHeightProperty = BindableProperty.Create(nameof(SeparatorHeight), typeof(double), typeof(RepeaterView), 1.5d);

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

        public bool ShowSeparator { get; set; }

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
                    BindSelectedItemCommand(view);
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
                var observable = oldValue as INotifyCollectionChanged;

                if (observable != null)
                {
                    observable.CollectionChanged -= repeater.CollectionChanged;
                }
            }

            if (newValue != null)
            {
                repeater.UpdateItems();

                var observable = repeater.ItemsSource as INotifyCollectionChanged;

                if (observable != null)
                {
                    observable.CollectionChanged += repeater.CollectionChanged;
                }
            }
        }

        private void UpdateItems()
        {
            if (ItemsSource.Count == 0 && (EmptyTextTemplate != null || !string.IsNullOrEmpty(EmptyText)))
            {
                BuildEmptyText();
            }
            else
                BuildItems(ItemsSource);

            OnDataUpdate?.Invoke(this, new EventArgs());
        }

        private View BuildSeparator()
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

        private void BuildEmptyText()
        {
            Children.Clear();

            if (EmptyTextTemplate == null)
                Children.Add(new Label { Text = EmptyText });
            else
            {
                var content = EmptyTextTemplate.CreateContent();
                if (!(content is View) && !(content is ViewCell))
                {
                    throw new InvalidViewException("Templated control must be a View or a ViewCell");
                }

                var view = (content is View) ? content as View : ((ViewCell)content).View;

                Children.Add(view);
            }
        }

        public void BuildItems(ICollection sourceItems)
        {
            Children.Clear();

            foreach (object item in sourceItems)
            {
                Children.Add(GetItemView(item));
            }
        }

        private View GetItemView(object item)
        {
            var content = ItemTemplate.CreateContent();
            if (!(content is View) && !(content is ViewCell))
            {
                throw new InvalidViewException("Templated control must be a View or a ViewCell");
            }

            var view = content is View ? content as View : ((ViewCell)content).View;

            view.BindingContext = item;

            if (SelectedItemCommand != null && SelectedItemCommand.CanExecute(item))
                BindSelectedItemCommand(view);

            if (ShowSeparator && ItemsSource.Cast<object>().Last() != item)
            {
                var container = new StackLayout { Spacing = 0 };

                container.Children.Add(view);
                container.Children.Add(BuildSeparator());

                return container;
            }

            return view;
        }

        private void BindSelectedItemCommand(View view)
        {
            if (!SelectedItemCommand.CanExecute(view.BindingContext))
                return;

            var tapGestureRecognizer = new TapGestureRecognizer { Command = SelectedItemCommand, CommandParameter = view.BindingContext };

            if (view.GestureRecognizers.Any())
                view.GestureRecognizers.Clear();

            view.GestureRecognizers.Add(tapGestureRecognizer);
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

                    BuildItems(ItemsSource);
                    break;
            }

            OnDataUpdate?.Invoke(this, new EventArgs());
        }
    }
}
