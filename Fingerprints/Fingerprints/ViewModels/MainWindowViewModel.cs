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

        private MinutiaState selectedComboboxItem;
        public MinutiaState SelectedComboboxItem { get { return selectedComboboxItem; } set { SetProperty(ref selectedComboboxItem, value); } }

        public DrawingService LeftDrawingService { get; }

        public DrawingService RightDrawingService { get; }

        public ObservableCollection<MinutiaStateBase> LeftDrawingData
        { get { return LeftDrawingService.DrawingData; } }

        public ObservableCollection<MinutiaStateBase> RightDrawingData
        { get { return RightDrawingService.DrawingData; } }

        public ListBoxContextMenu LeftListBoxContextMenu { get; }

        public ListBoxContextMenu RightListBoxContextMenu { get; }

        public string ProjectName { get; set; }

        public ICommand SaveClickCommand { get; }
        public ICommand MinutiaeStatesSelectionChanged { get; }
        public ICommand SaveAsClickCommand { get; }
        public ICommand NewMinutiaCommand { get; }
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

                //Get project Name
                ProjectName = Database.GetProjectName();

                //Init context menu for listboxes
                LeftListBoxContextMenu = new ListBoxContextMenu(LeftDrawingService, RightDrawingService);
                RightListBoxContextMenu = new ListBoxContextMenu(RightDrawingService, LeftDrawingService);

                //button clicks delegates
                SaveClickCommand = new DelegateCommand(SaveClick);
                MinutiaeStatesSelectionChanged = new DelegateCommand<MinutiaState>(MinutiaStatesSelectionChanged, CanComboBoxChangeCurrentDrawing);
                SaveAsClickCommand = new DelegateCommand(SaveAsClick);
                LoadLeftImageCommand = new DelegateCommand(LoadLeftImage);
                LoadRightImageCommand = new DelegateCommand(LoadRightImage);
                LeftDrawingService.CurrentDrawingChanged += LeftDrawingService_CurrentDrawingChanged;
                RightDrawingService.CurrentDrawingChanged += RightDrawingService_CurrentDrawingChanged;
                NewMinutiaCommand = new DelegateCommand(NewMinutia);
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
        /// Event occurs when CurrentDrawing changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RightDrawingService_CurrentDrawingChanged(object sender, EventArgs e)
        {
            try
            {
                _bCanComboBoxChangeCurrentDrawing = false;

                SetComboboxTitle(sender);
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
                _bCanComboBoxChangeCurrentDrawing = false;

                SetComboboxTitle(sender);
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

        /// <summary>
        /// Load Right image
        /// Fill DrawingData with Empty if amount of objects in DrawingData is different
        /// </summary>
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

        /// <summary>
        /// Load Left image
        /// Fill DrawingData with Empty if amount of objects in DrawingData is different
        /// </summary>
        private void LoadLeftImage()
        {
            try
            {
                LeftDrawingService.LoadImage();
                SortDrawingData();

                FillEmpty(LeftDrawingService, RightDrawingData.Count - LeftDrawingData.Count);
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        private void SortDrawingData()
        {
            List<MinutiaStateBase> tmpLeftData = null;
            List<MinutiaStateBase> tmpRightData = null;
            try
            {
                //Sort only if both collections have at least one element
                if (LeftDrawingData.Count == 0 || RightDrawingData.Count == 0)
                    return;

                //Init tmp tmp data
                tmpLeftData = LeftDrawingData.ToList();
                tmpRightData = RightDrawingData.ToList();

                //Fill empty in one List to match count
                if (tmpLeftData.Count > tmpRightData.Count)
                    FillEmptyTmpList(RightDrawingService, tmpRightData, tmpLeftData.Count - tmpRightData.Count);
                else
                    FillEmptyTmpList(LeftDrawingService, tmpLeftData, tmpRightData.Count - tmpLeftData.Count);

                //Sort lists
                SortTmpLists(ref tmpLeftData, ref tmpRightData);


                //Clear


            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }            
        }

        private void SortTmpLists(ref List<MinutiaStateBase> _tmpLeftData, ref List<MinutiaStateBase> _tmpRightData)
        {
            var orderedZip = _tmpLeftData.Zip(_tmpRightData, (x, y) => new { x, y })
                      .OrderBy(pair => pair.x.Id == pair.y.Id)
                      .ToList();
            _tmpLeftData = orderedZip.Select(pair => pair.x).ToList();
            _tmpRightData = orderedZip.Select(pair => pair.y).ToList();
        }

        private void FillEmptyTmpList(DrawingService _DrawingService, List<MinutiaStateBase> _tmpDataList, int _count)
        {
            try
            {
                for (int i = 0; i < _count; i++)
                {
                    _tmpDataList.Add(new EmptyState(_DrawingService));
                }
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

                CollectionChangedActions(senderObject, _eventArgs, LeftDrawingService);
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

                CollectionChangedActions(senderObject, _eventArgs, RightDrawingService);
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
        private void CollectionChangedActions(ObservableCollection<MinutiaStateBase> _senderObject, NotifyCollectionChangedEventArgs _eventArgs, DrawingService _oppositeDrawingService)
        {
            if (_eventArgs.Action == NotifyCollectionChangedAction.Add && _senderObject.Count > 0)
            {
                AssignNewIDIfCan(_eventArgs);

                AddEmptyToOppositeIfCan(_senderObject, _oppositeDrawingService);
            }

            if (_eventArgs.Action == NotifyCollectionChangedAction.Replace)
            {
                AssignIDOnReplace(_senderObject, _eventArgs, _oppositeDrawingService);
            }
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
                _oppositeDrawingService.AddMinutiaToDrawingData(new EmptyState(_oppositeDrawingService) { Id = _senderObject.LastOrDefault().Id });
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
                    _drawingService.AddMinutiaToDrawingData(new EmptyState(_drawingService));
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
            string leftPath = String.Empty;
            string rightPath = String.Empty;
            try
            {              

                //get path to save data as BackgroundImage file name with txt extension
                if (LeftDrawingService.BackgroundImage != null)
                    leftPath = Path.ChangeExtension(LeftDrawingService.BackgroundImage.UriSource.AbsolutePath, ".txt");
                if (RightDrawingService.BackgroundImage != null)
                    rightPath = Path.ChangeExtension(RightDrawingService.BackgroundImage.UriSource.AbsolutePath, ".txt");

                ExportService.SaveTxt(LeftDrawingData.ToList(), leftPath, RightDrawingData.ToList(), rightPath);
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }


    }
}
