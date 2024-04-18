using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FreshGuidance
{
    public class GuideMaskHelper : DependencyObject
    {
        public static readonly DependencyProperty HelpBindTargetControlProperty =
        DependencyProperty.RegisterAttached(
          "HelpBindTargetControl",
          typeof(string),
          typeof(GuideMaskHelper),
          new PropertyMetadata(string.Empty, HelpBindTargetControlChangedCallback)
    );

        private static void 
            HelpBindTargetControlChangedCallback(
                DependencyObject d, 
                DependencyPropertyChangedEventArgs e)
        {
            
        }

        public static string GetHelpBindTargetControl(UIElement target) =>
            (string)target.GetValue(HelpBindTargetControlProperty);

        public static void SetHelpBindTargetControl(UIElement target, string value) =>
            target.SetValue(HelpBindTargetControlProperty, value);
    }
}
