using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Windows;
using System.Windows.Input;
using Fingerprints.Resources;
using Fingerprints.Models;
using System.Collections.Specialized;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using ExceptionLogger;
using System.Linq;
using Fingerprints.Interfaces;

namespace Fingerprints.ViewModels
{
    class MainWindowViewModel : BindableBase, IDisposable
    {
        public ItemsChangeObservableCollection<MinutiaeStateViewModel> LeftDrawingData;
        public ItemsChangeObservableCollection<MinutiaeStateViewModel> RightDrawingData;
        public ICommand SaveClickCommand { get; }
        public WriteableBitmap LeftWriteableBmp { get; set; }

        private MinutiaeStateViewModel CurrentLeftDrawing;

        public MainWindowViewModel()
        {
            try
            {
                LeftWriteableBmp = new WriteableBitmap(620, 620, 96, 96, PixelFormats.Bgra32, null);

                LeftDrawingData = new ItemsChangeObservableCollection<MinutiaeStateViewModel>();
                RightDrawingData = new ItemsChangeObservableCollection<MinutiaeStateViewModel>();


                SaveClickCommand = new DelegateCommand(SaveClick);

                CurrentLeftDrawing = new CurveLineViewModel(LeftWriteableBmp);
                CurrentLeftDrawing.Minutia = new SelfDefinedMinutiae() { TypeId = 3, Name = "Krzywa!" };
                LeftDrawingData.Add(CurrentLeftDrawing);
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        public void LeftMouseMoveMethod(object sender, MouseEventArgs args)
        {
            try
            {
                if (CurrentLeftDrawing is IMouseMoveable)
                {
                    var drawing = (IMouseMoveable)(CurrentLeftDrawing);
                    drawing.MouseMoveMethod(sender, args);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        public void LeftMouseDownMethod(object sender, MouseButtonEventArgs args)
        {
            try
            {
                if (CurrentLeftDrawing is IMouseClickable)
                {
                    var drawing = (IMouseClickable)CurrentLeftDrawing;
                    drawing.MouseDownMethod(sender, args);
                }
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
                //FileTransfer.Save();

            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            try
            {
                if (!disposedValue)
                {
                    if (disposing)
                    {
                        // TODO: dispose managed state (managed objects).
                    }

                    // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                    // TODO: set large fields to null.

                    disposedValue = true;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~MainWindowViewModel() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
