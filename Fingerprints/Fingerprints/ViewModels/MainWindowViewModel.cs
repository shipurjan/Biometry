using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Windows.Input;
using Fingerprints.Resources;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using ExceptionLogger;
using Fingerprints.Interfaces;
using Fingerprints.MinutiaeTypes;

namespace Fingerprints.ViewModels
{
    class MainWindowViewModel : BindableBase, IDisposable
    {
        public ItemsChangeObservableCollection<MinutiaStateBase> LeftDrawingData;
        public ItemsChangeObservableCollection<MinutiaStateBase> RightDrawingData;
        public ICommand SaveClickCommand { get; }
        public WriteableBitmap LeftWriteableBmp { get; set; }

        private MinutiaStateBase CurrentLeftDrawing;

        public MainWindowViewModel()
        {
            try
            {
                LeftWriteableBmp = new WriteableBitmap(620, 620, 96, 96, PixelFormats.Bgra32, null);

                LeftDrawingData = new ItemsChangeObservableCollection<MinutiaStateBase>();
                RightDrawingData = new ItemsChangeObservableCollection<MinutiaStateBase>();

                SaveClickCommand = new DelegateCommand(SaveClick);

                CurrentLeftDrawing = new CurveLineState(LeftWriteableBmp, this);
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
                    ((IMouseMoveable)CurrentLeftDrawing).MouseMove(sender, args);
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
                    ((IMouseClickable)CurrentLeftDrawing).MouseClick(sender, args);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        public void Draw()
        {
            try
            {
                LeftWriteableBmp.Clear();
                foreach (var item in LeftDrawingData)
                {
                    if (item is IDrawable)
                    {
                        ((IDrawable)item).DrawProcedure();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        public void NewDrawing()
        {
            CurrentLeftDrawing = new LineViewState(LeftWriteableBmp, this);
            CurrentLeftDrawing.Minutia = new SelfDefinedMinutiae() { Name = "Prosta" };
        }

        public void AddToList()
        {
            LeftDrawingData.Add(CurrentLeftDrawing);
        }

        public void SaveClick()
        {
            try
            {
                CurrentLeftDrawing = new LineViewState(LeftWriteableBmp, this);
                CurrentLeftDrawing.Minutia = new SelfDefinedMinutiae() { Name = "Prosta" };
                LeftDrawingData.Add(CurrentLeftDrawing);
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
