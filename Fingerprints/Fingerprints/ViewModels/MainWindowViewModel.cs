using ExceptionLogger;
using Fingerprints.Factories;
using Fingerprints.MinutiaeTypes;
using Fingerprints.Models;
using Fingerprints.Tools;
using Fingerprints.Tools.Exporters;
using Microsoft.Win32;
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

                //DrawingService must be initialize after picture load
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
            SaveFileDialog saveFileDialog = null;
            DataExporter leftDataExporter = null;
            DataExporter rightDataExporter = null;
            try
            {
                saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Bozorth (.xyt)|*.xyt|Default (.txt)|*.txt";
                saveFileDialog.Title = "Save an Image File";
                saveFileDialog.ShowDialog();

                if (saveFileDialog.FileName != "")
                {
                    switch (saveFileDialog.FilterIndex)
                    {
                        case 1:
                            leftDataExporter = new XytExporter(LeftDrawingData.ToList());
                            rightDataExporter = new XytExporter(RightDrawingData.ToList());
                            break;
                        case 2:
                            leftDataExporter = new TxtExporter(LeftDrawingData.ToList());
                            rightDataExporter = new TxtExporter(RightDrawingData.ToList());
                            break;
                    }

                    if (leftDataExporter != null && rightDataExporter != null)
                    {
                        leftDataExporter.FormatData();
                        rightDataExporter.FormatData();

                        leftDataExporter.Export(FileTransfer.getPath(saveFileDialog.FileName, "1.jpg"));
                        rightDataExporter.Export(FileTransfer.getPath(saveFileDialog.FileName, "2.jpg"));
                    }
                }
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
                //FileTransfer.Save();
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }
    }
}
