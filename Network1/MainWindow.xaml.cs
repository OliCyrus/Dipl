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
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DBHelper.Create_Db_File("1.sdf");
                DBHelper.Create_DB();
                
            }
            catch
            {
                MessageBox.Show("PIZDEC!");
            }
            if (!DBHelper.IsDbFileExist)
                MessageBox.Show("pizdec!");
        }
    }
}
