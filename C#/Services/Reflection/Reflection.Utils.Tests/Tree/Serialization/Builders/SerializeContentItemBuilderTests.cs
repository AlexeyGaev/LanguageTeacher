using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Reflection.Utils.PropertyTree.Serialization.Tests {
    [TestClass]
    public class SerializeContentItemBuilderTests {
        [TestMethod]
        public void CheckAllNull() {
            PropertyItem propertyItem = new PropertyItem(null, null, null);
            SerializeContentItem contentItem = SerializeContentItemBuilder.Create(propertyItem);
            Assert.AreEqual(4, contentItem.Header.Count());
            Assert.AreEqual(null, contentItem.Content);

            SerializeItem item1 = contentItem.Header.ElementAt(0);
            Assert.AreEqual(SerializeItemMode.TwoValues, item1.Mode);
            Assert.AreEqual(Localization.IdName, item1.FirstValue);
            Assert.AreEqual(Localization.NullValue, item1.SecondValue);

            SerializeItem item2 = contentItem.Header.ElementAt(1);
            Assert.AreEqual(SerializeItemMode.TwoValues, item2.Mode);
            Assert.AreEqual(Localization.TypeName, item2.FirstValue);
            Assert.AreEqual(Localization.NullValue, item2.SecondValue);

            SerializeItem item3 = contentItem.Header.ElementAt(2);
            Assert.AreEqual(SerializeItemMode.Delimeter, item3.Mode);
                      
            SerializeItem item4 = contentItem.Header.ElementAt(3);
            Assert.AreEqual(SerializeItemMode.TwoValues, item4.Mode);
            Assert.AreEqual(Localization.ValueName, item4.FirstValue);
            Assert.AreEqual(Localization.NullValue, item4.SecondValue);
        }

        [TestMethod]
        public void CheckEmptyIdNullTypeNullValue() {
            PropertyItem propertyItem = new PropertyItem(String.Empty, null, null);
            SerializeContentItem contentItem = SerializeContentItemBuilder.Create(propertyItem);
            Assert.AreEqual(4, contentItem.Header.Count());
            Assert.AreEqual(null, contentItem.Content);

            SerializeItem item1 = contentItem.Header.ElementAt(0);
            Assert.AreEqual(SerializeItemMode.TwoValues, item1.Mode);
            Assert.AreEqual(Localization.IdName, item1.FirstValue);
            Assert.AreEqual(Localization.EmptyValue, item1.SecondValue);

            SerializeItem item2 = contentItem.Header.ElementAt(1);
            Assert.AreEqual(SerializeItemMode.TwoValues, item2.Mode);
            Assert.AreEqual(Localization.TypeName, item2.FirstValue);
            Assert.AreEqual(Localization.NullValue, item2.SecondValue);

            SerializeItem item3 = contentItem.Header.ElementAt(2);
            Assert.AreEqual(SerializeItemMode.Delimeter, item3.Mode);

            SerializeItem item4 = contentItem.Header.ElementAt(3);
            Assert.AreEqual(SerializeItemMode.TwoValues, item4.Mode);
            Assert.AreEqual(Localization.ValueName, item4.FirstValue);
            Assert.AreEqual(Localization.NullValue, item4.SecondValue);
        }

        [TestMethod]
        public void CheckStringIdNullTypeNullValue() {
            PropertyItem propertyItem = new PropertyItem("Test", null, null);
            SerializeContentItem contentItem = SerializeContentItemBuilder.Create(propertyItem);
            Assert.AreEqual(4, contentItem.Header.Count());
            Assert.AreEqual(null, contentItem.Content);

            SerializeItem item1 = contentItem.Header.ElementAt(0);
            Assert.AreEqual(SerializeItemMode.TwoValues, item1.Mode);
            Assert.AreEqual(Localization.IdName, item1.FirstValue);
            Assert.AreEqual("Test", item1.SecondValue);

            SerializeItem item2 = contentItem.Header.ElementAt(1);
            Assert.AreEqual(SerializeItemMode.TwoValues, item2.Mode);
            Assert.AreEqual(Localization.TypeName, item2.FirstValue);
            Assert.AreEqual(Localization.NullValue, item2.SecondValue);

            SerializeItem item3 = contentItem.Header.ElementAt(2);
            Assert.AreEqual(SerializeItemMode.Delimeter, item3.Mode);

            SerializeItem item4 = contentItem.Header.ElementAt(3);
            Assert.AreEqual(SerializeItemMode.TwoValues, item4.Mode);
            Assert.AreEqual(Localization.ValueName, item4.FirstValue);
            Assert.AreEqual(Localization.NullValue, item4.SecondValue);
        }

        [TestMethod]
        public void CheckStringIdIntTypeNullValue() {
            PropertyItem propertyItem = new PropertyItem("Test", typeof(int), null);
            SerializeContentItem contentItem = SerializeContentItemBuilder.Create(propertyItem);
            Assert.AreEqual(4, contentItem.Header.Count());
            Assert.AreEqual(null, contentItem.Content);

            SerializeItem item1 = contentItem.Header.ElementAt(0);
            Assert.AreEqual(SerializeItemMode.TwoValues, item1.Mode);
            Assert.AreEqual(Localization.IdName, item1.FirstValue);
            Assert.AreEqual("Test", item1.SecondValue);

            SerializeItem item2 = contentItem.Header.ElementAt(1);
            Assert.AreEqual(SerializeItemMode.TwoValues, item2.Mode);
            Assert.AreEqual(Localization.TypeName, item2.FirstValue);
            Assert.AreEqual("Int32", item2.SecondValue);

            SerializeItem item3 = contentItem.Header.ElementAt(2);
            Assert.AreEqual(SerializeItemMode.Delimeter, item3.Mode);

            SerializeItem item4 = contentItem.Header.ElementAt(3);
            Assert.AreEqual(SerializeItemMode.TwoValues, item4.Mode);
            Assert.AreEqual(Localization.ValueName, item4.FirstValue);
            Assert.AreEqual(Localization.NullValue, item4.SecondValue);
        }

        [TestMethod]
        public void CheckStringIdNullableIntTypeNullValue() {
            PropertyItem propertyItem = new PropertyItem("Test", typeof(int?), null);
            SerializeContentItem contentItem = SerializeContentItemBuilder.Create(propertyItem);
            Assert.AreEqual(4, contentItem.Header.Count());
            Assert.AreEqual(null, contentItem.Content);

            SerializeItem item1 = contentItem.Header.ElementAt(0);
            Assert.AreEqual(SerializeItemMode.TwoValues, item1.Mode);
            Assert.AreEqual(Localization.IdName, item1.FirstValue);
            Assert.AreEqual("Test", item1.SecondValue);

            SerializeItem item2 = contentItem.Header.ElementAt(1);
            Assert.AreEqual(SerializeItemMode.TwoValues, item2.Mode);
            Assert.AreEqual(Localization.TypeName, item2.FirstValue);
            Assert.AreEqual(Localization.Nullable + " Int32", item2.SecondValue);

            SerializeItem item3 = contentItem.Header.ElementAt(2);
            Assert.AreEqual(SerializeItemMode.Delimeter, item3.Mode);

            SerializeItem item4 = contentItem.Header.ElementAt(3);
            Assert.AreEqual(SerializeItemMode.TwoValues, item4.Mode);
            Assert.AreEqual(Localization.ValueName, item4.FirstValue);
            Assert.AreEqual(Localization.NullValue, item4.SecondValue);
        }

        [TestMethod]
        public void CheckIntIdIntTypeEmptyValue() {
            PropertyItem propertyItem = new PropertyItem(1, typeof(int), String.Empty);
            SerializeContentItem contentItem = SerializeContentItemBuilder.Create(propertyItem);
            Assert.AreEqual(5, contentItem.Header.Count());
            Assert.AreEqual(null, contentItem.Content);

            SerializeItem item1 = contentItem.Header.ElementAt(0);
            Assert.AreEqual(SerializeItemMode.TwoValues, item1.Mode);
            Assert.AreEqual(Localization.IdName, item1.FirstValue);
            Assert.AreEqual("1", item1.SecondValue);

            SerializeItem item2 = contentItem.Header.ElementAt(1);
            Assert.AreEqual(SerializeItemMode.TwoValues, item2.Mode);
            Assert.AreEqual(Localization.TypeName, item2.FirstValue);
            Assert.AreEqual(typeof(int).Name, item2.SecondValue);

            SerializeItem item3 = contentItem.Header.ElementAt(2);
            Assert.AreEqual(SerializeItemMode.Delimeter, item3.Mode);

            SerializeItem item4 = contentItem.Header.ElementAt(4);
            Assert.AreEqual(SerializeItemMode.TwoValues, item4.Mode);
            Assert.AreEqual(Localization.TypeName, item4.FirstValue);
            Assert.AreEqual("String", item4.SecondValue);

            SerializeItem item5 = contentItem.Header.ElementAt(3);
            Assert.AreEqual(SerializeItemMode.TwoValues, item5.Mode);
            Assert.AreEqual(Localization.ValueName, item5.FirstValue);
            Assert.AreEqual(Localization.EmptyValue, item5.SecondValue);
        }

        [TestMethod]
        public void CheckIntIdIntTypeStringValue() {
            PropertyItem propertyItem = new PropertyItem(1, typeof(int), "Test");
            SerializeContentItem contentItem = SerializeContentItemBuilder.Create(propertyItem);
            Assert.AreEqual(5, contentItem.Header.Count());
            Assert.AreEqual(null, contentItem.Content);

            SerializeItem item1 = contentItem.Header.ElementAt(0);
            Assert.AreEqual(SerializeItemMode.TwoValues, item1.Mode);
            Assert.AreEqual(Localization.IdName, item1.FirstValue);
            Assert.AreEqual("1", item1.SecondValue);

            SerializeItem item2 = contentItem.Header.ElementAt(1);
            Assert.AreEqual(SerializeItemMode.TwoValues, item2.Mode);
            Assert.AreEqual(Localization.TypeName, item2.FirstValue);
            Assert.AreEqual(typeof(int).Name, item2.SecondValue);

            SerializeItem item3 = contentItem.Header.ElementAt(2);
            Assert.AreEqual(SerializeItemMode.Delimeter, item3.Mode);

            SerializeItem item4 = contentItem.Header.ElementAt(4);
            Assert.AreEqual(SerializeItemMode.TwoValues, item4.Mode);
            Assert.AreEqual(Localization.TypeName, item4.FirstValue);
            Assert.AreEqual("String", item4.SecondValue);

            SerializeItem item5 = contentItem.Header.ElementAt(3);
            Assert.AreEqual(SerializeItemMode.TwoValues, item5.Mode);
            Assert.AreEqual(Localization.ValueName, item5.FirstValue);
            Assert.AreEqual("Test", item5.SecondValue);
        }

        [TestMethod]
        public void CheckIntIdIntTypeObjectValue() {
            PropertyItem propertyItem = new PropertyItem(1, typeof(int), new object());
            SerializeContentItem contentItem = SerializeContentItemBuilder.Create(propertyItem);
            Assert.AreEqual(5, contentItem.Header.Count());
            Assert.AreEqual(null, contentItem.Content);

            SerializeItem item1 = contentItem.Header.ElementAt(0);
            Assert.AreEqual(SerializeItemMode.TwoValues, item1.Mode);
            Assert.AreEqual(Localization.IdName, item1.FirstValue);
            Assert.AreEqual("1", item1.SecondValue);

            SerializeItem item2 = contentItem.Header.ElementAt(1);
            Assert.AreEqual(SerializeItemMode.TwoValues, item2.Mode);
            Assert.AreEqual(Localization.TypeName, item2.FirstValue);
            Assert.AreEqual(typeof(int).Name, item2.SecondValue);

            SerializeItem item3 = contentItem.Header.ElementAt(2);
            Assert.AreEqual(SerializeItemMode.Delimeter, item3.Mode);

            SerializeItem item4 = contentItem.Header.ElementAt(4);
            Assert.AreEqual(SerializeItemMode.TwoValues, item4.Mode);
            Assert.AreEqual(Localization.TypeName, item4.FirstValue);
            Assert.AreEqual("Object", item4.SecondValue);

            SerializeItem item5 = contentItem.Header.ElementAt(3);
            Assert.AreEqual(SerializeItemMode.TwoValues, item5.Mode);
            Assert.AreEqual(Localization.ValueName, item5.FirstValue);
            Assert.AreEqual("System.Object", item5.SecondValue);
        }

        [TestMethod]
        public void CheckIntIdIntTypeNullableIntValue() {
            PropertyItem propertyItem = new PropertyItem(1, typeof(int?), (int?)1);
            SerializeContentItem contentItem = SerializeContentItemBuilder.Create(propertyItem);
            Assert.AreEqual(5, contentItem.Header.Count());
            Assert.AreEqual(null, contentItem.Content);

            SerializeItem item1 = contentItem.Header.ElementAt(0);
            Assert.AreEqual(SerializeItemMode.TwoValues, item1.Mode);
            Assert.AreEqual(Localization.IdName, item1.FirstValue);
            Assert.AreEqual("1", item1.SecondValue);

            SerializeItem item2 = contentItem.Header.ElementAt(1);
            Assert.AreEqual(SerializeItemMode.TwoValues, item2.Mode);
            Assert.AreEqual(Localization.TypeName, item2.FirstValue);
            Assert.AreEqual(Localization.Nullable + " " + typeof(int).Name, item2.SecondValue);

            SerializeItem item3 = contentItem.Header.ElementAt(2);
            Assert.AreEqual(SerializeItemMode.Delimeter, item3.Mode);

            SerializeItem item4 = contentItem.Header.ElementAt(4);
            Assert.AreEqual(SerializeItemMode.TwoValues, item4.Mode);
            Assert.AreEqual(Localization.TypeName, item4.FirstValue);
            Assert.AreEqual(typeof(int).Name, item4.SecondValue);

            SerializeItem item5 = contentItem.Header.ElementAt(3);
            Assert.AreEqual(SerializeItemMode.TwoValues, item5.Mode);
            Assert.AreEqual(Localization.ValueName, item5.FirstValue);
            Assert.AreEqual("1", item5.SecondValue);
        }

        [TestMethod]
        public void CheckIntIdIntTypeNullableNullIntValue() {
            PropertyItem propertyItem = new PropertyItem(1, typeof(int?), (int?)null);
            SerializeContentItem contentItem = SerializeContentItemBuilder.Create(propertyItem);
            Assert.AreEqual(4, contentItem.Header.Count());
            Assert.AreEqual(null, contentItem.Content);

            SerializeItem item1 = contentItem.Header.ElementAt(0);
            Assert.AreEqual(SerializeItemMode.TwoValues, item1.Mode);
            Assert.AreEqual(Localization.IdName, item1.FirstValue);
            Assert.AreEqual("1", item1.SecondValue);

            SerializeItem item2 = contentItem.Header.ElementAt(1);
            Assert.AreEqual(SerializeItemMode.TwoValues, item2.Mode);
            Assert.AreEqual(Localization.TypeName, item2.FirstValue);
            Assert.AreEqual(Localization.Nullable + " " + typeof(int).Name, item2.SecondValue);

            SerializeItem item3 = contentItem.Header.ElementAt(2);
            Assert.AreEqual(SerializeItemMode.Delimeter, item3.Mode);

            SerializeItem item4 = contentItem.Header.ElementAt(3);
            Assert.AreEqual(SerializeItemMode.TwoValues, item4.Mode);
            Assert.AreEqual(Localization.ValueName, item4.FirstValue);
            Assert.AreEqual(Localization.NullValue, item4.SecondValue);
        }
    }
}
