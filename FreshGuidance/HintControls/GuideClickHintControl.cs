using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace FreshGuidance
{
    [TemplatePart(Name = PartMainBorder, Type = typeof(Border))]
    [TemplatePart(Name = PartGlowEllipse, Type = typeof(Ellipse))]
    [TemplatePart(Name = PartCanvas, Type = typeof(Canvas))]
    [TemplatePart(Name = PartSkipButton, Type = typeof(Button))]
    public class GuideClickHintControl : HintControlBase
    {
        private const string PartGlowEllipse = "PART_Glow_Ellipse";
        private const string PartMainBorder = "PART_Main_Border";
        private const string PartCanvas = "PART_Canvas";
        private const string PartSkipButton = "PART_Skip_Button";

        private Ellipse _glowEllipse;
        private Border _mainBorder;
        private Canvas _canvas;
        private Button _skipButton;

        static GuideClickHintControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GuideClickHintControl),
                new FrameworkPropertyMetadata(typeof(GuideClickHintControl)));
        }

        static public readonly DependencyProperty GlowColorProperty
            = DependencyProperty.Register(
                "GlowColor",
                typeof(Color),
                typeof(GuideClickHintControl),
                new PropertyMetadata(Colors.Blue, null));

        public Color GlowColor
        {
            get => (Color)GetValue(GlowColorProperty);
            set => SetValue(GlowColorProperty, value);
        }

        public static readonly DependencyProperty SecondButtonContentProperty
            = DependencyProperty.Register("SecondButtonContent",
                typeof(string),
                typeof(GuideClickHintControl),
                new PropertyMetadata("Skip", null));

        public string SecondButtonContent
        {
            get => (string)GetValue(SecondButtonContentProperty);
            set => SetValue(SecondButtonContentProperty, value);
        }

        public override void SetGuideHintControlPosition(FrameworkElement ownerContainer, Point targetControlPoint)
        {

            double Left, Top;

            Size mainBorderSize = _mainBorder.RenderSize;
            Size glowEllipseSize = new Size(TargetControl.ActualWidth + 100, TargetControl.ActualHeight + 100);

            switch (Placement)
            {
                case GUIDE_HINT_CONTROL_PLACEMENT.LEFT:

                    Width = mainBorderSize.Width + glowEllipseSize.Width;
                    Height = mainBorderSize.Height;
                    _glowEllipse.Width = glowEllipseSize.Width;
                    _glowEllipse.Height = glowEllipseSize.Height;
                    Left = targetControlPoint.X - mainBorderSize.Width;
                    Top = targetControlPoint.Y - (Height - TargetControl.ActualHeight) / 2;
                    if (Top < 0) Top = 0;
                    if (Top + Height > ownerContainer.ActualHeight) Top = ownerContainer.ActualHeight - Height;
                    Canvas.SetLeft(this, Left);
                    Canvas.SetTop(this, Top);
                    Dispatcher.BeginInvoke(DispatcherPriority.Render, new Action(() =>
                    {
                        var targetPosOnCanvas1 = TargetControl.TranslatePoint(new Point(0, 0), _canvas);
                        Canvas.SetTop(_glowEllipse, targetPosOnCanvas1.Y - 50);
                        Canvas.SetLeft(_glowEllipse, targetPosOnCanvas1.X - 50);
                        Canvas.SetLeft(_mainBorder, ActualWidth - TargetControl.ActualWidth - _mainBorder.ActualWidth);
                        Canvas.SetTop(_mainBorder, 0);
                    }));
                    break;

                case GUIDE_HINT_CONTROL_PLACEMENT.TOP:
                    Width = mainBorderSize.Width;
                    Height = mainBorderSize.Height + glowEllipseSize.Height;
                    _glowEllipse.Width = glowEllipseSize.Width;
                    _glowEllipse.Height = glowEllipseSize.Height;
                    Left = targetControlPoint.X - (Width - TargetControl.ActualWidth) / 2;
                    Top = targetControlPoint.Y - mainBorderSize.Height;
                    if (Left < 0) Left = 0;
                    if (Left + Width > ownerContainer.ActualWidth) Left = ownerContainer.ActualWidth - Width;
                    Canvas.SetLeft(this, Left);
                    Canvas.SetTop(this, Top);
                    Dispatcher.BeginInvoke(DispatcherPriority.Render, new Action(() =>
                    {
                        var targetPosOnCanvas2 = TargetControl.TranslatePoint(new Point(0, 0), _canvas);
                        Canvas.SetTop(_glowEllipse, targetPosOnCanvas2.Y - 50);
                        Canvas.SetLeft(_glowEllipse, targetPosOnCanvas2.X - 50);
                        Canvas.SetLeft(_mainBorder, 0);
                        Canvas.SetTop(_mainBorder, ActualHeight - TargetControl.ActualHeight - _mainBorder.ActualHeight);
                    }));
                    break;

                case GUIDE_HINT_CONTROL_PLACEMENT.RIGHT:
                    Width = mainBorderSize.Width + glowEllipseSize.Width;
                    Height = mainBorderSize.Height;
                    _glowEllipse.Width = glowEllipseSize.Width;
                    _glowEllipse.Height = glowEllipseSize.Height;
                    Left = targetControlPoint.X;
                    Top = targetControlPoint.Y - (Height - TargetControl.ActualHeight) / 2;
                    if (Top < 0) Top = 0;
                    if (Top + Height > ownerContainer.ActualHeight) Top = ownerContainer.ActualHeight - Height;
                    Canvas.SetLeft(this, Left);
                    Canvas.SetTop(this, Top);
                    Dispatcher.BeginInvoke(DispatcherPriority.Render, new Action(() =>
                    {
                        var targetPosOnCanvas3 = TargetControl.TranslatePoint(new Point(0, 0), _canvas);
                        Canvas.SetTop(_glowEllipse, targetPosOnCanvas3.Y - 50);
                        Canvas.SetLeft(_glowEllipse, targetPosOnCanvas3.X - 50);
                        Canvas.SetLeft(_mainBorder, TargetControl.ActualWidth);
                        Canvas.SetTop(_mainBorder, 0);
                    }));
                    break;

                case GUIDE_HINT_CONTROL_PLACEMENT.BOTTOM:
                    Width = mainBorderSize.Width;
                    Height = mainBorderSize.Height + glowEllipseSize.Height;
                    _glowEllipse.Width = glowEllipseSize.Width;
                    _glowEllipse.Height = glowEllipseSize.Height;
                    Left = targetControlPoint.X - (Width - TargetControl.ActualWidth) / 2;
                    Top = targetControlPoint.Y;
                    if (Left < 0) Left = 0;
                    if (Left + Width > ownerContainer.ActualWidth) Left = ownerContainer.ActualWidth - Width;
                    Canvas.SetLeft(this, Left);
                    Canvas.SetTop(this, Top);
                    Dispatcher.BeginInvoke(DispatcherPriority.Render, new Action(() =>
                    {
                        var targetPosOnCanvas4 = TargetControl.TranslatePoint(new Point(0, 0), _canvas);
                        Canvas.SetTop(_glowEllipse, targetPosOnCanvas4.Y - 50);
                        Canvas.SetLeft(_glowEllipse, targetPosOnCanvas4.X - 50);
                        Canvas.SetLeft(_mainBorder, 0);
                        Canvas.SetTop(_mainBorder, TargetControl.ActualHeight);
                    }));
                    break;
            }
        }

        public GuideClickHintControl() 
        {
            CanInvokeTargetControl = true;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _glowEllipse = GetTemplateChild(PartGlowEllipse) as Ellipse;
            _mainBorder = GetTemplateChild(PartMainBorder) as Border;
            _canvas = GetTemplateChild(PartCanvas) as Canvas;
            _skipButton = GetTemplateChild(PartSkipButton) as Button;
            if (_skipButton != null)
            {
                _skipButton.Click += _skipButton_Click;
            }
        }

        private void _skipButton_Click(object sender, RoutedEventArgs e)
        {
            OnPartSkipButtonPressed(this);
        }
    }
}
