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

        private List<Project> projects;

        public ProjectWindow()
        {
            try
            {
                //Get path of exe and set it as DataDirectory for EF
                string executable = System.Reflection.Assembly.GetExecutingAssembly().Location;
                string path = (System.IO.Path.GetDirectoryName(executable));
                AppDomain.CurrentDomain.SetData("DataDirectory", path);
                InitializeComponent();
                listBoxRefresh();
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }

        }
        private void listBoxRefresh()
        {
            try
            {
                projects = Database.ShowProject();
                ProjectsListBox.ItemsSource = projects;
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
            
        }

        private void DialogHost_OnDialogClosing(Object sender, DialogClosingEventArgs eventArgs)
        {
            try
            {
                if ((bool)eventArgs.Parameter)
                {
                    if (IsValidationCorrent())
                    {
                        Database.AddNewProject(ProjectNameTextBox.Text);
                        listBoxRefresh();
                        ProjectNameTextBox.Text = "";
                        ProjectNameValidationLabel.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        ProjectNameValidationLabel.Visibility = Visibility.Visible;
                        eventArgs.Cancel();
                    }
                }
                else
                {
                    ProjectNameValidationLabel.Visibility = Visibility.Collapsed;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
            
        }

        private bool IsValidationCorrent()
        {
            bool result = true;
            try
            {
                if (String.IsNullOrWhiteSpace(ProjectNameTextBox.Text))
                {
                    result = false;
                    ProjectNameValidationLabel.Content = "Nazwa projektu jest wymagana";
                }

                if (projects.Exists(x => x.Name.ToLower() == ProjectNameTextBox.Text.ToLower()))
                {
                    result = false;
                    ProjectNameValidationLabel.Content = "Projekt o takiej nazwie jest już dodany";
                }
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
            return result;
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

        private void PackIcon_MouseUp(object sender, MouseButtonEventArgs e)
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
