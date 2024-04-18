using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using Button = System.Windows.Controls.Button;

namespace FreshGuidance
{

    [TemplatePart(Name = PartNextButton, Type = typeof(Button))]
    [TemplatePart(Name = PartSkipButton, Type = typeof(Button))]
    [TemplatePart(Name = PartMainBorder, Type = typeof(Border))]
    [TemplatePart(Name = PartLeftTriangle, Type = typeof(Path))]
    [TemplatePart(Name = PartTopTriangle, Type = typeof(Path))]
    [TemplatePart(Name = PartRightTriangle, Type = typeof(Path))]
    [TemplatePart(Name = PartBottomTriangle, Type = typeof(Path))]
    [TemplatePart(Name = PartCanvas, Type = typeof(TextBlock))]
    public class GuideTipHintControl: HintControlBase
    {
        private const string PartNextButton = "PART_Next_Button";
        private const string PartSkipButton = "PART_Skip_Button";
        private const string PartMainBorder = "PART_Main_Border";
        private const string PartLeftTriangle = "PART_Left_Triangle";
        private const string PartTopTriangle = "PART_Top_Triangle";
        private const string PartRightTriangle = "PART_Right_Triangle";
        private const string PartBottomTriangle = "PART_Bottom_Triangle";
        private const string PartCanvas = "PART_Canvas";
        private const int pathSize = 10;

        private Button _nextButton;
        private Button _skipButton;
        private Path _leftTriangle;
        private Path _topTriangle;
        private Path _rightTriangle;
        private Path _bottomTriangle;
        private Canvas _canvas;


        public static readonly DependencyProperty MainButtonContentProperty
            = DependencyProperty.Register("MainButtonContent",
                typeof(string),
                typeof(GuideTipHintControl),
                new PropertyMetadata("Understand", null));

        public string MainButtonContent
        {
            get => (string)GetValue(MainButtonContentProperty);
            set => SetValue(MainButtonContentProperty, value);
        }

        public static readonly DependencyProperty SecondButtonContentProperty
            = DependencyProperty.Register("SecondButtonContent",
                typeof(string),
                typeof(GuideTipHintControl),
                new PropertyMetadata("Skip", null));

        public string SecondButtonContent
        {
            get => (string)GetValue(SecondButtonContentProperty);
            set => SetValue(SecondButtonContentProperty, value);
        }

        public static readonly DependencyProperty BorderMarginProperty
            = DependencyProperty.Register("BorderMargin",
                typeof(Thickness),
                typeof(GuideTipHintControl),
                new PropertyMetadata(new Thickness(16,22,16,16), null));

        public Thickness BorderMargin
        {
            get => (Thickness)GetValue(BorderMarginProperty);
            set => SetValue(BorderMarginProperty, value);
        }

