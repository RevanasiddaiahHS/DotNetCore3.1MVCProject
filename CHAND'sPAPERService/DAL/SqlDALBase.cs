using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace CHAND_sPAPERService.DAL
{
  
   public class SqlDALBase
    {
       //public string DefaultConnectionString = Common.contrainingString;
       //public string ConnectionString;
        public SqlConnection GetConnection()
        {
            // return new SqlConnection(ConnectionString ?? DefaultConnectionString);
            return new SqlConnection(@"Data Source=LAPTOP-VT657LCV\SQLEXPRESS;Initial Catalog=CHANDsPAPER;Integrated Security=True");
            //return new SqlConnection(@"workstation id=ChandsPaperDB.mssql.somee.com;packet size=4096;user id=RevanaHiremath_SQLLogin_1;pwd=zspwxjtv1a;data source=ChandsPaperDB.mssql.somee.com;persist security info=False;initial catalog=ChandsPaperDB");
        }
        
    }
}
