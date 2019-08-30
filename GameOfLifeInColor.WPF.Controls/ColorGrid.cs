using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Threading;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using System.Xml;
using GameOfLifeInColor.Core.Models;
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
    public class ColorGrid : FrameworkElement
    {
        

        private IList<Visual> _visualList;
        private DrawingVisual _gridVisual;
        private DrawingVisual _cellsVisual;
        private DrawingVisual _cursorVisual;


        private double BorderOffset
        {
            get { return BorderThickness / 2D; }
        }

        private double CellTotalSideLength
        {
            get { return (CellSquareSize + GridLineThickness); }
        }

        private double GridLineOffset
        {
            get { return GridLineThickness / 2D; }
        }



        public static readonly DependencyProperty BackgroundProperty =
            DependencyProperty.Register("Background", typeof(Brush), typeof(ColorGrid),
                                         new FrameworkPropertyMetadata(Brushes.White, FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty BorderBrushProperty =
            DependencyProperty.Register("BorderBrush", typeof (Brush), typeof (ColorGrid), 
                                         new FrameworkPropertyMetadata(Brushes.White,FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty BorderThicknessProperty =
            DependencyProperty.Register("BorderThickness", typeof (double), typeof (ColorGrid),
                                        new FrameworkPropertyMetadata(5D, FrameworkPropertyMetadataOptions.AffectsMeasure));

        public static readonly DependencyProperty CellsProperty =
            DependencyProperty.Register("Cells", typeof (CellCollection), typeof (ColorGrid),
                                        new FrameworkPropertyMetadata(new CellCollection(0,0), FrameworkPropertyMetadataOptions.AffectsMeasure,
                                        new PropertyChangedCallback(OnCellsProperyChanged)));

        public static readonly DependencyProperty CellSquareSizeProperty =
            DependencyProperty.Register("CellSquareSize", typeof(Double), typeof(ColorGrid), 
                                        new FrameworkPropertyMetadata(25D, FrameworkPropertyMetadataOptions.AffectsMeasure));

        public static readonly DependencyProperty ColumnsProperty =
            DependencyProperty.Register("Columns", typeof (int), typeof (ColorGrid),
                                        new FrameworkPropertyMetadata(5, FrameworkPropertyMetadataOptions.AffectsMeasure,
                                        new PropertyChangedCallback(OnColumnsPropertyChanged)));

        public static readonly DependencyProperty CurrentColorProperty =
            DependencyProperty.Register("CurrentColor", typeof (ColorHSV), typeof (ColorGrid),
                                        new PropertyMetadata(new ColorHSV(0,0,1)));

        public static readonly DependencyProperty GridLineThicknessProperty =
            DependencyProperty.Register("GridLineThickness", typeof (double), typeof (ColorGrid),
                                        new FrameworkPropertyMetadata(2.5D, FrameworkPropertyMetadataOptions.AffectsMeasure));

        public static readonly DependencyProperty GridLineBrushProperty =
            DependencyProperty.Register("GridLineBrush", typeof (Brush), typeof (ColorGrid),
                                        new FrameworkPropertyMetadata(Brushes.Black, FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty IsPaintingEnabledProperty =
            DependencyProperty.Register("IsPaintingEnabled", typeof(bool), typeof(ColorGrid),
                                        new PropertyMetadata(true));

        public static readonly DependencyProperty RowsProperty =
            DependencyProperty.Register("Rows", typeof (int), typeof (ColorGrid),
                                        new FrameworkPropertyMetadata(5, FrameworkPropertyMetadataOptions.AffectsMeasure,
                                        new PropertyChangedCallback(OnRowsPropertyChanged)));


        private static void OnCellsProperyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var cells = (CellCollection)e.NewValue;

            if (cells != null)
            {
                var columns = (int)d.GetValue(ColumnsProperty);
                var rows = (int)d.GetValue(RowsProperty);

                if (cells.Columns != columns || cells.Rows != rows) cells.Resize(columns,rows);

            }
        }

        private static void OnColumnsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var columns = (int)e.NewValue;
            var cells = (CellCollection)d.GetValue(CellsProperty);
            
            if (columns != cells.Columns) cells.ResizeColumns(columns);
        }

        private static void OnRowsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var rows = (int)e.NewValue;
            var cells = (CellCollection)d.GetValue(CellsProperty);

            if (rows != cells.Rows) cells.ResizeRows(rows);

        }


        public Brush Background
        {
            get { return (Brush)GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }

        public Brush BorderBrush
        {
            get { return (Brush)GetValue(BorderBrushProperty); }
            set { SetValue(BorderBrushProperty, value); }
        }

        public double BorderThickness
        {
            get { return (double)GetValue(BorderThicknessProperty); }
            set { SetValue(BorderThicknessProperty, value); }
        }

        public CellCollection Cells
        {
            get { return (CellCollection)GetValue(CellsProperty); }
            set { SetValue(CellsProperty, value); }
        }

        public Double CellSquareSize
        {
            get { return (Double) GetValue(CellSquareSizeProperty); }
            set { SetValue(CellSquareSizeProperty, value); }
        }

        public int Columns
        {
            get { return (int)GetValue(ColumnsProperty); }
            set { SetValue(ColumnsProperty, value); }
        }

        public ColorHSV CurrentColor
        {
            get { return (ColorHSV)GetValue(CurrentColorProperty); }
            set { SetValue(CurrentColorProperty, value); }
        }

        public Brush GridLineBrush
        {
            get { return (Brush) GetValue(GridLineBrushProperty); }
            set { SetValue(GridLineBrushProperty, value); }
        }

        public double GridLineThickness
        {
            get { return (double)GetValue(GridLineThicknessProperty); }
            set { SetValue(GridLineThicknessProperty, value); }
        }

        public bool IsPaintingEnabled
        {
            get { return (bool)GetValue(IsPaintingEnabledProperty); }
            set { SetValue(IsPaintingEnabledProperty, value); }
        }

        public int Rows
        {
            get { return (int)GetValue(RowsProperty); }
            set { SetValue(RowsProperty, value); }
        }


        static ColorGrid()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ColorGrid), new FrameworkPropertyMetadata(typeof(ColorGrid)));
        }

        public ColorGrid()
        {
            Cells = new CellCollection(Columns, Rows);
            
            _visualList = new List<Visual>();

            _gridVisual = new DrawingVisual();
            _cellsVisual = new DrawingVisual();
            _cursorVisual = new DrawingVisual();

            AddVisual(_gridVisual);
            AddVisual(_cellsVisual);
            AddVisual(_cursorVisual);            

            AddHandler(Control.MouseLeftButtonDownEvent,
                       new MouseButtonEventHandler(ColorGrid_LeftMouseDown));
            AddHandler(Control.MouseLeftButtonUpEvent,
                       new MouseButtonEventHandler(ColorGrid_LeftMouseUp));
            AddHandler(Control.MouseEnterEvent,
                       new MouseEventHandler(ColorGrid_MouseEnter));
            AddHandler(Control.MouseLeaveEvent,
                       new MouseEventHandler(ColorGrid_MouseLeave));
            AddHandler(Control.MouseMoveEvent,
                       new MouseEventHandler(ColorGrid_MouseMove));
        }



        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();            

            DrawCells();
            DrawGrid();
        }

        protected override int VisualChildrenCount
        {
            get { return _visualList.Count; }
        }

        protected override Visual GetVisualChild(int index)
        {
            return _visualList[index];
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            if (Cells == null) return base.MeasureOverride(availableSize);

            return new Size( GetTotalWidth(), GetTotalHeight() );
        }
        
        protected override void OnRender(DrawingContext drawingContext)
        {            
            DrawGrid();
            DrawCells();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            
            DrawGrid();
            DrawCells();
        }

        public override void EndInit()
        {
            base.EndInit();
            Cells.CollectionUpdated += (s, e) => DrawCells();
        }


        private void ColorGrid_LeftMouseDown(object sender, MouseButtonEventArgs e)
        {
            //if (IsPaintingEnabled) PaintCell(e.GetPosition(this));
        }

        private void ColorGrid_LeftMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (IsMouseCaptureWithin) ReleaseMouseCapture();
        }

        private void ColorGrid_MouseEnter(object sender, MouseEventArgs e)
        {
            if (!IsPaintingEnabled) return;
            
            Mouse.OverrideCursor = Cursors.None;
            AddVisual(_cursorVisual);
        }
       
        private void ColorGrid_MouseLeave(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = null;
            RemoveVisual(_cursorVisual);
        }

        private async void ColorGrid_MouseMove(object sender, MouseEventArgs e)
        {
            if (!IsPaintingEnabled) return;            
            var pos = Mouse.GetPosition(this);
            
            DrawCursor(pos);
            if (e.LeftButton == MouseButtonState.Pressed) PaintCell(pos);
        }


        private void DrawCells()
        {
            if (Cells == null) return;

            using (var dc = _cellsVisual.RenderOpen())
            {
                for (int x = 0; x < Cells.Columns; x++)
                {
                    for (int y = 0; y < Cells.Rows; y++)
                    {
                        if (! Cells[x, y].IsAlive) continue;

                        var top = BorderOffset + (CellTotalSideLength * y);
                        var left = BorderOffset + (CellTotalSideLength * x);

                        var rect = new Rect(left, top, CellSquareSize, CellSquareSize);
                        dc.DrawRectangle(Cells[x,y].Color.ToBrush(), null, rect);
                    }
                }
            }
        }

        private void DrawCursor(Point pos)
        {            
            using (var dc = _cursorVisual.RenderOpen())
            {
                dc.PushClip(new RectangleGeometry(_gridVisual.ContentBounds));
                dc.DrawRoundedRectangle(null, new Pen(Brushes.White, 2), new Rect(pos.X - 7.5, pos.Y - 7.5, 15, 15), 2, 2);
                dc.DrawRoundedRectangle(null, new Pen(Brushes.Black, 2), new Rect(pos.X - 9.5, pos.Y - 9.5, 19, 19), 2, 2);                
                dc.Pop();
                
            }
        }

        private void DrawGrid()
        {
            if (Cells == null) return;

            using (var dc = _gridVisual.RenderOpen())
            {
                var grid = new Rect(new Size(GetTotalWidth(), GetTotalHeight()));
                dc.DrawRectangle(Background, new Pen(BorderBrush, BorderThickness), grid);

                if (GridLineThickness > 0)
                {
                    DrawHorizontalLines(dc);
                    DrawVerticalLines(dc);
                }
            }
        }

        private void DrawHorizontalLines(DrawingContext dc)
        {
            var currentHeight = 0D;

            for (int y = 0; y < Cells.Rows - 1; y++)
            {
                if (y == 0) currentHeight = CellSquareSize + BorderOffset + GridLineOffset;
                else currentHeight += CellTotalSideLength;

                dc.DrawLine(new Pen(GridLineBrush, GridLineThickness),
                    new Point(BorderOffset, currentHeight),
                    new Point((GetTotalWidth() - BorderOffset), currentHeight));
            }
        }

        private void DrawVerticalLines(DrawingContext dc)
        {
            var currentheight = 0D;

            for (int x = 0; x < Cells.Columns - 1; x++)
            {
                if (x == 0) currentheight = CellSquareSize + BorderOffset + GridLineOffset;
                else currentheight += CellTotalSideLength;

                dc.DrawLine(new Pen(GridLineBrush, GridLineThickness),
                    new Point(currentheight, BorderOffset),
                    new Point(currentheight, (GetTotalHeight() - BorderOffset)));
            }
        }


        private void PaintCell(Point p)
        {
            var border = BorderOffset;
            var cellLength = CellTotalSideLength;
            
            var x = (p.X - border)/cellLength;
            var y = (p.Y - border)/cellLength;

            var column = Convert.ToInt32(Math.Floor(x).Clamp(0, Cells.Columns - 1));
            var row = Convert.ToInt32(Math.Floor(y).Clamp(0, Cells.Rows - 1));

            Cells[column, row] = new Cell(CurrentColor);
            Cells.OnCollectionUpdated();
        }


        private void AddVisual(Visual child)
        {
            if (_visualList.Contains(child)) return;

            AddLogicalChild(child);
            AddVisualChild(child);
            _visualList.Add(child);
        }

        private void RemoveVisual(Visual child)
        {
            if (! _visualList.Contains(child)) return;

            RemoveLogicalChild(child);
            RemoveVisualChild(child);
            _visualList.Remove(child);
        }


        private double GetTotalWidth()
        {
            var columns = (double)Columns;

            var width = (BorderThickness) +
                        (CellSquareSize * columns) +
                        (GridLineThickness * (columns-1));

            return width;
        }

        private double GetTotalHeight()
        {
            var rows = (double)Rows;

            var height = (BorderThickness) +
                         (CellSquareSize*rows) +
                         (GridLineThickness*(rows-1));

            return height;
        }
    }
}
