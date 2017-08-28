using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Fingerprints.Resources;
using System.Collections.ObjectModel;
using Fingerprints.Models;
using System.Collections.Specialized;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace Fingerprints.ViewModels
{
    class MainWindowViewModel : BindableBase, IDisposable
    {
        public ItemsChangeObservableCollection<MinutiaState> LeftDrawingData;
        public ItemsChangeObservableCollection<MinutiaState> RightDrawingData;
        public ICommand SaveClickCommand { get; }
        public WriteableBitmap LeftWriteableBmp { get; set; }

        private MinutiaState mState;

        public MainWindowViewModel()
        {
            LeftWriteableBmp = new WriteableBitmap(620, 620, 96, 96, PixelFormats.Bgra32, null);

            LeftDrawingData = new ItemsChangeObservableCollection<MinutiaState>();
            RightDrawingData = new ItemsChangeObservableCollection<MinutiaState>();
            LeftDrawingData.CollectionChanged += LeftDrawingDataChanged;
            RightDrawingData.CollectionChanged += RightDrawingDataChanged;
            
            SaveClickCommand = new DelegateCommand(SaveClick);

            mState = mState = new MinutiaState();
            mState.PropertyChanged += (s, e) =>
            {
                Draw();
            };
            mState.Minutia = new SelfDefinedMinutiae() { TypeId = 3 };
            LeftDrawingData.Add(mState);
        }

        public void LeftDrawingDataChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            Draw();
        }

        public void RightDrawingDataChanged(object sender, NotifyCollectionChangedEventArgs args)
        {

        }

        public void LeftMouseMoveMethod(object sender, MouseEventArgs args)
        {
            if (args.MouseDevice.RightButton == MouseButtonState.Pressed)
            {
                var point = args.GetPosition((IInputElement)sender).ToFloorPoint();
                mState.Points.Add(point);
                mState.Id = mState.Points.Count;
            }
        }

        public void LeftMouseDownMethod(object sender, MouseButtonEventArgs args)
        {
            if (args.MouseDevice.RightButton == MouseButtonState.Pressed)
            {
                var point = args.GetPosition((IInputElement)sender).ToFloorPoint();
                mState.Points.Add(point);
                mState.Id = mState.Points.Count;
            }
        }

        private void Draw()
        {
            foreach (var item in LeftDrawingData)
            {
                if (item.Minutia.TypeId == 3 && item.Points.Count > 0)
                {
                    LeftWriteableBmp.DrawPolyline(item.intPoints.ToArray(), Colors.Red);
                }
            }
        }

        public void SaveClick()
        {
            FileTransfer.Save();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    LeftDrawingData.CollectionChanged -= LeftDrawingDataChanged;
                    RightDrawingData.CollectionChanged -= RightDrawingDataChanged;
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
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
