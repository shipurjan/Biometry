﻿using ExceptionLogger;
using Fingerprints.Resources;
using Fingerprints.Tools.ImageFilters;
using MaterialDesignThemes.Wpf;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Fingerprints.ViewModels
{
    class FilterWindowViewModel : ViewModel_Base, INotifyPropertyChanged
    {
        #region Properties and variables
        public string FilterName { get; set; }

        public int minSlider { get; set; }

        public int maxSlider { get; set; }

        public int TickFrequency { get; set; }

        public int SliderCurrentValue { get; set; }

        private FilterImageType FilterType = FilterImageType.None;

        private DrawingService DrawingService;
        #endregion

        #region Commands

        public ICommand FilterClickCommand { get; }

        #endregion

        public FilterWindowViewModel(DrawingService _drawingService, FilterImageType _filterType)
        {
            try
            {
                DrawingService = _drawingService;

                SetSlider(_filterType);

                FilterClickCommand = new DelegateCommand(FilterClick);                
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        private void FilterClick()
        {
            Task result = null;
            try
            {
                DialogHost.CloseDialogCommand.Execute(true, null);
                DrawingService.IsLoading = true;
                result = Task.Run(() =>
                {                    
                    DrawingService.BackgroundImage = DrawingService.FilterImage.Filter(FilterType, SliderCurrentValue).Get().ToBitmapImage();
                    DrawingService.IsLoading = false;
                });
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        private void SetSlider(FilterImageType _filterType)
        {
            try
            {
                if (_filterType == FilterImageType.Sobel)
                {
                    minSlider = 1;
                    maxSlider = 7;
                    TickFrequency = 2;
                    SliderCurrentValue = 3;
                    FilterType = _filterType;
                }
                else
                {
                    minSlider = 1;
                    maxSlider = 31;
                    TickFrequency = 2;
                    SliderCurrentValue = 5;
                    FilterType = _filterType;
                }
                
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }
    }
}
