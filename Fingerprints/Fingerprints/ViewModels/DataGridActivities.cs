﻿using ExceptionLogger;
using Fingerprints.Converters;
using Fingerprints.Factories;
using Fingerprints.MinutiaeTypes;
using Fingerprints.Tools;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Input;

namespace Fingerprints.ViewModels
{
    class DataGridActivities : BindableBase
    {
        private DrawingService LeftDrawingService { get; }
        private DrawingService RightDrawingService { get; }

        private DrawingDecorator LeftDrawingDecorator { get; }
        private DrawingDecorator RightDrawingDecorator { get; }

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

                if (clickedPosition.CellIndex == Columns.FirstImage)
                {
                    LeftDrawingService.SelectedIndex = clickedPosition.RowIndex;
                }
                else if (clickedPosition.CellIndex == Columns.SecondImage)
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

            LeftDrawingDecorator = new DrawingDecorator(_leftDrawingService);
            RightDrawingDecorator = new DrawingDecorator(_rightDrawingService);

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

                SetCurrentDrawingBasedOnClick();
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        private void SetCurrentDrawingBasedOnClick()
        {
            try
            {
                if (ClickedPosition.CellIndex == Columns.FirstImage)
                {
                    SetCurrentDrawing(LeftDrawingService, RightDrawingService);

                    LeftDrawingDecorator.ShowOnlyIndex(ClickedPosition.RowIndex);
                }
                else if (ClickedPosition.CellIndex == Columns.SecondImage)
                {
                    SetCurrentDrawing(RightDrawingService, LeftDrawingService);

                    RightDrawingDecorator.ShowOnlyIndex(ClickedPosition.RowIndex);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        private void SetCurrentDrawing(DrawingService _drawingService, DrawingService _oppositeDrawingService)
        {
            try
            {
                if (_drawingService.SelectedIndex.HasValue && _drawingService.DrawingData.Count > 0 && _drawingService.DrawingData[_drawingService.SelectedIndex.Value].GetType() == typeof(EmptyState))
                {
                    //get SelfDefinedMinutiae
                    var minutia = _oppositeDrawingService.DrawingData[_drawingService.SelectedIndex.Value].Minutia;

                    //sets CurrentDrawing by clicked empty item on specific index
                    _drawingService.CurrentDrawing = MinutiaStateFactory.Create(minutia, _drawingService.WriteableBitmap, _drawingService.SelectedIndex);

                    //sets CurrentDrawing in DrawingService which draws without index
                    _oppositeDrawingService.CurrentDrawing = MinutiaStateFactory.Create(minutia, _oppositeDrawingService.WriteableBitmap);
                }
                else if (_drawingService.SelectedIndex.HasValue && _oppositeDrawingService.DrawingData.Count > 0 && _oppositeDrawingService.DrawingData[_drawingService.SelectedIndex.Value].GetType() == typeof(EmptyState))
                {
                    //get SelfDefinedMinutiae
                    var minutia = _drawingService.DrawingData[_drawingService.SelectedIndex.Value].Minutia;

                    //sets CurrentDrawing by clicked empty item on specific index
                    _oppositeDrawingService.CurrentDrawing = MinutiaStateFactory.Create(minutia, _oppositeDrawingService.WriteableBitmap, _drawingService.SelectedIndex);

                    //sets CurrentDrawing in DrawingService which draws without index
                    _drawingService.CurrentDrawing = MinutiaStateFactory.Create(minutia, _drawingService.WriteableBitmap);
                }
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

                    DisableDeleteButtonOnLastPosition();
                }

                //Replace object in grid
                if (_eventArgs.Action == NotifyCollectionChangedAction.Replace && senderObject.Count > 0)
                {
                    GridViewModelList[_eventArgs.NewStartingIndex].RightDrawingObject = senderObject[_eventArgs.NewStartingIndex];
                }

                if (_eventArgs.Action == NotifyCollectionChangedAction.Reset)
                {

                    for (int i = 0; i < GridViewModelList.Count - 1; i++)
                    {
                        GridViewModelList[i].RightDrawingObject = null;
                        if (GridViewModelList[i].LeftDrawingObject == null)
                        {
                            GridViewModelList.RemoveAt(i);
                        }
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

                    DisableDeleteButtonOnLastPosition();
                }

                //Replace object in grid
                if (_eventArgs.Action == NotifyCollectionChangedAction.Replace && senderObject.Count > 0)
                {
                    GridViewModelList[_eventArgs.NewStartingIndex].LeftDrawingObject = senderObject[_eventArgs.NewStartingIndex];
                }

                //Set Null to Object, if left and right objects are null, remove from GridViewModelList
                if (_eventArgs.Action == NotifyCollectionChangedAction.Reset)
                {
                    for (int i = 0; i < GridViewModelList.Count - 1; i++)
                    {
                        GridViewModelList[i].LeftDrawingObject = null;
                        if (GridViewModelList[i].RightDrawingObject == null)
                        {
                            GridViewModelList.RemoveAt(i);
                        }
                    }
                }

            }
            catch (Exception ex)
            {

                Logger.WriteExceptionLog(ex);
            }
        }

        /// <summary>
        /// Disable delete button on last line
        /// </summary>
        public void DisableDeleteButtonOnLastPosition()
        {
            try
            {
                GridViewModelList.All(x => x.DeleteButtonVisible = true);

                GridViewModelList.LastOrDefault().DeleteButtonVisible = false;
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
