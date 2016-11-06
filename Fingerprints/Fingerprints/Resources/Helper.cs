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

        public IDraw GetMinutiaeTypeToDraw(string minutiaeName)
        {
            string kolor = controller.GetColorOfSelectedMinutiae(minutiaeName);
            double thickness = controller.GetThicknessOfSelectedMinutiae(minutiaeName);
            double size = controller.GetSizeOfSelectedMinutiae(minutiaeName);
            IDraw draw = null;
            if (controller.GetTypeIdOfSelectedMinutiae(minutiaeName) == 2)
            {
                draw = new Vector(minutiaeName, kolor, size, thickness);
            }
            if (controller.GetTypeIdOfSelectedMinutiae(minutiaeName) == 1)
            {
                draw = new SinglePoint(minutiaeName, kolor, size, thickness);
            }
            if (controller.GetTypeIdOfSelectedMinutiae(minutiaeName) == 3)
            {
                draw = new CurveLine(minutiaeName, kolor, thickness, null);
            }
            if (controller.GetTypeIdOfSelectedMinutiae(minutiaeName) == 4)
            {
                draw = new Triangle(minutiaeName, kolor, thickness);
            }
            if (controller.GetTypeIdOfSelectedMinutiae(minutiaeName) == 5)
            {
                draw = new Peak(minutiaeName, kolor, thickness);
            }
            return draw;
        }
    }
}
