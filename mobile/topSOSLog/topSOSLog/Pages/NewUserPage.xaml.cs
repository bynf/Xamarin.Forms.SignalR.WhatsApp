using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace topSOSLog.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewUserPage : ContentPage
    {
        public NewUserPage()
        {
            InitializeComponent();
        }
        public async Task NewUserEventButton(object sender, EventArgs e)
        {
            if ((Button)sender != null)
            {
                // password kontrolü felan yapalım
                if (entPassword1.Text != entPassword2.Text)
                {
                    MessageAlert("Hata", "Girdiğiniz şifreler birbiri ile uyuşmuyor!", "Tamam");
                    return;
                }
                if (string.IsNullOrEmpty(entKullanici.Text) ||
                    string.IsNullOrEmpty(entPassword1.Text) ||
                    string.IsNullOrEmpty(entPassword2.Text) ||
                    string.IsNullOrEmpty(entMail.Text) ||
                    string.IsNullOrEmpty(entTel.Text))
                {
                    MessageAlert("Hata", "Lütfen hiçbir alanı boş bırakmayınız.", "Tamam");
                    return;
                }
                actLoading.IsVisible = true;
                ((Button)sender).IsVisible = false;
                // kullanıcı arayüzde boşa beklemesin 
                IHubProxy _Hub;
                var connection = new HubConnection(@"http://topsoslog.azurewebsites.net/");
                _Hub = connection.CreateHubProxy("ChatHub");

                // gelen veriyi işleyeceğimiz alan
                _Hub.On<bool, string>("sendNewUserResault", (durum, aciklama) =>
                Task.Run(() =>
                {
                    if (durum == false)
                    {
                        MessageAlert("Hata", aciklama, "Tamam");
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            actLoading.IsVisible = false;
                            btnNewUser.IsVisible = true;
                        });
                    }
                    else
                    {
                        MessageAlert("Başarılı",
                            "Üyeliğiniz oluşturuldu.", "Tamam");
                        base.OnBackButtonPressed();
                    }
                }));


                // çalışacak method
                await connection.Start();
                await _Hub.Invoke("AddNewUser",
                    entKullanici.Text, entPassword1.Text, entMail.Text, entTel.Text);




            }
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
