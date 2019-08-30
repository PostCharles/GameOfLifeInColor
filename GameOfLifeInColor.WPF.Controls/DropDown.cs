using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;


namespace GameOfLifeInColor.WPF.Controls
{
    [TemplatePart(Name = DisplayPresenterPart, Type = typeof(ContentPresenter))]
    [TemplatePart(Name = PopUpPresenterPart, Type = typeof(ContentPresenter))]
    [TemplatePart(Name = PopUpContainerPart, Type = typeof(Border))]
    [TemplatePart(Name = PopUpPart, Type = typeof(Popup))]
    public class DropDown : ContentControl
    {
        private const string DisplayPresenterPart = "PART_DisplayPresenter";
        private const string PopUpPresenterPart = "PART_PopUpPresenter";
        private const string PopUpContainerPart = "PART_PopUpContainer";
        private const string PopUpPart = "PART_PopUp";

        private delegate Rect CalculatePlacementRectangle(Point target, Size popUpSize, Size displaySize);

        private IDictionary<DropDownPlacement, CalculatePlacementRectangle> _delegateDictionary;
        private ContentPresenter _displayPresenterElement;
        private ContentPresenter _popUpPresenterElement;
        private Border _popUpContainerElement;
        private Popup _popUpElement;       
        private Window _parentWindow;


        public ContentPresenter DisplayPresenterElement
        {
            get { return _displayPresenterElement; }
            set
            {
                _displayPresenterElement = value;
                InitializeDisplayPresenterElement();
            }
        }
        
        public Window ParentWindow
        {
            get { return _parentWindow; }
            set
            {
                _parentWindow = value;
                InitializeParentWindow();
            }
        }

        public ContentPresenter PopUpPresenterElement
        {
            get { return _popUpPresenterElement; }
            set
            {
                _popUpPresenterElement = value;
                InitializePopUpPresenterElement();
            }
        }

        public Border PopUpContainerElement
        {
            get { return _popUpContainerElement; }
            set { _popUpContainerElement = value; }
        }
        
        public Popup PopUpElement
        {
            get { return _popUpElement; }
            set
            {
                _popUpElement = value;
                InitializePopUpElement();
            }
        }



        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof (CornerRadius), typeof (DropDown));

        public static readonly DependencyProperty DisplayContentProperty =
            DependencyProperty.Register("DisplayContent", typeof(object), typeof(DropDown));

