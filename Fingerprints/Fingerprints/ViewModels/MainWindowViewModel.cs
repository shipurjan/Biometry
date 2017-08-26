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
    class MainWindowViewModel : BindableBase
    {
        public ObservableCollection<MinutiaState> LeftDrawingData;
        public ObservableCollection<MinutiaState> RightDrawingData;
        private WriteableBitmap _writeableBmp;

        private MinutiaState mState;

        public WriteableBitmap WriteableBmp
        {
            get { return _writeableBmp; }
            set { SetProperty(ref _writeableBmp, value); }
        }

        public MainWindowViewModel()
        {
            LeftDrawingData = new ObservableCollection<MinutiaState>();
            RightDrawingData = new ObservableCollection<MinutiaState>();
            LeftDrawingData.CollectionChanged += LeftDrawingDataChanged;
            RightDrawingData.CollectionChanged += LeftDrawingDataChanged;
            WriteableBmp = new WriteableBitmap(620, 620, 96, 96, PixelFormats.Bgra32, null);

            SaveClickCommand = new DelegateCommand(SaveClick);

            mState = mState = new MinutiaState();
            mState.Minutia = new SelfDefinedMinutiae() { TypeId = 3 };
            LeftDrawingData.Add(mState);
        }

        public ICommand SaveClickCommand { get; }

        public void SaveClick()
        {
            var pointsToTest = new List<Point>();
            Random r = new Random();
            pointsToTest.Clear();
            for (int i = 0; i < 1000; i++)
            {
                pointsToTest.Add(new Point(r.Next(0, 620), r.Next(0, 620)));
            }
            var minutiaState = new MinutiaState();
            minutiaState.Minutia = new SelfDefinedMinutiae() { TypeId = 3 };
            minutiaState.Points = pointsToTest;

            LeftDrawingData.Add(minutiaState);
        }

        public void LeftDrawingDataChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            foreach (var item in LeftDrawingData)
            {
                if (item.Minutia.TypeId == 3 && item.Points.Count > 0)
                {
                    WriteableBmp.DrawPolyline(item.intPoints.ToArray(), Colors.Red);
                }
            }
        }

        public void RightDrawingDataChanged(object sender, NotifyCollectionChangedEventArgs args)
        {

        }

        public void MouseMoveMethod(object sender, MouseEventArgs args)
        {
            if (args.MouseDevice.RightButton == MouseButtonState.Pressed)
            {
                var point = args.GetPosition((IInputElement)sender).ToFloorPoint();
                mState.Points.Add(point);
                RaisePropertyChanged();
            }
        }

        public void MouseRightButtonDownMethod(object sender, MouseButtonEventArgs args)
        {
            Console.WriteLine(args.ButtonState);
        }
    }
}
