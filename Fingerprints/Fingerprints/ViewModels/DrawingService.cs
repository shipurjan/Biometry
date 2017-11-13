using System;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using ExceptionLogger;
using Fingerprints.Interfaces;
using Fingerprints.MinutiaeTypes;
using Fingerprints.Factories;
using Prism.Mvvm;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using Fingerprints.Tools.Importers;
using Fingerprints.Resources;
using Fingerprints.Tools;
using Fingerprints.EventArgsObjects;
using System.Collections.Specialized;
using System.Linq;
using System.ComponentModel;
using Fingerprints.Tools.ImageFilters;
using Prism.Commands;

namespace Fingerprints.ViewModels
{
    public class DrawingService : BindableBase, IDisposable
    {
        /// <summary>
        /// Event raised when current drawing change
        /// </summary>
        public event EventHandler CurrentDrawingChanged;

        public event EventHandler NewDrawingInitialized;

        public event EventHandler DrawingObjectAdded;

        public MyObservableCollection<MinutiaStateBase> DrawingData { get; }

        private Point mousePosition;
        private FilterImage filterImage;
        public FilterImage FilterImage
        {
            get { return filterImage; }
            set { SetProperty(ref filterImage, value); }
        }

        #region Props
        /// <summary>
        /// Current Drawing, on set raise event CurrentDrawingChanged
        /// </summary>
        private MinutiaStateBase currentDrawing;
        public MinutiaStateBase CurrentDrawing
        {
            get { return currentDrawing; }
            set
            {
                try
                {
                    if (currentDrawing != null)
                    {
                        DeleteCurrentDrawingEvents();
                    }
                    SetProperty(ref currentDrawing, value);
                    CurrentDrawingChanged(this, new CurrentDrawingChangedEventArgs() { CurrentDrawing = value });

                    AddCurrentDrawingEvents();
                }
                catch (Exception ex)
                {
                    Logger.WriteExceptionLog(ex);
                }
            }
        }

        private WriteableBitmap writeableBitmap;
        public WriteableBitmap WriteableBitmap
        {
            get { return writeableBitmap; }
            set { SetProperty(ref writeableBitmap, value); }
        }

        private BitmapImage backgroundImage;
        public BitmapImage BackgroundImage
        {
            get { return backgroundImage; }
            set { SetProperty(ref backgroundImage, value); }
        }

        /// <summary>
        /// Property indicates what index in listbox is selected
        /// </summary>
        private int? selectedIndex;
        public int? SelectedIndex
        {
            get
            { return selectedIndex; }
            set
            { SetProperty(ref selectedIndex, value != -1 ? value : null); }
        }

        #endregion

        public ICommand NoneFilterCommand { get; }
        public ICommand SobelFilterCommand { get; }

