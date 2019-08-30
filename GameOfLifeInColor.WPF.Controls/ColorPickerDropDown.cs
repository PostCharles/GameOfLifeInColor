using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    [TemplatePart(Name = ColorPickerPart, Type = typeof(Canvas))]
    [TemplatePart(Name = ColorSelectorPart, Type = typeof(Canvas))]
    [TemplatePart(Name = ColorSliderPart, Type = typeof(BaseColorSlider))]
    [TemplatePart(Name = DisplayColorPart, Type = typeof(Rectangle))]
    [TemplatePart(Name = DropDownPart, Type = typeof(Popup))]
    [TemplatePart(Name = FirstTintStopPart, Type = typeof(GradientStop))]
    [TemplatePart(Name = FirstShadeStopPart, Type = typeof(GradientStop))]
    [TemplatePart(Name = TintShadeSliderPart, Type = typeof(TintShadeSlider))]
    public class ColorPickerDropDown : Control
    {
        private const string ColorPickerPart = "PART_ColorPicker";
        private const string ColorSelectorPart = "PART_ColorSelector";
        private const string ColorSliderPart = "PART_ColorSlider";
        private const string DisplayColorPart = "PART_DisplayColor";
        private const string DropDownPart = "PART_DropDown";
        private const string FirstTintStopPart = "PART_FirstTintGradientStop";
        private const string FirstShadeStopPart = "PART_FirstShadeGradientStop";
        private const string TintShadeSliderPart = "PART_TintShadeSlider";

        private Canvas _colorPickerElement;
        private Canvas _colorSelectorElement;
        private BaseColorSlider _colorSliderElement;
        private Rectangle _displayColorElement;
        private Popup _dropDownElement;
        private GradientStop _firstTintStopElement;
        private GradientStop _firstShadeStopElement;
        private TintShadeSlider _tintShadeSliderElement;

        public Canvas ColorPickerElement
        {
            get { return _colorPickerElement; }
            set
            {
                _colorPickerElement = value;
                InitializeColorPickerElement();
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

        public Rectangle DisplayColorElement
        {
            get { return _displayColorElement; }
            set
            {
                _displayColorElement = value;
                InitializeDisplayColorElement();
            }
        }

        public Popup DropDownElement
        {
            get { return _dropDownElement; }
            set
            {
                _dropDownElement = value;
                InitializeDropDownElement();
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
            DependencyProperty.RegisterReadOnly("BaseColor", typeof(SolidColorBrush), typeof(ColorPickerDropDown),
                                                new PropertyMetadata(new SolidColorBrush(new ColorHSV(0, 1, 1).ToColor())));
        public static readonly DependencyProperty BaseColorProperty = BaseColorPropertyKey.DependencyProperty;

        public static readonly DependencyProperty CurrentColorBrushProperty =
            DependencyProperty.Register("CurrentColorBrush", typeof(SolidColorBrush), typeof(ColorPickerDropDown),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty CurrentColorProperty =
            DependencyProperty.Register("CurrentColor", typeof(ColorHSV), typeof(ColorPickerDropDown),
                new FrameworkPropertyMetadata(new ColorHSV(), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty DropDownWidthProperty =
            DependencyProperty.Register("DropDownWidth", typeof(double), typeof(ColorPickerDropDown),
                                        new PropertyMetadata(100D));

        public static readonly DependencyPropertyKey TintAndShadePercentPropertyKey =
            DependencyProperty.RegisterReadOnly("TintAndShadePercent", typeof(int), typeof(ColorPickerDropDown),
                                                new FrameworkPropertyMetadata(50, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, null,
                                                new CoerceValueCallback(TintAndShadePercentCohersion)));
        public static readonly DependencyProperty TintAndShadePercentProperty = TintAndShadePercentPropertyKey.DependencyProperty;

        public static readonly DependencyProperty RProperty =
            DependencyProperty.Register("R", typeof(string), typeof(ColorPickerDropDown),
                                        new PropertyMetadata("255"));

        public static readonly DependencyProperty GProperty =
            DependencyProperty.Register("G", typeof(string), typeof(ColorPickerDropDown),
                                        new PropertyMetadata("255"));

        public static readonly DependencyProperty BProperty =
            DependencyProperty.Register("B", typeof(string), typeof(ColorPickerDropDown),
                                        new PropertyMetadata("255"));


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

        public Double DropDownWidth
        {
            get { return (Double)GetValue(DropDownWidthProperty); }
            set { SetValue(DropDownWidthProperty, value); }
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



        private byte TintAndShadeOpacity
        {
            get { return Convert.ToByte(TintAndShadePercent * 2.55D); }
        }



        static ColorPickerDropDown()
        {
            DefaultStyleKeyProperty
                .OverrideMetadata(typeof(ColorPickerDropDown),
                new FrameworkPropertyMetadata(typeof(ColorPickerDropDown)));
        }

        public ColorPickerDropDown()
        {
            Mouse.AddPreviewMouseDownOutsideCapturedElementHandler(this, OutsideControl_Click);
            Keyboard.AddKeyDownHandler(this, OutsideControl_KeyDown);

            AddHandler(ColorPickerDropDown.MouseLeaveEvent,
                       new RoutedEventHandler(DropDown_MouseLeave));
        }




        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (ColorPickerElement == null) ColorPickerElement = GetTemplateChild(ColorPickerPart) as Canvas;
            if (ColorSelectorElement == null) ColorSelectorElement = GetTemplateChild(ColorSelectorPart) as Canvas;
            if (ColorSliderElement == null) ColorSliderElement = GetTemplateChild(ColorSliderPart) as BaseColorSlider;
            if (DisplayColorElement == null) DisplayColorElement = GetTemplateChild(DisplayColorPart) as Rectangle;
            if (DropDownElement == null) DropDownElement = GetTemplateChild(DropDownPart) as Popup;
            if (FirstShadeStopElement == null) FirstShadeStopElement = GetTemplateChild(FirstShadeStopPart) as GradientStop;
            if (FirstTintStopElement == null) FirstTintStopElement = GetTemplateChild(FirstTintStopPart) as GradientStop;
            if (TintShadeSliderElement == null) TintShadeSliderElement = GetTemplateChild(TintShadeSliderPart) as TintShadeSlider;

            CalculateInitalColor();
        }



        private void InitializeColorPickerElement()
        {
            if (ColorPickerElement == null) return;

            ColorPickerElement.AddHandler(Canvas.MouseLeftButtonDownEvent,
                                          new MouseButtonEventHandler(ColorPicker_LeftDown));
            ColorPickerElement.AddHandler(Canvas.MouseLeftButtonUpEvent,
                                          new MouseButtonEventHandler(ColorPicker_LeftUp));
            ColorPickerElement.AddHandler(Canvas.MouseMoveEvent,
                                          new MouseEventHandler(ColorPicker_MouseMove));
        }

        private void InitializeColorSliderElement()
        {
            if (ColorSliderElement == null) return;

            ColorSliderElement.AddHandler(BaseColorSlider.ValueChangedEvent,
                                          new RoutedPropertyChangedEventHandler<Double>(ColorSlider_ValueChanged));
        }

        private void InitializeDisplayColorElement()
        {
            if (DisplayColorElement == null) return;

            DisplayColorElement.AddHandler(Rectangle.MouseLeftButtonDownEvent,
                                           new RoutedEventHandler(DisplayColor_LeftDown));
        }

        private void InitializeDropDownElement()
        {
            if (DropDownElement == null) return;

            DropDownElement.AddHandler(Popup.MouseLeaveEvent,
                                       new RoutedEventHandler(DropDown_MouseLeave));

            DropDownElement.LayoutUpdated += DropDown_LayoutUpdated;
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



        private void ColorPicker_LeftDown(object sender, MouseButtonEventArgs e)
        {
            var mousePosition = e.GetPosition(ColorPickerElement);
            MoveColorSelector(mousePosition);
            ColorPickerElement.CaptureMouse();
        }

        private void ColorPicker_LeftUp(object sender, MouseButtonEventArgs e)
        {
            ColorPickerElement.ReleaseMouseCapture();
        }

        private void ColorPicker_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var mousePosition = e.GetPosition(ColorPickerElement);

                mousePosition.X = mousePosition.X.Clamp(0, ColorPickerElement.Width);
                mousePosition.Y = mousePosition.Y.Clamp(0, ColorPickerElement.Height);

                MoveColorSelector(mousePosition);
                CalculateColor(mousePosition);

                Mouse.Synchronize();
            }
        }


        private void ColorSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var baseColor = new ColorHSV(e.NewValue, 1, 1).ToColor();

            BaseColor = new SolidColorBrush(baseColor);
            CalculateColor();
        }


        private void DisplayColor_LeftDown(object sender, RoutedEventArgs e)
        {
            if (DropDownElement.IsOpen) DropDownElement.IsOpen = false;
            else
            {
                DropDownElement.IsOpen = true;
                Focus();
            }
        }


        private void DropDown_LayoutUpdated(object sender, EventArgs e)
        {
            if (DisplayColorElement == null) return;

            var centerOffset = (DisplayColorElement.ActualWidth - DropDownElement.Child.RenderSize.Width) / 2D;

            DropDownElement.HorizontalOffset = centerOffset;
        }

        private void DropDown_MouseLeave(object sender, RoutedEventArgs e)
        {
            if (!IsMouseCaptured && DropDownElement.IsOpen) CaptureMouse();
        }


        private void OutsideControl_Click(object sender, MouseButtonEventArgs e)
        {
            if (!IsWithin(DropDownElement.Child, e) && !IsWithin(DisplayColorElement, e))
            {
                DropDownElement.IsOpen = false;
            }

            ReleaseMouseCapture();
        }

        private void OutsideControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (DropDownElement.IsOpen) DropDownElement.IsOpen = false;
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
            if (ColorSelectorElement == null || ColorPickerElement == null) return;

            var centerOffSetX = (ColorSelectorElement.ActualWidth / 2.0D);
            var centerOffSetY = (ColorSelectorElement.ActualHeight / 2.0D);

            var adjustedX = (newPosition.X - centerOffSetX)
                            .Clamp(0, ColorPickerElement.Width, centerOffSetX);
            var adjustedY = (newPosition.Y - centerOffSetY)
                            .Clamp(0, ColorPickerElement.Height, centerOffSetY);

            ColorSelectorElement.SetValue(Canvas.LeftProperty, adjustedX);
            ColorSelectorElement.SetValue(Canvas.TopProperty, adjustedY);
        }

        private void CalculateColor()
        {
            if (ColorPickerElement == null || ColorSelectorElement == null) return;

            var selectorPoint = ColorSelectorElement.TranslatePoint(new Point(), ColorPickerElement);

            selectorPoint.X = selectorPoint.X + (ColorSelectorElement.Width / 2);
            selectorPoint.Y = selectorPoint.Y + (ColorSelectorElement.Height / 2);

            CalculateColor(selectorPoint);
        }

        private void CalculateColor(Point selectorPosition)
        {
            if (ColorPickerElement == null || ColorSliderElement == null) return;

            var offset = TintAndShadePercent / 100D;

            var saturation = (1 - offset) + (selectorPosition.X / ColorPickerElement.ActualWidth) * offset;
            var value = (1 - offset) + (1 - selectorPosition.Y / ColorPickerElement.ActualHeight) * offset;

            var color = new ColorHSV(ColorSliderElement.Value, saturation, value).ToColor();

            R = color.R.ToString();
            G = color.G.ToString();
            B = color.B.ToString();

            SetColor(color.ToHSV());
        }

        private void CalculateInitalColor()
        {
            var saturation = 1 - (TintAndShadePercent / 100D);

            SetColor(new ColorHSV(0, saturation, 1));
        }

        private void SetColor(ColorHSV color)
        {
            CurrentColor = color;
            CurrentColorBrush = color.ToBrush();
        }


        private bool IsWithin(UIElement baseElement, MouseButtonEventArgs e)
        {
            var element = baseElement as FrameworkElement;

            if (element == null) return false;

            var bounds = new Rect(0, 0, element.ActualWidth, element.ActualHeight);
            var mousePosition = e.GetPosition(element);

            return bounds.Contains(mousePosition);
        }
    }
}