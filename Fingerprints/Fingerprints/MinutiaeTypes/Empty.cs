﻿using Fingerprints.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Fingerprints
{
    class Empty : Minutiae, IDraw
    {
        public Empty()
        {
            this.state = new MinutiaState();
            this.state.Minutia = new SelfDefinedMinutiae() { Name = "Puste" };
            this.state.Id = 0;
        }
        public void DeleteEvent(Image image, OverridedCanvas canvas)
        {
        }

        public void Draw(OverridedCanvas canvas, Image image, int index = -1)
        {
            Point singlePoint = new Point(1, 1);
            EllipseGeometry myEllipseGeometry = new EllipseGeometry();
            myEllipseGeometry.Center = singlePoint;
            myEllipseGeometry.RadiusX = 0;
            myEllipseGeometry.RadiusY = 0;
            Path myPath = new Path();
            myPath.StrokeThickness = 0.3;
            myPath.Data = myEllipseGeometry;
            myPath.Opacity = 1;
            myPath.Name = "Puste";
            myPath.Tag = "Puste";
            canvas.AddLogicalChild(myPath, index);
            AddElementToSaveList(canvas.Tag.ToString(), index);
        }

        public void DrawFromFile(OverridedCanvas canvas)
        {
            Point singlePoint = new Point(1, 1);
            EllipseGeometry myEllipseGeometry = new EllipseGeometry();
            myEllipseGeometry.Center = singlePoint;
            myEllipseGeometry.RadiusX = 0;
            myEllipseGeometry.RadiusY = 0;
            Path myPath = new Path();
            myPath.StrokeThickness = 0.3;
            myPath.Data = myEllipseGeometry;
            myPath.Opacity = 1;
            myPath.Name = "Puste";
            myPath.Tag = "Puste";
            canvas.AddLogicalChild(myPath);
        }

        public string ToJson()
        {
            JObject minutiaeJson = new JObject();
            minutiaeJson["id"] = 0;
            minutiaeJson["name"] = "Puste";

            return minutiaeJson.ToString();
        }

        public override string ToString()
        {
            return 0 + ";" + "Puste";
        }
    }
}
