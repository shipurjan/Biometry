using ExceptionLogger;
using Fingerprints.MinutiaeTypes;
using Prism.Commands;
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
    class DataGridActivities
    {
        private DrawingService leftDrawingService { get; }
        private DrawingService rightDrawingService { get; }

        public ObservableCollection<GridViewModel> GridViewModelList { get; }

        public ICommand DeleteButtonCommand { get; }

        public DataGridActivities(DrawingService _leftDrawingService, DrawingService _rightDrawingService)
        {
            GridViewModelList = new ObservableCollection<GridViewModel>();

            leftDrawingService = _leftDrawingService;
            rightDrawingService = _rightDrawingService;

            leftDrawingService.DrawingData.CollectionChanged += LeftDrawingDataChanged;
            rightDrawingService.DrawingData.CollectionChanged += RightDrawingDataChanged;

            DeleteButtonCommand = new DelegateCommand<object>(DeleteButtonClick);
        }

        private void DeleteButtonClick(object _gridViewModel)
        {
            GridViewModel gridViewModel;
            try
            {
                gridViewModel = (GridViewModel)_gridViewModel;

                leftDrawingService.DrawingData.Remove(gridViewModel.LeftDrawingObject);
                rightDrawingService.DrawingData.Remove(gridViewModel.RightDrawingObject);
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
            catch (Exception)
            {

                throw;
            }
        }
    }
}
