using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace Xamarin.CustomControls
{
    public class WrapRepeaterView : Grid
    {
        private readonly Dictionary<View, SizeRequest> _layoutCache = new Dictionary<View, SizeRequest>();

        public static readonly BindableProperty SpacingProperty = BindableProperty.Create(nameof(Spacing), typeof(double), typeof(WrapRepeaterView), 5d, propertyChanged: (bindable, oldValue, newValue) => ((WrapRepeaterView)bindable)._layoutCache.Clear());

        public static readonly BindableProperty ItemTemplateProperty = BindableProperty.Create(nameof(ItemTemplate), typeof(DataTemplate), typeof(WrapRepeaterView), default(DataTemplate));
        public static readonly BindableProperty EmptyTextTemplateProperty = BindableProperty.Create(nameof(EmptyTextTemplate), typeof(DataTemplate), typeof(WrapRepeaterView), default(DataTemplate));
        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(ICollection), typeof(WrapRepeaterView), new List<object>(), BindingMode.TwoWay, null, propertyChanged: (bindable, oldValue, newValue) => { ItemsChanged(bindable, (ICollection)newValue); });

        public static readonly BindableProperty EmptyTextProperty = BindableProperty.Create(nameof(EmptyText), typeof(string), typeof(WrapRepeaterView), string.Empty);

        public static readonly BindableProperty SelectedItemCommandProperty = BindableProperty.Create(nameof(SelectedItemCommand), typeof(ICommand), typeof(WrapRepeaterView), default(ICommand));

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

        public double Spacing
        {
            get { return (double)GetValue(SpacingProperty); }
            set { SetValue(SpacingProperty, value); }
        }

        public int MaxColumns { get; set; } = 3;
        public float TileHeight { get; set; } = 100;

        public WrapRepeaterView()
        {
            VerticalOptions = HorizontalOptions = LayoutOptions.FillAndExpand;
        }

        private static void ItemsChanged(BindableObject bindable, ICollection newValue)
        {
            var repeater = (WrapRepeaterView)bindable;

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

        private static void UpdateItems(ICollection sourceItems, WrapRepeaterView repeater)
        {
            repeater.Children.Clear();

            if (sourceItems.Count == 0 && (repeater.EmptyTextTemplate != null || !string.IsNullOrEmpty(repeater.EmptyText)))
            {
                repeater.BuildEmptyText();

                return;
            }

            if (repeater.RowDefinitions.Any())
            {
                repeater.RowDefinitions.Clear();
            }

            var numberOfRows = Math.Ceiling(sourceItems.Count / (float)repeater.MaxColumns);
            for (var i = 0; i < numberOfRows; i++)
            {
                repeater.RowDefinitions.Add(new RowDefinition { Height = repeater.TileHeight });
            }

            var dataTemplate = repeater.ItemTemplate;

            var index = 0;
            foreach (var item in sourceItems)
            {
                var content = dataTemplate.CreateContent();
                if (!(content is View) && !(content is ViewCell))
                {
                    throw new InvalidViewException("Templated control must be a View or a ViewCell");
                }

                var view = (content is View) ? content as View : ((ViewCell)content).View;

                if (repeater.SelectedItemCommand != null)
                {
                    view.GestureRecognizers.Add(new TapGestureRecognizer { Command = repeater.SelectedItemCommand, CommandParameter = item });
                }

                view.BindingContext = item;

                var column = index % repeater.MaxColumns;
                var row = (int)Math.Floor(index / (float)repeater.MaxColumns);

                repeater.Children.Add(view, column, row);

                index++;
            }
        }

        private void BuildEmptyText()
        {
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
    }
}