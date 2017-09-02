using ExceptionLogger;
using Fingerprints.MinutiaeTypes;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Fingerprints.ViewModels
{
    class MainWindowViewModel
    {
        public DrawingService LeftDrawingService { get; }

        public DrawingService RightDrawingService { get; }

        public ICommand SaveClickCommand { get; }

        public MainWindowViewModel()
        {
            try
            {
                LeftDrawingService = new DrawingService();
                RightDrawingService = new DrawingService();
                
                SaveClickCommand = new DelegateCommand(SaveClick);
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        public void SaveClick()
        {
            try
            {
                LeftDrawingService.CurrentDrawing = new LineViewState(LeftDrawingService.WriteableBitmap, LeftDrawingService);
                LeftDrawingService.CurrentDrawing.Minutia = new SelfDefinedMinutiae() { Name = "Prosta" };

                RightDrawingService.CurrentDrawing = new LineViewState(RightDrawingService.WriteableBitmap, RightDrawingService);
                RightDrawingService.CurrentDrawing.Minutia = new SelfDefinedMinutiae() { Name = "Prosta" };
                //FileTransfer.Save();

            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }
    }
}
