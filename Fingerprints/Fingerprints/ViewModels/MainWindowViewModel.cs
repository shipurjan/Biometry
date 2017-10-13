using ExceptionLogger;
using Fingerprints.Factories;
using Fingerprints.MinutiaeTypes;
using Fingerprints.Models;
using Fingerprints.Resources;
using Fingerprints.Tools.Exporters;
using Fingerprints.Windows.Controls;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Fingerprints.ViewModels
{
    class MainWindowViewModel : BindableBase
    {
        private MinutiaeTypeController dbController;

        private bool _bCanComboBoxChangeCurrentDrawing;

        public ObservableCollection<MinutiaState> MinutiaeStates { get; set; }

        public DrawingService LeftDrawingService { get; }

        public DrawingService RightDrawingService { get; }


        public ObservableCollection<MinutiaStateBase> LeftDrawingData
        {
            get { return LeftDrawingService.DrawingData; }
        }

        public ObservableCollection<MinutiaStateBase> RightDrawingData
        {
            get { return RightDrawingService.DrawingData; }
        }

        public ListBoxContextMenu LeftListBoxContextMenu { get; }

        public ListBoxContextMenu RightListBoxContextMenu { get; }

        public ICommand SaveClickCommand { get; }
        public ICommand MinutiaeStatesSelectionChanged { get; }
        public ICommand SaveAsClickCommand { get; }
        public ICommand LoadLeftImageCommand { get; }
        public ICommand LoadRightImageCommand { get; }

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

                //Init context menu for listboxes
                LeftListBoxContextMenu = new ListBoxContextMenu(LeftDrawingService, RightDrawingService);
                RightListBoxContextMenu = new ListBoxContextMenu(RightDrawingService, LeftDrawingService);

                //button clicks delegates
                SaveClickCommand = new DelegateCommand(SaveClick);
                MinutiaeStatesSelectionChanged = new DelegateCommand<MinutiaState>(MinutiaStatesSelectionChanged, CanComboBoxChangeCurrentDrawing);
                SaveAsClickCommand = new DelegateCommand(SaveAsClick);
                LoadLeftImageCommand = new DelegateCommand(LoadLeftImage);
                LoadRightImageCommand = new DelegateCommand(LoadRightImage);
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        private void LoadRightImage()
        {
            try
            {
                RightDrawingService.LoadImage();
                FillEmpty(RightDrawingService, LeftDrawingData.Count - RightDrawingData.Count);
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        private void LoadLeftImage()
        {
            try
            {
                LeftDrawingService.LoadImage();
                FillEmpty(LeftDrawingService, RightDrawingData.Count - LeftDrawingData.Count);
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        /// <summary>
        /// Peform SaveAs dialog window to save data
        /// </summary>
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

        /// <summary>
        /// Methods indicates if MinutiaStatesSelectionChanged method can run
        /// </summary>
        /// <param name="_oSelectedMinutiaState"></param>
        /// <returns></returns>
        private bool CanComboBoxChangeCurrentDrawing(MinutiaState _oSelectedMinutiaState)
        {
            return _bCanComboBoxChangeCurrentDrawing;
        }

        /// <summary>
        /// Initiate new drawing for left and right drawing serivce
        /// </summary>
        /// <param name="_oSelectedMinutiaState"></param>
        private void MinutiaStatesSelectionChanged(MinutiaState _oSelectedMinutiaState)
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

        private void RightDrawingDataChanged(object _sender, NotifyCollectionChangedEventArgs _eventArgs)
        {
            //code for inserting empty minutiae
            //code for asign minutiaID
            ObservableCollection<MinutiaStateBase> senderObject = null;
            try
            {
                senderObject = (ObservableCollection<MinutiaStateBase>)_sender;

                if (_eventArgs.Action == NotifyCollectionChangedAction.Add && senderObject.Count > 0)
                {
                    AssignNewIDIfCan(_eventArgs);

                    AddEmptyToOppositeIfCan(senderObject, LeftDrawingService);
                }

                if (_eventArgs.Action == NotifyCollectionChangedAction.Replace)
                {
                    senderObject[_eventArgs.NewStartingIndex].Id = LeftDrawingService.DrawingData[_eventArgs.NewStartingIndex].Id;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        public void LeftDrawingDataChanged(object _sender, NotifyCollectionChangedEventArgs _eventArgs)
        {
            //code for inserting empty minutiae
            //code for asign minutiaID
            ObservableCollection<MinutiaStateBase> senderObject = null;
            try
            {
                senderObject = (ObservableCollection<MinutiaStateBase>)_sender;

                if (_eventArgs.Action == NotifyCollectionChangedAction.Add && senderObject.Count > 0)
                {
                    AssignNewIDIfCan(_eventArgs);

                    AddEmptyToOppositeIfCan(senderObject, RightDrawingService);
                }

                if (_eventArgs.Action == NotifyCollectionChangedAction.Replace)
                {
                    senderObject[_eventArgs.NewStartingIndex].Id = RightDrawingService.DrawingData[_eventArgs.NewStartingIndex].Id;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        /// <summary>
        /// insert Empty object to opposite Drawing data if count is diferrent
        /// </summary>
        /// <param name="_senderObject"></param>
        /// <param name="_oppositeDrawingService"></param>
        private void AddEmptyToOppositeIfCan(ObservableCollection<MinutiaStateBase> _senderObject, DrawingService _oppositeDrawingService)
        {
            if (_oppositeDrawingService.WriteableBitmap == null)
            {
                return;
            }

            if (_senderObject.Count > _oppositeDrawingService.DrawingData.Count)
            {
                _oppositeDrawingService.DrawingData.Add(new EmptyState(_oppositeDrawingService) { Id = _senderObject.LastOrDefault().Id });
            }
        }

        private void FillEmpty(DrawingService _drawingService, int _count)
        {
            try
            {
                if (_count <= 0)
                {
                    return;
                }

                for (int i = 0; i < _count; i++)
                {
                    _drawingService.DrawingData.Add(new EmptyState(RightDrawingService));
                }
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        /// <summary>
        /// Assigns Guid to Minutia ID is null or empty
        /// </summary>
        /// <param name="_eventArgs"></param>
        private void AssignNewIDIfCan(NotifyCollectionChangedEventArgs _eventArgs)
        {
            foreach (MinutiaStateBase item in _eventArgs.NewItems)
            {
                if (string.IsNullOrEmpty(item.Id))
                {
                    item.Id = Guid.NewGuid().ToString();
                }
            }
        }

        /// <summary>
        /// Save Data to files named as BackgroundImage file
        /// </summary>
        public void SaveClick()
        {
            try
            {
                //get path to save data as BackgroundImage file name with txt extension
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