        static GuideTipHintControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GuideTipHintControl), 
                new FrameworkPropertyMetadata(typeof(GuideTipHintControl)));
        }

        public override void SetGuideHintControlPosition(FrameworkElement ownerContainer ,Point targetControlPoint)
        {

            double Left, Top;

            switch (Placement)
            {
                case GUIDE_HINT_CONTROL_PLACEMENT.LEFT:
                    Left = targetControlPoint.X - ActualWidth;
                    Top = targetControlPoint.Y - (ActualHeight - TargetControl.ActualHeight) / 2;
                    if (Top < 0) Top = 0;
                    if (Top + ActualHeight > ownerContainer.ActualHeight) Top = ownerContainer.ActualHeight - ActualHeight;
                    Canvas.SetLeft(this, Left);
                    Canvas.SetTop(this, Top);
                    Dispatcher.BeginInvoke(DispatcherPriority.Render, new Action(() =>
                    {
                        var targetPosOnCanvas1 = TargetControl.TranslatePoint(new Point(0, 0), _canvas);
                        Canvas.SetTop(_rightTriangle, targetPosOnCanvas1.Y + TargetControl.ActualHeight / 2 - pathSize / 2);
                        Canvas.SetLeft(_rightTriangle, ActualWidth - pathSize);
                    }));
                    break;

                case GUIDE_HINT_CONTROL_PLACEMENT.TOP:
                    Left = targetControlPoint.X - (ActualWidth - TargetControl.ActualWidth) / 2;
                    Top = targetControlPoint.Y - ActualHeight;
                    if (Left < 0) Left = 0;
                    if (Left + ActualWidth > ownerContainer.ActualWidth) Left = ownerContainer.ActualWidth - ActualWidth;
                    Canvas.SetLeft(this, Left);
                    Canvas.SetTop(this, Top);
                    Dispatcher.BeginInvoke(DispatcherPriority.Render, new Action(() =>
                    {
                        var targetPosOnCanvas2 = TargetControl.TranslatePoint(new Point(0, 0), _canvas);
                        Canvas.SetTop(_bottomTriangle, ActualHeight - pathSize);
                        Canvas.SetLeft(_bottomTriangle, targetPosOnCanvas2.X + TargetControl.ActualWidth / 2 - pathSize / 2);
                    }));
                    break;

                case GUIDE_HINT_CONTROL_PLACEMENT.RIGHT:
                    Left = targetControlPoint.X + TargetControl.ActualWidth;
                    Top = targetControlPoint.Y - (ActualHeight - TargetControl.ActualHeight) / 2;
                    if (Top < 0) Top = 0;
                    if (Top + ActualHeight > ownerContainer.ActualHeight) Top = ownerContainer.ActualHeight - ActualWidth;
                    Canvas.SetLeft(this, Left);
                    Canvas.SetTop(this, Top);
                    Dispatcher.BeginInvoke(DispatcherPriority.Render, new Action(() =>
                    {
                        var targetPosOnCanvas3 = TargetControl.TranslatePoint(new Point(0, 0), _canvas);
                        Canvas.SetTop(_leftTriangle, targetPosOnCanvas3.Y + TargetControl.ActualHeight / 2 - pathSize / 2);
                        Canvas.SetLeft(_leftTriangle, 0);
                    }));
                    break;

                case GUIDE_HINT_CONTROL_PLACEMENT.BOTTOM:
                    Left = targetControlPoint.X - (ActualWidth - TargetControl.ActualWidth) / 2;
                    Top = targetControlPoint.Y + TargetControl.ActualHeight;
                    if (Left < 0) Left = 0;
                    if (Left + ActualWidth > ownerContainer.ActualWidth) Left = ownerContainer.ActualWidth - ActualWidth;
                    Canvas.SetLeft(this, Left);
                    Canvas.SetTop(this, Top);
                    Dispatcher.BeginInvoke(DispatcherPriority.Render, new Action(() =>
                    {
                        var targetPosOnCanvas4 = TargetControl.TranslatePoint(new Point(0, 0), _canvas);
                        Canvas.SetTop(_topTriangle, 0);
                        Canvas.SetLeft(_topTriangle, targetPosOnCanvas4.X + TargetControl.ActualWidth / 2 - pathSize / 2);
                    }));
                    break;
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _nextButton = GetTemplateChild(PartNextButton) as Button;
            _skipButton = GetTemplateChild(PartSkipButton) as Button;
            _bottomTriangle = GetTemplateChild(PartBottomTriangle) as Path;
            _leftTriangle = GetTemplateChild(PartLeftTriangle) as Path;
            _topTriangle = GetTemplateChild(PartTopTriangle) as Path;
            _rightTriangle = GetTemplateChild(PartRightTriangle) as Path;
            _canvas = GetTemplateChild(PartCanvas) as Canvas;
            if (_nextButton != null)
            {
                _nextButton.Click += _nextButton_Click;
            }
            if(_skipButton != null) 
            {
                _skipButton.Click += _skipButton_Click;
            }
        }

        private void _skipButton_Click(object sender, RoutedEventArgs e)
        {
            OnPartSkipButtonPressed(this);
        }

        private void _nextButton_Click(object sender, RoutedEventArgs e)
        {
            OnPartNextButtonPressed(this);
        }
    }
}
