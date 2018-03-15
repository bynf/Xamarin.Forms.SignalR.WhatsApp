using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using topSOSLog.Pages.main;

namespace topSOSLog
{
    public static class UserInfo
    {
        public static string UserID { get; set; }
        public static string Username { get; set; }
        public static MainPage mp { get; set; }
        public static IHubProxy _Hub { get; set; }

        public static HubConnection connection { get; set; }
    }
}
