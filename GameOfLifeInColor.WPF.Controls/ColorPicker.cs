using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GameOfLifeInColor.Core.Models;


namespace GameOfLifeInColor.WPF.Controls
{
    [TemplatePart(Name = ColorFieldPart, Type = typeof(Canvas))]
    [TemplatePart(Name = ColorSelectorPart, Type = typeof(Canvas))]
    [TemplatePart(Name = ColorSliderPart, Type = typeof(BaseColorSlider))]
    [TemplatePart(Name = FirstTintStopPart, Type = typeof(GradientStop))]
    [TemplatePart(Name = FirstShadeStopPart, Type = typeof(GradientStop))]
    [TemplatePart(Name = TintShadeSliderPart, Type = typeof(TintShadeSlider))]
    public class ColorPicker : Control
    {
        private const string ColorFieldPart = "PART_ColorField";
        private const string ColorSelectorPart = "PART_ColorSelector";
        private const string ColorSliderPart = "PART_ColorSlider";
        private const string FirstTintStopPart = "PART_FirstTintGradientStop";
        private const string FirstShadeStopPart = "PART_FirstShadeGradientStop";
        private const string TintShadeSliderPart = "PART_TintShadeSlider";

        private Canvas _colorFieldElement;
        private Canvas _colorSelectorElement;
        private BaseColorSlider _colorSliderElement;
        private GradientStop _firstTintStopElement;
        private GradientStop _firstShadeStopElement;
        private TintShadeSlider _tintShadeSliderElement;

        public Canvas ColorFieldElement
        {
            get { return _colorFieldElement; }
            set
            {
                _colorFieldElement = value;
                InitializeColorFieldElement();
            }
        }

        public Canvas ColorSelectorElement
        {
            get { return _colorSelectorElement; }
            set { _colorSelectorElement = value; }
        }

        public BaseColorSlider ColorSliderElement
        {
            get { return _colorSliderElement; }
            set
            {
                _colorSliderElement = value;
                InitializeColorSliderElement();
            }
        }

        public GradientStop FirstShadeStopElement
        {
            get { return _firstShadeStopElement; }
            set
            {
                _firstShadeStopElement = value;
                InitializeFirstShadeStopElement();
            }
        }

        public GradientStop FirstTintStopElement
        {
            get { return _firstTintStopElement; }
            set
            {
                _firstTintStopElement = value;
                InitializeFirstTintStopElement();
            }
        }

        public TintShadeSlider TintShadeSliderElement
        {
            get { return _tintShadeSliderElement; }
            set
            {
                _tintShadeSliderElement = value;
                InitializeTintShadeSliderElement();
            }
        }



        public static readonly DependencyPropertyKey BaseColorPropertyKey =
            DependencyProperty.RegisterReadOnly("BaseColor", typeof(SolidColorBrush), typeof(ColorPicker),
                                                new PropertyMetadata(new SolidColorBrush(new ColorHSV(0, 1, 1).ToColor())));
        public static readonly DependencyProperty BaseColorProperty = BaseColorPropertyKey.DependencyProperty;

        public static readonly DependencyProperty ColorCanvasHeightProperty =
            DependencyProperty.Register("ColorCanvasHeight", typeof (double), typeof (ColorPicker),
                                        new PropertyMetadata(50D));

        public static readonly DependencyProperty CurrentColorBrushProperty =
            DependencyProperty.Register("CurrentColorBrush", typeof(SolidColorBrush), typeof(ColorPicker),
            new FrameworkPropertyMetadata(new SolidColorBrush(), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, 
                                          new PropertyChangedCallback(CurrentColorBrushChanged)));

        public static readonly DependencyProperty CurrentColorProperty =
            DependencyProperty.Register("CurrentColor", typeof(ColorHSV), typeof(ColorPicker),
                new FrameworkPropertyMetadata(new ColorHSV(0,0,1), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                                              new PropertyChangedCallback(CurrentColorChanged)));

        public static readonly new DependencyProperty WidthProperty =
            DependencyProperty.Register("Width", typeof(double), typeof(ColorPicker),
                                        new PropertyMetadata(100D));

