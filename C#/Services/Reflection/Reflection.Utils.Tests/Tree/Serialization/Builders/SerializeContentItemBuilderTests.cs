using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Reflection.Utils.PropertyTree.Serialization.Tests {
    [TestClass]
    public class SerializeContentItemBuilderTests {
        [TestMethod]
        public void CheckAllNull() {
            PropertyField propertyField = new PropertyField(null, null);
            PropertyItem propertyItem = new PropertyItem(propertyField, null);
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
            Assert.AreEqual(SerializeItemMode.OneValue, item3.Mode);
            Assert.AreEqual(Localization.Delimeter, item3.FirstValue);
            
            SerializeItem item4 = contentItem.Header.ElementAt(3);
            Assert.AreEqual(SerializeItemMode.TwoValues, item4.Mode);
            Assert.AreEqual(Localization.ValueName, item4.FirstValue);
            Assert.AreEqual(Localization.NullValue, item4.SecondValue);
        }

        [TestMethod]
        public void CheckEmptyIdNullTypeNullValue() {
            PropertyField propertyField = new PropertyField(String.Empty, null);
            PropertyItem propertyItem = new PropertyItem(propertyField, null);
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
            Assert.AreEqual(SerializeItemMode.OneValue, item3.Mode);
            Assert.AreEqual(Localization.Delimeter, item3.FirstValue);

            SerializeItem item4 = contentItem.Header.ElementAt(3);
            Assert.AreEqual(SerializeItemMode.TwoValues, item4.Mode);
            Assert.AreEqual(Localization.ValueName, item4.FirstValue);
            Assert.AreEqual(Localization.NullValue, item4.SecondValue);
        }

        [TestMethod]
        public void CheckStringIdNullTypeNullValue() {
            PropertyField propertyField = new PropertyField("Test", null);
            PropertyItem propertyItem = new PropertyItem(propertyField, null);
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
            Assert.AreEqual(SerializeItemMode.OneValue, item3.Mode);
            Assert.AreEqual(Localization.Delimeter, item3.FirstValue);

            SerializeItem item4 = contentItem.Header.ElementAt(3);
            Assert.AreEqual(SerializeItemMode.TwoValues, item4.Mode);
            Assert.AreEqual(Localization.ValueName, item4.FirstValue);
            Assert.AreEqual(Localization.NullValue, item4.SecondValue);
        }

        [TestMethod]
        public void CheckStringIdIntTypeNullValue() {
            PropertyField propertyField = new PropertyField("Test", typeof(int));
            PropertyItem propertyItem = new PropertyItem(propertyField, null);
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
            Assert.AreEqual(SerializeItemMode.OneValue, item3.Mode);
            Assert.AreEqual(Localization.Delimeter, item3.FirstValue);

            SerializeItem item4 = contentItem.Header.ElementAt(3);
            Assert.AreEqual(SerializeItemMode.TwoValues, item4.Mode);
            Assert.AreEqual(Localization.ValueName, item4.FirstValue);
            Assert.AreEqual(Localization.NullValue, item4.SecondValue);
        }

        [TestMethod]
        public void CheckStringIdNullableIntTypeNullValue() {
            PropertyField propertyField = new PropertyField("Test", typeof(int?));
            PropertyItem propertyItem = new PropertyItem(propertyField, null);
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
            Assert.AreEqual(SerializeItemMode.OneValue, item3.Mode);
            Assert.AreEqual(Localization.Delimeter, item3.FirstValue);

            SerializeItem item4 = contentItem.Header.ElementAt(3);
            Assert.AreEqual(SerializeItemMode.TwoValues, item4.Mode);
            Assert.AreEqual(Localization.ValueName, item4.FirstValue);
            Assert.AreEqual(Localization.NullValue, item4.SecondValue);
        }

        [TestMethod]
        public void CheckIntIdIntTypeEmptyValue() {
            PropertyField propertyField = new PropertyField(1, typeof(int));
            PropertyItem propertyItem = new PropertyItem(propertyField, String.Empty);
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
            Assert.AreEqual("Int32", item2.SecondValue);

            SerializeItem item3 = contentItem.Header.ElementAt(2);
            Assert.AreEqual(SerializeItemMode.OneValue, item3.Mode);
            Assert.AreEqual(Localization.Delimeter, item3.FirstValue);
            
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
            PropertyField propertyField = new PropertyField(1, typeof(int));
            PropertyItem propertyItem = new PropertyItem(propertyField, "Test");
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
            Assert.AreEqual("Int32", item2.SecondValue);

            SerializeItem item3 = contentItem.Header.ElementAt(2);
            Assert.AreEqual(SerializeItemMode.OneValue, item3.Mode);
            Assert.AreEqual(Localization.Delimeter, item3.FirstValue);

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
            PropertyField propertyField = new PropertyField(1, typeof(int));
            PropertyItem propertyItem = new PropertyItem(propertyField, new object());
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
            Assert.AreEqual("Int32", item2.SecondValue);

            SerializeItem item3 = contentItem.Header.ElementAt(2);
            Assert.AreEqual(SerializeItemMode.OneValue, item3.Mode);
            Assert.AreEqual(Localization.Delimeter, item3.FirstValue);

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
            PropertyField propertyField = new PropertyField(1, typeof(int));
            int? value = 1;
            PropertyItem propertyItem = new PropertyItem(propertyField, value);
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
            Assert.AreEqual("Int32", item2.SecondValue);

            SerializeItem item3 = contentItem.Header.ElementAt(2);
            Assert.AreEqual(SerializeItemMode.OneValue, item3.Mode);
            Assert.AreEqual(Localization.Delimeter, item3.FirstValue);

            SerializeItem item4 = contentItem.Header.ElementAt(4);
            Assert.AreEqual(SerializeItemMode.TwoValues, item4.Mode);
            Assert.AreEqual(Localization.TypeName, item4.FirstValue);
            Assert.AreEqual("Int32", item4.SecondValue);

            SerializeItem item5 = contentItem.Header.ElementAt(3);
            Assert.AreEqual(SerializeItemMode.TwoValues, item5.Mode);
            Assert.AreEqual(Localization.ValueName, item5.FirstValue);
            Assert.AreEqual("1", item5.SecondValue);
        }

        [TestMethod]
        public void CheckIntIdIntTypeNullableNullIntValue() {
            PropertyField propertyField = new PropertyField(1, typeof(int));
            int? value = null;
            PropertyItem propertyItem = new PropertyItem(propertyField, value);
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
            Assert.AreEqual("Int32", item2.SecondValue);

            SerializeItem item3 = contentItem.Header.ElementAt(2);
            Assert.AreEqual(SerializeItemMode.OneValue, item3.Mode);
            Assert.AreEqual(Localization.Delimeter, item3.FirstValue);

            SerializeItem item4 = contentItem.Header.ElementAt(3);
            Assert.AreEqual(SerializeItemMode.TwoValues, item4.Mode);
            Assert.AreEqual(Localization.ValueName, item4.FirstValue);
            Assert.AreEqual(Localization.NullValue, item4.SecondValue);
        }
    }
}
