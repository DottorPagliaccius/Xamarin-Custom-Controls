using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Xamarin.Forms;

namespace Xamarin.CustomControls
{
	public class AccordionRepeaterView : AccordionView
	{
		public static readonly BindableProperty AccordionItemTemplateProperty = BindableProperty.Create(nameof(AccordionItemTemplate), typeof(DataTemplate), typeof(AccordionRepeaterView), default(DataTemplate));
		public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(ICollection), typeof(AccordionRepeaterView), new List<object>(), BindingMode.TwoWay, null, propertyChanged: (bindable, oldValue, newValue) => { ItemsChanged(bindable, (ICollection)oldValue, (ICollection)newValue); });

		public ICollection ItemsSource
		{
			get => (ICollection)GetValue(ItemsSourceProperty);
			set => SetValue(ItemsSourceProperty, value);
		}

		public DataTemplate AccordionItemTemplate
		{
			get => (DataTemplate)GetValue(AccordionItemTemplateProperty);
			set => SetValue(AccordionItemTemplateProperty, value);
		}

		private static void ItemsChanged(BindableObject bindable, ICollection oldValue, ICollection newValue)
		{
			var repeater = (AccordionRepeaterView)bindable;

			if (oldValue != null)
			{
				if (oldValue is INotifyCollectionChanged observable)
				{
					observable.CollectionChanged -= repeater.CollectionChanged;
				}
			}

			if (newValue != null)
			{
				repeater.BuildItems();

				if (repeater.ItemsSource is INotifyCollectionChanged observable)
				{
					observable.CollectionChanged += repeater.CollectionChanged;
				}
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

		private AccordionItemView GetItemView(object item)
		{
			var content = AccordionItemTemplate.CreateContent();
			if (!(content is AccordionItemView view))
			{
				throw new InvalidViewException($"Templated control must be an AccordionItemView ({nameof(AccordionItemTemplate)})");
			}

			view.BindingContext = item;

			return view;
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

					BuildItems();
					break;
			}
		}
	}
}