        /// <summary>
        /// Initializes new instance
        /// </summary>
        public DrawingService()
        {
            try
            {
                DrawingData = new MyObservableCollection<MinutiaStateBase>();
                DrawingData.CollectionChanged += DrawingDataCollectionChanged;
                NoneFilterCommand = new DelegateCommand(NoneFilter);
                SobelFilterCommand = new DelegateCommand(SobelFilter);
                //AcceptButtonVisibility = false;
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        private void DrawingDataCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            try
            {
                Draw();
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        private void CurrentDrawingPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            try
            {
                Draw();
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        private void CurrentDrawingCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            try
            {
                if (e.Action == NotifyCollectionChangedAction.Add && e.NewStartingIndex == 0)
                {
                    if (DrawingData.LastOrDefault()?.GetType() == typeof(EmptyState) && !CurrentDrawing.InsertIndex.HasValue)
                    {
                        AddMinutiaToDrawingData(CurrentDrawing, DrawingData.Count - 1);
                    }
                    else
                    {
                        AddMinutiaToDrawingData(CurrentDrawing, CurrentDrawing.InsertIndex);
                    }
                }

                Draw();
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        #region IMouseMoveable, IMouseClickable, IDrawable methods

        /// <summary>
        /// Method launched when MouseMove event is triggered
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void MouseMoveMethod(object sender, MouseEventArgs args)
        {
            try
            {
                if (CurrentDrawing is IMouseMoveable)
                {
                    ((IMouseMoveable)CurrentDrawing).MouseMove(sender, args);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        /// <summary>
        /// Method launched when MouseDown event is triggered
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void MouseDownMethod(object sender, MouseButtonEventArgs args)
        {
            try
            {
                if (CurrentDrawing is IMouseClickable)
                {
                    ((IMouseClickable)CurrentDrawing).MouseClick(sender, args);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        /// <summary>
        /// its clear WriteableBitmap and draws all items from DrawingData by lauching their DrawProcedure method
        /// </summary>
        public void Draw()
        {
            try
            {
                WriteableBitmap.Clear();
                foreach (var item in DrawingData)
                {
                    if (item is IDrawable)
                    {
                        ((IDrawable)item).DrawProcedure();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        #endregion

        #region zoom and move

        /// <summary>
        /// Performs zoom of container which contains BackgroundImage and WriteableBitmap
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void MouseWheelGroupMethod(object sender, MouseWheelEventArgs args)
        {
            UIElement uiElement = null;
            try
            {
                uiElement = (UIElement)sender;

                Matrix matline = uiElement.RenderTransform.Value;
                Point mouse = args.GetPosition(uiElement);

                if (args.Delta > 0)
                {
                    matline.ScaleAtPrepend(1.15, 1.15, mouse.X, mouse.Y);
                }
                else
                {
                    matline.ScaleAtPrepend(1 / 1.15, 1 / 1.15, mouse.X, mouse.Y);
                }

                MatrixTransform mtfl = new MatrixTransform(matline);
                uiElement.RenderTransform = mtfl;
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        /// <summary>
        /// saves position of mouse cursor and starts capturing mouse on senfer object ( in this case UIElement )
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void MouseLeftButtonDownGroupMethod(object sender, MouseButtonEventArgs args)
        {
            try
            {
                mousePosition = args.GetPosition(Application.Current.MainWindow);
                ((UIElement)sender).CaptureMouse();
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        /// <summary>
        /// Realeases Mouse capture from sender object
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void MouseLeftButtonUpGroupMethod(object sender, MouseButtonEventArgs args)
        {
            try
            {
                ((UIElement)sender).ReleaseMouseCapture();
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        /// <summary>
        /// Moves container with WriteableBitmap and BackgroundImage
        /// Calculates difference between last remembered mouse position and new and moves container by this value
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void PreviewMouseMoveGroupMethod(object sender, MouseEventArgs args)
        {
            UIElement uiElement = null;
            Point temp;
            Point res;
            try
            {
                if (args.LeftButton == MouseButtonState.Pressed)
                {
                    uiElement = (UIElement)sender;

                    temp = args.GetPosition(Application.Current.MainWindow);
                    res = new Point(mousePosition.X - temp.X, mousePosition.Y - temp.Y);

                    Canvas.SetLeft(uiElement, Canvas.GetLeft(uiElement) - res.X);
                    Canvas.SetTop(uiElement, Canvas.GetTop(uiElement) - res.Y);

                    mousePosition = temp;
                }

            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        #endregion

        /// <summary>
        /// Opens OpenFileDialog for load image, creates new instance of WriteableBitmap and assigns BackroundImage
        /// </summary>
        public bool LoadImage()
        {
            bool result = false;

            ImportResult importResult = null;
            try
            {
                OpenFileDialog openFile = new OpenFileDialog();
                openFile.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png, *.bmp) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png; *.bmp";

                if (openFile.ShowDialog() == true)
                {
                    FilterImage = new FilterImage(new System.Drawing.Bitmap(openFile.FileName), openFile.FileName);                   

                    BackgroundImage = FilterImage.OryginalImage.ToBitmapImage();

                    WriteableBitmap = new WriteableBitmap(FilterImage.OryginalImage.Width, FilterImage.OryginalImage.Height, 96, 96, PixelFormats.Bgra32, null);

                    //Reset drawing
                    DrawingData.Clear();
                    if (CurrentDrawing != null)
                    {
                        //create new empty CuurentDrawing
                        CurrentDrawing = MinutiaStateFactory.Create(CurrentDrawing.Minutia, WriteableBitmap);
                    }

                    //import data from file
                    importResult = ImporterService.Import(Path.ChangeExtension(openFile.FileName, ".txt"), this);

                    if (importResult.ResultData.AnyOrNotNull())
                    {
                        //create MitutiaStateBase objects in drawing service
                        MinutiaStateFactory.AddMinutiaeFileToDrawingService(importResult.ResultData, this);
                    }

                    result = true;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }

            return result;
        }

        /// <summary>
        /// Sets color to DrawingObject which will be replaced
        /// </summary>
        /// <param name="_itemIndex"></param>
        public void SetToReplaceColor(int? _itemIndex)
        {
            try
            {
                foreach (var item in DrawingData)
                {
                    item.WillBeReplaced = false;
                }

                if (_itemIndex.HasValue && _itemIndex.Value > -1)
                {
                    DrawingData[_itemIndex.Value].WillBeReplaced = true;
                }


                if (!_itemIndex.HasValue)
                {
                    DrawingData.LastOrDefault().WillBeReplaced = true;
                }

            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        /// <summary>
        /// Inititates new drawing for CurrenDrawing object
        /// </summary>
        private void InitiateNewDrawing(object _sender, EventArgs _args)
        {
            try
            {
                CurrentDrawing = MinutiaStateFactory.Create(CurrentDrawing.Minutia, WriteableBitmap);

                NewDrawingInitialized(this, null);

                if (CurrentDrawing.Minutia.DrawingType == Models.DrawingType.CurveLine)
                {
                    InitializeActionButtonForCurveLine();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        private void InitializeActionButtonForCurveLine()
        {
            try
            {
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        /// <summary>
        /// Adds CurrentDrawing to DrawingData list, 
        /// When this method is launched, CurrentDrawing will appear on WriteableBitmap and listbox
        /// </summary>
        public void AddMinutiaToDrawingData(MinutiaStateBase _minutiaStateBase, int? _insertIndex = null)
        {
            try
            {
                if (_insertIndex.HasValue && DrawingData.Count > _insertIndex.Value)
                {
                    DrawingData[_insertIndex.Value] = _minutiaStateBase;
                }
                else
                {
                    DrawingData.Add(_minutiaStateBase);
                }

                DrawingObjectAdded(this, null);
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        /// <summary>
        /// Deltes all events from CurrentDrawing object
        /// </summary>
        private void DeleteCurrentDrawingEvents()
        {
            try
            {
                currentDrawing.Points.CollectionChanged -= CurrentDrawingCollectionChanged;
                currentDrawing.PropertyChanged -= CurrentDrawingPropertyChanged;
                currentDrawing.InitiateNewDrawing -= InitiateNewDrawing;
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        /// <summary>
        /// Adds events to CurrentDrawing object
        /// </summary>
        private void AddCurrentDrawingEvents()
        {
            try
            {
                currentDrawing.Points.CollectionChanged += CurrentDrawingCollectionChanged;
                currentDrawing.PropertyChanged += CurrentDrawingPropertyChanged;
                currentDrawing.InitiateNewDrawing += InitiateNewDrawing;
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        private void NoneFilter()
        {
            try
            {
                BackgroundImage = FilterImage.Filter(FilterImageType.None).Get().ToBitmapImage();
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        private void SobelFilter()
        {
            try
            {
                BackgroundImage = FilterImage.Filter(FilterImageType.Sobel).Get().ToBitmapImage();
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
            try
            {
                if (!disposedValue)
                {
                    if (disposing)
                    {
                        // TODO: dispose managed state (managed objects).
                    }

                    // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                    // TODO: set large fields to null.

                    disposedValue = true;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~MainWindowViewModel() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
