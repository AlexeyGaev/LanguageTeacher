using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Reflection.Utils.PropertyTree.Serialization.Tests {
    [TestClass]
    public class SerializeItemsBuilderTests {
        public static void CheckCollectionCountHeader(IEnumerable<SerializeItem> items, string name, int count) {
            Assert.AreEqual(2, items.Count());

            SerializeItem item1 = items.ElementAt(0);
            Assert.AreEqual(name, item1.FirstValue);
            Assert.AreEqual(SerializeItemMode.OneValue, item1.Mode);

            SerializeItem item2 = items.ElementAt(1);
            Assert.AreEqual(Localization.Count, item2.FirstValue);
            Assert.AreEqual(count.ToString(), item2.SecondValue);
            Assert.AreEqual(SerializeItemMode.TwoValues, item2.Mode);
        }

        public static void CheckCollectionCycleHeader(IEnumerable<SerializeItem> items, string name, string cycle) {
            Assert.AreEqual(3, items.Count());

            SerializeItem item1 = items.ElementAt(0);
            Assert.AreEqual(name, item1.FirstValue);
            Assert.AreEqual(SerializeItemMode.OneValue, item1.Mode);

            SerializeItem item2 = items.ElementAt(1);
            Assert.AreEqual(Localization.Count, item2.FirstValue);
            Assert.AreEqual("0", item2.SecondValue);
            Assert.AreEqual(SerializeItemMode.TwoValues, item2.Mode);

            SerializeItem headerItem3 = items.ElementAt(2);
            Assert.AreEqual(cycle, headerItem3.FirstValue);
            Assert.AreEqual(SerializeItemMode.OneValue, headerItem3.Mode);
        }

        [TestMethod]
        public void CreateCollectionCountHeader() {
            string name = "Test";
            IEnumerable<SerializeItem> items = SerializeItemsBuilder.CreateCollectionHeader(name, 2, CycleMode.None);
            CheckCollectionCountHeader(items, name, 2);
        }

        [TestMethod]
        public void CreateCollectionCycleHeader() {
            string name = "Test";
            IEnumerable<SerializeItem> items = SerializeItemsBuilder.CreateCollectionHeader(name, 0, CycleMode.Value);
            CheckCollectionCycleHeader(items, name, Localization.ValueCycle);
        }
        
        [TestMethod]
        public void CreateItemsFromPropertyValue_Null() {
            IEnumerable<SerializeItem> items = SerializeItemsBuilder.CreateItemsFromPropertyValue(null);
            Assert.AreEqual(1, items.Count());

            SerializeItem item1 = items.ElementAt(0);
            Assert.AreEqual(Localization.ValueName, item1.FirstValue);
            Assert.AreEqual(Localization.NullValue, item1.SecondValue);
            Assert.AreEqual(SerializeItemMode.TwoValues, item1.Mode);
        }

        [TestMethod]
        public void CreateItemsFromPropertyValue_Object() {
            object obj = new object();
            IEnumerable<SerializeItem> items = SerializeItemsBuilder.CreateItemsFromPropertyValue(obj);
            Assert.AreEqual(2, items.Count());

            SerializeItem item1 = items.ElementAt(0);
            Assert.AreEqual(Localization.ValueName, item1.FirstValue);
            Assert.AreEqual(obj.ToString(), item1.SecondValue);
            Assert.AreEqual(SerializeItemMode.TwoValues, item1.Mode);

            SerializeItem item2 = items.ElementAt(1);
            Assert.AreEqual(Localization.TypeName, item2.FirstValue);
            Assert.AreEqual(typeof(object).Name, item2.SecondValue);
            Assert.AreEqual(SerializeItemMode.TwoValues, item2.Mode);
        }

        [TestMethod]
        public void CreatePropertyItemHeader() {
            IEnumerable<SerializeItem> items = SerializeItemsBuilder.CreatePropertyItemHeader(null, null, null);
            Assert.AreEqual(4, items.Count());
          
            SerializeItem item1 = items.ElementAt(0);
            Assert.AreEqual(SerializeItemMode.TwoValues, item1.Mode);
            Assert.AreEqual(Localization.IdName, item1.FirstValue);
            Assert.AreEqual(Localization.NullValue, item1.SecondValue);

            SerializeItem item2 = items.ElementAt(1);
            Assert.AreEqual(SerializeItemMode.TwoValues, item2.Mode);
            Assert.AreEqual(Localization.TypeName, item2.FirstValue);
            Assert.AreEqual(Localization.NullValue, item2.SecondValue);

            SerializeItem item3 = items.ElementAt(2);
            Assert.AreEqual(SerializeItemMode.Delimeter, item3.Mode);
          
            SerializeItem item4 = items.ElementAt(3);
            Assert.AreEqual(SerializeItemMode.TwoValues, item4.Mode);
            Assert.AreEqual(Localization.ValueName, item4.FirstValue);
            Assert.AreEqual(Localization.NullValue, item4.SecondValue);
        }
    }
}
