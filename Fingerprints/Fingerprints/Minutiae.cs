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
                if (FileTransfer.ListL.Count > 0)
                {
                    if (FileTransfer.ListL.Last().ToString() != ToString())
                    {
                        FileTransfer.ListL.Add(ToString());
                    }
                }
                else
                {
                    FileTransfer.ListL.Add(ToString());
                }

            }
            else
            {
                if (FileTransfer.ListR.Count > 0)
                {
                    if (FileTransfer.ListR.Last().ToString() != ToString())
                    {
                        FileTransfer.ListR.Add(ToString());
                    }
                }
                else
                {
                    FileTransfer.ListR.Add(ToString());
                }
            }

        }
    }
}
