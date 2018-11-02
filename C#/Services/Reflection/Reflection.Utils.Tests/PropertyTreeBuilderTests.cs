using Microsoft.VisualStudio.TestTools.UnitTesting;
using Reflection.Utils.PropertyTree;
using Reflection.Utils.Tree;
using System;

namespace Reflection.Utils.Tests {
    [TestClass]
    public class PropertyTreeBuilderTests_Root {
        [TestMethod]
        public void Null_Null_Null() {
            Tree<PropertyDescription> tree = PropertyTreeBuilder.Create(null, null, null);

            TreeItem<PropertyDescription> rootItem = tree.RootItem;
            Assert.AreEqual(0, rootItem.Children.Count);

            PropertyDescription property = rootItem.Value;
            Assert.IsNull(property.PropertyName);
            Assert.IsNull(property.PropertyType);
            Assert.IsNull(property.PropertyValue);
            Assert.IsNull(property.Owner);
        }

        [TestMethod]
        public void Null_Empty_Null() {
            Tree<PropertyDescription> tree = PropertyTreeBuilder.Create(null, String.Empty, null);

            TreeItem<PropertyDescription> rootItem = tree.RootItem;
            Assert.AreEqual(0, rootItem.Children.Count);

            PropertyDescription property = rootItem.Value;
            Assert.AreEqual(String.Empty, property.PropertyName);
            Assert.IsNull(property.PropertyType);
            Assert.IsNull(property.PropertyValue);
            Assert.IsNull(property.Owner);
        }

        [TestMethod]
        public void ObjectValue() {
            object source = new object();
            string sourceName = "Object";
            Type sourceType = typeof(object);
            Tree<PropertyDescription> tree = PropertyTreeBuilder.Create(source, sourceName, sourceType);

            TreeItem<PropertyDescription> rootItem = tree.RootItem;
            Assert.AreEqual(0, rootItem.Children.Count);

            PropertyDescription property = rootItem.Value;
            Assert.AreEqual(sourceName, property.PropertyName);
            Assert.AreEqual(sourceType, property.PropertyType);
            Assert.IsNull(property.Owner);

            PropertyValueClass propertyValue = property.PropertyValue as PropertyValueClass;
            Assert.IsTrue(Object.ReferenceEquals(source, propertyValue.Value));
            Assert.IsFalse(propertyValue.HasChildren);
            Assert.AreEqual(PropertyValueType.Class, propertyValue.Type);
        }

        [TestMethod]
        public void IntValue() {
            int source = 1;
            string sourceName = "integer";
            Type sourceType = typeof(int);
            Tree<PropertyDescription> tree = PropertyTreeBuilder.Create(source, sourceName, sourceType);

            TreeItem<PropertyDescription> rootItem = tree.RootItem;
            Assert.AreEqual(0, rootItem.Children.Count);

            PropertyDescription property = rootItem.Value;
            Assert.AreEqual(sourceName, property.PropertyName);
            Assert.AreEqual(sourceType, property.PropertyType);
            Assert.IsNull(property.Owner);

            PropertyValuePrimitive propertyValue = property.PropertyValue as PropertyValuePrimitive;
            Assert.AreEqual(1, propertyValue.Value);
            Assert.IsFalse(propertyValue.IsNullable);
            Assert.IsFalse(propertyValue.HasChildren);
            Assert.AreEqual(PropertyValueType.Primitive, propertyValue.Type);
        }

