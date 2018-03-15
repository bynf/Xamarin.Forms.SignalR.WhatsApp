using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using topSOSLog.Pages.newGroup;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace topSOSLog.Pages.main
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent(); 
            ListeGuncelle(); 
        }
        protected override bool OnBackButtonPressed()
        {
            base.OnBackButtonPressed();
            return false;
        } 
        public async void ListeGuncelle()
        {
            lstView.IsRefreshing = true;
            await GetGroups();
        }
        public class GroupChatInfo
        {
            public string ID { get; set; }
            public string Title { get; set; }
            public string ImageUrl { get; set; }
            public string TextMessage { get; set; }
            public string AdminID { get; set; }
        }
        public async Task GetGroups()
        {
            IHubProxy _Hub;
            // burada sqllite ile geçmiş mesaj tablosu çekilecek. Aynı zamanda da yeni gelen mesajlar üstüne eklenecek.
            try
            {
                string url = @"http://topsoslog.azurewebsites.net/";
                var connection = new HubConnection(url);
                _Hub = connection.CreateHubProxy("ChatHub");

                _Hub.On<IList<GroupChatInfo>>("sendGroupsList", i =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        lstView.IsRefreshing = false;
                        lstView.ItemsSource = i;
                    });
                });
                await connection.Start();
                await _Hub.Invoke("SendGroupsList", UserInfo.UserID);
            }
            catch
            { }
        }
        public void onListViewRefreshing(object sender, EventArgs e)
        {
            ListeGuncelle();
        } 
        public void onSelected(object sender, SelectedItemChangedEventArgs e)
        {
            
            GroupChatInfo selectedItem = (GroupChatInfo)e.SelectedItem;
            if (selectedItem == null) return;
            Navigation.PushModalAsync(new GroupChatPage(selectedItem));

            ((ListView)sender).SelectedItem = null;
        }
        public void YeniGrupClickEvent(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new newGroupPage()); 
        }
    } 
}
