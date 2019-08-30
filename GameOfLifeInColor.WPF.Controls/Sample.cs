using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace GameOfLifeInColor.WPF.Controls
{
    internal class Sample
    {
        [TemplatePart(Name = PART_DropDownButton, Type = typeof (ToggleButton))]
        [TemplatePart(Name = PART_ContentPresenter, Type = typeof (ContentPresenter))]
        [TemplatePart(Name = PART_Popup, Type = typeof (Popup))]
        public class DropDownButton : ContentControl, ICommandSource
        {
            const string PART_DropDownButton = "PART_DropDownButton";
            const string PART_ContentPresenter = "PART_ContentPresenter";
            const string PART_Popup = "PART_Popup";

            #region Members 

            ContentPresenter _contentPresenter;
            Popup _popup;

            #endregion

            #region Constructors

            static DropDownButton()
            {
                DefaultStyleKeyProperty.OverrideMetadata(typeof (DropDownButton),
                    new FrameworkPropertyMetadata(typeof (DropDownButton)));
            }

            public DropDownButton()
            {
                Keyboard.AddKeyDownHandler(this, OnKeyDown);
                Mouse.AddPreviewMouseDownOutsideCapturedElementHandler(this, OnMouseDownOutsideCapturedElement);
            }

            #endregion //Constructors

            #region Properties

            System.Windows.Controls.Primitives.ButtonBase _button;

            protected System.Windows.Controls.Primitives.ButtonBase Button
            {
                get
                {
                    return _button;
                }
                set
                {
                    if (_button != null)
                        _button.Click -= DropDownButton_Click;

                    _button = value;

                    if (_button != null)
                        _button.Click += DropDownButton_Click;
                }
            }

            #region DropDownContent

            public static readonly DependencyProperty DropDownContentProperty =
                DependencyProperty.Register("DropDownContent", typeof (object), typeof (DropDownButton),
                    new UIPropertyMetadata(null, OnDropDownContentChanged));

            public object DropDownContent
            {
                get
                {
                    return (object) GetValue(DropDownContentProperty);
                }
                set
                {
                    SetValue(DropDownContentProperty, value);
                }
            }

            static void OnDropDownContentChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
            {
                DropDownButton dropDownButton = o as DropDownButton;
                if (dropDownButton != null)
                    dropDownButton.OnDropDownContentChanged((object) e.OldValue, (object) e.NewValue);
            }

            protected virtual void OnDropDownContentChanged(object oldValue, object newValue)
            {
                // TODO: Add your property changed side-effects. Descendants can override as well.
            }

            #endregion //DropDownContent

            #region IsOpen

            public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register("IsOpen",
                typeof (bool), typeof (DropDownButton), new UIPropertyMetadata(false, OnIsOpenChanged));

            public bool IsOpen
            {
                get
                {
                    return (bool) GetValue(IsOpenProperty);
                }
                set
                {
                    SetValue(IsOpenProperty, value);
                }
            }

            static void OnIsOpenChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
            {
                DropDownButton dropDownButton = o as DropDownButton;
                if (dropDownButton != null)
                    dropDownButton.OnIsOpenChanged((bool) e.OldValue, (bool) e.NewValue);
            }

            protected virtual void OnIsOpenChanged(bool oldValue, bool newValue)
            {
                if (newValue)
                    RaiseRoutedEvent(DropDownButton.OpenedEvent);
                else
                    RaiseRoutedEvent(DropDownButton.ClosedEvent);
            }

            #endregion //IsOpen

            #endregion //Properties

            #region Base Class Overrides

            public override void OnApplyTemplate()
            {
                base.OnApplyTemplate();
                Button = GetTemplateChild(PART_DropDownButton) as ToggleButton;

                _contentPresenter = GetTemplateChild(PART_ContentPresenter) as ContentPresenter;

                if (_popup != null)
                    _popup.Opened -= Popup_Opened;

                _popup = GetTemplateChild(PART_Popup) as Popup;

                if (_popup != null)
                    _popup.Opened += Popup_Opened;
            }

            #endregion //Base Class Overrides

            #region Events

            public static readonly RoutedEvent ClickEvent = EventManager.RegisterRoutedEvent("Click",
                RoutingStrategy.Bubble, typeof (RoutedEventHandler), typeof (DropDownButton));

            public event RoutedEventHandler Click
            {
                add
                {
                    AddHandler(ClickEvent, value);
                }
                remove
                {
                    RemoveHandler(ClickEvent, value);
                }
            }

            public static readonly RoutedEvent OpenedEvent = EventManager.RegisterRoutedEvent("Opened",
                RoutingStrategy.Bubble, typeof (RoutedEventHandler), typeof (DropDownButton));

            public event RoutedEventHandler Opened
            {
                add
                {
                    AddHandler(OpenedEvent, value);
                }
                remove
                {
                    RemoveHandler(OpenedEvent, value);
                }
            }

            public static readonly RoutedEvent ClosedEvent = EventManager.RegisterRoutedEvent("Closed",
                RoutingStrategy.Bubble, typeof (RoutedEventHandler), typeof (DropDownButton));

            public event RoutedEventHandler Closed
            {
                add
                {
                    AddHandler(ClosedEvent, value);
                }
                remove
                {
                    RemoveHandler(ClosedEvent, value);
                }
            }

            #endregion //Events

            #region Event Handlers

            void OnKeyDown(object sender, KeyEventArgs e)
            {
                if (!IsOpen)
                {
                    if (KeyboardUtilities.IsKeyModifyingPopupState(e))
                    {
                        IsOpen = true;
                        // ContentPresenter items will get focus in Popup_Opened().
                        e.Handled = true;
                    }
                }
                else
                {
                    if (KeyboardUtilities.IsKeyModifyingPopupState(e))
                    {
                        CloseDropDown(true);
                        e.Handled = true;
                    }
                    else if (e.Key == Key.Escape)
                    {
                        CloseDropDown(true);
                        e.Handled = true;
                    }
                }
            }

            void OnMouseDownOutsideCapturedElement(object sender, MouseButtonEventArgs e)
            {
                CloseDropDown(false);
            }

            void DropDownButton_Click(object sender, RoutedEventArgs e)
            {
                OnClick();
            }

            void CanExecuteChanged(object sender, EventArgs e)
            {
                CanExecuteChanged();
            }

            void Popup_Opened(object sender, EventArgs e)
            {
                // Set the focus on the content of the ContentPresenter.
                if (_contentPresenter != null)
                {
                    _contentPresenter.MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
                }
            }

            #endregion //Event Handlers

            #region Methods

            void CanExecuteChanged()
            {
                if (Command != null)
                {
                    RoutedCommand command = Command as RoutedCommand;

                    // If a RoutedCommand.
                    if (command != null)
                        IsEnabled = command.CanExecute(CommandParameter, CommandTarget) ? true : false;
                        // If a not RoutedCommand.
                    else
                        IsEnabled = Command.CanExecute(CommandParameter) ? true : false;
                }
            }


            void CloseDropDown(bool isFocusOnButton)
            {
                if (IsOpen)
                    IsOpen = false;
                ReleaseMouseCapture();

                if (isFocusOnButton)
                    Button.Focus();
            }

            protected virtual void OnClick()
            {
                RaiseRoutedEvent(DropDownButton.ClickEvent);
                RaiseCommand();
            }

            void RaiseRoutedEvent(RoutedEvent routedEvent)
            {
                RoutedEventArgs args = new RoutedEventArgs(routedEvent, this);
                RaiseEvent(args);
            }

            /// <summary>
            /// Raises the command's Execute event.
            /// </summary>
            void RaiseCommand()
            {
                if (Command != null)
                {
                    RoutedCommand routedCommand = Command as RoutedCommand;

                    if (routedCommand == null)
                        ((ICommand) Command).Execute(CommandParameter);
                    else
                        routedCommand.Execute(CommandParameter, CommandTarget);
                }
            }

            /// <summary>
            /// Unhooks a command from the Command property.
            /// </summary>
            /// <param name="oldCommand">The old command.</param>
            /// <param name="newCommand">The new command.</param>
            void UnhookCommand(ICommand oldCommand, ICommand newCommand)
            {
                EventHandler handler = CanExecuteChanged;
                oldCommand.CanExecuteChanged -= handler;
            }

            /// <summary>
            /// Hooks up a command to the CanExecuteChnaged event handler.
            /// </summary>
            /// <param name="oldCommand">The old command.</param>
            /// <param name="newCommand">The new command.</param>
            void HookUpCommand(ICommand oldCommand, ICommand newCommand)
            {
                EventHandler handler = new EventHandler(CanExecuteChanged);
                canExecuteChangedHandler = handler;
                if (newCommand != null)
                    newCommand.CanExecuteChanged += canExecuteChangedHandler;
            }

            #endregion //Methods

            #region ICommandSource Members

            // Keeps a copy of the CanExecuteChnaged handler so it doesn't get garbage collected.
            EventHandler canExecuteChangedHandler;

            #region Command

            public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command",
                typeof (ICommand), typeof (DropDownButton), new PropertyMetadata((ICommand) null, OnCommandChanged));

            [TypeConverter(typeof (CommandConverter))]
            public ICommand Command
            {
                get
                {
                    return (ICommand) GetValue(CommandProperty);
                }
                set
                {
                    SetValue(CommandProperty, value);
                }
            }

            static void OnCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
            {
                DropDownButton dropDownButton = d as DropDownButton;
                if (dropDownButton != null)
                    dropDownButton.OnCommandChanged((ICommand) e.OldValue, (ICommand) e.NewValue);
            }

            protected virtual void OnCommandChanged(ICommand oldValue, ICommand newValue)
            {
                // If old command is not null, then we need to remove the handlers.
                if (oldValue != null)
                    UnhookCommand(oldValue, newValue);

                HookUpCommand(oldValue, newValue);

                CanExecuteChanged(); //may need to call this when changing the command parameter or target.
            }

            #endregion //Command

            public static readonly DependencyProperty CommandParameterProperty =
                DependencyProperty.Register("CommandParameter", typeof (object), typeof (DropDownButton),
                    new PropertyMetadata(null));

            public object CommandParameter
            {
                get
                {
                    return GetValue(CommandParameterProperty);
                }
                set
                {
                    SetValue(CommandParameterProperty, value);
                }
            }

            public static readonly DependencyProperty CommandTargetProperty =
                DependencyProperty.Register("CommandTarget", typeof (IInputElement), typeof (DropDownButton),
                    new PropertyMetadata(null));

            public IInputElement CommandTarget
            {
                get
                {
                    return (IInputElement) GetValue(CommandTargetProperty);
                }
                set
                {
                    SetValue(CommandTargetProperty, value);
                }
            }

            #endregion //ICommandSource Members
        }
    }
}
