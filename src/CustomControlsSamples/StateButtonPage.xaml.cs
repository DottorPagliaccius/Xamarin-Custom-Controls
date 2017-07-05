using System.Windows.Input;
using Xamarin.Forms;

namespace CustomControlsSamples
{
    public partial class StateButtonPage : ContentPage
    {
        private ICommand _selectedCommand;

        public ICommand SelectCommand => _selectedCommand ?? (_selectedCommand = new Command((selectedItem) => TopLabel.Text = $"Button {selectedItem.ToString()} pressed"));

        public StateButtonPage()
        {
            InitializeComponent();

            BindingContext = this;

            TestButton.InnerPadding = new Thickness(10, 0);
        }
    }
}
