using Microsoft.VisualStudio.TestTools.UnitTesting;
using Fingerprints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using Fingerprints.Models;

namespace Fingerprints.Tests
{
    [TestClass()]
    public class CurveLineTests
    {
        CurveLine curve;
        PointCollection points;

        [ClassInitialize()]
        public static void ClassInit(TestContext context)
        {
            App application = new App();
        }

        [TestInitialize()]
        public void Initialize()
        {
            curve = new CurveLine("krzywa", "#333", 1.0);
            Point p1 = new Point() { X = 26, Y = 32 };
            Point p2 = new Point() { X = 101, Y = 163 };
            points = new PointCollection();
            points.Add(p1);
            points.Add(p2);
        }

        [TestMethod()]
        public void convertLinesToPointsTest()
        {
            Point p1 = new Point() { X = 20, Y = 20 };
            Point p2 = new Point() { X = 22, Y = 22 };
            Point p3 = new Point() { X = 25, Y = 25 };
            PointCollection points = new PointCollection();
            points.Add(p1);
            points.Add(p2);

            PointCollection expected = new PointCollection();
            expected.Add(p1);
            expected.Add(new Point() { X = 21, Y = 21 });
            expected.Add(p2);
            expected.Add(new Point() { X = 23, Y = 23 });
            expected.Add(new Point() { X = 24, Y = 24 });
            expected.Add(p3);

            PointCollection actual = curve.convertLinesToPoints(points);

            //Assert.AreEqual<PointCollection>(expected, actual);
            for (int i = 0; i < actual.Count - 1; i++)
            {
                Assert.AreEqual(actual[i], expected[i]);
            }
        }

        [TestMethod()]
        public void convertLinesToPointsTest2()
        {
            Point p1 = new Point() { X = 20, Y = 25 };
            Point p2 = new Point() { X = 25, Y = 20 };
            PointCollection points = new PointCollection();
            points.Add(p1);
            points.Add(p2);

            PointCollection expected = new PointCollection();
            expected.Add(p1);
            expected.Add(new Point() { X = 21, Y = 24 });
            expected.Add(new Point() { X = 22, Y = 23 });
            expected.Add(new Point() { X = 23, Y = 22 });
            expected.Add(new Point() { X = 24, Y = 21 });
            expected.Add(p2);


            PointCollection actual = curve.convertLinesToPoints(points);

            //Assert.AreEqual<PointCollection>(expected, actual);
            for (int i = 0; i < actual.Count - 1; i++)
            {
                Assert.AreEqual(actual[i], expected[i]);
            }
        }

        [TestMethod()]
        public void convertLinesToPointsTest3()
        {
            Point p1 = new Point() { X = 25, Y = 20 };
            Point p2 = new Point() { X = 20, Y = 25 };
            PointCollection points = new PointCollection();
            points.Add(p1);
            points.Add(p2);

            PointCollection expected = new PointCollection();
            expected.Add(p1);
            expected.Add(new Point() { X = 24, Y = 21 });
            expected.Add(new Point() { X = 23, Y = 22 });
            expected.Add(new Point() { X = 22, Y = 23 });
            expected.Add(new Point() { X = 21, Y = 24 });
            expected.Add(p2);


            PointCollection actual = curve.convertLinesToPoints(points);

            //Assert.AreEqual<PointCollection>(expected, actual);
            for (int i = 0; i < actual.Count - 1; i++)
            {
                Assert.AreEqual(actual[i], expected[i]);
            }
        }

        [TestMethod()]
        public void convertLinesToPointsTest4()
        {
            Point p1 = new Point() { X = 5, Y = 12 };
            Point p2 = new Point() { X = 8, Y = 4 };
            PointCollection points = new PointCollection();
            points.Add(p1);
            points.Add(p2);

            PointCollection expected = new PointCollection();
            expected.Add(p1);
            expected.Add(new Point() { X = 5, Y = 11 });
            expected.Add(new Point() { X = 5, Y = 10 });
            expected.Add(new Point() { X = 6, Y = 9 });
            expected.Add(new Point() { X = 6, Y = 8 });
            expected.Add(new Point() { X = 6, Y = 7 });
            expected.Add(new Point() { X = 7, Y = 6 });
            expected.Add(new Point() { X = 7, Y = 5 });
            expected.Add(p2);


            PointCollection actual = curve.convertLinesToPoints(points);

            //Assert.AreEqual<PointCollection>(expected, actual);
            for (int i = 0; i < actual.Count - 1; i++)
            {
                Assert.AreEqual(actual[i], expected[i]);
            }
        }

        [TestMethod()]
        public void calculateATest()
        {
            double expected = -131;

            double actual = curve.calculateA(points[0], points[1]);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void calculateBTest()
        {
            double expected = 75;

            double actual = curve.calculateB(points[0], points[1]);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void calculateCTest()
        {
            double expected = 1006;

            double actual = curve.calculateC(points[0], points[1]);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void lengthOfLineTest()
        {
            Point p1 = new Point() { X = 5, Y = 12 };
            Point p2 = new Point() { X = 6, Y = 9 };

            double expected = 3.16;

            double actual = Math.Round(curve.lengthOfLine(p1, p2), 2);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void fillSpaceBetweenPointsAndAddPointTest()
        {
            Point p1 = new Point() { X = 5, Y = 12 };
            Point p2 = new Point() { X = 6, Y = 9 };

            PointCollection points = new PointCollection();
            points.Add(p1);

            PointCollection pointsReturned = curve.fillSpaceBetweenPointsAndAddPoint(points, p2);
        }
    }
}