        public static readonly DependencyProperty DropDownPlacementProperty =
            DependencyProperty.Register("DropDownPlacement", typeof (DropDownPlacement), typeof (DropDown),
                                        new PropertyMetadata(DropDownPlacement.BottomCenter));

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius) GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }
        
        public object DisplayContent
        {
            get { return GetValue(DisplayContentProperty); }
            set { SetValue(DisplayContentProperty, value); }
        }

        public DropDownPlacement DropDownPlacement
        {
            get { return (DropDownPlacement)GetValue(DropDownPlacementProperty); }
            set { SetValue(DropDownPlacementProperty, value); }
        }

        
        private CalculatePlacementRectangle CalculateRectangle { get; set; }


        static DropDown()
        {
            DefaultStyleKeyProperty
                .OverrideMetadata(typeof(DropDown),
                new FrameworkPropertyMetadata(typeof(DropDown)));
        }

        public DropDown()
        {
            Mouse.AddPreviewMouseDownOutsideCapturedElementHandler(this, CapturedMouse_Click);
            Mouse.AddPreviewMouseMoveHandler(this, CapturedMouse_Move);

            Keyboard.AddKeyDownHandler(this, CapturedKeyboard_KeyDown);

            AddHandler(DropDown.MouseLeaveEvent,
                       new RoutedEventHandler(PopUp_MouseLeave));

            InitializeDelegateDictionary();
        }



        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (DisplayPresenterElement == null) DisplayPresenterElement = GetTemplateChild(DisplayPresenterPart) as ContentPresenter;
            if (PopUpElement == null) PopUpElement = GetTemplateChild(PopUpPart) as Popup;
            if (PopUpPresenterElement == null) PopUpPresenterElement = GetTemplateChild(PopUpPresenterPart) as ContentPresenter;
            if (PopUpContainerElement == null) PopUpContainerElement = GetTemplateChild(PopUpContainerPart) as Border;
            
            if (ParentWindow == null) ParentWindow = Window.GetWindow(this);
        }



        
        private void InitializeDisplayPresenterElement()
        {
            if (DisplayPresenterElement == null) return;

            DisplayPresenterElement.AddHandler(ContentPresenter.MouseLeftButtonDownEvent,
                                           new RoutedEventHandler(DisplayPresenter_LeftDown));
        }

        private void InitializeParentWindow()
        {
            if (ParentWindow == null) return;

            ParentWindow.LayoutUpdated += Window_LayoutUpdated;

        }

        private void InitializePopUpElement()
        {
            if (PopUpElement == null) return;

            PopUpElement.AddHandler(Popup.MouseLeaveEvent,
                                    new RoutedEventHandler(PopUp_MouseLeave));

            PopUpElement.Placement = PlacementMode.AbsolutePoint;

            CalculateRectangle = _delegateDictionary[DropDownPlacement];
        }

        private void InitializePopUpPresenterElement()
        {
            PopUpPresenterElement.AddHandler(ContentPresenter.LoadedEvent,
                new RoutedEventHandler(PopUpPresenter_Loaded));
        }

        private void InitializeDelegateDictionary()
        {
            _delegateDictionary = new Dictionary<DropDownPlacement, CalculatePlacementRectangle>
            {
                {DropDownPlacement.Left, CalculateLeft},
                {DropDownPlacement.BottomLeft, CalculateBottomLeft},
                {DropDownPlacement.BottomLeftSwingRight, CalculateBottomLeftSwingRight},
                {DropDownPlacement.BottomCenter, CalculateBottomCenter},
                {DropDownPlacement.BottomRightSwingLeft, CalculateBottomRightSwingLeft},
                {DropDownPlacement.BottomRight, CalculateBottomRight},
                {DropDownPlacement.Right, CalculateRight}          
            };
        }




        private void CapturedKeyboard_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.SystemKey == Key.LeftAlt || e.SystemKey == Key.RightAlt || e.Key == Key.LWin || e.Key == Key.RWin)
            {
                if (PopUpElement.IsOpen) PopUpElement.IsOpen = false;
            }
        }


        private void CapturedMouse_Click(object sender, MouseButtonEventArgs e)
        {
            if (!IsWithin(PopUpElement.Child, e) && !IsWithin(DisplayPresenterElement, e))
            {
                PopUpElement.IsOpen = false;
            }

            ReleaseMouseCapture();
        }

        private void CapturedMouse_Move(object sender, MouseEventArgs e)
        {
            if (IsMouseCaptured && IsWithin(PopUpElement.Child, e)) ReleaseMouseCapture();
        }


        private void DisplayPresenter_LeftDown(object sender, RoutedEventArgs e)
        {
            if (PopUpElement.IsOpen) PopUpElement.IsOpen = false;
            else
            {
                PopUpElement.IsOpen = true;
                Focus();
            }
        }

        private void PopUp_MouseLeave(object sender, RoutedEventArgs e)
        {
            if (!IsMouseCaptured && PopUpElement.IsOpen)
            {
                if (IsChildPopUp())
                {
                    PopUpElement.IsOpen = false;
                }
                else CaptureMouse();
            }
        }

        private void PopUpPresenter_Loaded(object sender, RoutedEventArgs e)
        {
            CalculatePopUpPosition();
        }

        private void Window_LayoutUpdated(object sender, EventArgs e)
        {
            CalculatePopUpPosition();
        }




        private void CalculatePopUpPosition()
        {
            if (PopUpElement == null || ! PopUpElement.IsOpen || PopUpContainerElement == null || DisplayPresenterElement == null) return;

            var displayPosition = DisplayPresenterElement.PointToScreen(new Point(0, 0));
            var popUpSize = new Size(PopUpContainerElement.ActualWidth, PopUpPresenterElement.ActualHeight);
            var displaySize = new Size(DisplayPresenterElement.ActualWidth, DisplayPresenterElement.ActualHeight);

            if (CalculateRectangle != null) PopUpElement.PlacementRectangle = CalculateRectangle(displayPosition, popUpSize, displaySize);
        }

        private Rect CalculateLeft(Point target, Size popUpSize, Size displaySize)
        {
            var x = target.X;
            var y = target.Y;
            var position = new Point(x, y);

            return new Rect(position, position);
        }

        private Rect CalculateBottomLeft(Point target, Size popUpSize, Size displaySize)
        {
            var x = target.X;
            var y = target.Y + displaySize.Height;
            var position = new Point(x, y);

            return new Rect(position, position);
        }

        private Rect CalculateBottomLeftSwingRight(Point target, Size popUpSize, Size displaySize)
        {
            var x = target.X ;
            var y = target.Y + displaySize.Height;
            var position = new Point(x, y);

            return new Rect(position, position);
        }

        private Rect CalculateBottomCenter(Point target, Size popUpSize, Size displaySize)
        {
            var centerOffset = (displaySize.Width - popUpSize.Width)/2D;
            
            var x = target.X + centerOffset;
            var y = target.Y + displaySize.Height;
            var position = new Point(x, y);

            return new Rect(position, position);
        }

        private Rect CalculateBottomRightSwingLeft(Point target, Size popUpSize, Size displaySize)
        {
            var x = target.X + displaySize.Width;
            var y = target.Y + displaySize.Height;
            var position = new Point(x, y);

            return new Rect(position, position);
        }

        private Rect CalculateBottomRight(Point target, Size popUpSize, Size displaySize)
        {
            var x = target.X + displaySize.Width + popUpSize.Width;
            var y = target.Y + displaySize.Height;
            var position = new Point(x, y);

            return new Rect(position, position);
        }

        private Rect CalculateRight(Point target, Size popUpSize, Size displaySize)
        {
            var x = target.X + displaySize.Width + popUpSize.Width;
            var y = target.Y;
            var position = new Point(x, y);

            return new Rect(position, position);
        }



        private bool IsChildPopUp()
        {
            FrameworkElement control = this;

            while (control != null)
            {
                control = control.Parent as FrameworkElement;
                
                if (control != null && control.GetType() == typeof (DropDown)) return true;
            }

            return false;
        }

        private bool IsWithin(UIElement baseElement, MouseEventArgs e)
        {
            var element = baseElement as FrameworkElement;

            if (element == null) return false;

            var bounds = new Rect(0, 0, element.ActualWidth, element.ActualHeight);
            var mousePosition = e.GetPosition(element);

            return bounds.Contains(mousePosition);
        }
    }

    public enum DropDownPlacement
    {
        Left,
        BottomLeft,
        BottomLeftSwingRight,
        BottomCenter,
        BottomRightSwingLeft,
        BottomRight,
        Right
    }
}