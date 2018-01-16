using ExceptionLogger;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fingerprints.Windows.UserControls.Dialogs
{
    class MindtctDialogViewModel
    {
        public ObservableCollection<KeyValuePair<ImportTypes, String>> Types { get; }
        
        public int MinutiaQuantity { get; set; }

        public KeyValuePair<ImportTypes, String> SelectedType { set; get; }

        public MindtctDialogViewModel()
        {
            try
            {
                Types = new ObservableCollection<KeyValuePair<ImportTypes, string>>();
                Types.Add(new KeyValuePair<ImportTypes, string>(ImportTypes.min, ".min (Domyślny)"));
                Types.Add(new KeyValuePair<ImportTypes, string>(ImportTypes.xyt, ".xyt (Plik z formatem dla Bozorth3)"));

                SelectedType = Types.FirstOrDefault();
                MinutiaQuantity = 50;
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }
    }
}
