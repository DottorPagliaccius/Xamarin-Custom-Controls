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
    public enum SuggestionPlacement
    {
        Bottom,
        Top
    }

    public partial class AutoCompleteView : ContentView
    {
        private Layout<View> _optionalSuggestionsPanelContainer;
        private SuggestionPlacement _suggestionPlacement = SuggestionPlacement.Bottom;

        private PropertyInfo _searchMemberCachePropertyInfo;

        private ObservableCollection<object> _availableSuggestions;

        public event EventHandler OnSuggestionOpen;
        public event EventHandler OnSuggestionClose;

        public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(AutoCompleteView), string.Empty);
        public static readonly BindableProperty ItemTemplateProperty = BindableProperty.Create(nameof(ItemTemplate), typeof(DataTemplate), typeof(AutoCompleteView), default(DataTemplate));
        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable), typeof(AutoCompleteView), new List<object>());
        public static readonly BindableProperty EmptyTextProperty = BindableProperty.Create(nameof(EmptyText), typeof(string), typeof(AutoCompleteView), string.Empty);

        public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create(nameof(SelectedItem), typeof(object), typeof(AutoCompleteView), null, BindingMode.TwoWay);
        public static readonly BindableProperty SelectedItemCommandProperty = BindableProperty.Create(nameof(SelectedItemCommand), typeof(ICommand), typeof(AutoCompleteView), default(ICommand));

        public static readonly BindableProperty SuggestionBackgroundColorProperty = BindableProperty.Create(nameof(SuggestionBackgroundColor), typeof(Color), typeof(AutoCompleteView), Color.White);
        public static readonly BindableProperty SuggestionBorderColorProperty = BindableProperty.Create(nameof(SuggestionBorderColor), typeof(Color), typeof(AutoCompleteView), Color.Silver);

        public static readonly BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(AutoCompleteView), Color.Black);
        public static readonly BindableProperty PlaceholderTextColorProperty = BindableProperty.Create(nameof(PlaceholderTextColor), typeof(Color), typeof(AutoCompleteView), Color.Silver);
        public static readonly BindableProperty FontSizeProperty = BindableProperty.Create(nameof(FontSize), typeof(double), typeof(AutoCompleteView), Font.Default.FontSize);

        public static readonly BindableProperty SearchMemberProperty = BindableProperty.Create(nameof(SearchMember), typeof(string), typeof(AutoCompleteView), string.Empty);

        public static readonly BindableProperty SeparatorColorProperty = BindableProperty.Create(nameof(SeparatorColor), typeof(Color), typeof(AutoCompleteView), Color.Silver);
        public static readonly BindableProperty SeparatorHeightProperty = BindableProperty.Create(nameof(SeparatorHeight), typeof(double), typeof(AutoCompleteView), 1.5d);

        public static readonly BindableProperty EntryLineColorProperty = BindableProperty.Create(nameof(EntryLineColor), typeof(Color), typeof(AutoCompleteView), Color.Black);
        public static readonly BindableProperty EntryLineHeightProperty = BindableProperty.Create(nameof(EntryLineHeight), typeof(double), typeof(AutoCompleteView), 1d);

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

        public bool OpenOnFocus { get; set; }
        public int MaxResults { get; set; }

        public SuggestionPlacement SuggestionPlacement
        {
            get { return _suggestionPlacement; }
            set
            {
                _suggestionPlacement = value;

                OnPropertyChanged();
                PlaceSuggestionPanel();
            }
        }

        public bool ShowEntryLine
        {
            get
            {
                return EntryLine.IsVisible;
            }

            set
            {
                EntryLine.IsVisible = value;
            }
        }

        public Color EntryLineColor
        {
            get { return (Color)GetValue(EntryLineColorProperty); }
            set { SetValue(EntryLineColorProperty, value); }
        }

        public double EntryLineHeight
        {
            get { return (double)GetValue(EntryLineHeightProperty); }
            set { SetValue(EntryLineHeightProperty, value); }
        }

        public Layout<View> OptionalSuggestionsPanelContainer
        {
            get { return _optionalSuggestionsPanelContainer; }
            set
            {
                _optionalSuggestionsPanelContainer = value;

                if (value == null)
                    return;

                OnPropertyChanged();
                PlaceSuggestionPanel();
            }
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

            if (propertyName == SelectedItemProperty.PropertyName)
            {
                if (SelectedItem != null)
                {
                    var propertyInfo = GetSearchMember(SelectedItem.GetType());

                    var selectedItem = ItemsSource.Cast<object>().SingleOrDefault(x => propertyInfo.GetValue(x).ToString() == propertyInfo.GetValue(SelectedItem).ToString());

                    if (selectedItem != null)
                    {
                        try
                        {
                            MainEntry.TextChanged -= SearchText_TextChanged;

                            MainEntry.Text = propertyInfo.GetValue(SelectedItem).ToString();
                        }
                        finally
                        {
                            MainEntry.TextChanged -= SearchText_TextChanged;
                        }

                        FilterSuggestions(MainEntry.Text, false);

                        MainEntry.TextColor = TextColor;
                    }
                    else
                    {
                        MainEntry.Text = Placeholder;
                        MainEntry.TextColor = PlaceholderTextColor;
                    }
                }
                else
                {
                    MainEntry.Text = Placeholder;
                    MainEntry.TextColor = PlaceholderTextColor;
                }
            }

            if (propertyName == SearchMemberProperty.PropertyName)
            {
                _searchMemberCachePropertyInfo = null;
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
                SuggestedItemsOuterContainer.BackgroundColor = SuggestionBackgroundColor;
            }

            if (propertyName == SuggestionBorderColorProperty.PropertyName)
            {
                SuggestedItemsOuterContainer.OutlineColor = SuggestionBorderColor;
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

            if (propertyName == EntryLineColorProperty.PropertyName)
            {
                EntryLine.Color = EntryLineColor;
            }

            if (propertyName == SeparatorHeightProperty.PropertyName)
            {
                EntryLine.HeightRequest = EntryLineHeight;
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

        private void FilterSuggestions(string text, bool openSuggestionPanel = true)
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

            ShowHideListbox(openSuggestionPanel);
        }

        private PropertyInfo GetSearchMember(Type type)
        {
            if (_searchMemberCachePropertyInfo != null)
                return _searchMemberCachePropertyInfo;

            if (string.IsNullOrEmpty(SearchMember))
                throw new MemberNotFoundException("You must specify SearchMember property");

            _searchMemberCachePropertyInfo = type.GetRuntimeProperty(SearchMember);

            if (_searchMemberCachePropertyInfo == null)
                throw new MemberNotFoundException($"There's no corrisponding property the matches SearchMember value '{SearchMember}'");

            if (_searchMemberCachePropertyInfo.PropertyType != typeof(string))
                throw new SearchMemberPropertyTypeException($"Property '{SearchMember}' must be of type string");

            return _searchMemberCachePropertyInfo;
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

        private void PlaceSuggestionPanel()
        {
            if (OptionalSuggestionsPanelContainer == null)
            {
                SuggestedItemsContainerTop.IsVisible = false;
                SuggestedItemsContainerBottom.IsVisible = false;

                if (SuggestionPlacement == SuggestionPlacement.Bottom)
                {
                    if (SuggestedItemsContainerTop.Children.Any())
                    {
                        var suggestionPanel = SuggestedItemsContainerTop.Children.First();

                        SuggestedItemsContainerTop.Children.Remove(suggestionPanel);
                        SuggestedItemsContainerBottom.Children.Add(suggestionPanel);
                    }
                }
                else
                {
                    if (SuggestedItemsContainerBottom.Children.Any())
                    {
                        var suggestionPanel = SuggestedItemsContainerBottom.Children.First();

                        SuggestedItemsContainerBottom.Children.Remove(suggestionPanel);
                        SuggestedItemsContainerTop.Children.Add(suggestionPanel);
                    }
                }
            }
            else
            {
                if (SuggestedItemsContainerTop.Children.Any())
                {
                    var suggestionPanel = SuggestedItemsContainerTop.Children.First();

                    SuggestedItemsContainerTop.Children.Remove(suggestionPanel);
                    OptionalSuggestionsPanelContainer.Children.Add(suggestionPanel);
                }

                if (SuggestedItemsContainerBottom.Children.Any())
                {
                    var suggestionPanel = SuggestedItemsContainerBottom.Children.First();

                    SuggestedItemsContainerBottom.Children.Remove(suggestionPanel);
                    OptionalSuggestionsPanelContainer.Children.Add(suggestionPanel);
                }
            }
        }

        private void ShowHideListbox(bool show)
        {
            if (show)
            {
                SuggestedItemsRepeaterView.ItemsSource = _availableSuggestions;

                if (!SuggestedItemsContainerTop.IsVisible && !SuggestedItemsContainerBottom.IsVisible)
                    OnSuggestionOpen?.Invoke(this, new EventArgs());
            }
            else
            {
                if (SuggestedItemsContainerTop.IsVisible || SuggestedItemsContainerBottom.IsVisible)
                {
                    MainEntry.Unfocus();
                    Unfocus();

                    OnSuggestionClose?.Invoke(this, new EventArgs());
                }
            }

            if (SuggestionPlacement == SuggestionPlacement.Top)
                SuggestedItemsContainerTop.IsVisible = show;
            else
                SuggestedItemsContainerBottom.IsVisible = show;
        }
    }
}
