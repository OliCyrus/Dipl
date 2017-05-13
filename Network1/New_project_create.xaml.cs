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

namespace Network1
{
    /// <summary>
    /// Логика взаимодействия для New_project_create.xaml
    /// </summary>
    public partial class New_project_create : Window
    {
        public New_project_create()
        {
            InitializeComponent();
            NPRJ_Textbox.Focus();
            NPRJ_Textbox.SelectAll();
        }

        private void NPRJ_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DBHelper.Create_Db_File(NPRJ_Textbox.Text + ".sdf");
                DBHelper.Create_DB();
                this.Close();
            }
            catch
            {
                MessageBox.Show("Не удалось создать проект!\n Попробуйте использовать другое имя проекта.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