        [TestMethod]
        public void NullableIntValue_Null() {
            int? source = null;
            string sourceName = "nullable integer";
            Type sourceType = typeof(int?);
            Tree<PropertyDescription> tree = PropertyTreeBuilder.Create(source, sourceName, sourceType);

            TreeItem<PropertyDescription> rootItem = tree.RootItem;
            Assert.AreEqual(0, rootItem.Children.Count);

            PropertyDescription property = rootItem.Value;
            Assert.AreEqual(sourceName, property.PropertyName);
            Assert.AreEqual(sourceType, property.PropertyType);
            Assert.IsNull(property.Owner);

            PropertyValuePrimitive propertyValue = property.PropertyValue as PropertyValuePrimitive;
            Assert.IsNull(propertyValue.Value);
            Assert.IsTrue(propertyValue.IsNullable);
            Assert.IsFalse(propertyValue.HasChildren);
            Assert.AreEqual(PropertyValueType.Primitive, propertyValue.Type);
        }

        [TestMethod]
        public void NullableIntValue_One() {
            int? source = 1;
            string sourceName = "nullable integer";
            Type sourceType = typeof(int?);
            Tree<PropertyDescription> tree = PropertyTreeBuilder.Create(source, sourceName, sourceType);

            TreeItem<PropertyDescription> rootItem = tree.RootItem;
            Assert.AreEqual(0, rootItem.Children.Count);

            PropertyDescription property = rootItem.Value;
            Assert.AreEqual(sourceName, property.PropertyName);
            Assert.AreEqual(sourceType, property.PropertyType);
            Assert.IsNull(property.Owner);

            PropertyValuePrimitive propertyValue = property.PropertyValue as PropertyValuePrimitive;
            Assert.AreEqual(1, propertyValue.Value);
            Assert.IsTrue(propertyValue.IsNullable);
            Assert.IsFalse(propertyValue.HasChildren);
            Assert.AreEqual(PropertyValueType.Primitive, propertyValue.Type);
        }

        enum TestEnum {
            Value1, 
            Value2,
        }

        [TestMethod]
        public void TestEnumValue_Value1() {
            TestEnum source = TestEnum.Value1;
            string sourceName = "Test enum";
            Type sourceType = typeof(TestEnum);
            Tree<PropertyDescription> tree = PropertyTreeBuilder.Create(source, sourceName, sourceType);

            TreeItem<PropertyDescription> rootItem = tree.RootItem;
            Assert.AreEqual(0, rootItem.Children.Count);

            PropertyDescription property = rootItem.Value;
            Assert.AreEqual(sourceName, property.PropertyName);
            Assert.AreEqual(sourceType, property.PropertyType);
            Assert.IsNull(property.Owner);

            PropertyValueEnum propertyValue = property.PropertyValue as PropertyValueEnum;
            Assert.AreEqual(TestEnum.Value1, propertyValue.Value);
            Assert.IsFalse(propertyValue.IsNullable);
            Assert.IsFalse(propertyValue.HasChildren);
            Assert.AreEqual(sourceType, propertyValue.EnumType);
            Assert.AreEqual(PropertyValueType.Enum, propertyValue.Type);
        }

        [TestMethod]
        public void NullableEnumValue_Null() {
            TestEnum? source = null;
            string sourceName = "Test enum";
            Type sourceType = typeof(TestEnum?);
            Tree<PropertyDescription> tree = PropertyTreeBuilder.Create(source, sourceName, sourceType);

            TreeItem<PropertyDescription> rootItem = tree.RootItem;
            Assert.AreEqual(0, rootItem.Children.Count);

            PropertyDescription property = rootItem.Value;
            Assert.AreEqual(sourceName, property.PropertyName);
            Assert.AreEqual(sourceType, property.PropertyType);
            Assert.IsNull(property.Owner);

            PropertyValueEnum propertyValue = property.PropertyValue as PropertyValueEnum;
            Assert.IsNull(propertyValue.Value);
            Assert.IsTrue(propertyValue.IsNullable);
            Assert.IsFalse(propertyValue.HasChildren);
            Assert.AreEqual(typeof(TestEnum), propertyValue.EnumType);
            Assert.AreEqual(PropertyValueType.Enum, propertyValue.Type);
        }