        public static readonly DependencyPropertyKey TintAndShadePercentPropertyKey =
            DependencyProperty.RegisterReadOnly("TintAndShadePercent", typeof(int), typeof(ColorPicker),
                                                new FrameworkPropertyMetadata(50, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, null,
                                                new CoerceValueCallback(TintAndShadePercentCohersion)));
        public static readonly DependencyProperty TintAndShadePercentProperty = TintAndShadePercentPropertyKey.DependencyProperty;

        public static readonly DependencyProperty RProperty =
            DependencyProperty.Register("R", typeof(string), typeof(ColorPicker),
                                        new PropertyMetadata("255"));

        public static readonly DependencyProperty GProperty =
            DependencyProperty.Register("G", typeof(string), typeof(ColorPicker),
                                        new PropertyMetadata("255"));

        public static readonly DependencyProperty BProperty =
            DependencyProperty.Register("B", typeof(string), typeof(ColorPicker),
                                        new PropertyMetadata("255"));


        private static void CurrentColorBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var hsv = (e.NewValue as SolidColorBrush).Color.ToHSV();
            var currentColor = (ColorHSV)d.GetValue(CurrentColorProperty);

            if (currentColor != hsv) d.SetValue(CurrentColorProperty, hsv);
        }

        private static void CurrentColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var brush = ((ColorHSV)e.NewValue).ToBrush();
            var currentBrush = (SolidColorBrush)d.GetValue(CurrentColorBrushProperty);

