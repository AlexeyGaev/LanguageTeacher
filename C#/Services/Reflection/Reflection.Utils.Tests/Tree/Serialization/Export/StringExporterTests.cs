using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Reflection.Utils.PropertyTree.Serialization.Tests {
    [TestClass]
    public class StringExporterTests {
        [TestMethod]
        public void Export_ContentIsNull() {
            StringWriter writer = new StringWriter();

            StringExporter.Export(writer, null);

            Assert.AreEqual(null, writer.Result);
        }

        [TestMethod]
        public void Export_ContentWithHeaderOnly() {
            StringWriter writer = new StringWriter();
            List<SerializeItem> serializeItems = new List<SerializeItem>();
            serializeItems.Add(SerializeItem.CreateOneValue("Test"));
            SerializeContentItem item = new SerializeContentItem(serializeItems);

            StringExporter.Export(writer, item, 2);

            Assert.AreEqual(1, writer.Result.Count());
            Assert.AreEqual("  Test", writer.Result.ElementAt(0));
        }

        [TestMethod]
        public void Export_ContentWithChildren() {
            StringWriter writer = new StringWriter();
            List<SerializeItem> serializeItems = new List<SerializeItem>();
            serializeItems.Add(SerializeItem.CreateOneValue("Test"));
            SerializeContentItem item = new SerializeContentItem(serializeItems);
            List<SerializeContentItemCollection> contentCollection = new List<SerializeContentItemCollection>();
            List<SerializeItem> header = new List<SerializeItem>() { SerializeItem.CreateTwoValues("Test1", "Test2") };
            SerializeContentItemCollection collection = new SerializeContentItemCollection(header);
            contentCollection.Add(collection);
            item.Content = contentCollection;

            StringExporter.Export(writer, item, 0);

            Assert.AreEqual(2, writer.Result.Count());
            Assert.AreEqual("Test", writer.Result.ElementAt(0));
            Assert.AreEqual(" Test1=Test2", writer.Result.ElementAt(1));
        }
    }
}
