
using Xamarin.Forms.Platform.iOS;
using topSOSLog.CustomControls;
using topSOSLog.iOS.CustomRenderers;
using Xamarin.Forms;
using System.ComponentModel;
using UIKit;
using System.Reflection;
using CoreGraphics;

[assembly:ExportRenderer(typeof(LoginPageEntry),typeof(LoginPageEntryRenderer))]
namespace topSOSLog.iOS.CustomRenderers
{
    public class LoginPageEntryRenderer:EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if(Control!=null)
            {
                Control.Layer.CornerRadius = ((LoginPageEntry)Element).CornerRadius;
                Control.Layer.BackgroundColor = UIColor.FromRGB(71, 74, 85).CGColor;
                Control.BorderStyle = UITextBorderStyle.None;
                Control.Layer.MasksToBounds = true;
            }
        }
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName==VisualElement.WidthProperty.PropertyName
                || e.PropertyName==VisualElement.HeightProperty.PropertyName)
            {
                if(Element.Width * Element.Height >0)
                {
                    UIImage leftImage = UIImage.FromResource(Assembly.GetAssembly(typeof(LoginPageEntry)),
                        ((LoginPageEntry)Element).LeftImageSource);
                    var width = leftImage.Size.Width * 1 / 2;
                    var height = leftImage.Size.Height * 1 / 2;
                    var x = (Element.Height - width) / 2;
                    var y = (Element.Height - height) / 2;
                    UIImageView imageview = new UIImageView(new CGRect(x, y, width, height))
                    {
                        Image = leftImage
                    };
                    var leftView = new UIView(new CGRect(0, 0, Element.Height, Element.Height));
                    leftView.AddSubview(imageview);
                    Control.LeftViewMode = UITextFieldViewMode.Always;
                    Control.LeftView = leftView;
                }
            }
            base.OnElementPropertyChanged(sender, e);
        }
    }
}