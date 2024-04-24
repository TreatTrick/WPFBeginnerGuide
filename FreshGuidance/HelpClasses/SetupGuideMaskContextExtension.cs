using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Markup;

namespace FreshGuidance
{
    public class SetupGuideMaskContextExtension : MarkupExtension
    {
        //[ConstructorArgument("guideMaskKey")]
        public string GuideMaskKey { get; set; }
        public int HintControlIndex { get; set; }
        public HintControlBase HintControl { get; set; }

        public string EventName { get; set; } = string.Empty;
        public SetupGuideMaskContextExtension(string guideMaskKey)
        {
            GuideMaskKey = guideMaskKey;
        }

        public SetupGuideMaskContextExtension()
        {
            
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var valueService = (IProvideValueTarget)serviceProvider.GetService(typeof(IProvideValueTarget));
            var targetObject = valueService.TargetObject;
            GuideMask guideMask = GuideMask.GuideMaskFactory(GuideMaskKey);
            if (guideMask.GuideHintControls.ContainsKey(HintControlIndex))
            {
                Console.WriteLine($"Warning: HintControlIndex {HintControlIndex} already exists.");
            }
            guideMask.GuideHintControls[HintControlIndex] = HintControl;
            HintControl.TargetControl = targetObject as FrameworkElement;
            HintControl.TargetControlEventName = EventName;
            return string.Empty;
        }
    }
}
