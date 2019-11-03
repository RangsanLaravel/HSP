using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace ITUtility
{
    public class Utility
    {
         public static DataTable FillDataTable(SqlCommand sCmd)
        {
            using (SqlDataReader oReader = sCmd.ExecuteReader())
            {
                using (DataTable dataTable = new DataTable())
                {
                    dataTable.Load(oReader);
                    oReader.Close();
                    return dataTable;
                }
            }
        }
    }
}