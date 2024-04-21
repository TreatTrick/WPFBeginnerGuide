using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace FreshGuidance
{
    [TemplatePart(Name = GuideMask.PartBorderBackground, Type = typeof(Border))]
    [TemplatePart(Name = GuideMask.PartTransparentBorder, Type = typeof(Border))]
    [TemplatePart(Name = GuideMask.PartCanvasHint, Type = typeof(Canvas))]
    public class GuideMask : Control
    {
        private const string PartBorderBackground = "PART_Border_Background";
        private const string PartTransparentBorder = "PART_Transparent_Border";
        private const string PartCanvasHint = "PART_Canvas_Hint";

        private int _collectionIndex = 0;
        private Border _borderBackground;
        private Border _transparentBorder;
        private Canvas _canvasHint;
        private PathGeometry _pathClipGeometry = new PathGeometry();

        public Dictionary<int, HintControlBase> GuideHintControls { get; set; } 
            = new Dictionary<int, HintControlBase>();

        static GuideMask()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GuideMask),
                new FrameworkPropertyMetadata(typeof(GuideMask)));
        }

        public GuideMask()
        {
            Visibility = Visibility.Hidden;
        }

        static public GuideMask GuideMaskFactory(string key)
        {
            if(key == null) throw new ArgumentNullException(nameof(key));
            if(GuideMasksCache.ContainsKey(key)) return GuideMasksCache[key];
            GuideMasksCache.Add(key, new GuideMask());
            return GuideMasksCache[key];
        }

        static private Dictionary<string, GuideMask> GuideMasksCache = new Dictionary<string, GuideMask>();

        public void Show()
        {
            Visibility = Visibility.Visible;
            _collectionIndex = 0;
            _canvasHint.Children?.Clear();
            foreach (var control in GuideHintControls.Values)
            {
                _canvasHint?.Children.Add(control);
            }
            Dispatcher.BeginInvoke(DispatcherPriority.Render, new Action(() =>
            {
                ShowNextGuide();
            }));
        }

        public void ShowNextGuide()
        {
            _canvasHint?.Children.Clear();
            if (_collectionIndex >= GuideHintControls.Count)
            {
                Visibility = Visibility.Collapsed;
                return;
            }
            int selectKey = GuideHintControls.Keys.ElementAt(_collectionIndex);
            HintControlBase hintControl = GuideHintControls[selectKey];
            if (hintControl == null) 
                return;
            hintControl.OnPartNextButtonPressed -= OnNextButtonPressed;
            hintControl.OnPartSkipButtonPressed -= OnSkipButtonPressed;
            hintControl.OnPartNextButtonPressed += OnNextButtonPressed;
            hintControl.OnPartSkipButtonPressed += OnSkipButtonPressed;
            _transparentBorder.Visibility = hintControl.CanInvokeTargetControl ? Visibility.Collapsed : Visibility.Visible;

            FrameworkElement targetControl = hintControl.TargetControl;
            BindEventHandlerToTargetControl(targetControl, hintControl.TargetControlEventName);
            

            Panel container = GetParentPanel();
            if (container == null)
            {
                return;   
            }

            _canvasHint?.Children.Add(hintControl);

            container.SizeChanged -= Container_SizeChanged;
            container.SizeChanged += Container_SizeChanged;

            Point point = targetControl.TransformToAncestor(container).Transform(new Point(0, 0)); 

            RectangleGeometry rg = new RectangleGeometry() { Rect = new Rect(0, 0, container.ActualWidth, container.ActualHeight) };
            CombineHint(rg, targetControl, point);
            hintControl.SetGuideHintControlPosition(this, point);
            _collectionIndex++;
        }

        private void OnSkipButtonPressed(HintControlBase hintControl)
        {
            Visibility = Visibility.Collapsed;
        }

        private void OnNextButtonPressed(HintControlBase hintControl)
        {
            ShowNextGuide();
        }

        private void Container_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (_collectionIndex - 1 >= GuideHintControls.Count 
                || _collectionIndex - 1 < 0)
            {
                return;
            }

            int selectKey = GuideHintControls.Keys.ElementAt(_collectionIndex - 1);
            HintControlBase hintControl = GuideHintControls[selectKey];
            if (hintControl == null)
                return;

            FrameworkElement targetControl = hintControl.TargetControl;

            Panel container = GetParentPanel();
            if (container == null)
            {
                return;
            }

            Point point = targetControl.TransformToAncestor(container).Transform(new Point(0, 0));

            RectangleGeometry rg = new RectangleGeometry() { Rect = new Rect(0, 0, container.ActualWidth, container.ActualHeight) };
            CombineHint(rg, targetControl, point);
            hintControl.SetGuideHintControlPosition(this, point);
        }

        private Panel GetParentPanel()
        {
            FrameworkElement parent = VisualTreeHelper.GetParent(this) as FrameworkElement;
            while (parent != null)
            {
                if (parent is Panel)
                {
                    return parent as Panel;
                }
                parent = VisualTreeHelper.GetParent(parent) as FrameworkElement;
            }
            return null;
        }

        private void CombineHint(RectangleGeometry rg, FrameworkElement targetControl, Point targetControlPoint)
        {
            _pathClipGeometry = Geometry.Combine(_pathClipGeometry, rg, GeometryCombineMode.Union, null);
            _borderBackground.Clip = _pathClipGeometry;

            RectangleGeometry rg1 = new RectangleGeometry()
            {
                RadiusX = 3,
                RadiusY = 3,
                Rect = new Rect(targetControlPoint.X, targetControlPoint.Y, targetControl.ActualWidth,
                    targetControl.ActualHeight)
            };
            _pathClipGeometry = Geometry.Combine(_pathClipGeometry, rg1, GeometryCombineMode.Exclude, null);

            _borderBackground.Clip = _pathClipGeometry;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _borderBackground = GetTemplateChild(PartBorderBackground) as Border;
            _canvasHint = GetTemplateChild(PartCanvasHint) as Canvas;
            _transparentBorder = GetTemplateChild(PartTransparentBorder) as Border;
            _collectionIndex = 0;
        }


        private void BindEventHandlerToTargetControl(FrameworkElement targetControl, string eventName)
        {
            if(string.IsNullOrEmpty(eventName))
            {
                return;
            }

            Type type = targetControl.GetType();

            BindingFlags bindingFlags = BindingFlags.Public 
                | BindingFlags.Static 
                | BindingFlags.FlattenHierarchy;

            eventName = eventName.Replace("Event", "");

            FieldInfo routedEventProperty = type.GetField(eventName + "Event",  bindingFlags);
            if (routedEventProperty != null)
            {
                RoutedEvent routedEvent = routedEventProperty.GetValue(null) as RoutedEvent;
                var action = GetType().GetMethod("InvokeEventArgsCommand", BindingFlags.NonPublic | BindingFlags.Instance);
                var del = Delegate.CreateDelegate(routedEvent.HandlerType, this, action);
                if (del == null)
                    throw new Exception($"The Event of {routedEvent.HandlerType.Name} in target control {type.Name} does not " +
                        $"match the delegate InvokeEventArgsCommand(object sender, EventArgs e)");
                targetControl.RemoveHandler(routedEvent, del);
                targetControl.AddHandler(routedEvent, del, true);
            }
        }

        private void InvokeEventArgsCommand(object sender, EventArgs e)
        {
            ShowNextGuide();
        }
    }
}
