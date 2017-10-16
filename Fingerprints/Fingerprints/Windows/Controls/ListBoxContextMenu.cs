using ExceptionLogger;
using Fingerprints.Factories;
using Fingerprints.MinutiaeTypes;
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

        DrawingService drawingService;

        DrawingService oppositeDrawingService;
        public ListBoxContextMenu(DrawingService _drawingService, DrawingService _oppositeDrawingService)
        {
            dbController = new MinutiaeTypeController();
            Style = Application.Current.FindResource("contextMenuStyles") as Style;

            Items.Add(BuildInsertMenuItem());
            Items.Add(BuildDeleteMenuItem());

            drawingService = _drawingService;
            oppositeDrawingService = _oppositeDrawingService;
        }

        private MenuItem BuildDeleteMenuItem()
        {
            MenuItem result = null;
            try
            {
                result = new MenuItem() { Header = "Usuń" };

                result.Click += (s, e) =>
                {
                    if (drawingService.ListBoxSelectedIndex.HasValue)
                    {
                        drawingService.DrawingData.RemoveAt(drawingService.ListBoxSelectedIndex.Value);
                        drawingService.Draw();
                    }
                };
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
            return result;
        }

        /// <summary>
        /// Creates context menu for listbox
        /// </summary>
        /// <returns></returns>
        private MenuItem BuildInsertMenuItem()
        {
            MenuItem result = null;
            var minutiaes = dbController.getStates();

            try
            {
                result = new MenuItem() { Header = "Wstaw" };
                result.Style = Application.Current.FindResource("menuItemStyles") as Style;

                //create menuitem foreach minutia state with click event to start new drawing
                foreach (var minutia in minutiaes)
                {
                    MenuItem menuItem = new MenuItem() { Header = minutia.MinutiaName };
                    menuItem.Click += (s, e) =>
                    {
                        drawingService.CurrentDrawing = MinutiaStateFactory.Create(minutia.Minutia, drawingService, drawingService.ListBoxSelectedIndex);
                        oppositeDrawingService.CurrentDrawing = MinutiaStateFactory.Create(minutia.Minutia, oppositeDrawingService);
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
