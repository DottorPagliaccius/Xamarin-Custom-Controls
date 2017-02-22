using System;
using System.Collections;
using System.Reflection;
using System.Windows.Input;
using Xamarin.Forms;

namespace Xamarin.CustomControls
{
    public class BindablePicker : Picker
    {
        private bool _disableEvents;

        public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create(nameof(SelectedItem), typeof(object), typeof(BindablePicker), null, BindingMode.TwoWay, null, (bindable, oldValue, newValue) => OnSelectedItemChanged(bindable, oldValue, newValue));
        public static readonly BindableProperty SelectedItemCommandProperty = BindableProperty.Create(nameof(SelectedItemCommand), typeof(ICommand), typeof(BindablePicker), null);
        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable), typeof(BindablePicker), null, BindingMode.TwoWay, null, (bindable, oldValue, newValue) => OnItemsSourceChanged(bindable, oldValue, newValue));
        public static readonly BindableProperty DisplayPropertyProperty = BindableProperty.Create(nameof(DisplayProperty), typeof(string), typeof(BindablePicker), null, BindingMode.OneWay, null, (bindable, oldValue, newValue) => OnDisplayPropertyChanged(bindable, oldValue, newValue));

        public IList ItemsSource
        {
            get { return (IList)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public ICommand SelectedItemCommand
        {
            get { return (ICommand)GetValue(SelectedItemCommandProperty); }
            set { SetValue(SelectedItemCommandProperty, value); }
        }

        public object SelectedItem
        {
            get { return GetValue(SelectedItemProperty); }
            set
            {
                SetValue(SelectedItemProperty, value);

                if (ItemsSource != null && SelectedItem != null)
                {
                    if (ItemsSource.Contains(SelectedItem))
                    {
                        SelectedIndex = ItemsSource.IndexOf(SelectedItem);
                    }
                    else
                    {
                        SelectedIndex = -1;
                    }
                }
            }
        }

        public string DisplayProperty
        {
            get { return (string)GetValue(DisplayPropertyProperty); }
            set { SetValue(DisplayPropertyProperty, value); }
        }

        public BindablePicker()
        {
            SelectedIndexChanged += OnSelectedIndexChanged;
        }

        private void OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (_disableEvents)
                return;

            if (SelectedIndex == -1)
            {
                SelectedItem = null;
            }
            else
            {
                SelectedItem = ItemsSource[SelectedIndex];

                SelectedItemCommand.Execute(SelectedItem);
            }
        }

        private static void OnSelectedItemChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var picker = (BindablePicker)bindable;

            picker.SelectedItem = newValue;

            if (picker.ItemsSource == null || picker.SelectedItem == null)
                return;

            for (var i = 0; i < picker.ItemsSource.Count; i++)
            {
                if (picker.ItemsSource[i] == picker.SelectedItem)
                {
                    picker.SelectedIndex = i;
                    break;
                }
            }
        }

        private static void OnDisplayPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var picker = (BindablePicker)bindable;

            picker.DisplayProperty = (string)newValue;

            LoadItemsAndSetSelected(bindable);
        }

        private static void OnItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var picker = (BindablePicker)bindable;

            picker.ItemsSource = (IList)newValue;

            LoadItemsAndSetSelected(bindable);
        }

        private static void LoadItemsAndSetSelected(BindableObject bindable)
        {
            var picker = (BindablePicker)bindable;
            if (picker.ItemsSource as IEnumerable != null)
            {
                picker._disableEvents = true;
                picker.SelectedIndex = -1;
                picker.Items.Clear();

                var property = picker.ItemsSource.Count > 0 && !string.IsNullOrEmpty(picker.DisplayProperty) ? picker.ItemsSource[0].GetType().GetRuntimeProperty(picker.DisplayProperty) : null;

                for (var i = 0; i < picker.ItemsSource.Count; i++)
                {
                    var item = picker.ItemsSource[i];
                    var value = picker.DisplayProperty != null && property != null ? property.GetValue(item).ToString() : item.ToString();

                    picker.Items.Add(value);

                    if (picker.SelectedItem != null && picker.SelectedItem == item)
                    {
                        picker.SelectedIndex = i;
                    }
                }

                picker._disableEvents = false;
            }
            else
            {
                picker.Items.Clear();
            }
        }
    }
}