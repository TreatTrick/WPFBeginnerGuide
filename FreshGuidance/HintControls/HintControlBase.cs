using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace FreshGuidance
{
    public enum GUIDE_HINT_CONTROL_PLACEMENT
    {
        LEFT, TOP, RIGHT, BOTTOM,
    }

    public abstract class HintControlBase : ContentControl
    {

        public Action<HintControlBase> OnPartSkipButtonPressed;
        public Action<HintControlBase> OnPartNextButtonPressed;

        public static readonly DependencyProperty TargetControlProperty
            = DependencyProperty.Register("TargetControl",
                typeof(FrameworkElement),
                typeof(HintControlBase),
                new PropertyMetadata(null, null));


        public FrameworkElement TargetControl
        {
            get => (FrameworkElement)GetValue(TargetControlProperty);
            set => SetValue(TargetControlProperty, value);
        }

        static readonly DependencyProperty PlacementProperty
                = DependencyProperty.Register("Placement",
                typeof(GUIDE_HINT_CONTROL_PLACEMENT),
                typeof(HintControlBase),
                new PropertyMetadata(GUIDE_HINT_CONTROL_PLACEMENT.BOTTOM, null));

        public GUIDE_HINT_CONTROL_PLACEMENT Placement
        {
            get => (GUIDE_HINT_CONTROL_PLACEMENT)GetValue(PlacementProperty);
            set => SetValue(PlacementProperty, value);
        }

        public static readonly DependencyProperty TargetControlEventNameProperty
                = DependencyProperty.Register("TargetControlEventName",
                typeof(string),
                typeof(HintControlBase),
                new PropertyMetadata(string.Empty, null));

        public string TargetControlEventName
        {
            get => (string)GetValue(TargetControlEventNameProperty);
            set => SetValue(TargetControlEventNameProperty, value);
        }

        public bool CanInvokeTargetControl { get; protected set; } = false;

        public abstract void SetGuideHintControlPosition(FrameworkElement ownerContainer, Point targetControlPoint);
    }
}
