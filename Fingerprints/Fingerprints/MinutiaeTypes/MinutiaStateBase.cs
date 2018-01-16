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
using System.Linq;
using System.Windows.Media;
using Fingerprints.Models;
using System.Windows.Input;

namespace Fingerprints.MinutiaeTypes
{
    public abstract class MinutiaStateBase : BindableBase, IDisposable
    {
        public SelfDefinedMinutiae Minutia { get; set; }

        public ObservableCollection<Point> Points { get; }
        public WriteableBitmap WriteableBmp { get; }

        public event EventHandler InitiateNewDrawing;

        private double _Angle;
        public double Angle
        {
            get { return _Angle; }
            set { SetProperty(ref _Angle, value); }
        }

        private string _Id;
        public string Id
        {
            get { return _Id; }
            set { SetProperty(ref _Id, value); }
        }

        private Color _Color;
        public Color Color
        {
            get { return _Color; }
            set { SetProperty(ref _Color, value); }
        }

        private int? insertIndex;
        public int? InsertIndex
        {
            get { return insertIndex; }
            set
            {
                insertIndex = value;
            }
        }

        private bool willBeReplaced;
        public bool WillBeReplaced
        {
            get { return willBeReplaced; }
            set { SetProperty(ref willBeReplaced, value); }
        }

        private bool isSelected;
        public bool IsSelected
        {
            get { return isSelected; }
            set { SetProperty(ref isSelected, value); }
        }

        private bool acceptButtonVisibility;
        public bool AcceptButtonVisibility
        {
            get { return acceptButtonVisibility; }
            set { SetProperty(ref acceptButtonVisibility, value); }
        }

        public string MinutiaName
        {
            get
            {
                string minutiaName = string.Empty;
                try
                {
                    minutiaName = Minutia.Name;
                }
                catch (Exception ex)
                {
                    Logger.WriteExceptionLog(ex);
                }
                return minutiaName;
            }
        }

        /// <summary>
        /// array of Points ( gets Points List and copy to array of int (x, y, x, y...))
        /// </summary>
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

        public ICommand AcceptButtonCommand { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="_oDrawingService"></param>
        /// <param name="_atIndex">index where Minutia must be added</param>
        public MinutiaStateBase(SelfDefinedMinutiae _minutia, WriteableBitmap _writeableBitmap, int? _atIndex = null)
        {
            try
            {
                Points = new ObservableCollection<Point>();
                InsertIndex = _atIndex;
                Minutia = _minutia;
                WriteableBmp = _writeableBitmap;
                AcceptButtonVisibility = false;
                IsSelected = false;

                if (Minutia != null && Minutia.DrawingType != DrawingType.Empty)
                {
                    WillBeReplaced = true;

                    Color = (Color)ColorConverter.ConvertFromString(Minutia.Color);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        protected void InitNewDrawing()
        {
            try
            {
                InitiateNewDrawing(this, null);
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
