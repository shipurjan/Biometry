using ExceptionLogger;
using Fingerprints.Converters;
using Fingerprints.MinutiaeTypes;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace Fingerprints.ViewModels
{
    class DataGridActivities : BindableBase
    {
        private DrawingService LeftDrawingService { get; }
        private DrawingService RightDrawingService { get; }

        public ObservableCollection<GridViewModel> GridViewModelList { get; }

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

        private void RightDrawingDataChanged(object _sender, NotifyCollectionChangedEventArgs _eventArgs)
        {
            ObservableCollection<MinutiaStateBase> senderObject = null;
            try
            {
                senderObject = (ObservableCollection<MinutiaStateBase>)_sender;

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

                if (_eventArgs.Action == NotifyCollectionChangedAction.Replace && senderObject.Count > 0)
                {
                    GridViewModelList[_eventArgs.NewStartingIndex].RightDrawingObject = senderObject[_eventArgs.NewStartingIndex];
                }
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        private void LeftDrawingDataChanged(object _sender, NotifyCollectionChangedEventArgs _eventArgs)
        {
            ObservableCollection<MinutiaStateBase> senderObject = null;
            try
            {
                senderObject = (ObservableCollection<MinutiaStateBase>)_sender;
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

                if (_eventArgs.Action == NotifyCollectionChangedAction.Replace && senderObject.Count > 0)
                {
                    GridViewModelList[_eventArgs.NewStartingIndex].LeftDrawingObject = senderObject[_eventArgs.NewStartingIndex];
                }
            }
            catch (Exception ex)
            {

                Logger.WriteExceptionLog(ex);
            }
        }
    }
}
