using ExceptionLogger;
using Fingerprints.Factories;
using Fingerprints.MinutiaeTypes;
using Fingerprints.Models;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Fingerprints.ViewModels
{
    class MainWindowViewModel
    {
        private MinutiaeTypeController dbController;

        private bool _bCanComboBoxChangeCurrectDrawing;

        public DrawingService LeftDrawingService { get; }

        public DrawingService RightDrawingService { get; }

        public ObservableCollection<MinutiaState> MinutiaeStates { get; set; }

        public ObservableCollection<MinutiaStateBase> LeftDrawingData
        {
            get
            {
                return LeftDrawingService.DrawingData;
            }
        }

        public ObservableCollection<MinutiaStateBase> RightDrawingData
        {
            get
            {
                return RightDrawingService.DrawingData;
            }
        }

        public ICommand SaveClickCommand { get; }

        public ICommand cbMinutiaeStatesSelectionChanged { get; }

        public MainWindowViewModel()
        {
            try
            {
                _bCanComboBoxChangeCurrectDrawing = true;

                dbController = new MinutiaeTypeController();

                //DrawingService must be initialize after picture load
                LeftDrawingService = new DrawingService();
                RightDrawingService = new DrawingService();

                LeftDrawingData.CollectionChanged += LeftDrawingDataChanged;
                RightDrawingData.CollectionChanged += RightDrawingDataChanged;

                MinutiaeStates = new ObservableCollection<MinutiaState>(dbController.getStates());

                SaveClickCommand = new DelegateCommand(SaveClick);
                cbMinutiaeStatesSelectionChanged = new DelegateCommand<MinutiaState>(cbMinutiaStatesSelectionChanged, CanComboBoxChangeCurrectDrawing);
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        private bool CanComboBoxChangeCurrectDrawing(MinutiaState _oSelectedMinutiaState)
        {
            return _bCanComboBoxChangeCurrectDrawing;
        }

        private void cbMinutiaStatesSelectionChanged(MinutiaState _oSelectedMinutiaState)
        {
            try
            {
                LeftDrawingService.CurrentDrawing = MinutiaStateFactory.Create(_oSelectedMinutiaState.Minutia, LeftDrawingService.WriteableBitmap, LeftDrawingService);
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        private void RightDrawingDataChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            //code for inserting empty minutiae
            //code for asign minutiaID
        }

        public void LeftDrawingDataChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            //code for inserting empty minutiae
            //code for asign minutiaID
        }

        public void SaveClick()
        {
            try
            {
                // for testing
                LeftDrawingService.CurrentDrawing = new SegmentState(LeftDrawingService.WriteableBitmap, LeftDrawingService);
                LeftDrawingService.CurrentDrawing.Minutia = new SelfDefinedMinutiae() { Name = "Prosta" };

                RightDrawingService.CurrentDrawing = new SegmentState(RightDrawingService.WriteableBitmap, RightDrawingService);
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
