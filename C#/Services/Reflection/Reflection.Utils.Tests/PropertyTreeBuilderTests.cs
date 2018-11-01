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
        public void EnumValue_Value1() {
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


    }
}
