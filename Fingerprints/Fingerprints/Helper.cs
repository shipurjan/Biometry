using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fingerprints
{
    class Helper
    {
        MainWindow mw;
        MinutiaeTypeController controller;
        public Helper(MainWindow mw, MinutiaeTypeController controller)
        {
            this.mw = mw;
            this.controller = controller;
        }

        public IDraw GetMinutiaeTypeToDraw
        {
            get  
            {
                string selectedValue = "";
                if (mw.comboBox.SelectedValue != null)
                    selectedValue = mw.comboBox.SelectedValue.ToString();

                string kolor = controller.GetColorOfSelectedMinutiae(selectedValue);
                double thickness = controller.GetThicknessOfSelectedMinutiae(selectedValue);
                double size = controller.GetSizeOfSelectedMinutiae(selectedValue);
                IDraw draw = null;
                if (controller.GetTypeIdOfSelectedMinutiae(selectedValue) == 2)
                {
                    draw = new Vector(selectedValue, kolor, size, thickness);
                }
                if (controller.GetTypeIdOfSelectedMinutiae(selectedValue) == 1)
                {
                    draw = new SinglePoint(selectedValue, kolor, size, thickness);
                }
                if (controller.GetTypeIdOfSelectedMinutiae(selectedValue) == 3)
                {
                    draw = new CurveLine(selectedValue, kolor, thickness, null, mw.curveLineCloseEvent);
                }
                if (controller.GetTypeIdOfSelectedMinutiae(selectedValue) == 4)
                {
                    draw = new Triangle(selectedValue, kolor, thickness);
                }
                if (controller.GetTypeIdOfSelectedMinutiae(selectedValue) == 5)
                {
                    draw = new Peak(selectedValue, kolor, thickness);
                }
                return draw;
            }
        }
    }
}
