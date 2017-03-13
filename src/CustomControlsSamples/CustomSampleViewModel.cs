using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmHelpers;
using Xamarin.Forms;

namespace CustomControlsSamples
{
    public class RandomObject
    {
        public string RandomProperty1 { get; set; }
        public string RandomProperty2 { get; set; }
        public string RandomProperty3 { get; set; }
        public string RandomProperty4 { get; set; }
    }

    public class CustomSampleViewModel : BaseViewModel
    {
        private int _loadingCount = 1;

        private ICommand _selectedItemCommand;
        private ICommand _reloadItemCommand;

        public ICommand SelectedItemCommand => _selectedItemCommand ?? (_selectedItemCommand = new Command((selectedItem) => Select((RandomObject)selectedItem)));
        public ICommand ReloadItemCommand => _reloadItemCommand ?? (_reloadItemCommand = new Command(async () => await LoadData(true)));

        public ObservableRangeCollection<RandomObject> Items { get; } = new ObservableRangeCollection<RandomObject>();

        public string SelectedValue { get; set; } = "No selection";

        public async Task LoadData(bool isReloading = false)
        {
            IsBusy = true;

            if (isReloading)
                _loadingCount++;

            try
            {
                var items = new List<RandomObject>
            {
                new RandomObject{ RandomProperty1=$"{_loadingCount}", RandomProperty2="red", RandomProperty3 = "cat",  RandomProperty4 = "apples"},
                new RandomObject{ RandomProperty1=$"{_loadingCount+1}", RandomProperty2="blue", RandomProperty3 = "dog",  RandomProperty4 = "oranges"},
                new RandomObject{ RandomProperty1=$"{_loadingCount+2}", RandomProperty2="green", RandomProperty3 = "fish",  RandomProperty4 = "kiwi"},
                new RandomObject{ RandomProperty1=$"{_loadingCount+3}", RandomProperty2="purple", RandomProperty3 = "platypus",  RandomProperty4 = "ananas"},
            };
                if (isReloading)
                    await Task.Delay(5000);

                Items.ReplaceRange(items);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void Select(RandomObject selectedItem)
        {
            SelectedValue = $"Selected item {Items.IndexOf(selectedItem)}: {selectedItem.RandomProperty1} => {selectedItem.RandomProperty2} {selectedItem.RandomProperty3} eats {selectedItem.RandomProperty4}";

            OnPropertyChanged(nameof(SelectedValue));
        }
    }
}

