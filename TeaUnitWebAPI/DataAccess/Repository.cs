using System;
using System.Data;
using DataAccessUtility;
using ITUtility;
using Microsoft.Data.SqlClient;
using TeaUnitWebAPI.Models;
using System.Linq;

namespace TeaUnitWebAPI.DataAccess
{
    public class Repository : RepositoryBase
    {
        public Repository(string ConnectionName) : base(ConnectionName)
        {
        }
        public Repository(SqlConnection connection) : base(connection)
        {
        }
         public void Register(Users user)
        {
            SqlCommand sCmd = new SqlCommand();
            sCmd.Connection = connection;
            sCmd.Transaction = transaction;
            sCmd.CommandType = System.Data.CommandType.Text;
            sCmd.CommandText = @"INSERT INTO  master.dbo.Userss
                    (Id
                    ,LastName
                    ,FirstName
                    ,Username
                    ,Password
                    ,PreName)
                    Values
                    (@Id
                    ,@LastName
                    ,@FirstName
                    ,@Username
                    ,@Password
                    ,@PreName)
                    ";
            sCmd.Parameters.Add(new SqlParameter("@Id", user.Id));
            sCmd.Parameters.Add(new SqlParameter("@LastName", user.LastName));
            sCmd.Parameters.Add(new SqlParameter("@FirstName", user.FirstName));
            sCmd.Parameters.Add(new SqlParameter("@Username", user.Username));
            sCmd.Parameters.Add(new SqlParameter("@Password", user.Password));
            sCmd.Parameters.Add(new SqlParameter("@PreName", user.PreName));
            sCmd.ExecuteNonQuery();
        }
        public void CheckUser(out Users user, string userName)
        {
            user = null;
            SqlCommand sCmd = new SqlCommand();
            sCmd.Connection = connection;
            sCmd.CommandType = System.Data.CommandType.Text;
            sCmd.CommandText = @"SELECT 
                    Id
                    ,LastName
                    ,FirstName
                    ,Username
                    ,Password
                    ,PreName
                FROM master.dbo.Userss
                 WHERE Username =@userName
                    ";
            sCmd.Parameters.Add(new SqlParameter("@userName", userName));
            using (DataTable dt = Utility.FillDataTable(sCmd))
            {
                if (dt.Rows.Count > 0)
                {
                    user = dt.AsEnumerable<Users>().First();
                }
            }
        }
    }
}