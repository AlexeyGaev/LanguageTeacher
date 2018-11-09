using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Reflection.Utils.PropertyTree.Serialization.Tests {
    [TestClass]
    public class StringWriterTests {
        [TestMethod]
        public void Default() {
            StringWriter writer = new StringWriter();
            Assert.AreEqual(null, writer.Result);
        }

        [TestMethod]
        public void Write() {
            StringWriter writer = new StringWriter();
            writer.Write("1");
            Assert.AreEqual(1, writer.Result.Count());
            Assert.AreEqual("1", writer.Result.ElementAt(0));
            writer.Write("2");
            Assert.AreEqual(1, writer.Result.Count());
            Assert.AreEqual("12", writer.Result.ElementAt(0));
        }

        [TestMethod]
        public void WriteLine() {
            StringWriter writer = new StringWriter();
            writer.WriteLine("1");
            Assert.AreEqual(1, writer.Result.Count());
            Assert.AreEqual("1", writer.Result.ElementAt(0));

            writer.WriteLine("2");
            Assert.AreEqual(2, writer.Result.Count());
            Assert.AreEqual("1", writer.Result.ElementAt(0));
            Assert.AreEqual("2", writer.Result.ElementAt(1));
        }

    }
}