            if (!currentBrush.Equals(brush)) d.SetValue(CurrentColorBrushProperty, brush);
        }

        private static object TintAndShadePercentCohersion(DependencyObject d, object baseValue)
        {
            if (!(baseValue is int)) return 100;

            var value = (int)baseValue;

            if (value > 100) return 100;
            if (value < 0) return 0;

            return value;
        }


        public SolidColorBrush BaseColor
        {
            get { return (SolidColorBrush)GetValue(BaseColorProperty); }
            set { SetValue(BaseColorPropertyKey, value); }
        }

        public Double ColorCanvasHeight
        {
            get { return (Double)GetValue(ColorCanvasHeightProperty); }
            set { SetValue(ColorCanvasHeightProperty, value); }
        }

        public ColorHSV CurrentColor
        {
            get { return (ColorHSV)GetValue(CurrentColorProperty); }
            set { SetValue(CurrentColorProperty, value); }
        }

        public SolidColorBrush CurrentColorBrush
        {
            get { return (SolidColorBrush)GetValue(CurrentColorBrushProperty); }
            set { SetValue(CurrentColorBrushProperty, value); }
        }

        public new Double Width
        {
            get { return (Double)GetValue(WidthProperty); }
            set { SetValue(WidthProperty, value); }
        }

        public int TintAndShadePercent
        {
            get { return (int)GetValue(TintAndShadePercentProperty); }
            set { SetValue(TintAndShadePercentPropertyKey, value); }
        }

        public string R
        {
            get { return (string)GetValue(RProperty); }
            set { SetValue(RProperty, value); }
        }

        public string G
        {
            get { return (string)GetValue(GProperty); }
            set { SetValue(GProperty, value); }
        }

        public string B
        {
            get { return (string)GetValue(BProperty); }
            set { SetValue(BProperty, value); }
        }



        private double ColorSelectorCenterOffSetX
        {
            get { return ColorSelectorElement.ActualWidth / 2; }
        }

        private double ColorSelectorCenterOffSetY
        {
            get { return ColorSelectorElement.ActualHeight / 2; }
        }

        private byte TintAndShadeOpacity
        {
            get { return Convert.ToByte(TintAndShadePercent * 2.55D); }
        }

        private double TintAndShadeOffset
        {
            get { return TintAndShadePercent / 100D; }
        }
       


        static ColorPicker()
        {
            DefaultStyleKeyProperty
                .OverrideMetadata(typeof(ColorPicker),
                new FrameworkPropertyMetadata(typeof(ColorPicker)));
        }

        public ColorPicker()
        {
            AddHandler(ColorPicker.LoadedEvent,
                       new RoutedEventHandler(ColorPicker_Loaded));            
        }



        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (ColorFieldElement == null) ColorFieldElement = GetTemplateChild(ColorFieldPart) as Canvas;
            if (ColorSelectorElement == null) ColorSelectorElement = GetTemplateChild(ColorSelectorPart) as Canvas;
            if (ColorSliderElement == null) ColorSliderElement = GetTemplateChild(ColorSliderPart) as BaseColorSlider;
            if (FirstShadeStopElement == null) FirstShadeStopElement = GetTemplateChild(FirstShadeStopPart) as GradientStop;
            if (FirstTintStopElement == null) FirstTintStopElement = GetTemplateChild(FirstTintStopPart) as GradientStop;
            if (TintShadeSliderElement == null) TintShadeSliderElement = GetTemplateChild(TintShadeSliderPart) as TintShadeSlider;
        }



        private void InitializeColorFieldElement()
        {
            if (ColorFieldElement == null) return;

            ColorFieldElement.AddHandler(Canvas.MouseLeftButtonDownEvent,
                                          new MouseButtonEventHandler(ColorField_LeftDown));
            ColorFieldElement.AddHandler(Canvas.MouseLeftButtonUpEvent,
                                          new MouseButtonEventHandler(ColorField_LeftUp));
            ColorFieldElement.AddHandler(Canvas.MouseMoveEvent,
                                          new MouseEventHandler(ColorField_MouseMove));
        }

        private void InitializeColorSliderElement()
        {
            if (ColorSliderElement == null) return;

            ColorSliderElement.AddHandler(BaseColorSlider.ValueChangedEvent,
                                          new RoutedPropertyChangedEventHandler<Double>(ColorSlider_ValueChanged));
        }

        private void InitializeFirstShadeStopElement()
        {
            if (FirstShadeStopElement == null) return;

            FirstShadeStopElement.Color = Color.FromArgb(TintAndShadeOpacity, 0, 0, 0);
        }

        private void InitializeFirstTintStopElement()
        {
            if (FirstTintStopElement == null) return;

            FirstTintStopElement.Color = Color.FromArgb(TintAndShadeOpacity, 255, 255, 255);
        }

        private void InitializeTintShadeSliderElement()
        {
            if (TintShadeSliderElement == null) return;

            TintShadeSliderElement.Value = TintAndShadePercent;

            TintShadeSliderElement.AddHandler(TintShadeSlider.ValueChangedEvent,
                                              new RoutedPropertyChangedEventHandler<Double>(TintShadeSlider_ValueChanged));
        }



        private void ColorField_LeftDown(object sender, MouseButtonEventArgs e)
        {
            var mousePosition = e.GetPosition(ColorFieldElement);
            MoveColorSelector(mousePosition);
            ColorFieldElement.CaptureMouse();
        }

        private void ColorField_LeftUp(object sender, MouseButtonEventArgs e)
        {
            ColorFieldElement.ReleaseMouseCapture();
        }      

        private void ColorField_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var mousePosition = e.GetPosition(ColorFieldElement);

                mousePosition.X = mousePosition.X.Clamp(0, ColorFieldElement.Width);
                mousePosition.Y = mousePosition.Y.Clamp(0, ColorFieldElement.Height);

                MoveColorSelector(mousePosition);
                CalculateColor(mousePosition);

                Mouse.Synchronize();
            }
        }


        private void ColorPicker_Loaded(object sender, RoutedEventArgs e)
        {
            CalculateInitialSettings();

            //ColorFieldElement.RemoveHandler(ColorPicker.LoadedEvent,
            //                                 new RoutedEventHandler(ColorPicker_Loaded));
        }


        private void ColorSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var baseColor = new ColorHSV(e.NewValue, 1, 1).ToColor();

            BaseColor = new SolidColorBrush(baseColor);
            CalculateColor();
        }


        private void TintShadeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (FirstTintStopElement == null || FirstShadeStopElement == null) return;

            TintAndShadePercent = (int)e.NewValue;
            FirstTintStopElement.Color = Color.FromArgb(TintAndShadeOpacity, 255, 255, 255);
            FirstShadeStopElement.Color = Color.FromArgb(TintAndShadeOpacity, 0, 0, 0);
            CalculateColor();
        }




        private void MoveColorSelector(Point newPosition)
        {
            if (ColorSelectorElement == null || ColorFieldElement == null) return;

            var adjustedX = (newPosition.X - ColorSelectorCenterOffSetX)
                            .Clamp(0, ColorFieldElement.Width, ColorSelectorCenterOffSetX);
            var adjustedY = (newPosition.Y - ColorSelectorCenterOffSetY)
                            .Clamp(0, ColorFieldElement.Height, ColorSelectorCenterOffSetY);

            ColorSelectorElement.SetValue(Canvas.LeftProperty, adjustedX);
            ColorSelectorElement.SetValue(Canvas.TopProperty, adjustedY);
        }

        private void CalculateColor()
        {
            if (ColorFieldElement == null || ColorSelectorElement == null) return;

            var selectorPoint = ColorSelectorElement.TranslatePoint(new Point(), ColorFieldElement);

            selectorPoint.X = selectorPoint.X + (ColorSelectorCenterOffSetX);
            selectorPoint.Y = selectorPoint.Y + (ColorSelectorCenterOffSetY);

            CalculateColor(selectorPoint);
        }

        private void CalculateColor(Point selectorPosition)
        {
            if (ColorFieldElement == null || ColorSliderElement == null) return;

            var saturation = (1 - TintAndShadeOffset) + (selectorPosition.X / ColorFieldElement.ActualWidth) * TintAndShadeOffset;
            var value = (1 - TintAndShadeOffset) + (1 - selectorPosition.Y / ColorFieldElement.ActualHeight) * TintAndShadeOffset;

            var color = new ColorHSV(ColorSliderElement.Value, saturation, value).ToColor();

            R = color.R.ToString();
            G = color.G.ToString();
            B = color.B.ToString();

            SetColor(color.ToHSV());
        }

        private void SetColor(ColorHSV color)
        {
            CurrentColor = color;
            CurrentColorBrush = color.ToBrush();
        }




        private void CalculateInitialSettings()
        {
            if (ColorFieldElement == null || ColorSliderElement == null || TintShadeSliderElement == null || ColorSelectorElement == null) return;

            if (CurrentColorBrush == null) CalculateColor();
            else
            {
                var hsv = CurrentColorBrush.Color.ToHSV();

                SetColorSlider(hsv);
                SetTintAndShadeSlider(hsv);

                var selectorPosition = GetColorSelectorPosition(hsv);
                MoveColorSelector(selectorPosition);

                CalculateColor(selectorPosition);
            }
        }

        private void SetColorSlider(ColorHSV hsv)
        {
            if (ColorSliderElement.Value != hsv.Hue)
            {
                ColorSliderElement.Value = hsv.Hue;
            }
        }

        private void SetTintAndShadeSlider(ColorHSV hsv)
        {
            
            var tintLevel = (1 - hsv.Saturation);
            var shadeLevel = (1 - hsv.Value);
            var newSliderValue = ( (tintLevel > shadeLevel) ? tintLevel : shadeLevel ) * 100;

            if (TintShadeSliderElement.Value < newSliderValue) TintShadeSliderElement.Value = newSliderValue;
        }

        private Point GetColorSelectorPosition(ColorHSV hsv)
        {
            var remainingPercent = 100D - TintAndShadePercent;

            var offsetedStepX = (ColorFieldElement.ActualWidth / TintAndShadePercent);
            var offsetedStepY = (ColorFieldElement.ActualHeight / TintAndShadePercent);

            var x = ((100 * hsv.Saturation) - remainingPercent) * offsetedStepX;
            var y = ((100 * (1 - hsv.Value))) * offsetedStepY;
            
            return new Point(x, y);
        }
    }
}