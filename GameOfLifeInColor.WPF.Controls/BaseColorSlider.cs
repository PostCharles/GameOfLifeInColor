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
using GameOfLifeInColor.Core.Models;

namespace GameOfLifeInColor.WPF.Controls
{
    [TemplatePart(Name = ColorDisplayPart, Type = typeof(Rectangle))]
    public class BaseColorSlider : Slider
    {
        public const string ColorDisplayPart = "PART_ColorDisplay";

        private Rectangle _colorDisplayElement;

        public Rectangle ColorDisplayElement
        {
            get { return _colorDisplayElement; }
            set
            {
                _colorDisplayElement = value;
                InitializeColorDisplayElement();
            }
        }
        


        public static readonly new DependencyProperty HeightProperty =
            DependencyProperty.Register("Height", typeof(Double), typeof(BaseColorSlider));

        public static readonly DependencyProperty ThumbStrokeThicknessProperty =
            DependencyProperty.Register("ThumbStrokeThickness", typeof(double), typeof(BaseColorSlider),
            new PropertyMetadata(3.0D));

        public static readonly DependencyProperty ThumbWidthProperty =
            DependencyProperty.Register("ThumbWidth", typeof (Double), typeof (BaseColorSlider),
            new PropertyMetadata(10D));

        public new static readonly DependencyProperty WidthProperty =
            DependencyProperty.Register("Width", typeof (Double), typeof (BaseColorSlider));


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
            get { return (Double) GetValue(ThumbWidthProperty); }
            set { SetValue(ThumbWidthProperty, value); }
        }

        public new Double Width
        {
            get { return (Double)GetValue(WidthProperty); }
            set { SetValue(WidthProperty, value); }
        }


        static BaseColorSlider()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BaseColorSlider), new FrameworkPropertyMetadata(typeof(BaseColorSlider)));
        }


        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (ColorDisplayElement == null) ColorDisplayElement = GetTemplateChild(ColorDisplayPart) as Rectangle;
        }


        private void InitializeColorDisplayElement()
        {
            if (ColorDisplayElement == null) return;
            
            FillBaseColorSlider();
        }

        private void FillBaseColorSlider()
        {
            var SpectrumBrush = new LinearGradientBrush();
            SpectrumBrush.StartPoint = new Point(0, 0.5);
            SpectrumBrush.EndPoint = new Point(1, 0.5);
            SpectrumBrush.ColorInterpolationMode = ColorInterpolationMode.SRgbLinearInterpolation;

            int steps = 30;
            int increment = 360/steps;

            for (int i = 0; i <= steps; i++)
            {
                var currentPosition = i*increment;
                var stop = (double) i * 1/steps;

                var color = new ColorHSV(currentPosition, 1, 1).ToColor();
                SpectrumBrush.GradientStops.Add(new GradientStop(color, stop));
            }
            
            ColorDisplayElement.Fill = SpectrumBrush;
        }
    }
}
