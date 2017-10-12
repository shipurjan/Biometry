using ExceptionLogger;
using Fingerprints.Factories;
using Fingerprints.Models;
using Fingerprints.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Fingerprints.Windows.Controls
{
    class ListBoxContextMenu : ContextMenu
    {
        MinutiaeTypeController dbController;

        MainWindowViewModel viewModel;
        public ListBoxContextMenu(MainWindowViewModel _viewModel)
        {
            dbController = new MinutiaeTypeController();
            Items.Add(BuildInsertMenuItem());
            viewModel = _viewModel;
        }

        private MenuItem BuildInsertMenuItem()
        {
            MenuItem result = null;
            var minutiaes = dbController.getStates();

            try
            {
                result = new MenuItem() { Header = "Wstaw" };

                foreach (var minutia in minutiaes)
                {
                    MenuItem menuItem = new MenuItem() { Header = minutia.MinutiaName };
                    menuItem.Click += (s, e) =>
                    {
                        viewModel.LeftDrawingService.CurrentDrawing = MinutiaStateFactory.Create(minutia.Minutia, viewModel.LeftDrawingService);
                    };
                    result.Items.Add(menuItem);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
            return result;
        }
    }
}
