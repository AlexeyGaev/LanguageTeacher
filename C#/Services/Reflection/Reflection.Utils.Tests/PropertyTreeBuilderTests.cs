using Microsoft.VisualStudio.TestTools.UnitTesting;
using Reflection.Utils.PropertyTree;
using Reflection.Utils.Tree;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Reflection.Utils.Tests {
    [TestClass]
    public class PropertyTreeBuilderTests_Root {
        TreeItem<PropertyDescription> CreateItem(IEnumerable<TreeItem<PropertyDescription>> parents, string propertyName, Type propertyType, object propertyOwner, object propertyValue, bool hasException) {
            return PropertyTreeBuilder.CreateItem(parents, propertyName, propertyType, propertyOwner, propertyValue, hasException);
        }

        [TestMethod]
        public void PropertyNameIsNull() {
            TreeItem<PropertyDescription> treeItem = CreateItem(null, null, null, null, null, false);

            Assert.IsNull(treeItem.Children);
            Assert.IsNull(treeItem.Parents);
            Assert.IsNull(treeItem.Value);
        }

        [TestMethod]
        public void PropertyTypeIsNull() {
            string propertyName = "Test";
            TreeItem<PropertyDescription> treeItem = CreateItem(null, propertyName, null, null, null, false);

            Assert.IsNull(treeItem.Children);
            Assert.IsNull(treeItem.Parents);
            Assert.IsNotNull(treeItem.Value);

            PropertyDescription description = treeItem.Value;
            Assert.AreEqual(propertyName, description.PropertyName);
            Assert.IsNull(description.PropertyOwner);
            Assert.IsNull(description.PropertyType);
            Assert.IsNull(description.PropertyValue);
            Assert.AreEqual(PropertyValueType.Undefined, description.PropertyValueType);
        }

        [TestMethod]
        public void ObjectValue() {
            IEnumerable<TreeItem<PropertyDescription>> parents = null;
            string propertyName = "Object";
            Type propertyType = typeof(object);
            object propertyOwner = null;
            object propertyValue = new object();
            bool hasException = false;
                                  
            TreeItem<PropertyDescription> treeItem = CreateItem(parents, propertyName, propertyType, propertyOwner, propertyValue, hasException);
            
            Assert.AreEqual(parents, treeItem.Parents);
            Assert.IsNull(treeItem.Children);
            Assert.IsNotNull(treeItem.Value);

            PropertyDescription property = treeItem.Value;
            Assert.AreEqual(propertyName, property.PropertyName);
            Assert.AreEqual(propertyType, property.PropertyType);
            Assert.AreEqual(propertyOwner, property.PropertyOwner);
            Assert.AreEqual(propertyValue, property.PropertyValue);
            Assert.AreEqual(PropertyValueType.Class, property.PropertyValueType);
        }

        //[TestMethod]
        //public void IntValue() {
        //    int source = 1;
        //    string sourceName = "integer";
        //    Type sourceType = typeof(int);
        //    Tree<PropertyDescription> tree = PropertyTreeBuilder.Create(source, sourceName, sourceType);

        //    TreeItem<PropertyDescription> rootItem = tree.RootItem;
        //    Assert.AreEqual(0, rootItem.Children.Count);

        //    PropertyDescription property = rootItem.Value;
        //    Assert.AreEqual(sourceName, property.PropertyName);
        //    Assert.AreEqual(sourceType, property.PropertyType);
        //    Assert.IsNull(property.Owner);

        //    PropertyValuePrimitive propertyValue = property.PropertyValue as PropertyValuePrimitive;
        //    Assert.AreEqual(1, propertyValue.Value);
        //    Assert.IsFalse(propertyValue.IsNullable);
        //    Assert.IsFalse(propertyValue.HasChildren);
        //    Assert.AreEqual(PropertyValueType.Primitive, propertyValue.Type);
        //}

        //[TestMethod]
        //public void NullableIntValue_Null() {
        //    int? source = null;
        //    string sourceName = "nullable integer";
        //    Type sourceType = typeof(int?);
        //    Tree<PropertyDescription> tree = PropertyTreeBuilder.Create(source, sourceName, sourceType);

        //    TreeItem<PropertyDescription> rootItem = tree.RootItem;
        //    Assert.AreEqual(0, rootItem.Children.Count);

        //    PropertyDescription property = rootItem.Value;
        //    Assert.AreEqual(sourceName, property.PropertyName);
        //    Assert.AreEqual(sourceType, property.PropertyType);
        //    Assert.IsNull(property.Owner);

        //    PropertyValuePrimitive propertyValue = property.PropertyValue as PropertyValuePrimitive;
        //    Assert.IsNull(propertyValue.Value);
        //    Assert.IsTrue(propertyValue.IsNullable);
        //    Assert.IsFalse(propertyValue.HasChildren);
        //    Assert.AreEqual(PropertyValueType.Primitive, propertyValue.Type);
        //}

        //[TestMethod]
        //public void NullableIntValue_One() {
        //    int? source = 1;
        //    string sourceName = "nullable integer";
        //    Type sourceType = typeof(int?);
        //    Tree<PropertyDescription> tree = PropertyTreeBuilder.Create(source, sourceName, sourceType);

        //    TreeItem<PropertyDescription> rootItem = tree.RootItem;
        //    Assert.AreEqual(0, rootItem.Children.Count);

        //    PropertyDescription property = rootItem.Value;
        //    Assert.AreEqual(sourceName, property.PropertyName);
        //    Assert.AreEqual(sourceType, property.PropertyType);
        //    Assert.IsNull(property.Owner);

        //    PropertyValuePrimitive propertyValue = property.PropertyValue as PropertyValuePrimitive;
        //    Assert.AreEqual(1, propertyValue.Value);
        //    Assert.IsTrue(propertyValue.IsNullable);
        //    Assert.IsFalse(propertyValue.HasChildren);
        //    Assert.AreEqual(PropertyValueType.Primitive, propertyValue.Type);
        //}

        //enum TestEnum {
        //    Value1, 
        //    Value2,
        //}

        //[TestMethod]
        //public void TestEnumValue_Value1() {
        //    TestEnum source = TestEnum.Value1;
        //    string sourceName = "Test enum";
        //    Type sourceType = typeof(TestEnum);
        //    Tree<PropertyDescription> tree = PropertyTreeBuilder.Create(source, sourceName, sourceType);

        //    TreeItem<PropertyDescription> rootItem = tree.RootItem;
        //    Assert.AreEqual(0, rootItem.Children.Count);

        //    PropertyDescription property = rootItem.Value;
        //    Assert.AreEqual(sourceName, property.PropertyName);
        //    Assert.AreEqual(sourceType, property.PropertyType);
        //    Assert.IsNull(property.Owner);

        //    PropertyValueEnum propertyValue = property.PropertyValue as PropertyValueEnum;
        //    Assert.AreEqual(TestEnum.Value1, propertyValue.Value);
        //    Assert.IsFalse(propertyValue.IsNullable);
        //    Assert.IsFalse(propertyValue.HasChildren);
        //    Assert.AreEqual(sourceType, propertyValue.EnumType);
        //    Assert.AreEqual(PropertyValueType.Enum, propertyValue.Type);
        //}

        //[TestMethod]
        //public void NullableEnumValue_Null() {
        //    TestEnum? source = null;
        //    string sourceName = "Test enum";
        //    Type sourceType = typeof(TestEnum?);
        //    Tree<PropertyDescription> tree = PropertyTreeBuilder.Create(source, sourceName, sourceType);

        //    TreeItem<PropertyDescription> rootItem = tree.RootItem;
        //    Assert.AreEqual(0, rootItem.Children.Count);

        //    PropertyDescription property = rootItem.Value;
        //    Assert.AreEqual(sourceName, property.PropertyName);
        //    Assert.AreEqual(sourceType, property.PropertyType);
        //    Assert.IsNull(property.Owner);

        //    PropertyValueEnum propertyValue = property.PropertyValue as PropertyValueEnum;
        //    Assert.IsNull(propertyValue.Value);
        //    Assert.IsTrue(propertyValue.IsNullable);
        //    Assert.IsFalse(propertyValue.HasChildren);
        //    Assert.AreEqual(typeof(TestEnum), propertyValue.EnumType);
        //    Assert.AreEqual(PropertyValueType.Enum, propertyValue.Type);
        //}

        //[TestMethod]
        //public void NullableEnumValue_Value1() {
        //    TestEnum? source = TestEnum.Value1;
        //    string sourceName = "Test enum";
        //    Type sourceType = typeof(TestEnum?);
        //    Tree<PropertyDescription> tree = PropertyTreeBuilder.Create(source, sourceName, sourceType);

        //    TreeItem<PropertyDescription> rootItem = tree.RootItem;
        //    Assert.AreEqual(0, rootItem.Children.Count);

        //    PropertyDescription property = rootItem.Value;
        //    Assert.AreEqual(sourceName, property.PropertyName);
        //    Assert.AreEqual(sourceType, property.PropertyType);
        //    Assert.IsNull(property.Owner);

        //    PropertyValueEnum propertyValue = property.PropertyValue as PropertyValueEnum;
        //    Assert.AreEqual(TestEnum.Value1, propertyValue.Value);
        //    Assert.IsTrue(propertyValue.IsNullable);
        //    Assert.IsFalse(propertyValue.HasChildren);
        //    Assert.AreEqual(typeof(TestEnum), propertyValue.EnumType);
        //    Assert.AreEqual(PropertyValueType.Enum, propertyValue.Type);
        //}

        //struct EmptyTestStruct { }

        //[TestMethod]
        //public void EmptyTestStructValue() {
        //    EmptyTestStruct source = new EmptyTestStruct();
        //    string sourceName = "Empty Test Struct";
        //    Type sourceType = typeof(EmptyTestStruct);
        //    Tree<PropertyDescription> tree = PropertyTreeBuilder.Create(source, sourceName, sourceType);

        //    TreeItem<PropertyDescription> rootItem = tree.RootItem;
        //    Assert.AreEqual(0, rootItem.Children.Count);

        //    PropertyDescription property = rootItem.Value;
        //    Assert.AreEqual(sourceName, property.PropertyName);
        //    Assert.AreEqual(sourceType, property.PropertyType);
        //    Assert.IsNull(property.Owner);

        //    PropertyValueStruct propertyValue = property.PropertyValue as PropertyValueStruct;
        //    Assert.AreEqual(source, propertyValue.Value);
        //    Assert.IsFalse(propertyValue.IsNullable);
        //    Assert.IsFalse(propertyValue.HasChildren);
        //    Assert.AreEqual(PropertyValueType.Struct, propertyValue.Type);
        //}

        //[TestMethod]
        //public void NullableEmptyTestStructValue_Null() {
        //    EmptyTestStruct? source = null;
        //    string sourceName = "Empty Test Struct";
        //    Type sourceType = typeof(EmptyTestStruct?);
        //    Tree<PropertyDescription> tree = PropertyTreeBuilder.Create(source, sourceName, sourceType);

        //    TreeItem<PropertyDescription> rootItem = tree.RootItem;
        //    Assert.AreEqual(0, rootItem.Children.Count);

        //    PropertyDescription property = rootItem.Value;
        //    Assert.AreEqual(sourceName, property.PropertyName);
        //    Assert.AreEqual(sourceType, property.PropertyType);
        //    Assert.IsNull(property.Owner);

        //    PropertyValueStruct propertyValue = property.PropertyValue as PropertyValueStruct;
        //    Assert.AreEqual(null, propertyValue.Value);
        //    Assert.IsTrue(propertyValue.IsNullable);
        //    Assert.IsFalse(propertyValue.HasChildren);
        //    Assert.AreEqual(PropertyValueType.Struct, propertyValue.Type);
        //}

        //[TestMethod]
        //public void NullableEmptyTestStructValue_NotNull() {
        //    EmptyTestStruct? source = new EmptyTestStruct();
        //    string sourceName = "Empty Test Struct";
        //    Type sourceType = typeof(EmptyTestStruct?);
        //    Tree<PropertyDescription> tree = PropertyTreeBuilder.Create(source, sourceName, sourceType);

        //    TreeItem<PropertyDescription> rootItem = tree.RootItem;
        //    Assert.AreEqual(0, rootItem.Children.Count);

        //    PropertyDescription property = rootItem.Value;
        //    Assert.AreEqual(sourceName, property.PropertyName);
        //    Assert.AreEqual(sourceType, property.PropertyType);
        //    Assert.IsNull(property.Owner);

        //    PropertyValueStruct propertyValue = property.PropertyValue as PropertyValueStruct;
        //    Assert.AreEqual(source, propertyValue.Value);
        //    Assert.IsTrue(propertyValue.IsNullable);
        //    Assert.IsFalse(propertyValue.HasChildren);
        //    Assert.AreEqual(PropertyValueType.Struct, propertyValue.Type);
        //}

        //struct TestStruct {
        //    object Property4 { get; set; }

        //    public int Property1 { get; set; }
        //    public TestEnum Property2 { get; set; }
        //    public object Property3 { get; set; }
        //}

        //[TestMethod]
        //public void TestStructValue() {
        //    TestStruct source = new TestStruct();
        //    string sourceName = "Test Struct";
        //    Type sourceType = typeof(TestStruct);
        //    Tree<PropertyDescription> tree = PropertyTreeBuilder.Create(source, sourceName, sourceType);

        //    TreeItem<PropertyDescription> rootItem = tree.RootItem;
        //    Assert.AreEqual(3, rootItem.Children.Count);

        //    PropertyDescription property = rootItem.Value;
        //    Assert.AreEqual(sourceName, property.PropertyName);
        //    Assert.AreEqual(sourceType, property.PropertyType);
        //    Assert.IsNull(property.Owner);

        //    PropertyValueStruct propertyValue = property.PropertyValue as PropertyValueStruct;
        //    Assert.AreEqual(source, propertyValue.Value);
        //    Assert.IsFalse(propertyValue.IsNullable);
        //    Assert.IsFalse(propertyValue.HasChildren);
        //    Assert.AreEqual(PropertyValueType.Struct, propertyValue.Type);

        //    TreeItem<PropertyDescription> childItem1 = rootItem.Children[0];
        //    Assert.AreEqual(0, childItem1.Children.Count);
        //    PropertyDescription property1 = childItem1.Value;
        //    Assert.AreEqual("Property1", property1.PropertyName);
        //    Assert.AreEqual(typeof(int), property1.PropertyType);
        //    Assert.AreEqual(source, property1.Owner);

        //    PropertyValuePrimitive propertyValue1 = property1.PropertyValue as PropertyValuePrimitive;
        //    Assert.AreEqual(0, propertyValue1.Value);
        //    Assert.IsFalse(propertyValue1.IsNullable);
        //    Assert.IsFalse(propertyValue1.HasChildren);
        //    Assert.AreEqual(PropertyValueType.Primitive, propertyValue1.Type);

        //    TreeItem<PropertyDescription> childItem2 = rootItem.Children[1];
        //    Assert.AreEqual(0, childItem2.Children.Count);
        //    PropertyDescription property2 = childItem2.Value;
        //    Assert.AreEqual("Property2", property2.PropertyName);
        //    Assert.AreEqual(typeof(TestEnum), property2.PropertyType);
        //    Assert.AreEqual(source, property2.Owner);

        //    PropertyValueEnum propertyValue2 = property2.PropertyValue as PropertyValueEnum;
        //    Assert.AreEqual(TestEnum.Value1, propertyValue2.Value);
        //    Assert.IsFalse(propertyValue2.IsNullable);
        //    Assert.IsFalse(propertyValue2.HasChildren);
        //    Assert.AreEqual(typeof(TestEnum), propertyValue2.EnumType);
        //    Assert.AreEqual(PropertyValueType.Enum, propertyValue2.Type);

        //    TreeItem<PropertyDescription> childItem3 = rootItem.Children[2];
        //    Assert.AreEqual(0, childItem3.Children.Count);
        //    PropertyDescription property3 = childItem3.Value;
        //    Assert.AreEqual("Property3", property3.PropertyName);
        //    Assert.AreEqual(typeof(object), property3.PropertyType);
        //    Assert.AreEqual(source, property3.Owner);

        //    PropertyValueClass propertyValue3 = property3.PropertyValue as PropertyValueClass;
        //    Assert.AreEqual(null, propertyValue3.Value);
        //    Assert.IsFalse(propertyValue3.HasChildren);
        //    Assert.AreEqual(PropertyValueType.Class, propertyValue3.Type);

        //}
    }
}
