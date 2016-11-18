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
        public virtual void Draw(OverridedCanvas canvas, Image image, int index = -1) { }
        public virtual void DeleteEvent(Image image, OverridedCanvas canvas) { }
        public virtual void DrawFromFile(OverridedCanvas canvas) { }

        public void AddElementToSaveList(string listType, int index = -1)
        {
            if (listType == "Left")
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
            //if (ToString() != "" && list.LastOrDefault() != ToString()) zakomentowane ponieważ nie dodawalo kilku pustych pod rząd 
            //{
                if (index > -1)
                {
                    list.Insert(index, ToString());
                }
                else
                {
                    list.Add(ToString());
                }
            //}
        }
    }
}
