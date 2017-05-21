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
        bool IsUpdatingTasks = false;
        bool IsUpdatingEmp_Ass = false;
        bool IsUpdatingAssingment = false;
        public MainWindow()
        {
            InitializeComponent();
            this.Title = "Проект " + DBHelper.DbFile.ToString();
            DBHelper.FillData(DBHelper.TasksSelect, Tasks_View);
            DBHelper.FillData(DBHelper.EmployeeSelect, Employee_View);
            DBHelper.FillData(DBHelper.AssingmentSelect, Assingment_View);
        }

        private void Exit_Menu_Item_Click(object sender, RoutedEventArgs e)
        {
            Start_Window str = new Start_Window();
            str.Show();
            this.Close();
        }

        private void Add_Task_Button_Click(object sender, RoutedEventArgs e)   //добавление или изменение задания
        {
            if (IsUpdatingTasks)
            {
                DBHelper.update_task(Tasks_View, Task_Name, Start_date_picker, Finish_date_picker);
                IsUpdatingTasks = false;
            }
            else
            {
                DBHelper.add_task(Task_Name, Start_date_picker, Finish_date_picker);
            }
            clear_task_fields();  //очистка полей
            unable_task_buttons();     // разблокирование кнопок, если происходило обновление
            DBHelper.FillData(DBHelper.TasksSelect, Tasks_View);
        }

        private void Delete_Task_Button_Click(object sender, RoutedEventArgs e) //удаление задания
        {
            DBHelper.delete_raw("Tasks", Tasks_View);
            DBHelper.FillData(DBHelper.TasksSelect, Tasks_View);
        }

        private void Update_tasks_list_button_Click(object sender, RoutedEventArgs e)
        {
            DBHelper.FillData(DBHelper.TasksSelect, Tasks_View);
        }

        private void Tasks_View_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsUpdatingTasks)
            {
                DBHelper.get_tasks_fields(Tasks_View, Task_Name, Start_date_picker, Finish_date_picker);
            }

        }

        private void Update_Task_Button_Click(object sender, RoutedEventArgs e)
        {
            if (IsUpdatingTasks)
            {
                IsUpdatingTasks = false;
                Update_Task_Button.Content = "Изменить";
                unable_task_buttons();
                clear_task_fields();
            }
            else
            {
                IsUpdatingTasks = true;
                Update_Task_Button.Content = "Отмена";
                unable_task_buttons();
            }


        }

        private void unable_task_buttons()
        {
            if (IsUpdatingTasks)
            {
                Delete_Task_Button.IsEnabled = false;
                Update_tasks_list_button.IsEnabled = false;
            }
            else if (!IsUpdatingTasks)
            {
                Delete_Task_Button.IsEnabled = true;
                Update_tasks_list_button.IsEnabled = true;
            }

        }

        private void unable_buttons()
        {
            //switch(IsUpdatingEmp_Ass)
            //{
            //    case true:
            //        Delete_button.IsEnabled = false;
            //        Update_list_button.IsEnabled = false;
            //        break;
            //    case false:
            //        Delete_button.IsEnabled = true;
            //        Update_list_button.IsEnabled = true;
            //        break;
            //}

            //switch(IsUpdatingAssingment)
            //{
            //    case true:

            //}

            if (IsUpdatingAssingment || IsUpdatingEmp_Ass)
            {
                Delete_button.IsEnabled = false;
                Update_list_button.IsEnabled = false;
            }
            else
            {
                Delete_button.IsEnabled = true;
                Update_list_button.IsEnabled = true;
            }
        }

        private void clear_task_fields()
        {
            Task_Name.Clear();
            Start_date_picker.Text = "";
            Finish_date_picker.Text = "";
        }


        private void Add_Button_Click(object sender, RoutedEventArgs e)
        {
            if (Data_Tab_Control.SelectedIndex == 1)
            {
                if (IsUpdatingEmp_Ass)
                {
                    DBHelper.update_employee(Employee_View, Employee_Name_Textbox);
                    Employee_Name_Textbox.Clear();
                    IsUpdatingEmp_Ass = false;
                    Update_button.Content = "Изменить";
                    unable_buttons();
                    DBHelper.FillData(DBHelper.EmployeeSelect, Employee_View);
                }
                else
                {
                    DBHelper.add_employee(Employee_Name_Textbox);
                    DBHelper.FillData(DBHelper.EmployeeSelect, Employee_View);
                    Employee_Name_Textbox.Clear();
                }


            }

            else if (Data_Tab_Control.SelectedIndex == 0)
            {
                if (IsUpdatingAssingment)
                {
                    DBHelper.update_assingment(Assingment_View, Tasks_Combo_Box, Empl_Combo_Box);
                    Tasks_Combo_Box.Text = "";
                    Empl_Combo_Box.Text = "";
                    IsUpdatingAssingment = false;
                    Update_button.Content = "Изменить";
                    unable_buttons();
                    DBHelper.FillData(DBHelper.AssingmentSelect, Assingment_View);

                }
                else
                {
                    DBHelper.add_assingment(Tasks_Combo_Box, Empl_Combo_Box);
                    DBHelper.FillData(DBHelper.AssingmentSelect, Assingment_View);
                    Tasks_Combo_Box.Text = "";
                    Empl_Combo_Box.Text = "";
                }

            }

        }

        private void Delete_button_Click(object sender, RoutedEventArgs e)
        {
            if (Data_Tab_Control.SelectedIndex == 1)
            {
                DBHelper.delete_raw("Employee", Employee_View);
                DBHelper.FillData(DBHelper.EmployeeSelect, Employee_View);
            }
            else if (Data_Tab_Control.SelectedIndex == 0)
            {
                DBHelper.delete_raw("Assingment", Assingment_View);
                DBHelper.FillData(DBHelper.AssingmentSelect, Assingment_View);
            }
        }

        private void Update_button_Click(object sender, RoutedEventArgs e)
        {
            if (Data_Tab_Control.SelectedIndex == 1)
            {
                if (IsUpdatingEmp_Ass)
                {
                    IsUpdatingEmp_Ass = false;
                    Update_button.Content = "Изменить";
                    Employee_Name_Textbox.Clear();
                    unable_buttons();
                }
                else if (!IsUpdatingEmp_Ass)
                {
                    IsUpdatingEmp_Ass = true;
                    Update_button.Content = "Отмена";
                    unable_buttons();
                }
            }

            else if (Data_Tab_Control.SelectedIndex == 0)
            {
                if (IsUpdatingAssingment)
                {
                    IsUpdatingAssingment = false;
                    Update_button.Content = "Изменить";
                    Tasks_Combo_Box.Text = "";
                    Empl_Combo_Box.Text = "";
                    unable_buttons();

                }
                else if (!IsUpdatingAssingment)
                {
                    IsUpdatingAssingment = true;
                    Update_button.Content = "Отмена";
                    unable_buttons();
                }
            }

        }

        private void Update_list_button_Click(object sender, RoutedEventArgs e)
        {
            if (Data_Tab_Control.SelectedIndex == 0)
            {
                DBHelper.FillData(DBHelper.AssingmentSelect, Assingment_View);
            }
            else if (Data_Tab_Control.SelectedIndex == 1)
            {
                DBHelper.FillData(DBHelper.EmployeeSelect, Employee_View);
            }
        }

        private void Tasks_Combo_Box_DropDownOpened(object sender, EventArgs e)
        {
            Tasks_Combo_Box.ItemsSource = DBHelper.GetDataString("select Name from Tasks", "Name");
        }

        private void Empl_Combo_Box_DropDownOpened(object sender, EventArgs e)
        {
            Empl_Combo_Box.ItemsSource = DBHelper.GetDataString("select Name from Employee", "Name");
        }

        private void Employee_View_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsUpdatingEmp_Ass)
            {
                DBHelper.get_emoloyee_fields(Employee_View, Employee_Name_Textbox);
            }
        }

        private void Assingment_View_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsUpdatingAssingment)
            {
                DBHelper.get_assingment_fields(Assingment_View, Tasks_Combo_Box, Empl_Combo_Box);
            }
        }

        private void Data_Tab_Control_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsUpdatingAssingment || IsUpdatingEmp_Ass)
            {
                IsUpdatingEmp_Ass = false;
                IsUpdatingAssingment = false;
                unable_buttons();
                Update_button.Content = "Изменить";
            }
        }
    }
}
