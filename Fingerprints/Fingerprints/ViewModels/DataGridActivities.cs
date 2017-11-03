using ExceptionLogger;
using Fingerprints.Converters;
using Fingerprints.MinutiaeTypes;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Input;

namespace Fingerprints.ViewModels
{
    class DataGridActivities : BindableBase
    {
        private DrawingService LeftDrawingService { get; }
        private DrawingService RightDrawingService { get; }

        public ObservableCollection<GridViewModel> GridViewModelList { get; }

        /// <summary>
        /// Position of Clicked Cell on grid
        /// Sets SelectedIndex to DrawingServices
        /// </summary>
        private GridClickedItemPosition clickedPosition;
        public GridClickedItemPosition ClickedPosition
        {
            get { return clickedPosition; }
            set
            {
                clickedPosition = value;

                if (clickedPosition.CellIndex == 1)
                {
                    LeftDrawingService.SelectedIndex = clickedPosition.RowIndex;
                }
                else if (clickedPosition.CellIndex == 2)
                {
                    RightDrawingService.SelectedIndex = ClickedPosition.RowIndex;
                }

            }
        }


        public ICommand DrawingObjectClickChangedCommand { get; }

        public ICommand DeleteButtonCommand { get; }

        public DataGridActivities(DrawingService _leftDrawingService, DrawingService _rightDrawingService)
        {
            GridViewModelList = new ObservableCollection<GridViewModel>();

            LeftDrawingService = _leftDrawingService;
            RightDrawingService = _rightDrawingService;

            LeftDrawingService.DrawingData.CollectionChanged += LeftDrawingDataChanged;
            RightDrawingService.DrawingData.CollectionChanged += RightDrawingDataChanged;

            DeleteButtonCommand = new DelegateCommand<object>(DeleteButtonClick);
            DrawingObjectClickChangedCommand = new DelegateCommand<GridClickedItemPosition>(DrawingObjectClickChanged);
        }

        /// <summary>
        /// Event occurs when selection of DrawingObject changed on grid
        /// </summary>
        /// <param name="_args"></param>
        private void DrawingObjectClickChanged(GridClickedItemPosition _args)
        {
            try
            {
                ClickedPosition = _args;
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        /// <summary>
        /// Deletes DrawingObjects from DrawingServices and GridViewModelList
        /// </summary>
        /// <param name="_gridViewModel"></param>
        private void DeleteButtonClick(object _gridViewModel)
        {
            GridViewModel gridViewModel;
            try
            {
                gridViewModel = (GridViewModel)_gridViewModel;

                LeftDrawingService.DrawingData.Remove(gridViewModel.LeftDrawingObject);
                RightDrawingService.DrawingData.Remove(gridViewModel.RightDrawingObject);
                GridViewModelList.Remove(gridViewModel);
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        /// <summary>
        /// Occurs when DrawingData in RightDrawingService changed
        /// </summary>
        /// <param name="_sender"></param>
        /// <param name="_eventArgs"></param>
        private void RightDrawingDataChanged(object _sender, NotifyCollectionChangedEventArgs _eventArgs)
        {
            ObservableCollection<MinutiaStateBase> senderObject = null;
            try
            {
                senderObject = (ObservableCollection<MinutiaStateBase>)_sender;

                //Add row to grid
                if (_eventArgs.Action == NotifyCollectionChangedAction.Add && senderObject.Count > 0)
                {
                    if (GridViewModelList.Count >= senderObject.Count)
                    {
                        GridViewModelList[_eventArgs.NewStartingIndex].RightDrawingObject = senderObject[_eventArgs.NewStartingIndex];
                    }
                    else
                    {
                        GridViewModelList.Add(new GridViewModel() { RightDrawingObject = senderObject[_eventArgs.NewStartingIndex] });
                    }
                }

                //Replace object in grid
                if (_eventArgs.Action == NotifyCollectionChangedAction.Replace && senderObject.Count > 0)
                {
                    GridViewModelList[_eventArgs.NewStartingIndex].RightDrawingObject = senderObject[_eventArgs.NewStartingIndex];
                }

                if (_eventArgs.Action == NotifyCollectionChangedAction.Reset)
                {
                    foreach (var item in GridViewModelList)
                    {
                        item.RightDrawingObject = null;
                    }
                }
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
        private void LeftDrawingDataChanged(object _sender, NotifyCollectionChangedEventArgs _eventArgs)
        {
            ObservableCollection<MinutiaStateBase> senderObject = null;
            try
            {
                senderObject = (ObservableCollection<MinutiaStateBase>)_sender;

                //Add row to grid
                if (_eventArgs.Action == NotifyCollectionChangedAction.Add && senderObject.Count > 0)
                {
                    if (GridViewModelList.Count >= senderObject.Count)
                    {
                        GridViewModelList[_eventArgs.NewStartingIndex].LeftDrawingObject = senderObject[_eventArgs.NewStartingIndex];
                    }
                    else
                    {
                        GridViewModelList.Add(new GridViewModel() { LeftDrawingObject = senderObject[_eventArgs.NewStartingIndex] });
                    }
                }

                //Replace object in grid
                if (_eventArgs.Action == NotifyCollectionChangedAction.Replace && senderObject.Count > 0)
                {
                    GridViewModelList[_eventArgs.NewStartingIndex].LeftDrawingObject = senderObject[_eventArgs.NewStartingIndex];
                }

                if (_eventArgs.Action == NotifyCollectionChangedAction.Reset)
                {
                    foreach (var item in GridViewModelList)
                    {
                        item.LeftDrawingObject = null;
                    }
                }

            }
            catch (Exception ex)
            {

                Logger.WriteExceptionLog(ex);
            }
        }

        /// <summary>
        /// Sets color for cell where CurrentDrawing will be placed
        /// </summary>
        /// <param name="_sender"></param>
        public void SetToReplaceColor(object _sender)
        {
            DrawingService senderObject = null;
            try
            {
                senderObject = ((DrawingService)_sender);
                if (senderObject.CurrentDrawing.InsertIndex.HasValue)
                {
                    senderObject.SetToReplaceColor(senderObject.CurrentDrawing.InsertIndex.Value);
                }
                else
                {
                    senderObject.SetToReplaceColor(senderObject.DrawingData.Count - 1);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }
    }
}
