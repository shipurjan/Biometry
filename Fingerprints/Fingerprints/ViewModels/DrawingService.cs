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
using System.Collections.ObjectModel;

namespace Fingerprints.ViewModels
{
    public class DrawingService : IDisposable
    {
        public ObservableCollection<MinutiaStateBase> DrawingData;
        
        public WriteableBitmap WriteableBitmap { get; set; }

        public MinutiaStateBase CurrentDrawing { get; set; }

        public DrawingService()
        {
            try
            {
                WriteableBitmap = new WriteableBitmap(620, 620, 96, 96, PixelFormats.Bgra32, null);

                DrawingData = new ObservableCollection<MinutiaStateBase>();

                CurrentDrawing = new CurveLineState(WriteableBitmap, this);
                CurrentDrawing.Minutia = new SelfDefinedMinutiae() { TypeId = 3, Name = "Krzywa!" };
                DrawingData.Add(CurrentDrawing);
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        /// <summary>
        /// Method launched when MouseMove event is triggered
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void MouseMoveMethod(object sender, MouseEventArgs args)
        {
            try
            {
                if (CurrentDrawing is IMouseMoveable)
                {
                    ((IMouseMoveable)CurrentDrawing).MouseMove(sender, args);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        /// <summary>
        /// Method launched when MouseDown event is triggered
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void MouseDownMethod(object sender, MouseButtonEventArgs args)
        {
            try
            {
                if (CurrentDrawing is IMouseClickable)
                {
                    ((IMouseClickable)CurrentDrawing).MouseClick(sender, args);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        /// <summary>
        /// its clear WriteableBitmap and draws all items from DrawingData by lauching their DrawProcedure method
        /// </summary>
        public void Draw()
        {
            try
            {
                WriteableBitmap.Clear();
                foreach (var item in DrawingData)
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

        /// <summary>
        /// Inititates new drawing for CurrenDrawing object
        /// </summary>
        public void InitiateNewDrawing()
        {
            CurrentDrawing = new SegmentState(WriteableBitmap, this);
            CurrentDrawing.Minutia = new SelfDefinedMinutiae() { Name = "Prosta" };
        }

        /// <summary>
        /// Adds CurrentDrawing to DrawingData list, 
        /// When this method is launched, CurrentDrawing will appear on WriteableBitmap and listbox
        /// </summary>
        public void AddCurrentDrawingToDrawingData()
        {
            DrawingData.Add(CurrentDrawing);
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
