using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GameOfLifeInColor.WPF.Controls
{
    [TemplatePart(Name = ColorDisplayPart, Type = typeof(Rectangle))]
    [TemplatePart(Name = TintDisplayPart, Type = typeof(Rectangle))]
    [TemplatePart(Name = ShadeDisplayPart, Type = typeof(Rectangle))]
    public class TintShadeSlider : Slider
    {
        public const string ColorDisplayPart = "PART_ColorDisplay";
        public const string TintDisplayPart = "PART_TintDisplay";
        public const string ShadeDisplayPart = "PART_ShadeDisplay";

        private Rectangle _colorDisplayElement;
        private Rectangle _tintDisplayElement;
        private Rectangle _shadeDisplayElement;

        public Rectangle ColorDisplayElement
        {
            get { return _colorDisplayElement; }
            set
            {
                _colorDisplayElement = value;
            }
        }

        public Rectangle TintDisplayElement
        {
            get { return _tintDisplayElement; }
            set
            {
                _tintDisplayElement = value;
                InitializeTintDisplayElement();
            }
        }

        public Rectangle ShadeDisplayElement
        {
            get { return _shadeDisplayElement; }
            set
            {
                _shadeDisplayElement = value;
                InitializeShadeDisplayElement();
            }
        }



        public static readonly new DependencyProperty HeightProperty =
            DependencyProperty.Register("Height", typeof(Double), typeof(TintShadeSlider));

        public static readonly DependencyProperty ThumbStrokeThicknessProperty =
            DependencyProperty.Register("ThumbStrokeThickness", typeof(double), typeof(TintShadeSlider),
            new PropertyMetadata(3.0D));

        public static readonly DependencyProperty ThumbWidthProperty =
            DependencyProperty.Register("ThumbWidth", typeof(Double), typeof(TintShadeSlider),
            new PropertyMetadata(10D));

        public new static readonly DependencyProperty WidthProperty =
            DependencyProperty.Register("Width", typeof (Double), typeof (TintShadeSlider));



        public new Double Height
        {
            get { return (Double)GetValue(HeightProperty); }
            set { SetValue(HeightProperty, value); }
        }

        public Double ThumbStrokeThickness
        {
            get { return (Double)GetValue(ThumbStrokeThicknessProperty); }
            set { SetValue(ThumbStrokeThicknessProperty, value); }
        }

        public Double ThumbWidth
        {
            get { return (Double)GetValue(ThumbWidthProperty); }
            set { SetValue(ThumbWidthProperty, value); }
        }

        public new Double Width
        {
            get { return (Double)GetValue(WidthProperty); }
            set { SetValue(WidthProperty, value); }
        }



        static TintShadeSlider()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TintShadeSlider), new FrameworkPropertyMetadata(typeof(TintShadeSlider)));
        }



        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (ColorDisplayElement == null) ColorDisplayElement = GetTemplateChild(ColorDisplayPart) as Rectangle;
            if (ShadeDisplayElement == null) ShadeDisplayElement = GetTemplateChild(ShadeDisplayPart) as Rectangle;
            if (TintDisplayElement == null) TintDisplayElement = GetTemplateChild(TintDisplayPart) as Rectangle;
        }


        private void InitializeTintDisplayElement()
        {
            if (TintDisplayElement == null) return;

            var tintBrush = InitialGradientBrush();
            tintBrush.GradientStops.Add(new GradientStop(Color.FromArgb(0, 255, 255, 255), 0));
            tintBrush.GradientStops.Add(new GradientStop(Color.FromRgb(255, 255, 255), 1));

            TintDisplayElement.Fill = tintBrush;
        }

        private void InitializeShadeDisplayElement()
        {
            if (ShadeDisplayElement == null) return;

            var shadeBrush = InitialGradientBrush();
            shadeBrush.GradientStops.Add(new GradientStop(Color.FromArgb(0, 0, 0, 0), 0));
            shadeBrush.GradientStops.Add(new GradientStop(Color.FromRgb(0, 0, 0), 1));

            ShadeDisplayElement.Fill = shadeBrush;
        }


        private LinearGradientBrush InitialGradientBrush()
        {
            var gradientBrush = new LinearGradientBrush();
            gradientBrush.StartPoint = new Point(0, 0.5);
            gradientBrush.EndPoint = new Point(1, 0.5);
            gradientBrush.ColorInterpolationMode = ColorInterpolationMode.SRgbLinearInterpolation;
            return gradientBrush;
        }
    }
}
