using Fingerprints.MinutiaeTypes;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fingerprints.ViewModels
{
    class DataGridObject : BindableBase
    {
        public int Index { get; set; } // +1

        private MinutiaStateBase leftDrawingObject;
        public MinutiaStateBase LeftDrawingObject { get { return leftDrawingObject; } set { SetProperty(ref leftDrawingObject, value); } }

        private MinutiaStateBase rightDrawingObject;
        public MinutiaStateBase RightDrawingObject { get { return rightDrawingObject; } set { SetProperty(ref rightDrawingObject, value); } }
    }
}
