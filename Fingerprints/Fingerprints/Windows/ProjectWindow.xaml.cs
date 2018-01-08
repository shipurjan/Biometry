using ExceptionLogger;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.IO;
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
    public partial class ProjectWindow : Window
    {
        public ProjectWindow()
        {
            //Get path of exe and set it as DataDirectory for EF
            string executable = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string path = (System.IO.Path.GetDirectoryName(executable));
            AppDomain.CurrentDomain.SetData("DataDirectory", path);
            InitializeComponent();
            listBoxRefresh();
        }
        private void listBoxRefresh()
        {
            ProjectsListBox.ItemsSource = Database.ShowProject();
        }

        private void DialogHost_OnDialogClosing(Object sender, DialogClosingEventArgs eventArgs)
        {
            if ((bool)eventArgs.Parameter)
            {
                Database.AddNewProject(ProjectNameTextBox.Text);
                listBoxRefresh();
                ProjectNameTextBox.Text = "";
            }

        }

        private void ProjectsListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                SelectProject();
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        private void SelectProject()
        {
            try
            {
                Project project = (Project)ProjectsListBox.SelectedItem;
                Database.currentProject = project.ProjectID;

                MainWindow win = new MainWindow();
                win.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        private void SelectProjectButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SelectProject();
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }



        private void ProjectsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (ProjectsListBox.SelectedIndex != -1)
                {
                    SelectProjectButton.IsEnabled = true;
                }
                else
                {
                    SelectProjectButton.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        private void MenuItemDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ProjectsListBox.SelectedIndex != -1)
                {
                    if (ProjectsListBox.SelectedValue != null && 
                        MessageBox.Show("Czy na pewno chcesz usunąć zaznaczony projekt?", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        Database.DeleteProject(ProjectsListBox.SelectedValue as Project);
                        listBoxRefresh();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }
    }
}
