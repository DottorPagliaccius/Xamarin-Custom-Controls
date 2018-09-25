using System;
using Lottie.Forms;
using Rg.Plugins.Popup.Animations;
using Rg.Plugins.Popup.Animations.Base;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;

namespace Xamarin.CustomControls.LottieLoader
{
    public class LottieLoader : PopupPage, IDisposable
    {
        private bool _disposed;
        private readonly INavigation _navigation;
        private readonly string _jsonAnimationFile;

        private static string DefaultJsonAnimationFile;
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

        public LottieLoader(INavigation navigation, string jsonAnimationFile = null, BaseAnimation animation = null, bool autoShow = true)
        {
            if (string.IsNullOrEmpty(DefaultJsonAnimationFile) && string.IsNullOrEmpty(jsonAnimationFile))
                throw new ArgumentException("Please specify a jsonAnimationFile or call Init() to set a deafult one", nameof(jsonAnimationFile));

            _navigation = navigation;
            _jsonAnimationFile = jsonAnimationFile;

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

            DefaultJsonAnimationFile = defaultJsonAnimationFile;
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
            var jsonAnimationFile = string.IsNullOrEmpty(_jsonAnimationFile) ? DefaultJsonAnimationFile : _jsonAnimationFile;

            Content = new StackLayout
            {
                Padding = new Thickness(90),

                Children = { new AnimationView { Loop = true, AutoPlay = true, Animation = jsonAnimationFile, VerticalOptions = LayoutOptions.FillAndExpand, HorizontalOptions = LayoutOptions.FillAndExpand } }
            };
        }

        public void Show()
        {
            _navigation.PushPopupAsync(this);
        }

        public void Close()
        {
            _navigation.PopPopupAsync();
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