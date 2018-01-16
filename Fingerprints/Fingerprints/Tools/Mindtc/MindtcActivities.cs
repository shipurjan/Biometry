using ExceptionLogger;
using Fingerprints.Factories;
using Fingerprints.Resources;
using Fingerprints.Tools.Importers;
using Fingerprints.ViewModels;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Fingerprints.Tools.Mindtc
{
    class MindtcActivity : ViewModel_Base, IDisposable
    {
        public DrawingService LeftDrawingService { get; }
        public DrawingService RightDrawingService { get; }

        private Mindtc mindtc { get; }
        Mindtc.DetectionComplatedDelegate DetectionCompletedHander;

        public ICommand MindtcDetectCommand { get; }

        public MindtcActivity(DrawingService _leftDrawingService, DrawingService _rightDrawingService)
        {
            try
            {
                LeftDrawingService = _leftDrawingService;
                RightDrawingService = _rightDrawingService;

                mindtc = new Mindtc();

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
        private void MindtcDetect(DrawingService _drawingService)
        {
            _drawingService.IsLoading = true;
            ImportResult importResult = null;
            string imagePath = "";
            DrawingService oppositeDrawingService = null;
            try
            {
                oppositeDrawingService = _drawingService == LeftDrawingService ? RightDrawingService : LeftDrawingService;

                imagePath = _drawingService.BackgroundImage.UriSource.AbsolutePath;

                mindtc.DetectImage(imagePath);

                DetectionCompletedHander = (_result) =>
                {
                    importResult = _result;

                    if (importResult.ResultData.AnyOrNotNull())
                    {
                        //create MitutiaStateBase objects in drawing service
                        MinutiaStateFactory.AddMinutiaeFileToDrawingService(importResult.ResultData, _drawingService);

                        FillDrawingDataWithEmptyObjects(oppositeDrawingService, _drawingService.DrawingData.Count - oppositeDrawingService.DrawingData.Count);

                        AddEmptyObjectOnLastPosition(_drawingService, oppositeDrawingService);

                        _drawingService.SetToReplaceColor(null);

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
