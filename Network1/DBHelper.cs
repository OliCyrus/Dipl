using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlServerCe;
using System.Windows;
using System.IO;

namespace Network1
{
    class DBHelper
    {
        public static string ConnectionString { get; private set; }
        public static string DbFile { get; private set; }

       

        public static void Create_Db_File(string fileName)
        {
            ConnectionString = String.Format(@"Data Source=|DataDirectory|\{0};Persist Security Info=True", fileName);
            DbFile = fileName;
        }

        public static void Create_DB()
        {
            var engine = new SqlCeEngine(ConnectionString);
            engine.CreateDatabase();

            SqlCeConnection conn = null;
            
            try
            {
                conn = CreateConnection();
                conn.Open();

                var cmd = conn.CreateCommand();
                cmd.CommandText = "Create Table Employee(" +
                                  "ID int identity(1,1) not null primary key," +
                                  "NAME nvarchar(100) not null)";
            }
            catch (Exception ex)
            {
                throw new Exception("Can't create db.", ex);
            }
            finally
            {
                if (conn != null)
                    conn.Close();
                MessageBox.Show("База данных успешно создана!");
            }
        }

        public static SqlCeConnection CreateConnection()
        {
            return new SqlCeConnection(ConnectionString);
        }

        public static bool IsDbFileExist
        {
            get { return File.Exists(DbFile); }
        }
    }
}