        [TestMethod]
        public void NullableEnumValue_Value1() {
            TestEnum? source = TestEnum.Value1;
            string sourceName = "Test enum";
            Type sourceType = typeof(TestEnum?);
            Tree<PropertyDescription> tree = PropertyTreeBuilder.Create(source, sourceName, sourceType);

            TreeItem<PropertyDescription> rootItem = tree.RootItem;
            Assert.AreEqual(0, rootItem.Children.Count);

            PropertyDescription property = rootItem.Value;
            Assert.AreEqual(sourceName, property.PropertyName);
            Assert.AreEqual(sourceType, property.PropertyType);
            Assert.IsNull(property.Owner);

            PropertyValueEnum propertyValue = property.PropertyValue as PropertyValueEnum;
            Assert.AreEqual(TestEnum.Value1, propertyValue.Value);
            Assert.IsTrue(propertyValue.IsNullable);
            Assert.IsFalse(propertyValue.HasChildren);
            Assert.AreEqual(typeof(TestEnum), propertyValue.EnumType);
            Assert.AreEqual(PropertyValueType.Enum, propertyValue.Type);
        }

        struct EmptyTestStruct { }

        [TestMethod]
        public void EmptyTestStructValue() {
            EmptyTestStruct source = new EmptyTestStruct();
            string sourceName = "Empty Test Struct";
            Type sourceType = typeof(EmptyTestStruct);
            Tree<PropertyDescription> tree = PropertyTreeBuilder.Create(source, sourceName, sourceType);

            TreeItem<PropertyDescription> rootItem = tree.RootItem;
            Assert.AreEqual(0, rootItem.Children.Count);

            PropertyDescription property = rootItem.Value;
            Assert.AreEqual(sourceName, property.PropertyName);
            Assert.AreEqual(sourceType, property.PropertyType);
            Assert.IsNull(property.Owner);

            PropertyValueStruct propertyValue = property.PropertyValue as PropertyValueStruct;
            Assert.AreEqual(source, propertyValue.Value);
            Assert.IsFalse(propertyValue.IsNullable);
            Assert.IsFalse(propertyValue.HasChildren);
            Assert.AreEqual(PropertyValueType.Struct, propertyValue.Type);
        }

        [TestMethod]
        public void NullableEmptyTestStructValue_Null() {
            EmptyTestStruct? source = null;
            string sourceName = "Empty Test Struct";
            Type sourceType = typeof(EmptyTestStruct?);
            Tree<PropertyDescription> tree = PropertyTreeBuilder.Create(source, sourceName, sourceType);

            TreeItem<PropertyDescription> rootItem = tree.RootItem;
            Assert.AreEqual(0, rootItem.Children.Count);

            PropertyDescription property = rootItem.Value;
            Assert.AreEqual(sourceName, property.PropertyName);
            Assert.AreEqual(sourceType, property.PropertyType);
            Assert.IsNull(property.Owner);

            PropertyValueStruct propertyValue = property.PropertyValue as PropertyValueStruct;
            Assert.AreEqual(null, propertyValue.Value);
            Assert.IsTrue(propertyValue.IsNullable);
            Assert.IsFalse(propertyValue.HasChildren);
            Assert.AreEqual(PropertyValueType.Struct, propertyValue.Type);
        }

        [TestMethod]
        public void NullableEmptyTestStructValue_NotNull() {
            EmptyTestStruct? source = new EmptyTestStruct();
            string sourceName = "Empty Test Struct";
            Type sourceType = typeof(EmptyTestStruct?);
            Tree<PropertyDescription> tree = PropertyTreeBuilder.Create(source, sourceName, sourceType);

            TreeItem<PropertyDescription> rootItem = tree.RootItem;
            Assert.AreEqual(0, rootItem.Children.Count);

            PropertyDescription property = rootItem.Value;
            Assert.AreEqual(sourceName, property.PropertyName);
            Assert.AreEqual(sourceType, property.PropertyType);
            Assert.IsNull(property.Owner);

            PropertyValueStruct propertyValue = property.PropertyValue as PropertyValueStruct;
            Assert.AreEqual(source, propertyValue.Value);
            Assert.IsTrue(propertyValue.IsNullable);
            Assert.IsFalse(propertyValue.HasChildren);
            Assert.AreEqual(PropertyValueType.Struct, propertyValue.Type);
        }

