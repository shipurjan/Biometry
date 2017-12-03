using ExceptionLogger;
using Fingerprints.Resources;
using Fingerprints.Tools.ImageFilters;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Fingerprints.ViewModels
{
    class FilterWindowViewModel : ViewModel_Base
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
            try
            {
                DrawingService.BackgroundImage = DrawingService.FilterImage.Filter(FilterType, SliderCurrentValue).Get().ToBitmapImage();
                
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
                minSlider = 1;
                maxSlider = 30;
                TickFrequency = 2;
                SliderCurrentValue = 3;
                FilterName = "Sobel";
                FilterType = _filterType;
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }
    }
}
