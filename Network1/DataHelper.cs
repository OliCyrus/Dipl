using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.IO;

namespace Network1
{
    class DataHelper
    {
        private static string path = Directory.GetCurrentDirectory().ToString() + @"\Projects\";
        public static void fill_list(ListBox lst)
        {
            lst.Items.Clear();
            string[] dirs = Directory.GetFiles(path, "*.sdf");

            foreach (string dir in dirs)
            {
                string fName = dir.Substring(path.Length);

                lst.Items.Add(fName);
            }

        }

        public static void delete_file(ListBox lst)
        {
            while(lst.SelectedItems.Count>0)
            {
                string fName = path+lst.SelectedItems[0].ToString();
                //MessageBox.Show(fName);
                FileInfo fli = new FileInfo(fName);
                fli.Delete();
                lst.Items.Clear();
            }
        }

        public static void open_project(ListBox lst)
        {
            while (lst.SelectedItems.Count > 0)
            {
                string fName = lst.SelectedItems[0].ToString();
                DBHelper.Create_Db_File(fName);
                break;
            }
        }
    }
}
