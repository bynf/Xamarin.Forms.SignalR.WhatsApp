using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace ChatUygulamasi
{
  
    public static class DBControls
    {
        public static SqlConnectionStringBuilder conBuilder = new SqlConnectionStringBuilder
        {
            DataSource = "topsos.database.windows.net",
            UserID = "bynf",
            Password="123987bB",
            InitialCatalog="topsossql"
        }; 
        
        public class LoginClass
        {
            public bool durum { get; set; }
            public string id { get; set; }
        }
        public static LoginClass SQLLoginControl(string Username,string UserPw)
        {
            LoginClass sonuc = new LoginClass();
            try
            {
                using (SqlConnection connection = new SqlConnection(conBuilder.ConnectionString))
                {
                    string query =
                    @"SELECT
	                    CASE Count([dbo].[Users].[id])
		                WHEN 0 THEN 'red'   
		                ELSE 'kabul'
	                    END as sonuc
                    FROM [dbo].[Users]
                    WHERE [dbo].[Users].[username] = @username 
	                    AND [dbo].[Users].[userpw] = @userpw;";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@username", Username);
                    command.Parameters.AddWithValue("@userpw", UserPw);
                    connection.Open();

                    sonuc.durum = (command.ExecuteScalar().ToString() == "kabul"
                        ? true : false);
                    if (sonuc.durum == true)
                    {
                        query = @"SELECT ID FROM [dbo].[Users] AS T1
                                WHERE T1.username='" + Username + "'";
                        command = new SqlCommand(query, connection);
                        sonuc.id = command.ExecuteScalar().ToString();
                    }
                    else
                        sonuc.id = "0";
                }
                return sonuc;
            }catch
            {
                sonuc.durum = false; ;
            }
            return sonuc;
        } 
        public class LoginSonuc
        {
            public bool durum = false;
            public string aciklama = null;
        }
        public static LoginSonuc SQLNewUserCreate(string Username, string userPw, string mail = null, string tel = null)
        {
            LoginSonuc sonuc = new LoginSonuc();
            string query = @"   DECLARE @durum int=0;
                                DECLARE @cikis char;

                                SELECT @durum = COUNT(*)
                                FROM [dbo].[Users]
                                WHERE [dbo].[Users].username = @username
                                GROUP BY [dbo].[Users].username;

                                IF @durum = 0 BEGIN
	                                INSERT INTO [dbo].[Users] (username,userpw,mail,tel,nick)
	                                    VALUES (@username,@userpw,@usermail,@usertel,@username)
	                                SET @cikis = 'b';
                                END
                                ELSE BEGIN
	                                SET @cikis = 'h';
                                END 

                                SELECT @cikis AS sonuc;";
            try
            {
                using (SqlConnection connection = new SqlConnection(conBuilder.ConnectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@username", Username);
                    command.Parameters.AddWithValue("@userpw", userPw);
                    command.Parameters.AddWithValue("@usermail", mail);
                    command.Parameters.AddWithValue("@usertel", tel);
                    connection.Open();
                    char durum = command.ExecuteScalar().ToString()[0];
                    if (durum == 'b')
                    {
                        sonuc.durum = true;
                        sonuc.aciklama = "Yeni üyelik oluşturuldu.";
                    }
                    else
                    {
                        sonuc.aciklama = "Aynı isimde farklı bir kullanıcı mevcut.";
                    }
                }
            }
            catch (Exception ex)
            {
                sonuc.aciklama = string.Format("Üyelik oluşturma hatası {0}",
                    ex.Message.ToString());
            }
            return sonuc;
        } 
        public class GroupChatInfo
        {
            public string ID { get; set; }
            public string Title { get; set; }
            public string ImageUrl { get; set; }
            public string TextMessage = null;
            public string AdminID { get; set; }
        }

        public static IList<GroupChatInfo> GetSqlGroups(string userID)
        {
            List<GroupChatInfo> sonuc = new List<GroupChatInfo>();
            try
            {
                string query = @"SELECT T1.ID,T1.Name, T1.ImageUrl, T1.Adminid
                                    FROM[dbo].[Groups] AS T1
                                INNER JOIN GroupAndUsers AS T2
                                    ON T1.ID = T2.groupID and T2.userID = @userid";

                using (SqlConnection connection = new SqlConnection(conBuilder.ConnectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@userid", userID);
                    connection.Open();
                    using (SqlDataReader oku = command.ExecuteReader())
                    {
                        while(oku.Read())
                        { 
                            GroupChatInfo gc = new GroupChatInfo();
                            gc.ID = oku[0].ToString();
                            gc.Title = oku[1].ToString();
                            gc.ImageUrl = oku[2].ToString();
                            gc.AdminID = oku[3].ToString();
                            sonuc.Add(gc);
                        }
                        oku.Close();
                    }
                }
            }
            catch
            {
            }
            return sonuc;

        } 
        public class UserClass
        {
            public string Userid { get; set; }
            public string Nick { get; set; }
            public string Username { get; set; }
            public string Mail { get; set; }
            public string Tel { get; set; }
        }
        public static IList<UserClass> getUserList (string nicktext =null)
        {
            List<UserClass> sonuc = new List<UserClass>();
            string query;
            query = @"SELECT id,username,mail,tel,nick FROM [dbo].[Users] AS T1
                            WHERE T1.nick LIKE '%" + nicktext + "%';";
            try
            {
                using (SqlConnection connection = new SqlConnection(conBuilder.ConnectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection); 
                    connection.Open();
                    using (SqlDataReader oku = command.ExecuteReader())
                    {
                        while (oku.Read())
                        {
                            UserClass uc = new UserClass();
                            uc.Userid = oku[0].ToString();
                            uc.Username = oku[1].ToString();
                            uc.Mail = oku[2].ToString();
                            uc.Tel = oku[3].ToString();
                            uc.Nick = oku[4].ToString();
                            sonuc.Add(uc);
                        }
                        oku.Close();
                    }
                }
            }
            catch { }
            return sonuc;
        }
        public static void DBNewGroup(List<string> users,string GroupName)
        {
            string query1 = @"INSERT INTO [dbo].[Groups] (Name,ImageUrl,Adminid)
                            OUTPUT INSERTED.ID
                            VALUES(@Name,'test.png',@Adminid);";
            string query2 = @"INSERT INTO [dbo].GroupAndUsers (userID,groupID)
                                    VALUES (@userID,@groupID)"; 
            try
            {
                using (SqlConnection connection = new SqlConnection(conBuilder.ConnectionString))
                {
                    SqlCommand command = new SqlCommand(query1, connection);
                    command.Parameters.AddWithValue("@Name", GroupName);
                    command.Parameters.AddWithValue("@Adminid",
                        users[users.Count - 1].ToString()); 
                    connection.Open();

                    string grupid = command.ExecuteScalar().ToString();

                    foreach (string item in users)
                    {
                        command = new SqlCommand(query2,connection);
                        command.Parameters.AddWithValue("@userID", item);
                        command.Parameters.AddWithValue("@groupID", grupid);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex) { }
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
        public static UserDetail GetUserInfo(string username,string connectionID)
        {
            UserDetail sonuc = new UserDetail();
            string query;
            query = @"SELECT id,username,mail,tel,nick FROM [dbo].[Users] AS T1
                            WHERE T1.username = '" + username + "';";
            try
            {
                using (SqlConnection connection = new SqlConnection(conBuilder.ConnectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();
                    using (SqlDataReader oku = command.ExecuteReader())
                    {
                        while (oku.Read())
                        {
                            sonuc.ConnectionId = connectionID;
                            sonuc.UserID = oku[0].ToString();
                            sonuc.UserName = oku[1].ToString();
                            sonuc.Mail = oku[2].ToString();
                            sonuc.Tel = oku[3].ToString();
                            sonuc.Nick = oku[4].ToString();
                        }
                        oku.Close();
                    }
                }
            }
            catch { }
            return sonuc;
        }
    }
}