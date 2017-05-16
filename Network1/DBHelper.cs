using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlServerCe;
using System.Windows;
using System.Windows.Controls;
using System.IO;

namespace Network1
{
    class DBHelper
    {
        public static string ConnectionString { get; private set; }
        public static string DbFile { get; private set; }

       

        public static void Create_Db_File(string fileName)
        {
 
            ConnectionString = String.Format(@"Data Source=|DataDirectory|\Projects\{0};Persist Security Info=True", fileName);
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
                                  "Name nvarchar(100) not null)";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "Create Table Tasks(" +
                                  "ID int identity(1000,1) not null primary key," +
                                  "Name nvarchar(100) not null)";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "Create Table Assingment(" +
                                  "ID int identity(10000,1) not null primary key," +
                                  "ID_Task int not null," +
                                  "foreign key(ID_Task) references Tasks(ID)," +
                                  "ID_Employee int not null," +
                                  "foreign key(ID_Employee) references Employee(ID))";
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Can't create db.", ex);
            }
            finally
            {
                if (conn != null)
                    conn.Close();
                MessageBox.Show("Проект успешно создан!", "",MessageBoxButton.OK, MessageBoxImage.Information);
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

        public static void FillData(string selectCommand, DataGrid data_grid)
        {
            try
            {
                using (var c = DBHelper.CreateConnection())
                {
                    c.Open();
                    using (var a = new SqlCeDataAdapter(selectCommand, c))
                    {
                        var t = new DataTable();
                        a.Fill(t);
                        data_grid.ItemsSource = t.DefaultView;
                    }
                }
            }
            catch
            {
                MessageBox.Show("Не удалось получить данные из базы данных.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static void add_task(TextBox txt)
        {
            if (txt.Text.Length != 0)
            {
                SqlCeConnection conn = null;

                try
                {
                    conn = new SqlCeConnection(DBHelper.ConnectionString);

                    conn.Open();

                    SqlCeCommand command = conn.CreateCommand();
                    command.CommandText = "INSERT INTO Tasks(Name) "
                    + "VALUES(@Name)";
                    command.Parameters.Add("@Name", SqlDbType.NVarChar, 100);
                    command.Parameters["@Name"].Value = txt.Text;
                    command.ExecuteScalar();
                    command.Parameters.Clear();
                }
                catch
                {
                    MessageBox.Show("Данный объект уже существует!");
                }
                finally
                {
                    if (conn != null)
                        conn.Close();
                }
            }
            else
                MessageBox.Show("Все поля должны быть заполнены!");
        }
    }
}
