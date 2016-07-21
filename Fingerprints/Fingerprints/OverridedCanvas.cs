using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Fingerprints
{
    class OverridedCanvas : Canvas
    {
        public event EventHandler ChildAdded;
        public event EventHandler ChildRemoved;
        public OverridedCanvas()
        {
        }

        protected internal new void AddLogicalChild(Object child, int index = -1)
        {
            if (index >= 0)
                this.Children.Insert(index, (UIElement)child);
            else
                this.Children.Add((UIElement)child);

            OnChildAdded(this, EventArgs.Empty);
        }

        protected internal new void RemoveLogicalChild(Object child)
        {
            this.Children.Remove((UIElement)child);
            OnChildRemoved(this, EventArgs.Empty);
        }

        protected override IEnumerator LogicalChildren
        {
            get
            {
                return this.Children.GetEnumerator();
            }
        }

        protected virtual void OnChildAdded(Object sender, EventArgs e)
        {
            if (this.ChildAdded != null)
            {
                this.ChildAdded(this, e);
            }
        }

        protected virtual void OnChildRemoved(Object sender, EventArgs e)
        {
            if (this.ChildRemoved != null)
            {
                this.ChildRemoved(this, e);
            }
        }
    }
}
