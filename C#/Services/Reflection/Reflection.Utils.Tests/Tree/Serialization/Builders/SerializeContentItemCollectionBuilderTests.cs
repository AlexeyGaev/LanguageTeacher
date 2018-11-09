using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Reflection.Utils.PropertyTree.Serialization.Tests {
    [TestClass]
    public class SerializeContentItemCollectionBuilderTests {
        public static void CheckCycle(SerializeContentItemCollection collection, string name) {
            Assert.AreEqual(0, collection.Count);
            SerializeItemsBuilderTests.CheckCollectionCycleHeader(collection.Header, name);
        }

        public static void CheckEmpty(SerializeContentItemCollection collection, string name) {
            Assert.AreEqual(0, collection.Count);
            SerializeItemsBuilderTests.CheckCollectionCountHeader(collection.Header, name, 0);
        }

        [TestMethod]
        public void CreateCycle() {
            SerializeContentItemCollection collection = SerializeContentItemCollectionBuilder.CreateCycle("Test");
            CheckCycle(collection, "Test");
        }

        [TestMethod]
        public void Create_Empty() {
            List<PropertyItem> propertyItems = new List<PropertyItem>();
            SerializeContentItemCollection collection = SerializeContentItemCollectionBuilder.Create(propertyItems, "Test");
            CheckEmpty(collection, "Test");
        }

        [TestMethod]
        public void Create_NotEmpty() {
            List<PropertyItem> propertyItems = new List<PropertyItem>() { new PropertyItem(new PropertyField(null, null), null) };
            SerializeContentItemCollection collection = SerializeContentItemCollectionBuilder.Create(propertyItems, "Test");
            SerializeItemsBuilderTests.CheckCollectionCountHeader(collection.Header, "Test", 1);

            Assert.AreEqual(1, collection.Count);

            SerializeContentItem contentItem = collection[0];
            Assert.AreEqual(4, contentItem.Header.Count());
            Assert.AreEqual(null, contentItem.Content);
            
            SerializeItem serializeItem1 = contentItem.Header.ElementAt(0);
            Assert.AreEqual(SerializeItemMode.TwoValues, serializeItem1.Mode);
            Assert.AreEqual(Localization.IdName, serializeItem1.FirstValue);
            Assert.AreEqual(Localization.NullValue, serializeItem1.SecondValue);

            SerializeItem serializeItem2 = contentItem.Header.ElementAt(1);
            Assert.AreEqual(SerializeItemMode.TwoValues, serializeItem2.Mode);
            Assert.AreEqual(Localization.TypeName, serializeItem2.FirstValue);
            Assert.AreEqual(Localization.NullValue, serializeItem2.SecondValue);

            SerializeItem serializeItem3 = contentItem.Header.ElementAt(2);
            Assert.AreEqual(SerializeItemMode.OneValue, serializeItem3.Mode);
            Assert.AreEqual(Localization.Delimeter, serializeItem3.FirstValue);

            SerializeItem serializeItem4 = contentItem.Header.ElementAt(3);
            Assert.AreEqual(SerializeItemMode.TwoValues, serializeItem4.Mode);
            Assert.AreEqual(Localization.ValueName, serializeItem4.FirstValue);
            Assert.AreEqual(Localization.NullValue, serializeItem4.SecondValue);
        }
    }
}
