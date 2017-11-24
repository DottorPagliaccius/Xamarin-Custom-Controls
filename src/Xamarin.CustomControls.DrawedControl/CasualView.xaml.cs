using System;
using System.Collections.Generic;
using Xamarin.Forms;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System.Linq;
using System.IO;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Xamarin.CustomControls
{
    public partial class CasualView : ContentView
    {
        private const double Speed = 10000;
        private const float Radius = 30;

        private SKPaint _linePaint;
        private SKPaint _dotPaint;
        private SKPoint _currentPosition;

        private Stopwatch _stopwatch;
        private bool _isActive;
        private float _currentDirection;

        private double _t0;

        /*public static readonly BindableProperty StartingImageSourceProperty = BindableProperty.Create(nameof(StartingImageSource), typeof(ImageSource), typeof(CasualView), default(FileImageSource));
        public static readonly BindableProperty SuccessImageSourceProperty = BindableProperty.Create(nameof(SuccessImageSource), typeof(ImageSource), typeof(CasualView), default(FileImageSource));
        public static readonly BindableProperty ErrorImageSourceProperty = BindableProperty.Create(nameof(ErrorImageSource), typeof(ImageSource), typeof(CasualView), default(FileImageSource));

        public ImageSource StartingImageSource
        {
            get { return (FileImageSource)GetValue(StartingImageSourceProperty); }
            set { SetValue(StartingImageSourceProperty, value); }
        }

        public ImageSource SuccessImageSource
        {
            get { return (FileImageSource)GetValue(SuccessImageSourceProperty); }
            set { SetValue(SuccessImageSourceProperty, value); }
        }

        public ImageSource ErrorImageSource
        {
            get { return (FileImageSource)GetValue(ErrorImageSourceProperty); }
            set { SetValue(ErrorImageSourceProperty, value); }
        }*/

        public CasualView()
        {
            InitializeComponent();

            _linePaint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = Color.Red.ToSKColor(),
                StrokeWidth = 2,
            };

            _dotPaint = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = Color.BlueViolet.ToSKColor(),
                StrokeWidth = 2
            };

            _stopwatch = new Stopwatch();

            _currentPosition = new SKPoint(30, 30);
            _currentDirection = 45;
        }

        private async void OnCanvasViewTapped(object sender, EventArgs args)
        {
            _isActive = !_isActive;

            if (_isActive)
            {
                _currentDirection = GetRandomDirection();

                await AnimationLoop();
            }
        }

        private async Task AnimationLoop()
        {
            _stopwatch.Start();

            while (_isActive)
            {
                var t = _stopwatch.Elapsed.TotalMilliseconds;

                _currentPosition = GetNextPoint(t - _t0);

                Debug.WriteLine($"delta t: {t - _t0}");
                Debug.WriteLine($"Position X: {_currentPosition.X}, Y: {_currentPosition.Y}");

                Canvas.InvalidateSurface();

                await Task.Delay(TimeSpan.FromSeconds(1.0 / 30));

                _t0 = _stopwatch.Elapsed.TotalMilliseconds;
            }

            _stopwatch.Stop();
        }

        private void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            var info = args.Info;
            var surface = args.Surface;
            var canvas = surface.Canvas;

            canvas.Clear();

            if (_currentPosition.X <= (Radius / 2) || _currentPosition.X >= info.Width - (Radius / 2))
            {
                _currentDirection = 180 - _currentDirection;
            }
            else if (_currentPosition.Y <= (Radius / 2) || _currentPosition.Y >= info.Height - (Radius / 2))
            {
                _currentDirection = 360 - _currentDirection;
            }

            canvas.DrawCircle(_currentPosition.X, _currentPosition.Y, Radius, _dotPaint);
        }

        private float GetRandomDirection()
        {
            var randomGenerator = new Random();

            return randomGenerator.Next(0, 359);
        }

        private SKPoint GetNextPoint(double t)
        {
            var x = _currentPosition.X + Speed * Math.Cos(ConvertToRadians(_currentDirection)) * t;
            var y = _currentPosition.Y + Speed * Math.Sin(ConvertToRadians(_currentDirection)) * t;

            return new SKPoint((float)x, (float)y);
        }

        public double ConvertToRadians(double angle)
        {
            return (Math.PI / 180) * angle;
        }
    }
}

