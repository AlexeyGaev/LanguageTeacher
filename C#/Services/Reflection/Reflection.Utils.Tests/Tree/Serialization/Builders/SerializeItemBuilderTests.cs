using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Reflection.Utils.PropertyTree.Serialization.Tests {
    [TestClass]
    public class SerializeItemBuilderTests {
        [TestMethod]
        public void CreateFieldIdItem() {
            string id = "Test";
            SerializeItem item = SerializeItemBuilder.CreateFieldIdItem(id);
            Assert.AreEqual(Localization.IdName, item.FirstValue);
            Assert.AreEqual(id, item.SecondValue);
            Assert.AreEqual(SerializeItemMode.TwoValues, item.Mode);
        }

        [TestMethod]
        public void CreateNullValueItem() {
            SerializeItem item = SerializeItemBuilder.CreateNullValueItem();
            Assert.AreEqual(Localization.ValueName, item.FirstValue);
            Assert.AreEqual(Localization.NullValue, item.SecondValue);
            Assert.AreEqual(SerializeItemMode.TwoValues, item.Mode);
        }

        [TestMethod]
        public void CreateTypeItem() {
            SerializeItem item = SerializeItemBuilder.CreateTypeItem(typeof(int));
            Assert.AreEqual(Localization.TypeName, item.FirstValue);
            Assert.AreEqual(typeof(int).Name, item.SecondValue);
            Assert.AreEqual(SerializeItemMode.TwoValues, item.Mode);
        }

        [TestMethod]
        public void CreateValueItem() {
            object obj = new object();
            SerializeItem item = SerializeItemBuilder.CreateValueItem(obj);
            Assert.AreEqual(Localization.ValueName, item.FirstValue);
            Assert.AreEqual(obj.ToString(), item.SecondValue);
            Assert.AreEqual(SerializeItemMode.TwoValues, item.Mode);
        }
    }
}
