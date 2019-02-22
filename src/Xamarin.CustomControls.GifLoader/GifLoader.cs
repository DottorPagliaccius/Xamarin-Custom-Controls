using System;
using Rg.Plugins.Popup.Animations;
using Rg.Plugins.Popup.Animations.Base;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;

namespace Xamarin.CustomControls
{
    public class GifLoader : PopupPage, IDisposable
    {
        private bool _disposed;
        private readonly INavigation _navigation;
        private readonly string _gifFile;

        private static string DefaultGifFile;
        private static BaseAnimation DefaultPopupAnimation = new ScaleAnimation
        {
            ScaleIn = 0.8,
            ScaleOut = 0.8,
            DurationIn = 400,
            DurationOut = 300,
            EasingIn = Easing.SinOut,
            EasingOut = Easing.SinIn,
            HasBackgroundAnimation = true
        };

        public GifLoader(INavigation navigation, string gifFile = null, BaseAnimation animation = null, bool autoShow = true)
        {
            if (string.IsNullOrEmpty(DefaultGifFile) && string.IsNullOrEmpty(gifFile))
                throw new ArgumentException("Please specify a jsonAnimationFile or call Init() to set a deafult one", nameof(gifFile));

            _navigation = navigation;
            _gifFile = gifFile;

            if (animation != null)
                SetAnimation(animation);

            SetContent();

            if (autoShow)
                Show();
        }

        public static void SetDefaultJsonFile(string defaultJsonAnimationFile)
        {
            if (string.IsNullOrEmpty(defaultJsonAnimationFile))
                throw new ArgumentException(nameof(defaultJsonAnimationFile));

            DefaultGifFile = defaultJsonAnimationFile;
        }

        public static void SetDefaultPopupAnimation(BaseAnimation defaultPopupAnimation)
        {
            DefaultPopupAnimation = defaultPopupAnimation ?? throw new ArgumentNullException(nameof(defaultPopupAnimation));
        }

        private void SetAnimation(BaseAnimation animation)
        {
            Animation = animation;
        }

        private void SetContent()
        {
            var gifFile = string.IsNullOrEmpty(_gifFile) ? DefaultGifFile : _gifFile;

            Content = new StackLayout
            {
                Padding = new Thickness(90),

                Children = { new FFImageLoading.Forms.CachedImage { Source = gifFile, VerticalOptions = LayoutOptions.FillAndExpand, HorizontalOptions = LayoutOptions.FillAndExpand } }
            };
        }

        public void Show()
        {
            _navigation.PushPopupAsync(this);
        }

        public void Close()
        {
            try
            {
                _navigation.PopPopupAsync();
            }
            catch //shitty and I hope temporary way to prevent occasional "There is not page in PopupStack" errors
            {

            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    Close();
                }

                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}