using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms.Composite.Helpers;
using Xamarin.Forms.Xaml;

namespace Xamarin.Forms.Composite.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CompositeSlider : TemplateView, ICompositeElementConfiguration
    {
        PanGestureRecognizer _panGesture;
        bool _initialPos;
        double _lastHeight = -1;
        double _lastWidth = -1;

        public event EventHandler<ValueChangedEventArgs> ValueChanged;

        public CompositeSlider()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty ValueProperty = BindableProperty.Create(nameof(Value), typeof(double), typeof(CompositeSlider), 0.0, BindingMode.TwoWay);

        public static readonly BindableProperty MaximumProperty = BindableProperty.Create(nameof(Maximum), typeof(double), typeof(CompositeSlider), 100.0, BindingMode.TwoWay);

        public static readonly BindableProperty MinimumProperty = BindableProperty.Create(nameof(Minimum), typeof(double), typeof(CompositeSlider), 0.0, BindingMode.TwoWay);

        public static readonly BindableProperty MinimumTrackColorProperty = BindableProperty.Create(nameof(MinimumTrackColor), typeof(Color), typeof(CompositeSlider), Color.Black);

        public static readonly BindableProperty MaximumTrackColorProperty = BindableProperty.Create(nameof(MaximumTrackColor), typeof(Color), typeof(CompositeSlider), Color.Black);

        public static readonly BindableProperty ThumbColorProperty = BindableProperty.Create(nameof(ThumbColor), typeof(Color), typeof(CompositeSlider), Color.Black);

        public double Value
        {
            get => (double)GetValue(ValueProperty);
            set
            {
                var oldVal = (double)GetValue(ValueProperty);

                if (Math.Abs(oldVal - value) > float.MinValue)
                    OnValueChanged(oldVal, value);

                SetValue(ValueProperty, value);
            }
        }

        public double Maximum
        {
            get => (double)GetValue(MaximumProperty);
            set => SetValue(MaximumProperty, value);
        }

        public double Minimum
        {
            get => (double)GetValue(MinimumProperty);
            set => SetValue(MinimumProperty, value);
        }

        public Color MinimumTrackColor
        {
            get => (Color)GetValue(MinimumTrackColorProperty);
            set => SetValue(MinimumTrackColorProperty, value);
        }

        public Color MaximumTrackColor
        {
            get => (Color)GetValue(MaximumTrackColorProperty);
            set => SetValue(MaximumTrackColorProperty, value);
        }

        public Color ThumbColor
        {
            get => (Color)GetValue(ThumbColorProperty);
            set => SetValue(ThumbColorProperty, value);
        }

        internal View Thumb { get; private set; }
        internal View MaximumTrack { get; private set; }
        internal View MinimumTrack { get; private set; }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ElementChanged(bool newElement)
        {
            if (newElement)
            {
                UpdateContent();
                UpdateValue();
                UpdateThumbColor();
                UpdateMinimumTrackColor();
                UpdateMaximumTrackColor();

                _panGesture = new PanGestureRecognizer();
                _panGesture.PanUpdated += OnPanUpdated;
                GestureRecognizers.Add(_panGesture);
            }
            else
            {
                _panGesture.PanUpdated -= OnPanUpdated;
                GestureRecognizers.Remove(_panGesture);
            }
        }

        protected void OnValueChanged(double oldValue, double newValue)
        {
            ValueChanged?.Invoke(this, new ValueChangedEventArgs(oldValue, newValue));
        }

        protected override void LayoutChildren(double x, double y, double width, double height)
        {
            base.LayoutChildren(x, y, width, height);

            if (Math.Abs(width * height) < float.MinValue || !(Value > 0) || _initialPos)
            {
                return;
            }

            UpdateThumbPosition();
            _initialPos = true;
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            if (Math.Abs(_lastWidth - width) < float.MinValue && Math.Abs(_lastHeight - height) < float.MinValue)
            {
                return;
            }

            _lastWidth = width;
            _lastHeight = height;

            UpdateThumbPosition();
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == nameof(Value))
                UpdateValue();
            else if (propertyName == nameof(ThumbColor))
                UpdateThumbColor();
            else if (propertyName == nameof(MinimumTrackColor))
                UpdateMinimumTrackColor();
            else if (propertyName == nameof(MaximumTrackColor))
                UpdateMaximumTrackColor();
        }

        void UpdateContent()
        {
            var template = ControlTemplate;

            if (template == null)
                return;

            if (!(template.CreateContent() is View content))
                return;

            Content = content;

            Thumb = VisualTreeHelper.GetTemplateChild<View>(this, "Thumb");
            MaximumTrack = VisualTreeHelper.GetTemplateChild<View>(this, "MaximumTrack");
            MinimumTrack = VisualTreeHelper.GetTemplateChild<View>(this, "MinimumTrack");
        }

        void UpdateValue()
        {
            UpdateThumbPosition();
        }

        void UpdateThumbColor()
        {
            if (Thumb != null && Thumb.BackgroundColor == Color.Default)
                Thumb.BackgroundColor = ThumbColor;
        }

        void UpdateMinimumTrackColor()
        {
            if (MinimumTrack != null && MinimumTrack.BackgroundColor == Color.Default)
                MinimumTrack.BackgroundColor = MinimumTrackColor;
        }

        void UpdateMaximumTrackColor()
        {
            if (MaximumTrack != null && MaximumTrack.BackgroundColor == Color.Default)
                MaximumTrack.BackgroundColor = ThumbColor;
        }

        void UpdateThumbPosition()
        {
            var percentage = Value / (Maximum - Minimum);

            if (Thumb != null)
                Thumb.TranslationX = percentage * (MaximumTrack != null ? MaximumTrack.Width : 0);

            if (MinimumTrack != null)
                MinimumTrack.WidthRequest = Thumb != null ? Thumb.TranslationX : 0;
        }

        void OnPanUpdated(object sender, PanUpdatedEventArgs e)
        {
            switch (e.StatusType)
            {
                case GestureStatus.Started:
                case GestureStatus.Running:
                    var percentage = (e.TotalX + MaximumTrack.Width / 2) * Maximum / MaximumTrack.Width;

                    if (percentage < Minimum)
                        percentage = Minimum;

                    if (percentage > Maximum)
                        percentage = Maximum;

                    Value = percentage;
                    break;
            }
        }
    }
}