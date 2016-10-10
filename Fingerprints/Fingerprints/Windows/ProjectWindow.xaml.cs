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

namespace Fingerprints.Windows
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
            listBoxRefresh();
        }
        private void listBoxRefresh()
        {
            project_list.ItemsSource = Database.ShowProject();
        }

        private void add_button_Click(object sender, RoutedEventArgs e)
        {
            Database.AddNewProject(textBox.Text);
            listBoxRefresh();
        }

        private void delete_button_Click(object sender, RoutedEventArgs e)
        {
            Database.DeleteProject(project_list.SelectedValue as Project);
            listBoxRefresh();
        }

        private void open_project_button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Project project = (Project)project_list.SelectedItem;
                Database.currentProject = project.ProjectID;
                MainWindow win = new MainWindow();
                win.Show();
                this.Close();                
            }
            catch
            {
                Console.Beep();
            }
        }

    }
}
