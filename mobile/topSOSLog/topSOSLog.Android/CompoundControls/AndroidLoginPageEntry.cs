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

namespace topSOSLog.Droid.CompoundControls
{
    public class AndroidLoginPageEntry : LinearLayout
    {
        public readonly FrameLayout LeftFrameLayout;
        public readonly ImageView LeftImageView;
        public readonly EditText EntryEditText;
        public AndroidLoginPageEntry(Context context) : base(context)
        {
            var inflater = LayoutInflater.From(context);
            inflater.Inflate(Resource.Layout.LoginPageEntryLayout, this);
            LeftFrameLayout = FindViewById<FrameLayout>(Resource.Id.LeftView);
            LeftImageView = FindViewById<ImageView>(Resource.Id.LeftImage);
            EntryEditText = FindViewById<EditText>(Resource.Id.Entry);
        }
    }
}