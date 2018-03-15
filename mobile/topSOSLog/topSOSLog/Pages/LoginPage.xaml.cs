using Microsoft.AspNet.SignalR.Client;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
namespace topSOSLog.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {  
        public LoginPage()
        {
            InitializeComponent();
        } 
        private void OnLoginPageSizeChanged(object sender, EventArgs e)
        {
            
            if (Width * Height < 0) return;
            if (Width< Height)
            {
                MainGrid.RowDefinitions[0].Height = new GridLength(1, GridUnitType.Star);
                MainGrid.RowDefinitions[1].Height = new GridLength(1, GridUnitType.Star);
                MainGrid.ColumnDefinitions[0].Width = new GridLength(1, GridUnitType.Star);
                MainGrid.ColumnDefinitions[1].Width = new GridLength(0, GridUnitType.Star);
                Grid.SetRow(ContentStacklayout, 1);
                Grid.SetColumn(ContentStacklayout, 0);
            }
            else
            {
                MainGrid.RowDefinitions[0].Height = new GridLength(1, GridUnitType.Star);
                MainGrid.RowDefinitions[1].Height = new GridLength(0, GridUnitType.Star);
                MainGrid.ColumnDefinitions[0].Width = new GridLength(1, GridUnitType.Star);
                MainGrid.ColumnDefinitions[1].Width = new GridLength(1, GridUnitType.Star);
                Grid.SetRow(ContentStacklayout, 0);
                Grid.SetColumn(ContentStacklayout, 1);
            }
        } 
        private void LoginButtonClicked_Event(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(entUserName.Text) || string.IsNullOrEmpty(entPassword.Text))
            {
                MessageAlert("Hata", "Kullanıcı adı veya şifre bölümü boş bırakılamaz!", "Tamam");
            }
            else
            {
                Navigation.PushModalAsync(
                    new LoadingPage(entUserName.Text, entPassword.Text)); 
            }
        }
        private void MessageAlert(string title,string message,string buttontitle)
        {
            Task.Run(() =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    DisplayAlert(title,message, buttontitle);
                });
            });
        }

        private void btnCreateNewUser(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new NewUserPage());
        }
    }
}
