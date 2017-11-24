using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace CustomControlsSamples
{
    public partial class BadgeButtonPage : ContentPage
    {
        public string BadgeText1 { get; private set; } = "0";

        public BadgeButtonPage()
        {
            InitializeComponent();

            BindingContext = this;
        }

        private void Handle_Clicked(object sender, EventArgs e)
        {
            var value = int.Parse(BadgeText1);

            if (value < 10)
                BadgeText1 = (++value).ToString();
            else
                BadgeText1 = "0";

            OnPropertyChanged(nameof(BadgeText1));
        }
    }
}
