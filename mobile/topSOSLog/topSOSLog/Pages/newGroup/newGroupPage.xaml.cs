using Microsoft.AspNet.SignalR.Client;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System;
using System.Threading.Tasks;

namespace topSOSLog.Pages.newGroup
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class newGroupPage : ContentPage
    {
        public newGroupPage()
        {
            InitializeComponent();
            groupList.IsRefreshing = true;
            GetUserList(null); 
        } 
        public class UserClass
        {
            public string Userid { get;set; }
            public string Nick { get; set; }
            public string Username { get; set; }
            public string Mail { get; set; }
            public string Tel { get; set; }
        } 
        public async void GetUserList(string A)
        {
            IHubProxy _Hub;
            try
            {
                string url = @"http://topsoslog.azurewebsites.net/";
                var connection = new HubConnection(url);
                _Hub = connection.CreateHubProxy("ChatHub");
                _Hub.On<IList<UserClass>>("SendUserListNewGroup", i =>
                    Device.BeginInvokeOnMainThread(
                        () =>
                    { 
                        foreach (UserClass item in i) // kendi userid sini çıkartalım.
                        {
                            if(item.Userid == UserInfo.UserID)
                            {
                                i.Remove(item);
                                break;
                            }
                        } 
                        groupList.ItemsSource = i;
                        groupList.IsRefreshing = false;
                    }));
                await connection.Start();
                await _Hub.Invoke("senduserListnewGroup", 
                    (A==null ? "" : A));
            }
            catch
            { 
            }
        }
        public async void onNewGroupButtonClick(object sender,EventArgs e)
        {
            if (Secilenler.Count == 0)
            {
                await DisplayAlert("Hata", "Kimseyi eklemeden topluluk oluşturamazsınız!", "Tamam");
                return;
            }
            if(string.IsNullOrEmpty(entToplulukAdi.Text))
            {
                await DisplayAlert("Hata", "Topluluk adı girmediniz!", "Tamam");
                return;
            }
            if(!Secilenler.Contains(UserInfo.UserID))
                Secilenler.Add(UserInfo.UserID);
            IHubProxy _Hub;
            try
            {
                string url = @"http://topsoslog.azurewebsites.net/";
                var connection = new HubConnection(url);
                _Hub = connection.CreateHubProxy("ChatHub");
                _Hub.On<string>("addnewGroup", i =>
                 {
                     BackMainPage(i);
                 });
                await connection.Start();
                actIndicator.IsVisible = true;
                ((Button)sender).IsVisible = false;
                await _Hub.Invoke("AddNewGroup", Secilenler, entToplulukAdi.Text); 
            }
            catch
            { 
            }
        } 
        private void BackMainPage(string i)
        { 
            Device.BeginInvokeOnMainThread(() =>
            {
                UserInfo.mp.ListeGuncelle();
                DisplayAlert("Başarılı", "Topluluk başarı ile oluşturuldu!", "Tamam");
                base.OnBackButtonPressed(); 
            });
            
        }

        public void onGetAllUsers()
        {
            groupList.IsRefreshing = true;
            GetUserList(null);
        }
        public void SearchButtonPressed(object sender)
        {
            groupList.IsRefreshing = true;
            GetUserList(((SearchBar)sender).Text);
        }
        public void onItemSelectedEvent(object sender, EventArgs e)
        {
            ListView list = (ListView)sender;
            list.SelectedItem = null;
        }
        private List<string> Secilenler = new List<string>();
        public void prListButtonClick(object sender, EventArgs e)
        {
            Button bt= (Button)sender;
            string userID = bt.CommandParameter.ToString();
            if(bt.BackgroundColor==Color.White)
            {
                bt.BackgroundColor = Color.DarkBlue;
                if(!Secilenler.Contains(userID))
                    Secilenler.Add(userID); 
            }
            else
            {
                bt.BackgroundColor = Color.White; 
                if(Secilenler.Contains(userID))
                    Secilenler.Remove(userID);
            }
            
        }
    }
}
