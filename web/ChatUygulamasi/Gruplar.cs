using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;

namespace ChatUygulamasi
{
    public class Gruplar
    {
        public static List<Gruplar.CLGrup> GrupListesi = new List<Gruplar.CLGrup>();
        public class CLGrup
        {
            public string GroupID { get; set; } // sql grup id
            public string GroupAdminID { get; set; }
            public string GroupName { get; set; }
            public List<DBControls.UserDetail> Users = new List<DBControls.UserDetail>();
        }
        public static List<CLGrup> GrupYukle()
        {
            List<CLGrup> GrupListesi = new List<CLGrup>();
            GrupListesi.Clear();

            string query = @"SELECT ID,Name,Adminid FROM GROUPS;";
            using (SqlConnection connection = new SqlConnection(DBControls.conBuilder.ConnectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                // burada grupları tablo olarak alalım da işimiz kolaylaşsın.
                DataTable dt = new DataTable();
                using (SqlDataAdapter dtA = new SqlDataAdapter(command))
                {
                    dtA.Fill(dt);
                } 
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    CLGrup clg = new CLGrup();
                    string grupid = dt.Rows[i][0].ToString();
                    command = new SqlCommand(@" SELECT GroupAndUsers.ID,userID,Name,Adminid,Users.id,
	                                                   Users.username,mail,tel,nick FROM GroupAndUsers
                                                INNER JOIN GROUPS
                                                    ON GROUPS.ID = groupID
                                                        AND GROUPS.ID = @groupID
                                                INNER JOIN USERS
                                                    ON USERS.ID = GroupAndUsers.userID", connection);
                    command.Parameters.AddWithValue("@groupID", grupid);
                    using (SqlDataReader oku = command.ExecuteReader())
                    {
                        while (oku.Read())
                        {
                            clg.GroupID = grupid;
                            clg.GroupAdminID = oku[3].ToString();
                            clg.GroupName = oku[2].ToString();
                            DBControls.UserDetail userDetail = new DBControls.UserDetail();
                            userDetail.ConnectionId = null; // suan user yok sonra set edeceğiz.
                            userDetail.UserID = oku[4].ToString();
                            userDetail.UserName = oku[5].ToString();
                            userDetail.Mail = oku[6].ToString();
                            userDetail.Tel = oku[7].ToString();
                            userDetail.Nick = oku[8].ToString();
                            clg.Users.Add(userDetail); 
                        }
                    }
                    GrupListesi.Add(clg);
                }
            }
            return GrupListesi;
        }

        public static void GrupYenile()
        {
            GrupYukle();
            System.Threading.Thread.Sleep(3000);
            foreach (DBControls.UserDetail item in ChatHub.ConnectedUsers)
            {
                KullanıcıEslestir(item.UserID, item.ConnectionId);
            }
        }
        public static void KullanıcıEslestir(string userid,string connectionid)
        {
            foreach (CLGrup item in GrupListesi)
            {
                foreach (DBControls.UserDetail user in item.Users)
                {
                    if (user.UserID == userid)
                        user.ConnectionId = connectionid;
                }
            }
        } 
        public static void GrupdanUyeDusur(DBControls.UserDetail CikanUser, string connectionid)
        {
            foreach(CLGrup item in GrupListesi)
            {
                foreach(DBControls.UserDetail user in item.Users)
                {
                    if (user == CikanUser)
                        user.ConnectionId = null;
                }
            }
        }
    }
}