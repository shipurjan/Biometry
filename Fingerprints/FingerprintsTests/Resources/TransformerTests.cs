using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Fingerprints.Resources.Tests
{
    [TestClass()]
    public class TransformerTests
    {
        Transformer transformer;
        Dictionary<string, string> minutiaeData;
        [ClassInitialize()]
        public static void ClassInit(TestContext context)
        {
            App application = new App();
        }

        [TestInitialize()]
        public void Initialize()
        {
            transformer = new Transformer();
            minutiaeData = new Dictionary<string, string>();
            minutiaeData.Add("peak", "1487502148306;Daszek;161;356;194;328;208;371");
            minutiaeData.Add("vector", "1486934055194;Prosta;233;385;-1,02784518636331");
        }

        [TestMethod()]
        public void transformPeakToXYTest()
        {
            string expected = "194 328 254";
            string actual = transformer.transformPeakToXYT(minutiaeData["peak"]);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void transformVectorToXYTTest()
        {
            string expected = "233 385 59";
            string actual = transformer.transformVectorToXYT(minutiaeData["vector"]);

            Assert.AreEqual(expected, actual);
        }
    }
}