using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using topSOSLog.CustomControls;
using topSOSLog.Droid.CompoundControls;
using System.Reflection;
using Android.Graphics.Drawables;

namespace topSOSLog.Droid.CustomRenderers
{
    public class LoginPageEntryRenderer:ViewRenderer<LoginPageEntry, AndroidLoginPageEntry>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<LoginPageEntry> e)
        {
            base.OnElementChanged(e);
            if(Control==null)
            {
                var androidLoginPageEntry = new AndroidLoginPageEntry(Context);
                androidLoginPageEntry.EntryEditText.Text = Element.Text;
                androidLoginPageEntry.EntryEditText.Hint = Element.Placeholder;
                androidLoginPageEntry.EntryEditText.SetTextColor(Element.TextColor.ToAndroid());
                androidLoginPageEntry.EntryEditText.SetHintTextColor(Element.PlaceholderColor.ToAndroid());
                if(Element.IsPassword)
                {
                    androidLoginPageEntry.EntryEditText.InputType |= Android.Text.InputTypes.TextVariationPassword;
                }
                else
                {
                    androidLoginPageEntry.EntryEditText.InputType &=
                        Android.Text.InputTypes.TextVariationPassword;
                }
                Assembly assembly = Assembly.GetAssembly(typeof(LoginPageEntry));
                string resource = ((LoginPageEntry)Element).LeftImageSource;
                System.IO.Stream stream= assembly.GetManifestResourceStream(resource);
                Drawable leftImageDrawable = Drawable.CreateFromStream(stream,
                    nameof(AndroidLoginPageEntry));
                androidLoginPageEntry.LeftImageView.SetImageDrawable(leftImageDrawable);

                SetNativeControl(androidLoginPageEntry);
            }
        }
    }
}