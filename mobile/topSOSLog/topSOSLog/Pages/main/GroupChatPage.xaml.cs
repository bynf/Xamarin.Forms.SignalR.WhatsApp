using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace topSOSLog.Pages.main
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GroupChatPage : ContentPage
    {
        public static MainPage.GroupChatInfo GroupCI;
        public class Message
        {
            public string GrupID;
            public UserDetail Gonderen;
            public string Text;
            public DateTime DateTime;
        }
        public class UserDetail
        {
            public string ConnectionId { get; set; }
            public string UserID { get; set; }
            public string UserName { get; set; }
            public string Tel { get; set; }
            public string Mail { get; set; }

            public string Nick { get; set; }
        } 
        public static IList<Message> Mesajlar;
        public GroupChatPage(MainPage.GroupChatInfo gcI)
        {
            InitializeComponent();
            GroupCI = gcI;
            //lbToplulukAdi.Text = GroupCI.Title;

            Mesajlar = new List<Message>(); 

            //if(Mesajlar.Count !=0)
            //    lstChat.ItemsSource = Mesajlar;
            
            MesajDinleAsync();
        }
        public async Task MesajDinleAsync()
        {
            try
            {
                string url = @"http://topsoslog.azurewebsites.net/";
                await UserInfo.connection.Start();

                UserInfo._Hub.On<Message>("GrupMesaj", i =>
                {
                    MesajBas(i);
                });

                //await UserInfo._Hub.Invoke("GrupMesajGonder", new Message
                //{
                //    GrupID = GroupCI.ID,
                //    DateTime = DateTime.Now,
                //    Text = "bynf bağlandı.",
                //    Gonderen = new UserDetail
                //    { 
                //        UserID = UserInfo.UserID,
                //        UserName = "bynf",
                //        Nick = "batuhan ozyon",
                //        Mail = "test@test.com",
                //        Tel = "123"
                //    }
                //});


                Device.StartTimer(TimeSpan.FromSeconds(10), () =>
                 {
                     UserInfo.connection.Start();
                     return true;
                 });
            }
            catch(Exception ex)
            {
            }
        }

        private void MesajBas(Message i)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Mesajlar.Insert(0, i); 
                StackLayout layout = new StackLayout();
                if (i.Gonderen.UserID == UserInfo.UserID)
                    layout.HorizontalOptions = LayoutOptions.EndAndExpand;
                else
                    layout.HorizontalOptions = LayoutOptions.StartAndExpand;
                layout.Children.Add(new Label // kullanıcı adı için
                {
                    Text = i.Gonderen.UserName,
                    HorizontalOptions = LayoutOptions.Start,
                    FontAttributes = FontAttributes.Bold,
                    FontSize = 8,
                    TextColor = (i.Gonderen.UserID == UserInfo.UserID ? Color.Green : Color.DarkRed)
                    
                });

                layout.Children.Add(new Label { Text = i.Text }); // mesaj için
                layout.Children.Add(new Label { Text = i.DateTime.ToString("H: mm"), FontSize=7, HorizontalOptions=LayoutOptions.End });

                SayfaStack.Children.Add(layout);

            });
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await UserInfo._Hub.Invoke("GrupMesajGonder", new Message
            {
                GrupID = GroupCI.ID,
                DateTime = DateTime.Now,
                Text = txtMessage.Text,
                Gonderen = new UserDetail
                {
                    UserID = UserInfo.UserID,
                    UserName = UserInfo.Username
                }
            });
            txtMessage.Text="";
        }
    }

}
