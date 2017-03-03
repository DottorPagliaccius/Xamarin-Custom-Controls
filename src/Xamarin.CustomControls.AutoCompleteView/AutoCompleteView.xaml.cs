using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
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

        public static readonly BindableProperty SuggestionBackgroundColorProperty = BindableProperty.Create(nameof(SuggestionBackgroundColor), typeof(Color), typeof(AutoCompleteView), Color.White);
        public static readonly BindableProperty SuggestionBorderColorProperty = BindableProperty.Create(nameof(SuggestionBorderColor), typeof(Color), typeof(AutoCompleteView), Color.Silver);
        public static readonly BindableProperty SuggestionBorderSizeProperty = BindableProperty.Create(nameof(SuggestionBorderSize), typeof(Thickness), typeof(AutoCompleteView), new Thickness(1));

        public static readonly BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(AutoCompleteView), Color.Black);
        public static readonly BindableProperty PlaceholderTextColorProperty = BindableProperty.Create(nameof(PlaceholderTextColor), typeof(Color), typeof(AutoCompleteView), Color.Silver);
        public static readonly BindableProperty FontSizeProperty = BindableProperty.Create(nameof(FontSize), typeof(double), typeof(AutoCompleteView), Font.Default.FontSize);

        public static readonly BindableProperty OpenOnFocusProperty = BindableProperty.Create(nameof(OpenOnFocus), typeof(bool), typeof(AutoCompleteView), false);
        public static readonly BindableProperty MaxResultsProperty = BindableProperty.Create(nameof(MaxResults), typeof(int), typeof(AutoCompleteView), 20);

        public static readonly BindableProperty SearchMemberProperty = BindableProperty.Create(nameof(SearchMember), typeof(string), typeof(AutoCompleteView), string.Empty);

        public static readonly BindableProperty SeparatorColorProperty = BindableProperty.Create(nameof(SeparatorColor), typeof(Color), typeof(AutoCompleteView), Color.Silver);
        public static readonly BindableProperty SeparatorHeightProperty = BindableProperty.Create(nameof(SeparatorHeight), typeof(double), typeof(AutoCompleteView), 1.5d);

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
            private set { SetValue(SelectedItemProperty, value); }
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

        public string SearchMember
        {
            get { return (string)GetValue(SearchMemberProperty); }
            set { SetValue(SearchMemberProperty, value); }
        }

        public Color SeparatorColor
        {
            get { return (Color)GetValue(SeparatorColorProperty); }
            set { SetValue(SeparatorColorProperty, value); }
        }

        public double SeparatorHeight
        {
            get { return (double)GetValue(SeparatorHeightProperty); }
            set { SetValue(SeparatorHeightProperty, value); }
        }

        public bool ShowSeparator
        {
            get { return SuggestedItemsRepeaterView.ShowSeparator; }
            set { SuggestedItemsRepeaterView.ShowSeparator = value; }
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

            if (propertyName == SeparatorColorProperty.PropertyName)
            {
                SuggestedItemsRepeaterView.SeparatorColor = SeparatorColor;
            }

            if (propertyName == SeparatorHeightProperty.PropertyName)
            {
                SuggestedItemsRepeaterView.SeparatorHeight = SeparatorHeight;
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

                    if (OpenOnFocus)
                    {
                        FilterSuggestions(MainEntry.Text);
                    }
                }
                else
                {
                    var items = ItemsSource.Cast<object>();

                    if (MainEntry.Text.Length == 0 || (items.Any() && !items.Any(x => string.Equals(GetSearchMember(items.First().GetType()).GetValue(x).ToString(), MainEntry.Text, StringComparison.Ordinal))))
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
            var filteredSuggestions = ItemsSource.Cast<object>();

            if (text.Length > 0 && filteredSuggestions.Any())
            {
                var property = GetSearchMember(filteredSuggestions.First().GetType());

                if (property == null)
                    throw new MemberNotFoundException($"There's no corrisponding property the matches SearchMember value '{SearchMember}'");

                if (property.PropertyType != typeof(string))
                    throw new SearchMemberPropertyTypeException($"Property '{SearchMember}' must be of type string");

                filteredSuggestions = filteredSuggestions.Where(x => property.GetValue(x).ToString().IndexOf(text, StringComparison.OrdinalIgnoreCase) >= 0).OrderByDescending(x => property.GetValue(x).ToString());
            }

            _availableSuggestions = new ObservableCollection<object>(filteredSuggestions.Take(MaxResults));

            ShowHideListbox(true);
        }

        private PropertyInfo GetSearchMember(Type type)
        {
            var property = type.GetRuntimeProperty(SearchMember);

            if (property == null)
                throw new MemberNotFoundException($"There's no corrisponding property the matches SearchMember value '{SearchMember}'");

            if (property.PropertyType != typeof(string))
                throw new SearchMemberPropertyTypeException($"Property '{SearchMember}' must be of type string");

            return property;
        }

        private void SuggestedRepeaterItemSelected(object selectedItem)
        {
            MainEntry.Text = GetSelectedText(selectedItem);
            MainEntry.TextColor = TextColor;

            ShowHideListbox(false);

            _availableSuggestions.Clear();

            SelectedItem = selectedItem;

            SelectedItemCommand?.Execute(selectedItem);
        }

        private string GetSelectedText(object selectedItem)
        {
            var property = selectedItem.GetType().GetRuntimeProperty(SearchMember);

            if (property == null)
                throw new MemberNotFoundException($"There's no corrisponding property the matches DisplayMember value '{SearchMember}'");

            return property.GetValue(selectedItem).ToString();
        }

        private void ShowHideListbox(bool show)
        {
            if (show)
                SuggestedItemsRepeaterView.ItemsSource = _availableSuggestions;

            SuggestedItemsContainer.IsVisible = show;
        }
    }
}
