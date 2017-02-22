using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace Xamarin.CustomControls
{
    public partial class AutoCompleteView : ContentView
    {
        private ObservableCollection<object> _availableSuggestions;

        public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(AutoCompleteView), string.Empty);
        public static readonly BindableProperty ItemTemplateProperty = BindableProperty.Create(nameof(ItemTemplate), typeof(DataTemplate), typeof(AutoCompleteView), default(DataTemplate));
        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable), typeof(AutoCompleteView), new List<object>());
        public static readonly BindableProperty EmptyTextProperty = BindableProperty.Create(nameof(EmptyText), typeof(string), typeof(AutoCompleteView), string.Empty);

        public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create(nameof(SelectedItem), typeof(object), typeof(AutoCompleteView), null, BindingMode.TwoWay);
        public static readonly BindableProperty SelectedItemCommandProperty = BindableProperty.Create(nameof(SelectedItemCommand), typeof(ICommand), typeof(AutoCompleteView), default(ICommand));

        public static readonly BindableProperty SuggestionsHeightRequestProperty = BindableProperty.Create(nameof(SuggestionsHeightRequest), typeof(double), typeof(AutoCompleteView), 250d);
        public static readonly BindableProperty SuggestionBackgroundColorProperty = BindableProperty.Create(nameof(SuggestionBackgroundColor), typeof(Color), typeof(AutoCompleteView), Color.White);
        public static readonly BindableProperty SuggestionBorderColorProperty = BindableProperty.Create(nameof(SuggestionBorderColor), typeof(Color), typeof(AutoCompleteView), Color.Black);
        public static readonly BindableProperty SuggestionBorderSizeProperty = BindableProperty.Create(nameof(SuggestionBorderSize), typeof(Thickness), typeof(AutoCompleteView), new Thickness(1));

        public static readonly BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(AutoCompleteView), Color.Black);
        public static readonly BindableProperty PlaceholderTextColorProperty = BindableProperty.Create(nameof(PlaceholderTextColor), typeof(Color), typeof(AutoCompleteView), Color.Silver);
        public static readonly BindableProperty FontSizeProperty = BindableProperty.Create(nameof(FontSize), typeof(double), typeof(AutoCompleteView), Font.Default.FontSize);

        public static readonly BindableProperty OpenOnFocusProperty = BindableProperty.Create(nameof(OpenOnFocus), typeof(bool), typeof(AutoCompleteView), false);
        public static readonly BindableProperty MaxResultsProperty = BindableProperty.Create(nameof(MaxResults), typeof(int), typeof(AutoCompleteView), 20);

        public double SuggestionsHeightRequest
        {
            get { return (double)GetValue(SuggestionsHeightRequestProperty); }
            set { SetValue(SuggestionsHeightRequestProperty, value); }
        }

        public string Placeholder
        {
            get { return (string)GetValue(PlaceholderProperty); }
            set { SetValue(PlaceholderProperty, value); }
        }

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); OnPropertyChanged(); }
        }

        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        public string EmptyText
        {
            get { return (string)GetValue(EmptyTextProperty); }
            set { SetValue(EmptyTextProperty, value); }
        }

        public Color TextColor
        {
            get { return (Color)GetValue(TextColorProperty); }
            set { SetValue(TextColorProperty, value); }
        }

        public Color SuggestionBackgroundColor
        {
            get { return (Color)GetValue(SuggestionBackgroundColorProperty); }
            set { SetValue(SuggestionBackgroundColorProperty, value); }
        }

        public Color SuggestionBorderColor
        {
            get { return (Color)GetValue(SuggestionBorderColorProperty); }
            set { SetValue(SuggestionBorderColorProperty, value); }
        }

        public Thickness SuggestionBorderSize
        {
            get { return (Thickness)GetValue(SuggestionBorderSizeProperty); }
            set { SetValue(SuggestionBorderSizeProperty, value); }
        }

        public double FontSize
        {
            get { return (double)GetValue(FontSizeProperty); }
            set { SetValue(FontSizeProperty, value); }
        }

        public Color PlaceholderTextColor
        {
            get { return (Color)GetValue(PlaceholderTextColorProperty); }
            set { SetValue(PlaceholderTextColorProperty, value); }
        }

        public ICommand SelectedItemCommand
        {
            get { return (ICommand)GetValue(SelectedItemCommandProperty); }
            set { SetValue(SelectedItemCommandProperty, value); }
        }

        public object SelectedItem
        {
            get { return GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        public bool OpenOnFocus
        {
            get { return (bool)GetValue(OpenOnFocusProperty); }
            set { SetValue(OpenOnFocusProperty, value); }
        }

        public int MaxResults
        {
            get { return (int)GetValue(MaxResultsProperty); }
            set { SetValue(MaxResultsProperty, value); }
        }

        public AutoCompleteView()
        {
            InitializeComponent();

            _availableSuggestions = new ObservableCollection<object>();

            SuggestedItemsRepeaterView.SelectedItemCommand = new Command(SuggestedRepeaterItemSelected);
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == SelectedItemProperty.PropertyName)
            {
                MainEntry.Text = SelectedItem.ToString();
                MainEntry.TextColor = TextColor;

                ShowHideListbox(false);
            }

            if (propertyName == PlaceholderProperty.PropertyName && SelectedItem == null)
            {
                MainEntry.Text = Placeholder;
            }

            if (propertyName == PlaceholderTextColorProperty.PropertyName)
            {
                MainEntry.TextColor = PlaceholderTextColor;
            }

            if (propertyName == ItemTemplateProperty.PropertyName)
            {
                SuggestedItemsRepeaterView.ItemTemplate = ItemTemplate;
            }

            if (propertyName == SuggestionsHeightRequestProperty.PropertyName)
            {
                SuggestedItemsRepeaterView.HeightRequest = SuggestionsHeightRequest;
            }

            if (propertyName == SuggestionBackgroundColorProperty.PropertyName)
            {
                SuggestedItemsInnerContainer.BackgroundColor = SuggestionBackgroundColor;
            }

            if (propertyName == SuggestionBorderColorProperty.PropertyName)
            {
                SuggestedItemsContainer.BackgroundColor = SuggestionBorderColor;
            }

            if (propertyName == SuggestionBorderSizeProperty.PropertyName)
            {
                SuggestedItemsContainer.Padding = SuggestionBorderSize;
            }

            if (propertyName == EmptyTextProperty.PropertyName)
            {
                SuggestedItemsRepeaterView.EmptyText = EmptyText;
            }
        }

        private void SearchText_Focused(object sender, FocusEventArgs e)
        {
            HandleFocus(e.IsFocused);
        }

        private void SearchText_Unfocused(object sender, FocusEventArgs e)
        {
            HandleFocus(e.IsFocused);
        }

        private void HandleFocus(bool isFocused)
        {
            MainEntry.TextChanged -= SearchText_TextChanged;

            try
            {
                if (isFocused)
                {
                    if (string.Equals(MainEntry.Text, Placeholder, StringComparison.OrdinalIgnoreCase))
                    {
                        MainEntry.Text = string.Empty;
                        MainEntry.TextColor = TextColor;
                    }

                    if (OpenOnFocus || MainEntry.Text.Length > 0)
                    {
                        FilterSuggestions(MainEntry.Text);
                    }
                }
                else
                {
                    if (MainEntry.Text.Length == 0 || !ItemsSource.Cast<object>().Any(x => string.Equals(x.ToString(), MainEntry.Text, StringComparison.Ordinal)))
                    {
                        MainEntry.Text = Placeholder;
                        MainEntry.TextColor = PlaceholderTextColor;
                    }
                    else
                        MainEntry.TextColor = TextColor;

                    ShowHideListbox(false);
                }
            }
            finally
            {
                MainEntry.TextChanged += SearchText_TextChanged;
            }
        }

        private void SearchText_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.Equals(MainEntry.Text, Placeholder, StringComparison.OrdinalIgnoreCase))
            {
                if (_availableSuggestions.Any())
                {
                    _availableSuggestions.Clear();

                    ShowHideListbox(false);
                }

                return;
            }

            FilterSuggestions(MainEntry.Text);
        }

        private void FilterSuggestions(string text)
        {
            var filteredSuggestions = text.Length == 0 ? ItemsSource.Cast<object>() : ItemsSource.Cast<object>().Where(x => x.ToString().IndexOf(text, StringComparison.OrdinalIgnoreCase) >= 0)
                                                                                                                .OrderByDescending(x => x.ToString());
            _availableSuggestions = new ObservableCollection<object>(filteredSuggestions.Take(MaxResults));

            ShowHideListbox(true);
        }

        private void SuggestedRepeaterItemSelected(object selectedItem)
        {
            MainEntry.Text = selectedItem.ToString();
            MainEntry.TextColor = TextColor;

            _availableSuggestions.Clear();

            ShowHideListbox(false);

            SelectedItem = selectedItem;

            SelectedItemCommand?.Execute(selectedItem);
        }

        private void ShowHideListbox(bool show)
        {
            SuggestedItemsContainer.IsVisible = show;

            if (show)
                SuggestedItemsRepeaterView.ItemsSource = _availableSuggestions;
        }
    }
}
