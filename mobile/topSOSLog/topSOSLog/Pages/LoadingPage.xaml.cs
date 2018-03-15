using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using topSOSLog.Pages.main;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace topSOSLog.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoadingPage : ContentPage
    { 
        public LoadingPage(string username,string userpw)
        { 
           InitializeComponent();
           Task.Delay(1000);// görsel olarak takılmaması için sleep attık ki loading kapağı dönsün
           Task.Run(() => GirisKontrol(username, userpw));
        } 
        private void GirisKontrol(string username, string userpw)
        {
            try
            { 
                string url = @"http://topsoslog.azurewebsites.net/"; 
                UserInfo.connection = new HubConnection(url);
                UserInfo._Hub = UserInfo.connection.CreateHubProxy("ChatHub");

                UserInfo.Username = username;
                UserInfo._Hub.On<String, String>("connectMessage", (i,j) =>
                Task.Run(() =>
                { 
                    if (i == "kabul")
                    {
                        Device.BeginInvokeOnMainThread(() =>
                            {
                                UserInfo.mp = new MainPage();
                                Navigation.PushModalAsync(new NavigationPage(UserInfo.mp));
                            });
                        UserInfo.UserID = j;
                    }
                    else
                    {
                        MessageAlert("Hata",
                            "Kullanıcı adı veya şifreniz hatalı!", "TAMAM");
                        BackMainPage();
                    }
                }));
                UserInfo.connection.Start().Wait();
                UserInfo._Hub.Invoke("Connect", username, userpw).Wait();
            }
            catch (Exception ex)
            {
                MessageAlert("Hata", "Teknik bir sorun oluştu! \nHata: " + ex.Message.ToString(), "Tamam");
                BackMainPage();
            }
        } 
        protected override bool OnBackButtonPressed()
        {
            base.OnBackButtonPressed();
            return false;
        }
        private void BackMainPage()
        {
            Task.Run(()=>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    Navigation.PushModalAsync(new Pages.LoginPage());
                });
            });
        }
        private void MessageAlert(string title, string message, string buttontitle)
        {
            Task.Run(() =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    DisplayAlert(title, message, buttontitle);
                });
            });
        }
    }
}
