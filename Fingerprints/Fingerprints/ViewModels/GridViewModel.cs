using ExceptionLogger;
using Fingerprints.MinutiaeTypes;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Fingerprints.ViewModels
{
    class GridViewModel : BindableBase
    {
        private MinutiaStateBase leftDrawingObject;
        public MinutiaStateBase LeftDrawingObject { get { return leftDrawingObject; } set { SetProperty(ref leftDrawingObject, value); } }

        private MinutiaStateBase rightDrawingObject;
        public MinutiaStateBase RightDrawingObject { get { return rightDrawingObject; } set { SetProperty(ref rightDrawingObject, value); } }

        private bool deleteButtonVisible;

        public bool DeleteButtonVisible
        {
            get { return deleteButtonVisible; }
            set { SetProperty(ref deleteButtonVisible, value); }
        }

        public GridViewModel()
        {
            try
            {
                deleteButtonVisible = true;
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

    }
}
