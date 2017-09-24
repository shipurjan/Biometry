using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;
using System.ComponentModel;
using System.Collections.Specialized;
using ExceptionLogger;
using System.Windows;
using Fingerprints.ViewModels;

namespace Fingerprints.MinutiaeTypes
{
    public abstract class MinutiaStateBase : BindableBase, IDisposable
    {
        public SelfDefinedMinutiae Minutia { get; set; }

        public ObservableCollection<Point> Points { get; }

        private double _Angle;

        public double Angle { get { return _Angle; } set { SetProperty(ref _Angle, value); } }

        private long _Id;

        public long Id { get { return _Id; } set { SetProperty(ref _Id, value); } }

        public WriteableBitmap WriteableBmp
        {
            get
            {
                return DrawingService.WriteableBitmap;
            }
        }

        public DrawingService DrawingService { get; }

        public string MinutiaName { get { return Minutia.Name; } }

        public int[] IntPoints
        {
            get
            {
                List<int> intList = new List<int>();
                try
                {
                    foreach (var item in Points)
                    {
                        intList.Add(Convert.ToInt16(item.X));
                        intList.Add(Convert.ToInt16(item.Y));
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteExceptionLog(ex);
                }

                return intList.ToArray();
            }
        }

        public MinutiaStateBase(DrawingService _oDrawingService)
        {
            try
            {
                DrawingService = _oDrawingService;
                Points = new ObservableCollection<Point>();
                PropertyChanged += PropertyChangeHandler;
                Points.CollectionChanged += CollectionChangedHandler;
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        private void CollectionChangedHandler(object sender, NotifyCollectionChangedEventArgs e)
        {
            try
            {
                if (e.Action == NotifyCollectionChangedAction.Add && e.NewStartingIndex == 0)
                {
                    DrawingService.AddCurrentDrawingToDrawingData();
                }
                DrawingService.Draw();
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        private void PropertyChangeHandler(object sender, PropertyChangedEventArgs e)
        {
            try
            {
                DrawingService.Draw();
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
                        PropertyChanged -= PropertyChangeHandler;
                        Points.CollectionChanged -= CollectionChangedHandler;
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
        // ~MinutiaeStateViewModel() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
