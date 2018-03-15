
using Xamarin.Forms;

namespace topSOSLog.CustomControls
{
    public class LoginPageEntry:Entry
    {
        public static readonly BindableProperty LeftImageSourceProperty =
            BindableProperty.Create(nameof(LeftImageSource), typeof(string), typeof(LoginPageEntry), string.Empty);
        public static readonly BindableProperty CornerRadiusProperty =
            BindableProperty.Create(nameof(CornerRadius), typeof(float), typeof(LoginPageEntry), 0f);
        public string LeftImageSource
        {
            get
            {
                return (string)GetValue(LeftImageSourceProperty);
            }
            set
            {
                SetValue(LeftImageSourceProperty, value);
            }
        }
        public float CornerRadius
        {
            get
            {
                return (float)GetValue(CornerRadiusProperty);
            }
            set
            {
                SetValue(CornerRadiusProperty, value);
            }
        }
    }
}
