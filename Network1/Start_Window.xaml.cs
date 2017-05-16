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
using System.Windows.Shapes;
using System.IO;

namespace Network1
{
    /// <summary>
    /// Логика взаимодействия для Start_Window.xaml
    /// </summary>
    public partial class Start_Window : Window
    {
        public Start_Window()
        {
            InitializeComponent();
            DataHelper.fill_list(Projects_List);
        }

        private void NEW_PRJ_Button_Click(object sender, RoutedEventArgs e)  //создать проект
        {
           // this.Opacity = 0.5;
            New_project_create npj = new New_project_create();
            npj.ShowDialog();

        }

        private void Button_Click(object sender, RoutedEventArgs e)  //обновить список
        {
            DataHelper.fill_list(Projects_List);
        }

        private void DEL_PRJ_Button_Click(object sender, RoutedEventArgs e)  //удалить проект(файл)
        {
            DataHelper.delete_file(Projects_List);
            DataHelper.fill_list(Projects_List);
        }

        private void OPEN_PRJ_Button_Click(object sender, RoutedEventArgs e)  //открыть проект для использования
        {
            try
            {
                DataHelper.open_project(Projects_List);
                MainWindow mwn = new MainWindow();
                mwn.Show();
                this.Hide();
            }
            catch
            {
                MessageBox.Show("Не удалось открыть проект", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
