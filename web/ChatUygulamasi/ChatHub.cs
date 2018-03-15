using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace ChatUygulamasi
{
    public class ChatHub : Hub
    {
        public static List<DBControls.UserDetail> ConnectedUsers = new List<DBControls.UserDetail>();


        public override Task OnConnected()
        {
            return base.OnConnected();
        }
        public void Connect(string userName, string userPw)
        {

            if (Gruplar.GrupListesi.Count == 0)
            {
                Gruplar.GrupListesi = Gruplar.GrupYukle();
            }

            var id = Context.ConnectionId;
            if (ConnectedUsers.Count(x => x.ConnectionId == id) == 0)
            {
                DBControls.LoginClass loginSonuc = DBControls.SQLLoginControl(userName, userPw);
                if (loginSonuc.durum == true)
                {
                    DBControls.UserDetail yeniUser = DBControls.GetUserInfo(userName, id);
                    ConnectedUsers.Add(yeniUser);
                    Gruplar.KullanıcıEslestir(yeniUser.UserID, yeniUser.ConnectionId);
                    ////send to caller
                    //Clients.Caller.onConnected(id, userName, ConnectedUsers, CurrentMessage);
                    ////send to all except caller client

                    //Clients.AllExcept(id).onNewUserConnected(id, userName);

                    Clients.Client(id).connectMessage("kabul", loginSonuc.id.ToString());
                }
                else
                {
                    Clients.Client(id).connectMessage("red", "0");
                }
            }
        }
        public class Message
        {
            public string GrupID;
            public DBControls.UserDetail Gonderen;
            public string Text;
            public DateTime DateTime;
        }
        public void GrupMesajGonder(Message mesaj)
        {
            try
            {
                string connectionID = Context.ConnectionId;
                // var MesajAtan = ConnectedUsers.FirstOrDefault(x => x.UserID == userid);
                foreach (var grup in Gruplar.GrupListesi)
                {
                    if (grup.GroupID == mesaj.GrupID)
                    {
                        foreach (var user in grup.Users)
                        {
                            if (user.ConnectionId != null)//user.UserID != mesaj.Gonderen.UserID &&
                            {
                                Clients.Client(user.ConnectionId).
                                    GrupMesaj(mesaj); // gönderen user detayı, alıcı grup bilgileri
                            }
                        }
                    }
                }
            }
            catch { }
            
        }

        public void AddNewUser(string username, string userpw, string mail, string tel)
        {
            var id = Context.ConnectionId;
            DBControls.LoginSonuc sonuc = DBControls.SQLNewUserCreate(username, userpw,
                                                        mail, tel);
            Clients.Client(id).sendNewUserResault(sonuc.durum, sonuc.aciklama);
        }
        public void SendGroupsList(string userid)
        {
            var id = Context.ConnectionId;
            
            IList<DBControls.GroupChatInfo> sendList = DBControls.GetSqlGroups(userid);

            Clients.Client(id).sendGroupsList(sendList);
        }
        public void AddNewGroup(List<string> users,string name)
        { 
            DBControls.DBNewGroup(users, name);
            var id = Context.ConnectionId;
            Clients.Client(id).addnewGroup("ok");
            Gruplar.GrupYenile();
        }
        public void SendUserListNewGroup(string text)
        {
            var id = Context.ConnectionId;
            Clients.Client(id).senduserListnewGroup(DBControls.getUserList(text));
        }

        //public void SendMessageToAll(string userName, string message)
        //{
        //    // store last 100 messages in cache
        //    AddMessageinCache(userName, message);

        //    // Broad cast message
        //    Clients.All.messageReceived(userName, message);
        ////}
        //public void SendPrivateMessage(string toUserId, string message)
        //{

        //    string fromUserId = Context.ConnectionId;

        //    var toUser = ConnectedUsers.FirstOrDefault(x => x.ConnectionId == toUserId);
        //    var fromUser = ConnectedUsers.FirstOrDefault(x => x.ConnectionId == fromUserId);

        //    if (toUser != null && fromUser != null)
        //    {
        //        // send to 
        //        Clients.Client(toUserId).sendPrivateMessage(fromUserId, fromUser.UserName, message);

        //        // send to caller user
        //        Clients.Caller.sendPrivateMessage(toUserId, fromUser.UserName, message);
        //    } 
        //}
        public override Task OnDisconnected()
        {
            string connectionid = Context.ConnectionId;
            var item = ConnectedUsers.FirstOrDefault(x => x.ConnectionId == connectionid);
            if (item != null)
            {
                ConnectedUsers.Remove(item);

                if(Gruplar.GrupListesi.Count!=0)
                {
                    Gruplar.GrupdanUyeDusur(item,connectionid);
                }
                //var id = Context.ConnectionId;
                //Clients.All.onUserDisconnected(id, item.UserName);

            }

            return base.OnDisconnected();
        }
        //private void AddMessageinCache(string userName, string message)
        //{
        //    CurrentMessage.Add(new MessageDetail { UserName = userName, Message = message });

        //    if (CurrentMessage.Count > 100)
        //        CurrentMessage.RemoveAt(0);
        //}
    }
}