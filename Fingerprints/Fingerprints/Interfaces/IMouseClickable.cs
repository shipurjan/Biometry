﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Fingerprints.Interfaces
{
    interface IMouseClickable
    {
        void MouseClick(object sender, MouseButtonEventArgs args);
    }
}
