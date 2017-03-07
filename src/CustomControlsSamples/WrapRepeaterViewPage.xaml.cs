using System.Collections.Generic;
using MvvmHelpers;
using Xamarin.Forms;

namespace CustomControlsSamples
{
    public partial class WrapRepeaterViewPage : ContentPage
    {
        public ObservableRangeCollection<string> Items { get; } = new ObservableRangeCollection<string>();

        public WrapRepeaterViewPage()
        {
            InitializeComponent();

            BindingContext = this;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var items = new List<string>
            {
                "https://upload.wikimedia.org/wikipedia/commons/thumb/b/b2/Confederacy_of_Independent_Systems.svg/149px-Confederacy_of_Independent_Systems.svg.png",
                "https://upload.wikimedia.org/wikipedia/commons/thumb/e/e0/Galactic_Republic.svg/600px-Galactic_Republic.svg.png",
                "https://upload.wikimedia.org/wikipedia/commons/f/fa/First_Galactic_Empire_emblem.png",
                "https://upload.wikimedia.org/wikipedia/commons/thumb/2/2a/Rebel_Alliance_logo.svg/300px-Rebel_Alliance_logo.svg.png",
                "https://upload.wikimedia.org/wikipedia/commons/thumb/4/4a/Emblem_of_the_First_Order.svg/457px-Emblem_of_the_First_Order.svg.png",
                "https://upload.wikimedia.org/wikipedia/commons/thumb/b/b2/Confederacy_of_Independent_Systems.svg/149px-Confederacy_of_Independent_Systems.svg.png",
                "https://upload.wikimedia.org/wikipedia/commons/thumb/e/e0/Galactic_Republic.svg/600px-Galactic_Republic.svg.png",
                "https://upload.wikimedia.org/wikipedia/commons/f/fa/First_Galactic_Empire_emblem.png",
                "https://upload.wikimedia.org/wikipedia/commons/thumb/2/2a/Rebel_Alliance_logo.svg/300px-Rebel_Alliance_logo.svg.png",
                "https://upload.wikimedia.org/wikipedia/commons/thumb/4/4a/Emblem_of_the_First_Order.svg/457px-Emblem_of_the_First_Order.svg.png"
            };

            Items.AddRange(items);
        }
    }
}
