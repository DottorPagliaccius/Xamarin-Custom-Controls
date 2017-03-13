using System.Collections.ObjectModel;
using Xamarin.CustomControls;

namespace CustomControlsSamples
{
    public class SegmentedViewModel
    {
        public ObservableCollection<ISegmentedViewItem> Items { get; } = new ObservableCollection<ISegmentedViewItem>();

        public SegmentedViewModel()
        {
            LoadItems();
        }

        private void LoadItems()
        {

        }
    }
}