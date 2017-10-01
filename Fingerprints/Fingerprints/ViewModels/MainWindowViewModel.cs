using ExceptionLogger;
using Fingerprints.Factories;
using Fingerprints.MinutiaeTypes;
using Fingerprints.Models;
using Fingerprints.Tools.Exporters;
using Prism.Commands;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Input;

namespace Fingerprints.ViewModels
{
    class MainWindowViewModel
    {
        private MinutiaeTypeController dbController;

        private ExportService exportService;

        private bool _bCanComboBoxChangeCurrentDrawing;

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
        public ICommand SaveAsClickCommand { get; }

        public MainWindowViewModel()
        {
            try
            {
                _bCanComboBoxChangeCurrentDrawing = true;

                dbController = new MinutiaeTypeController();
                exportService = new ExportService();

                //TODO DrawingService must be initialize after picture load
                LeftDrawingService = new DrawingService();
                RightDrawingService = new DrawingService();

                LeftDrawingData.CollectionChanged += LeftDrawingDataChanged;
                RightDrawingData.CollectionChanged += RightDrawingDataChanged;

                MinutiaeStates = new ObservableCollection<MinutiaState>(dbController.getStates());

                SaveClickCommand = new DelegateCommand(SaveClick);
                cbMinutiaeStatesSelectionChanged = new DelegateCommand<MinutiaState>(cbMinutiaStatesSelectionChanged, CanComboBoxChangeCurrentDrawing);
                SaveAsClickCommand = new DelegateCommand(SaveAsClick);
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        private void SaveAsClick()
        {
            try
            {
                exportService.SaveAsFileDialog(LeftDrawingData.ToList(), "1.jpg", RightDrawingData.ToList(), "2.jpg");
            }   
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        private bool CanComboBoxChangeCurrentDrawing(MinutiaState _oSelectedMinutiaState)
        {
            return _bCanComboBoxChangeCurrentDrawing;
        }

        private void cbMinutiaStatesSelectionChanged(MinutiaState _oSelectedMinutiaState)
        {
            try
            {
                LeftDrawingService.CurrentDrawing = MinutiaStateFactory.Create(_oSelectedMinutiaState.Minutia, LeftDrawingService);
                RightDrawingService.CurrentDrawing = MinutiaStateFactory.Create(_oSelectedMinutiaState.Minutia, RightDrawingService);
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
                //FileTransfer.Save();
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }
    }
}
