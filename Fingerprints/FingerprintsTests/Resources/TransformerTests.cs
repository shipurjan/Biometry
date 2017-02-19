using Microsoft.VisualStudio.TestTools.UnitTesting;
using Fingerprints.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fingerprints.Resources.Tests
{
    [TestClass()]
    public class TransformerTests
    {
        Transformer transformer;
        List<string> minutiaeData;
        [ClassInitialize()]
        public static void ClassInit(TestContext context)
        {
            App application = new App();
        }

        [TestInitialize()]
        public void Initialize()
        {
            transformer = new Transformer();
            minutiaeData = new List<string>();
            minutiaeData.Add("1487502148306;Daszek;161;356;194;328;208;371");
            minutiaeData.Add("1486934055194;Prosta;233;385;-1,02784518636331");
        }

        [TestMethod()]
        public void geBozorthFormatTest()
        {
            List<string> expected = new List<string>();
            expected.Add("194 328 255");
            expected.Add("233 385 59");
            List<string> actual = transformer.getBozorthFormat(minutiaeData);
        }

        [TestMethod()]
        public void transformPeakToXYTest()
        {
            string excepted = "192 328 ";
            string actual = transformer.transformPeakToXYT(minutiaeData[0]);
        }
    }
}