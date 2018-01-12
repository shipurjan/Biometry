using ExceptionLogger;
using Fingerprints.Factories;
using Fingerprints.Models;
using Fingerprints.Resources;
using Fingerprints.Tools.Importers;
using Fingerprints.ViewModels;
using Fingerprints.Windows.UserControls.Dialogs;
using MaterialDesignThemes.Wpf;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Fingerprints.Tools.Mindtc
{
    class MindtctActivity : ViewModel_Base, IDisposable
    {
        public DrawingService LeftDrawingService { get; }
        public DrawingService RightDrawingService { get; }

        private Mindtct mindtc { get; }
        Mindtct.DetectionComplatedDelegate DetectionCompletedHander;

        public ICommand MindtcDetectCommand { get; }

        public MindtctActivity(DrawingService _leftDrawingService, DrawingService _rightDrawingService)
        {
            try
            {
                LeftDrawingService = _leftDrawingService;
                RightDrawingService = _rightDrawingService;

                mindtc = new Mindtct();

                MindtcDetectCommand = new DelegateCommand<DrawingService>(MindtcDetect);
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        /// <summary>
        /// Starts mindtc detection
        /// </summary>
        /// <param name="_drawingService"></param>
        private async void MindtcDetect(DrawingService _drawingService)
        {
            MindtctDialogViewModel viewModel = null;
            try
            {
                var result = await DialogHost.Show(new MindtctDialog(), _drawingService.Dialog, delegate (object _sender, DialogClosingEventArgs _args)
                {
                    if ((bool)_args.Parameter)
                    {
                        viewModel = GetMindtctDialogViewModel(_args);
                        PerformDetection(_drawingService, viewModel);
                    }
                });
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        /// <summary>
        /// Returns viewModel from MindtctDialog user control
        /// </summary>
        /// <param name="_args"></param>
        /// <returns></returns>
        private static MindtctDialogViewModel GetMindtctDialogViewModel(DialogClosingEventArgs _args)
        {
            MindtctDialog dialog = null;
            MindtctDialogViewModel result = null;
            try
            {
                dialog = (MindtctDialog)_args.Session.Content;
                result = (MindtctDialogViewModel)dialog.DataContext;
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
            return result;
        }

        /// <summary>
        /// Persorms detection on image from drawingService with settings from dialog
        /// </summary>
        /// <param name="_drawingService"></param>
        /// <param name="_dialogViewModel"></param>
        private void PerformDetection(DrawingService _drawingService, MindtctDialogViewModel _dialogViewModel)
        {
            _drawingService.IsLoading = true;
            ImportResult importResult = null;
            string imagePath = "";
            DrawingService oppositeDrawingService = null;
            try
            {
                oppositeDrawingService = _drawingService == LeftDrawingService ? RightDrawingService : LeftDrawingService;

                //get path of image to be processed
                imagePath = _drawingService.BackgroundImage.UriSource.AbsolutePath;

                //Begin detection on image and return data from file with selected extension
                mindtc.DetectImage(imagePath, _dialogViewModel.SelectedType.Key);

                //Occurs when detection complete
                DetectionCompletedHander = (_result) =>
                {
                    importResult = _result;

                    if (importResult.ResultData.AnyOrNotNull())
                    {
                        ProcessImportedData(importResult, _dialogViewModel, _drawingService, oppositeDrawingService);
                    }
                    _drawingService.IsLoading = false;
                    mindtc.DetectionCompleted -= DetectionCompletedHander;
                };

                mindtc.DetectionCompleted += DetectionCompletedHander;
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        /// <summary>
        /// Parse imported data to MinutiaStateBase and corrects objects in drawing data
        /// </summary>
        /// <param name="importResult"></param>
        /// <param name="_dialogViewModel"></param>
        /// <param name="_drawingService"></param>
        /// <param name="oppositeDrawingService"></param>
        private void ProcessImportedData(ImportResult importResult, MindtctDialogViewModel _dialogViewModel, DrawingService _drawingService, DrawingService oppositeDrawingService)
        {
            List<MinutiaFileState> importedData = null;
            try
            {
                if (_dialogViewModel.SelectedType.Key == ImportTypes.xyt)
                {
                    CorrectImportedDataFromXyt(importResult, _drawingService.BackgroundImage.PixelHeight);
                }

                //Filter data by quantity
                importedData = importResult.ResultData.Where(x => ((MindtctFileState)x).Quantity >= _dialogViewModel.MinutiaQuantity).ToList();

                //create MitutiaStateBase objects in drawing service
                MinutiaStateFactory.AddMinutiaeFileToDrawingService(importedData, _drawingService);

                FillDrawingDataWithEmptyObjects(oppositeDrawingService, _drawingService.DrawingData.Count - oppositeDrawingService.DrawingData.Count);

                AddEmptyObjectOnLastPosition(_drawingService, oppositeDrawingService);

                _drawingService.SetToReplaceColor(null);
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        /// <summary>
        /// Changes y from cartesian system to screen drawing ( on screen we drawing from top left corner )
        /// </summary>
        /// <param name="_importResult"></param>
        /// <param name="_pixelHeight"></param>
        private void CorrectImportedDataFromXyt(ImportResult _importResult, int _pixelHeight)
        {
            try
            {
                foreach (var item in _importResult.ResultData)
                {
                    item.Points[0] = new Point(item.Points[0].X, _pixelHeight - item.Points[0].Y);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    mindtc.DetectionCompleted -= DetectionCompletedHander;
                    DetectionCompletedHander = null;
                    mindtc.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
