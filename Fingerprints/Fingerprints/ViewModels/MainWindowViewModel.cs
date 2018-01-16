using ExceptionLogger;
using Fingerprints.Factories;
using Fingerprints.MinutiaeTypes;
using Fingerprints.Models;
using Fingerprints.Resources;
using Fingerprints.Tools;
using Fingerprints.Tools.Exporters;
using Fingerprints.Tools.Importers;
using Fingerprints.Tools.Mindtc;
using Fingerprints.Windows.Controls;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Fingerprints.ViewModels
{
    class MainWindowViewModel : ViewModel_Base
    {
        #region Properties and variables

        private MinutiaeTypeController dbController;

        private bool _bCanComboBoxChangeCurrentDrawing;

        public ObservableCollection<MinutiaState> MinutiaeStates { get; set; }

        private MinutiaState selectedComboboxItem;
        public MinutiaState SelectedComboboxItem { get { return selectedComboboxItem; } set { SetProperty(ref selectedComboboxItem, value); } }

        public DrawingService LeftDrawingService { get; }

        public DrawingService RightDrawingService { get; }

        public GridContextMenu ContextMenuLeftObject { get; }
        public GridContextMenu ContextMenuRightObject { get; }

        public MyObservableCollection<MinutiaStateBase> LeftDrawingData
        { get { return LeftDrawingService.DrawingData; } }

        public MyObservableCollection<MinutiaStateBase> RightDrawingData
        { get { return RightDrawingService.DrawingData; } }

        public DataGridActivities DataGridActivities { get; }

        public MindtcActivity MindtcActivity { get; }

        public string ProjectName { get; set; }

        #endregion

        #region Commands

        public ICommand SaveClickCommand { get; }
        public ICommand MinutiaeStatesSelectionChanged { get; }
        public ICommand SaveAsClickCommand { get; }
        public ICommand NewMinutiaCommand { get; }
        public ICommand LoadLeftImageCommand { get; }
        public ICommand LoadRightImageCommand { get; }

        #endregion

        public MainWindowViewModel()
        {
            try
            {
                _bCanComboBoxChangeCurrentDrawing = true;

                dbController = new MinutiaeTypeController();

                //Initialize Drawing Services
                LeftDrawingService = new DrawingService(DrawingServiceSide.Left);
                RightDrawingService = new DrawingService(DrawingServiceSide.Right);

                //Add method for CollectinoChanged
                LeftDrawingData.CollectionChanged += LeftDrawingDataChanged;
                RightDrawingData.CollectionChanged += RightDrawingDataChanged;

                ContextMenuLeftObject = new GridContextMenu(LeftDrawingService, RightDrawingService);
                ContextMenuRightObject = new GridContextMenu(RightDrawingService, LeftDrawingService);

                //init grid with data from drawing services
                DataGridActivities = new DataGridActivities(LeftDrawingService, RightDrawingService);

                //init mindtc activity object for handling mindtc detection
                MindtcActivity = new MindtcActivity(LeftDrawingService, RightDrawingService);

                //Get MinutiaeStates for combobox
                MinutiaeStates = new ObservableCollection<MinutiaState>(dbController.getStates());

                //Get project Name
                ProjectName = Database.GetProjectName();

                //button clicks delegates
                SaveClickCommand = new DelegateCommand(SaveClick);
                MinutiaeStatesSelectionChanged = new DelegateCommand<MinutiaState>(MinutiaStatesSelectionChanged, CanComboBoxChangeCurrentDrawing);
                SaveAsClickCommand = new DelegateCommand(SaveAsClick);
                LoadLeftImageCommand = new DelegateCommand(LoadLeftImage);
                LoadRightImageCommand = new DelegateCommand(LoadRightImage);
                NewMinutiaCommand = new DelegateCommand(NewMinutia);

                //DrawingService events
                LeftDrawingService.CurrentDrawingChanged += LeftDrawingService_CurrentDrawingChanged;
                RightDrawingService.CurrentDrawingChanged += RightDrawingService_CurrentDrawingChanged;

                LeftDrawingService.DrawingObjectAdded += LeftDrawingService_DrawingObjectAdded;
                RightDrawingService.DrawingObjectAdded += RightDrawingService_DrawingObjectAdded;

                LeftDrawingService.NewDrawingInitialized += LeftDrawingService_NewDrawingInitialized;
                RightDrawingService.NewDrawingInitialized += RightDrawingService_NewDrawingInitialized;
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        #region DrawingService Events

        /// <summary>
        /// Event occurs when New Drawing is Initialized
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RightDrawingService_NewDrawingInitialized(object sender, EventArgs e)
        {
            try
            {
                if (CanChangeInsertIndexInOppositeCurrentDrawing(RightDrawingService, LeftDrawingService))
                {
                    ChangeInsertIndexInOppositeCurrentDrawing(RightDrawingService, LeftDrawingService);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        /// <summary>
        /// Event occurs when New Drawing is Initialized
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LeftDrawingService_NewDrawingInitialized(object sender, EventArgs e)
        {
            try
            {
                if (CanChangeInsertIndexInOppositeCurrentDrawing(LeftDrawingService, RightDrawingService))
                {
                    ChangeInsertIndexInOppositeCurrentDrawing(LeftDrawingService, RightDrawingService);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        /// <summary>
        /// Change InsertIndex in CurrentDrawing from opposite drawingService
        /// Sets cell color "to replace"
        /// </summary>
        /// <param name="_drawingService"></param>
        /// <param name="_oppositeDrawingService"></param>
        private void ChangeInsertIndexInOppositeCurrentDrawing(DrawingService _drawingService, DrawingService _oppositeDrawingService)
        {
            try
            {
                if (CanChangeInsertIndexInOppositeCurrentDrawing(_drawingService, _oppositeDrawingService))
                {
                    _oppositeDrawingService.CurrentDrawing.InsertIndex = _drawingService.DrawingData.Count - 2;
                    _oppositeDrawingService.SetToReplaceColor(_oppositeDrawingService.CurrentDrawing.InsertIndex.Value);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        /// <summary>
        /// Checks if InsertIndex can be changed in opposite current drawing
        /// </summary>
        /// <param name="_drawingService"></param>
        /// <param name="_oppositeDrawingService"></param>
        /// <returns></returns>
        private bool CanChangeInsertIndexInOppositeCurrentDrawing(DrawingService _drawingService, DrawingService _oppositeDrawingService)
        {
            bool result = true;
            var drawingData = _drawingService.DrawingData;
            var oppositeDrawingData = _oppositeDrawingService.DrawingData;

            try
            {
                if (_oppositeDrawingService.CurrentDrawing == null)
                    return false;

                //if oppositeDrawingData has less items than drawingData, return false
                if (oppositeDrawingData.Count <= drawingData.Count - 2)
                    return false;

                //if DrawingObject in oppositeDrawingData at position of last drew object is empty, return false
                if (oppositeDrawingData[drawingData.Count - 2].Minutia.DrawingType != DrawingType.Empty)
                    return false;

                //if DrawingObject in oppositeDrawingData at position of last drew object is different type than CurrentDrawing, return false
                if (drawingData[oppositeDrawingData.Count - 2].Minutia.DrawingType != _oppositeDrawingService.CurrentDrawing.Minutia.DrawingType)
                    return false;

            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
            return result;
        }

        /// <summary>
        /// Occurs when Drawing object is added to DrawingData 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RightDrawingService_DrawingObjectAdded(object sender, EventArgs e)
        {
            try
            {
                if (!(RightDrawingData.LastOrDefault() is EmptyState))
                {
                    AddEmptyObjectOnLastPosition(RightDrawingService, LeftDrawingService);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        /// <summary>
        /// Occurs when Drawing object is added to DrawingData 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LeftDrawingService_DrawingObjectAdded(object sender, EventArgs e)
        {
            try
            {
                if (!(LeftDrawingData.LastOrDefault() is EmptyState))
                {
                    AddEmptyObjectOnLastPosition(LeftDrawingService, RightDrawingService);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        /// <summary>
        /// Event occurs when CurrentDrawing changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RightDrawingService_CurrentDrawingChanged(object _sender, EventArgs e)
        {
            try
            {
                //disable event on Combobox change
                _bCanComboBoxChangeCurrentDrawing = false;

                SetComboboxTitle(_sender);

                //set color for cell which will be replaced
                DataGridActivities.SetToReplaceColor(_sender);
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
            finally
            {
                _bCanComboBoxChangeCurrentDrawing = true;
            }
        }

        /// <summary>
        /// Event occurs when CurrentDrawing changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LeftDrawingService_CurrentDrawingChanged(object sender, EventArgs e)
        {
            try
            {
                //disable event on Combobox change
                _bCanComboBoxChangeCurrentDrawing = false;

                SetComboboxTitle(sender);

                //set color for cell which will be replaced
                DataGridActivities.SetToReplaceColor(sender);
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
            finally
            {
                _bCanComboBoxChangeCurrentDrawing = true;
            }
        }

        #endregion

        #region DrawingData Changed events and methods 

        /// <summary>
        /// Occurs when DrawingData in RightDrawingService changed
        /// </summary>
        /// <param name="_sender"></param>
        /// <param name="_eventArgs"></param>
        private void RightDrawingDataChanged(object _sender, NotifyCollectionChangedEventArgs _eventArgs)
        {
            try
            {
                CollectionChangedActions(RightDrawingService, _eventArgs, LeftDrawingService);
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        /// <summary>
        /// Occurs when DrawingData in LeftDrawingService changed
        /// </summary>
        /// <param name="_sender"></param>
        /// <param name="_eventArgs"></param>
        public void LeftDrawingDataChanged(object _sender, NotifyCollectionChangedEventArgs _eventArgs)
        {
            try
            {
                CollectionChangedActions(LeftDrawingService, _eventArgs, RightDrawingService);
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        /// <summary>
        /// Method executes when DrawingData collection changed
        /// </summary>
        /// <param name="_senderObject"></param>
        /// <param name="_eventArgs"></param>
        /// <param name="_oppositeDrawingService"></param>
        private void CollectionChangedActions(DrawingService _drawingService, NotifyCollectionChangedEventArgs _eventArgs, DrawingService _oppositeDrawingService)
        {
            //runs methods when item was added to DrawingData
            if (_eventArgs.Action == NotifyCollectionChangedAction.Add && _drawingService.DrawingData.Count > 0)
            {
                AssignNewIDIfCan(_eventArgs);
            }

            //runs methods when item was replaced in DrawingData
            if (_eventArgs.Action == NotifyCollectionChangedAction.Replace)
            {
                AssignIDOnReplace(_drawingService.DrawingData, _eventArgs, _oppositeDrawingService);
            }

            //runs methods when item was removed from DrawingData
            if (_eventArgs.Action == NotifyCollectionChangedAction.Remove)
            {
                ResetInsertIndexInCurrentDrawing(LeftDrawingService);
            }
        }

        /// <summary>
        /// Resets InsertIndex in CurrentDrawing on deleting ( if e.g. InsertIndex has value greater that DrawingData count )
        /// </summary>
        /// <param name="_drawingService"></param>
        private void ResetInsertIndexInCurrentDrawing(DrawingService _drawingService)
        {
            try
            {
                if (CanResetInsertIndexInCurrentDrawing(_drawingService))
                {
                    _drawingService.CurrentDrawing.InsertIndex = null;
                    _drawingService.SetToReplaceColor(_drawingService.DrawingData.Count - 1);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        /// <summary>
        /// Checks if InsertIndex can be changed in current drawing
        /// </summary>
        /// <param name="_drawingService"></param>
        /// <returns></returns>
        private bool CanResetInsertIndexInCurrentDrawing(DrawingService _drawingService)
        {
            bool result = true;
            try
            {
                if (_drawingService.CurrentDrawing == null)
                    return false;

                //if InsertIndex is null, return false
                if (!_drawingService.CurrentDrawing.InsertIndex.HasValue)
                    return false;

                //If InsertIndex has proper value ( smaller than drawingData count ), return false
                if (_drawingService.CurrentDrawing.InsertIndex.Value < _drawingService.DrawingData.Count)
                    return false;
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
            return result;
        }

        /// <summary>
        /// Assigns id from opposite object or from old object
        /// </summary>
        /// <param name="_senderObject"></param>
        /// <param name="_eventArgs"></param>
        /// <param name="_oppositeDrawingService"></param>
        private void AssignIDOnReplace(ObservableCollection<MinutiaStateBase> _senderObject, NotifyCollectionChangedEventArgs _eventArgs, DrawingService _oppositeDrawingService)
        {
            try
            {
                if (_oppositeDrawingService.DrawingData?.Count > _eventArgs.NewStartingIndex)
                {
                    //get id from opposite drawing object
                    _senderObject[_eventArgs.NewStartingIndex].Id = _oppositeDrawingService.DrawingData[_eventArgs.NewStartingIndex].Id;
                }
                else
                {
                    //get id from old drawing object
                    _senderObject[_eventArgs.NewStartingIndex].Id = ((MinutiaStateBase)_eventArgs.OldItems[0]).Id;
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

        #endregion

        #region Command delegates

        /// <summary>
        /// Load Right image
        /// Fill DrawingData with Empty if amount of objects in DrawingData is different
        /// </summary>
        private void LoadRightImage()
        {
            IdSorter sorter = null;
            bool loadResult = false;
            try
            {
                loadResult = RightDrawingService.LoadImage();
                if (loadResult)
                {
                    sorter = new IdSorter(RightDrawingService, LeftDrawingService);
                    sorter.SortById();

                    FillDrawingDataWithEmptyObjects(RightDrawingService, LeftDrawingData.Count - RightDrawingData.Count);

                    AddEmptyObjectOnLastPosition(RightDrawingService, LeftDrawingService);

                    RightDrawingService.SetToReplaceColor(null);
                }

            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        /// <summary>
        /// Load Left image
        /// Fill DrawingData with Empty if amount of objects in DrawingData is different
        /// </summary>
        private void LoadLeftImage()
        {
            IdSorter sorter = null;
            bool loadResult = false;
            try
            {
                loadResult = LeftDrawingService.LoadImage();

                if (loadResult)
                {

                    sorter = new IdSorter(LeftDrawingService, RightDrawingService);
                    sorter.SortById();

                    FillDrawingDataWithEmptyObjects(LeftDrawingService, RightDrawingData.Count - LeftDrawingData.Count);

                    AddEmptyObjectOnLastPosition(LeftDrawingService, RightDrawingService);

                    LeftDrawingService.SetToReplaceColor(null);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        private void NewMinutia()
        {
            try
            {
                Window1 win = new Window1();
                win.ShowDialog();
                //drawer.stopDrawing();
                MinutiaeStates.Add(dbController.getStates().LastOrDefault());
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
                string leftFileName = LeftDrawingService.FilterImage.FileName;
                string rightFileName = RightDrawingService.FilterImage.FileName;

                ExportService.SaveAsFileDialog(LeftDrawingData.ToList(), leftFileName, RightDrawingData.ToList(), rightFileName);
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        /// <summary>
        /// Save Data to files named as BackgroundImage file
        /// </summary>
        public void SaveClick()
        {
            string leftPath = String.Empty;
            string rightPath = String.Empty;
            try
            {

                //get path to save data as BackgroundImage file name with txt extension
                if (LeftDrawingService.BackgroundImage != null)
                    leftPath = Path.ChangeExtension(LeftDrawingService.FilterImage.FilePath, ".txt");
                if (RightDrawingService.BackgroundImage != null)
                    rightPath = Path.ChangeExtension(RightDrawingService.FilterImage.FilePath, ".txt");

                ExportService.SaveTxt(LeftDrawingData.ToList(), leftPath, RightDrawingData.ToList(), rightPath);
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
                if (LeftDrawingService.WriteableBitmap != null)
                {
                    LeftDrawingService.CurrentDrawing = MinutiaStateFactory.Create(_oSelectedMinutiaState.Minutia, LeftDrawingService.WriteableBitmap);
                }

                if (RightDrawingService.WriteableBitmap != null)
                {
                    RightDrawingService.CurrentDrawing = MinutiaStateFactory.Create(_oSelectedMinutiaState.Minutia, RightDrawingService.WriteableBitmap);
                }

            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        #endregion

        #region Methods 

        /// <summary>
        /// sets SelectedItem in combobox
        /// </summary>
        /// <param name="_sender"></param>
        private void SetComboboxTitle(object _sender)
        {
            DrawingService senderObject = null;
            try
            {
                senderObject = ((DrawingService)_sender);
                SelectedComboboxItem = MinutiaeStates.FirstOrDefault(x => x.MinutiaName == senderObject.CurrentDrawing.MinutiaName);
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        #endregion

    }
}
