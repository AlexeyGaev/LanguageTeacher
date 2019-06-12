using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Reflection.Utils.PropertyTree.Serialization.Tests {
    [TestClass]
    public class SerializeItemTests {
        [TestMethod]
        public void CheckEmpty() {
            SerializeItem item = SerializeItem.Empty;
            Assert.AreEqual(null, item.FirstValue);
            Assert.AreEqual(null, item.SecondValue);
            Assert.AreEqual(SerializeItemMode.Empty, item.Mode);
        }

        [TestMethod]
        public void CheckDelimeter() {
            SerializeItem item = SerializeItem.Delimeter;
            Assert.AreEqual(null, item.FirstValue);
            Assert.AreEqual(null, item.SecondValue);
            Assert.AreEqual(SerializeItemMode.Delimeter, item.Mode);
        }

        [TestMethod]
        public void CheckOneValue() {
            SerializeItem item = SerializeItem.CreateOneValue("Test");
            Assert.AreEqual("Test", item.FirstValue);
            Assert.AreEqual("Test", item.SecondValue);
            Assert.AreEqual(SerializeItemMode.OneValue, item.Mode);
        }

        [TestMethod]
        public void CheckTwoValues() {
            SerializeItem item = SerializeItem.CreateTwoValues("Test1", "Test2");
            Assert.AreEqual("Test1", item.FirstValue);
            Assert.AreEqual("Test2", item.SecondValue);
            Assert.AreEqual(SerializeItemMode.TwoValues, item.Mode);
        }
    }
}
