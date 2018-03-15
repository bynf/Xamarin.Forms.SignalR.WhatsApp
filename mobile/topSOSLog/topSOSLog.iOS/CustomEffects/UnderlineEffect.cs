using Foundation;
using System;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using UnderlineEffect = topSOSLog.iOS.CustomEffects.UnderlineEffect;

[assembly: ResolutionGroupName(topSOSLog.CustomEffects.UnderlineEffect.EffectNamespace)]
[assembly: ExportEffect(typeof(UnderlineEffect), nameof(UnderlineEffect))]

namespace topSOSLog.iOS.CustomEffects
{
    public class UnderlineEffect : PlatformEffect
    {
        protected override void OnAttached()
        {
            SetUnderline(true);
        }
        protected override void OnDetached()
        {
            SetUnderline(false);
        }
        private void SetUnderline(bool underlined)
        {
            try
            {
                var label=(UILabel)Control;
                var text = (NSMutableAttributedString)label.AttributedText;
                var range = new NSRange(0, text.Length);
                if(underlined)
                {
                    text.AddAttribute(UIStringAttributeKey.UnderlineStyle,
                        NSNumber.FromInt32((int)NSUnderlineStyle.Single), range); 
                }
                else
                {
                    text.RemoveAttribute(UIStringAttributeKey.UnderlineStyle, range);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("cannot underline label. Error : ", ex.Message);
            }
        }
    }
}
