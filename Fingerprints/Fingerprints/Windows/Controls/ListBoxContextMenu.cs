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
            Items.Add(BuildInsertMenuItem());
            drawingService = _drawingService;
            oppositeDrawingService = _oppositeDrawingService;
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
