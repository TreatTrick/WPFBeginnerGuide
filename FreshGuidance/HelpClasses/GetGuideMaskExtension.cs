using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;

namespace FreshGuidance
{
    public class GetGuideMaskExtension : MarkupExtension
    {
        [ConstructorArgument("guideMaskKey")]
        public string GuideMaskKey { get; set; }

        public System.Windows.Media.Brush GuideMaskBackGroundBrush { get; set; }
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var guideMask = GuideMask.GuideMaskFactory(GuideMaskKey);
            guideMask.Background = GuideMaskBackGroundBrush;
            return guideMask;
        }

        public GetGuideMaskExtension(string guideMaskKey)
        {
            GuideMaskKey = guideMaskKey;
        }

        public GetGuideMaskExtension()
        {
            
        }
    }
}
