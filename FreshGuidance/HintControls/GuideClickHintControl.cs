using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace FreshGuidance
{
    internal struct RadialGradientParams
    {
        public double EllipseWidth;
        public double EllipseHeight;
        public double RadiusX;
        public double RadiusY;
        public double Offset;
    }


    [TemplatePart(Name = PartMainBorder, Type = typeof(Border))]
    [TemplatePart(Name = PartGlowEllipse, Type = typeof(Ellipse))]
    [TemplatePart(Name = PartCanvas, Type = typeof(Canvas))]
    [TemplatePart(Name = PartSkipButton, Type = typeof(Button))]
    [TemplatePart(Name = PartDoubleAnimation, Type = typeof(DoubleAnimation))]
    [TemplatePart(Name = PartRadialGradient, Type = typeof(RadialGradientBrush))]
    [TemplatePart(Name = PartGradientStop1, Type = typeof(GradientStop))]
    public class GuideClickHintControl : HintControlBase
    {
        private const string PartGlowEllipse = "PART_Glow_Ellipse";
        private const string PartMainBorder = "PART_Main_Border";
        private const string PartCanvas = "PART_Canvas";
        private const string PartSkipButton = "PART_Skip_Button";
        private const string PartDoubleAnimation = "PART_Double_Animation";
        private const string PartRadialGradient = "PART_Radial_Gradient";
        private const string PartGradientStop1 = "PART_Gradient_Stop1";

        private Ellipse _glowEllipse;
        private Border _mainBorder;
        private Canvas _canvas;
        private Button _skipButton;
        private DoubleAnimation _doubleAnimation;
        private RadialGradientBrush _radialGradientBrush;
        private GradientStop _gradientStop1;

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
            Size glowEllipseSize = new Size(TargetControl.ActualWidth + _glowEllipse.StrokeThickness * 2,
                                            TargetControl.ActualHeight + _glowEllipse.StrokeThickness * 2);
            RadialGradientParams radialGradientParams = GetRadialGradientParams(TargetControl.ActualWidth, TargetControl.ActualHeight);
            _doubleAnimation.From = radialGradientParams.Offset;
            _radialGradientBrush.RadiusX = radialGradientParams.RadiusX;
            _radialGradientBrush.RadiusY = radialGradientParams.RadiusY;
            _gradientStop1.Offset = radialGradientParams.Offset;

            switch (Placement)
            {
                case GUIDE_HINT_CONTROL_PLACEMENT.LEFT:

                    Width = mainBorderSize.Width + glowEllipseSize.Width;
                    Height = mainBorderSize.Height > glowEllipseSize.Height ? mainBorderSize.Height : glowEllipseSize.Height;
                    _glowEllipse.Width = glowEllipseSize.Width;
                    _glowEllipse.Height = glowEllipseSize.Height;
                    Left = targetControlPoint.X - mainBorderSize.Width;
                    Top = targetControlPoint.Y - (mainBorderSize.Height - TargetControl.ActualHeight) / 2;
                    if (Top < 0) Top = 0;
                    if (Top + mainBorderSize.Height > ownerContainer.ActualHeight) Top = ownerContainer.ActualHeight - mainBorderSize.Height;
                    Canvas.SetLeft(this, Left);
                    Canvas.SetTop(this, Top);
                    Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new Action(() =>
                    {
                        var targetPosOnCanvas1 = TargetControl.TranslatePoint(new Point(0, 0), _canvas);
                        Canvas.SetTop(_glowEllipse, targetPosOnCanvas1.Y - _glowEllipse.StrokeThickness);
                        Canvas.SetLeft(_glowEllipse, targetPosOnCanvas1.X - _glowEllipse.StrokeThickness);
                        Canvas.SetLeft(_mainBorder, 0);
                        Canvas.SetTop(_mainBorder, 0);
                    }));
                    break;

                case GUIDE_HINT_CONTROL_PLACEMENT.TOP:
                    Width = mainBorderSize.Width > glowEllipseSize.Width ? mainBorderSize.Width : glowEllipseSize.Width;
                    Height = mainBorderSize.Height + glowEllipseSize.Height;
                    _glowEllipse.Width = glowEllipseSize.Width;
                    _glowEllipse.Height = glowEllipseSize.Height;
                    Left = targetControlPoint.X - (mainBorderSize.Width - TargetControl.ActualWidth) / 2;
                    Top = targetControlPoint.Y - mainBorderSize.Height;
                    if (Left < 0) Left = 0;
                    if (Left + mainBorderSize.Width > ownerContainer.ActualWidth) Left = ownerContainer.ActualWidth - mainBorderSize.Width;
                    Canvas.SetLeft(this, Left);
                    Canvas.SetTop(this, Top);
                    Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new Action(() =>
                    {
                        var targetPosOnCanvas2 = TargetControl.TranslatePoint(new Point(0, 0), _canvas);
                        Canvas.SetTop(_glowEllipse, targetPosOnCanvas2.Y - _glowEllipse.StrokeThickness);
                        Canvas.SetLeft(_glowEllipse, targetPosOnCanvas2.X - _glowEllipse.StrokeThickness);
                        Canvas.SetLeft(_mainBorder, 0);
                        Canvas.SetTop(_mainBorder, 0);
                    }));
                    break;

                case GUIDE_HINT_CONTROL_PLACEMENT.RIGHT:
                    Width = mainBorderSize.Width + glowEllipseSize.Width;
                    Height = mainBorderSize.Height > glowEllipseSize.Height ? mainBorderSize.Height : glowEllipseSize.Height;
                    _glowEllipse.Width = glowEllipseSize.Width;
                    _glowEllipse.Height = glowEllipseSize.Height;
                    Left = targetControlPoint.X;
                    Top = targetControlPoint.Y - (mainBorderSize.Height - TargetControl.ActualHeight) / 2;
                    if (Top < 0) Top = 0;
                    if (Top + mainBorderSize.Height > ownerContainer.ActualHeight) Top = ownerContainer.ActualHeight - mainBorderSize.Height;
                    Canvas.SetLeft(this, Left);
                    Canvas.SetTop(this, Top);
                    Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new Action(() =>
                    {
                        var targetPosOnCanvas3 = TargetControl.TranslatePoint(new Point(0, 0), _canvas);
                        Canvas.SetTop(_glowEllipse, targetPosOnCanvas3.Y - _glowEllipse.StrokeThickness);
                        Canvas.SetLeft(_glowEllipse, targetPosOnCanvas3.X - _glowEllipse.StrokeThickness);
                        Canvas.SetLeft(_mainBorder, TargetControl.ActualWidth);
                        Canvas.SetTop(_mainBorder, 0);
                    }));
                    break;

                case GUIDE_HINT_CONTROL_PLACEMENT.BOTTOM:
                    Width = mainBorderSize.Width > glowEllipseSize.Width ? mainBorderSize.Width : glowEllipseSize.Width;
                    Height = mainBorderSize.Height + glowEllipseSize.Height;
                    _glowEllipse.Width = glowEllipseSize.Width;
                    _glowEllipse.Height = glowEllipseSize.Height;
                    Left = targetControlPoint.X - (mainBorderSize.Width - TargetControl.ActualWidth) / 2;
                    Top = targetControlPoint.Y;
                    if (Left < 0) Left = 0;
                    if (Left + mainBorderSize.Width > ownerContainer.ActualWidth) Left = ownerContainer.ActualWidth - mainBorderSize.Width;
                    Canvas.SetLeft(this, Left);
                    Canvas.SetTop(this, Top);
                    Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new Action(() =>
                    {
                        var targetPosOnCanvas4 = TargetControl.TranslatePoint(new Point(0, 0), _canvas);
                        Canvas.SetTop(_glowEllipse, targetPosOnCanvas4.Y - _glowEllipse.StrokeThickness);
                        Canvas.SetLeft(_glowEllipse, targetPosOnCanvas4.X - _glowEllipse.StrokeThickness);
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
            _doubleAnimation = GetTemplateChild(PartDoubleAnimation) as DoubleAnimation;
            _radialGradientBrush = GetTemplateChild(PartRadialGradient) as RadialGradientBrush;
            _gradientStop1 = GetTemplateChild(PartGradientStop1) as GradientStop;
            if (_skipButton != null)
            {
                _skipButton.Click += _skipButton_Click;
            }
        }

        private RadialGradientParams GetRadialGradientParams(double targetWidth, double targetHeight)
        {
            RadialGradientParams radialGradientParams = new RadialGradientParams();
            double strokeThickness = _glowEllipse.StrokeThickness;
            double ellipseWidth = targetWidth + strokeThickness * 2;
            double ellipseHeight = targetHeight + strokeThickness * 2;
            radialGradientParams.EllipseWidth = ellipseWidth;
            radialGradientParams.EllipseHeight = ellipseHeight;
            if (ellipseWidth > ellipseHeight)
            {
                radialGradientParams.RadiusX = 0.5;
                radialGradientParams.RadiusY = 0.5;
                radialGradientParams.Offset = 1 - strokeThickness / (ellipseHeight / 2);
            }
            else
            {
                radialGradientParams.RadiusX = 0.5;
                radialGradientParams.RadiusY = 0.5;
                radialGradientParams.Offset = 1 - strokeThickness / (ellipseWidth / 2);
            }
            return radialGradientParams;
        }

        private void _skipButton_Click(object sender, RoutedEventArgs e)
        {
            OnPartSkipButtonPressed(this);
        }
    }
}