        struct TestStruct {
            object Property4 { get; set; }

            public int Property1 { get; set; }
            public TestEnum Property2 { get; set; }
            public object Property3 { get; set; }
        }

        [TestMethod]
        public void TestStructValue() {
            TestStruct source = new TestStruct();
            string sourceName = "Test Struct";
            Type sourceType = typeof(TestStruct);
            Tree<PropertyDescription> tree = PropertyTreeBuilder.Create(source, sourceName, sourceType);

            TreeItem<PropertyDescription> rootItem = tree.RootItem;
            Assert.AreEqual(3, rootItem.Children.Count);

            PropertyDescription property = rootItem.Value;
            Assert.AreEqual(sourceName, property.PropertyName);
            Assert.AreEqual(sourceType, property.PropertyType);
            Assert.IsNull(property.Owner);

            PropertyValueStruct propertyValue = property.PropertyValue as PropertyValueStruct;
            Assert.AreEqual(source, propertyValue.Value);
            Assert.IsFalse(propertyValue.IsNullable);
            Assert.IsFalse(propertyValue.HasChildren);
            Assert.AreEqual(PropertyValueType.Struct, propertyValue.Type);

            TreeItem<PropertyDescription> childItem1 = rootItem.Children[0];
            Assert.AreEqual(0, childItem1.Children.Count);
            PropertyDescription property1 = childItem1.Value;
            Assert.AreEqual("Property1", property1.PropertyName);
            Assert.AreEqual(typeof(int), property1.PropertyType);
            Assert.AreEqual(source, property1.Owner);

            PropertyValuePrimitive propertyValue1 = property1.PropertyValue as PropertyValuePrimitive;
            Assert.AreEqual(0, propertyValue1.Value);
            Assert.IsFalse(propertyValue1.IsNullable);
            Assert.IsFalse(propertyValue1.HasChildren);
            Assert.AreEqual(PropertyValueType.Primitive, propertyValue1.Type);

            TreeItem<PropertyDescription> childItem2 = rootItem.Children[1];
            Assert.AreEqual(0, childItem2.Children.Count);
            PropertyDescription property2 = childItem2.Value;
            Assert.AreEqual("Property2", property2.PropertyName);
            Assert.AreEqual(typeof(TestEnum), property2.PropertyType);
            Assert.AreEqual(source, property2.Owner);

            PropertyValueEnum propertyValue2 = property2.PropertyValue as PropertyValueEnum;
            Assert.AreEqual(TestEnum.Value1, propertyValue2.Value);
            Assert.IsFalse(propertyValue2.IsNullable);
            Assert.IsFalse(propertyValue2.HasChildren);
            Assert.AreEqual(typeof(TestEnum), propertyValue2.EnumType);
            Assert.AreEqual(PropertyValueType.Enum, propertyValue2.Type);

            TreeItem<PropertyDescription> childItem3 = rootItem.Children[2];
            Assert.AreEqual(0, childItem3.Children.Count);
            PropertyDescription property3 = childItem3.Value;
            Assert.AreEqual("Property3", property3.PropertyName);
            Assert.AreEqual(typeof(object), property3.PropertyType);
            Assert.AreEqual(source, property3.Owner);

            PropertyValueClass propertyValue3 = property3.PropertyValue as PropertyValueClass;
            Assert.AreEqual(null, propertyValue3.Value);
            Assert.IsFalse(propertyValue3.HasChildren);
            Assert.AreEqual(PropertyValueType.Class, propertyValue3.Type);

        }
    }
}
