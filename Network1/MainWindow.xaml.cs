using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlServerCe;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Data.Odbc;
using System.Data;

namespace Network1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Title = "Проект " + DBHelper.DbFile.ToString();
            DBHelper.FillData("select ID[ID], Name[NAME] from Tasks ", Tasks_View);


        }

        private void Exit_Menu_Item_Click(object sender, RoutedEventArgs e)
        { 
            Start_Window str = new Start_Window();
            str.Show();
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DBHelper.FillData("select ID[ID], Name[NAME] from Tasks ", Tasks_View);
        }

        private void Add_Task_Button_Click(object sender, RoutedEventArgs e)   //добавление задания
        {
            DBHelper.add_task(Task_Name);
            DBHelper.FillData("select ID[ID], Name[NAME] from Tasks ", Tasks_View);
        }
    }
}
