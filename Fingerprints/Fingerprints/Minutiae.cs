using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Fingerprints
{
    public class Minutiae : AppInstance, IDraw
    {
        public string Name;
        public virtual void Draw(OverridedCanvas canvas, Image image, RadioButton radioButton1, RadioButton radioButton2, int index = -1) { }
        public virtual void DeleteEvent(Image image, OverridedCanvas canvas) { }
        public virtual void DrawFromFile(OverridedCanvas canvas) { }

        public void AddElementToSaveList(int index = -1)
        {
            if (!window.activeCanvasL.IsChecked.Value)
            {
                insertStringToList(FileTransfer.ListL, index);

            }
            else
            {
                insertStringToList(FileTransfer.ListR, index);
            }

        }

        private void insertStringToList(List<string> list, int index = -1)
        {
            if (ToString() != "" && list.LastOrDefault() != ToString())
            {
                if (index > -1)
                {
                    list.Insert(index, ToString());
                }
                else
                {
                    list.Add(ToString());
                }
            }
        }
    }
}
