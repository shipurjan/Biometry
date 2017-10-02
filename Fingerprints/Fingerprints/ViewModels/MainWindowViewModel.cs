using ExceptionLogger;
using Fingerprints.Factories;
using Fingerprints.MinutiaeTypes;
using Fingerprints.Models;
using Fingerprints.Resources;
using Fingerprints.Tools.Exporters;
using Prism.Commands;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Fingerprints.ViewModels
{
    class MainWindowViewModel
    {
        private MinutiaeTypeController dbController;

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

                //Initialize Drawing Services
                LeftDrawingService = new DrawingService();
                RightDrawingService = new DrawingService();

                //Add method for CollectinoChanged
                LeftDrawingData.CollectionChanged += LeftDrawingDataChanged;
                RightDrawingData.CollectionChanged += RightDrawingDataChanged;

                //Get MinutiaeStates for combobox
                MinutiaeStates = new ObservableCollection<MinutiaState>(dbController.getStates());

                //button clicks delegates
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
                string leftFileName = LeftDrawingService.BackgroundImage.GetFileName();
                string rightFileName = RightDrawingService.BackgroundImage.GetFileName();
                ExportService.SaveAsFileDialog(LeftDrawingData.ToList(), leftFileName, RightDrawingData.ToList(), rightFileName);
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
                string leftPath = Path.ChangeExtension(LeftDrawingService.BackgroundImage.UriSource.AbsolutePath, ".txt");
                string rightPath = Path.ChangeExtension(RightDrawingService.BackgroundImage.UriSource.AbsolutePath, ".txt");

                ExportService.SaveTxt(LeftDrawingData.ToList(), leftPath, RightDrawingData.ToList(), rightPath);
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }
    }
}
