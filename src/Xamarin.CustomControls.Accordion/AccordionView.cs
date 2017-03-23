using System;
using Xamarin.Forms;

namespace Xamarin.CustomControls
{
    [ContentProperty("Children")]
    public class AccordionView : StackLayout
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

        public bool KeepOnlyOneItemOpen { get; set; }

        protected override void OnChildRemoved(Element child)
        {
            base.OnChildRemoved(child);

            if (!KeepOnlyOneItemOpen)
                return;

            var accordionItemView = (AccordionItemView)child;

            accordionItemView.OnClick -= AccordionItemView_OnClick;
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == OrientationProperty.PropertyName)
            {
                if (Orientation == StackOrientation.Horizontal)
                    Orientation = StackOrientation.Vertical;
            }
        }

        protected override void OnChildAdded(Element child)
        {
            base.OnChildAdded(child);

            if (!KeepOnlyOneItemOpen)
                return;

            var accordionItemView = child as AccordionItemView;

            if (accordionItemView == null)
                throw new InvalidViewException($"Control's children must be of type {nameof(AccordionItemView)}");

            accordionItemView.IsOpen = false;

            accordionItemView.OnClick += AccordionItemView_OnClick;
        }

        private void AccordionItemView_OnClick(object sender, AccordionItemClickEventArgs e)
        {
            if (e.Item.IsOpen)
            {
                foreach (var child in Children)
                {
                    var accordionItem = (AccordionItemView)child;

                    if (accordionItem != e.Item)
                        accordionItem.IsOpen = false;
                }
            }
        }
    }
}

