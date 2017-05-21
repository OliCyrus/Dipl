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
        public static string TasksSelect = "select ID[ID], Name[Имя], Start_date[Дата начала], Finish_date[Дата окончания] from Tasks ";
        public static string EmployeeSelect = "select ID[ID], Name[Имя] from Employee ";
        public static string AssingmentSelect = "Select a.ID[ID], t.Name[Задача], e.Name[Работник] from Assingment a, Tasks t, Employee e where a.ID_Task = T.ID and a.ID_Employee = e.ID";



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
                                  "ID int identity(1,1) not null primary key," +
                                  "Name nvarchar(100) not null," +
                                  "Start_date datetime not null," +
                                  "Finish_date datetime not null)";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "Create Table Assingment(" +
                                  "ID int identity(1,1) not null primary key," +
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

        public static string[] GetDataString(string selectCommand, string Column)
        {
            var t = new DataTable();

            string[] result;

            //if (!DbProvider.IsDbFileExist)
            //    DbProvider.CreateDb();

            using (var c = DBHelper.CreateConnection())
            {
                c.Open();
                using (var a = new SqlCeDataAdapter(selectCommand, c))
                {
                    a.Fill(t);
                    result = new string[t.Rows.Count];
                }
                for (int i = 0; i < t.Rows.Count; i++)
                {
                    result[i] = t.Rows[i].Field<string>(Column);
                }
            }
            return result;
        }

        public static string[] GetDataInt(string selectCommand, string Column)
        {
            var t = new DataTable();

            string[] result;

            //if (!DbProvider.IsDbFileExist)
               // DbProvider.CreateDb();

            using (var c = DBHelper.CreateConnection())
            {
                c.Open();
                using (var a = new SqlCeDataAdapter(selectCommand, c))
                {
                    a.Fill(t);
                    result = new string[t.Rows.Count];
                }
                for (int i = 0; i < t.Rows.Count; i++)
                {
                    result[i] = t.Rows[i].Field<int>(Column).ToString();
                }
            }
            return result;
        }

        ///КОД МАНИПУЛЯЦИИ С ТАБЛИЦЕЙ ЗАДАНИЙ///
        public static void add_task(TextBox task_name, DatePicker start_date, DatePicker finish_date)
        {
            if (task_name.Text.Length != 0 && start_date.Text.Length != 0 && finish_date.Text.Length != 0)
            {
                SqlCeConnection conn = null;

                try
                {
                    conn = new SqlCeConnection(DBHelper.ConnectionString);

                    conn.Open();

                    SqlCeCommand command = conn.CreateCommand();
                    command.CommandText = "INSERT INTO Tasks(Name, Start_date, Finish_date) "
                    + "VALUES(@Name,@Start_date, @Finish_date)";
                    command.Parameters.Add("@Name", SqlDbType.NVarChar, 100);
                    command.Parameters["@Name"].Value = task_name.Text;
                    command.Parameters.Add("@Start_date", SqlDbType.DateTime);
                    command.Parameters["@Start_date"].Value = start_date.Text;
                    command.Parameters.Add("@Finish_date", SqlDbType.DateTime);
                    command.Parameters["@Finish_date"].Value = finish_date.Text;
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

        public static void get_tasks_fields(DataGrid dgr, TextBox task_name , DatePicker start_date, DatePicker finish_date)
        {
            DataRowView drv = (DataRowView)dgr.SelectedItem;
            task_name.Text = (drv[1]).ToString();
            start_date.Text = (drv[2]).ToString();
            finish_date.Text = (drv[3]).ToString();
        }

        public static void update_task(DataGrid dgr, TextBox task_name, DatePicker start_date, DatePicker finish_date)
        {
            if (task_name.Text.Length != 0 && start_date.Text.Length != 0 && finish_date.Text.Length != 0)
            {
                DataRowView drv = (DataRowView)dgr.SelectedItem;
                SqlCeConnection conn = null;

                try
                {
                    conn = new SqlCeConnection(DBHelper.ConnectionString);

                    conn.Open();

                    SqlCeCommand command = conn.CreateCommand();
                    command.CommandText = "Update Tasks set Name = @Name, Start_date = @Start_date, Finish_date = @Finish_date where ID = @ID";
                    command.Parameters.Add("@ID", SqlDbType.Int);
                    command.Parameters["@ID"].Value = (drv[0]).ToString();
                    command.Parameters.Add("@Name", SqlDbType.NVarChar, 100);
                    command.Parameters["@Name"].Value = task_name.Text;
                    command.Parameters.Add("@Start_date", SqlDbType.DateTime);
                    command.Parameters["@Start_date"].Value = start_date.Text;
                    command.Parameters.Add("@Finish_date", SqlDbType.DateTime);
                    command.Parameters["@Finish_date"].Value = finish_date.Text;
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


        ///КОД МАНИПУЛЯЦИИ С ТАБЛИЦЕЙ РАБОТНИКОВ///
        public static void add_employee(TextBox employee_name)
        {
            if (employee_name.Text.Length != 0)
            {
                SqlCeConnection conn = null;

                try
                {
                    conn = new SqlCeConnection(DBHelper.ConnectionString);

                    conn.Open();

                    SqlCeCommand command = conn.CreateCommand();
                    command.CommandText = "INSERT INTO Employee(Name) "
                    + "VALUES(@Name)";
                    command.Parameters.Add("@Name", SqlDbType.NVarChar, 100);
                    command.Parameters["@Name"].Value = employee_name.Text;
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

        public static void update_employee(DataGrid dgr, TextBox employee_name)
        {
            if (employee_name.Text.Length != 0)
            {
                DataRowView drv = (DataRowView)dgr.SelectedItem;
                SqlCeConnection conn = null;

                try
                {
                    conn = new SqlCeConnection(DBHelper.ConnectionString);

                    conn.Open();

                    SqlCeCommand command = conn.CreateCommand();
                    command.CommandText = "Update Employee set Name = @Name where ID = @ID";
                    command.Parameters.Add("@ID", SqlDbType.Int);
                    command.Parameters["@ID"].Value = (drv[0]).ToString();
                    command.Parameters.Add("@Name", SqlDbType.NVarChar, 100);
                    command.Parameters["@Name"].Value = employee_name.Text;
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

        public static void get_emoloyee_fields(DataGrid dgr, TextBox employee_name)
        {
            DataRowView drv = (DataRowView)dgr.SelectedItem;
            employee_name.Text = (drv[1]).ToString();
        }

        ///МАНИПУЛЯЦИИ С ТАБЛИЦЕЙ НАЗНАЧЕНИЙ///
        public static void add_assingment(ComboBox task_name, ComboBox employee_name)
        {
            if (employee_name.Text.Length != 0 && task_name.Text.Length!=0)
            {
                SqlCeConnection conn = null;

                try
                {
                    conn = new SqlCeConnection(DBHelper.ConnectionString);

                    conn.Open();

                    SqlCeCommand command = conn.CreateCommand();
                    command.CommandText = "INSERT INTO Assingment(ID_Task, ID_Employee) "
                    + "VALUES(@ID_Task, @ID_Employee)";
                    command.Parameters.Add("@ID_Task", SqlDbType.Int);
                    command.Parameters["@ID_Task"].Value = Convert.ToInt16(DBHelper.GetDataInt("select ID from Tasks where Name = '" + task_name.Text + "'","ID")[0]);
                    command.Parameters.Add("@ID_Employee", SqlDbType.Int);
                    command.Parameters["@ID_Employee"].Value = Convert.ToInt16(DBHelper.GetDataInt("select ID from Employee where Name = '" + employee_name.Text + "'", "ID")[0]);
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

        public static void update_assingment(DataGrid dgr, ComboBox task_name, ComboBox employee_name)
        {
            if (employee_name.Text.Length != 0 && task_name.Text.Length != 0)
            {
                DataRowView drv = (DataRowView)dgr.SelectedItem;
                SqlCeConnection conn = null;

                try
                {
                    conn = new SqlCeConnection(DBHelper.ConnectionString);

                    conn.Open();

                    SqlCeCommand command = conn.CreateCommand();
                    command.CommandText = "Update Assingment set ID_Task = @ID_Task, ID_Employee = @ID_Employee where ID = @ID ";
                    command.Parameters.Add("@ID_Task", SqlDbType.Int);
                    command.Parameters["@ID_Task"].Value = Convert.ToInt16(DBHelper.GetDataInt("select ID from Tasks where Name = '" + task_name.Text + "'", "ID")[0]);
                    command.Parameters.Add("@ID_Employee", SqlDbType.Int);
                    command.Parameters["@ID_Employee"].Value = Convert.ToInt16(DBHelper.GetDataInt("select ID from Employee where Name = '" + employee_name.Text + "'", "ID")[0]);
                    command.Parameters.Add("@ID", SqlDbType.Int);
                    command.Parameters["@ID"].Value = (drv[0]).ToString();
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

        public static void get_assingment_fields(DataGrid dgr, ComboBox task_name, ComboBox employee_name)
        {
            DataRowView drv = (DataRowView)dgr.SelectedItem;
            task_name.Text = (drv[1]).ToString();
            employee_name.Text = (drv[2]).ToString();
        }


        public static void delete_raw(string TableName, DataGrid dgr)
        {
            DataRowView drv = (DataRowView)dgr.SelectedItem;
            String result = (drv[0]).ToString();
            SqlCeConnection conn = null;
            try
            {
                conn = new SqlCeConnection(DBHelper.ConnectionString);
                conn.Open();
                SqlCeCommand command = conn.CreateCommand();
                command.CommandText = "DELETE from " + TableName + " where ID = " + result;
                command.ExecuteScalar();
            }
            catch
            {
                MessageBox.Show("Выделите строку для удаления!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (conn != null)
                    conn.Close();
            }
        }


    }
}
