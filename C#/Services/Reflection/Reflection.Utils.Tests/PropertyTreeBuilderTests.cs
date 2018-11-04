using Microsoft.VisualStudio.TestTools.UnitTesting;
using Reflection.Utils.PropertyTree;
using Reflection.Utils.Tree;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Reflection.Utils.Tests {
    [TestClass]
    public class PropertyTreeBuilderTests {
        TreeItem<PropertyDescription> CreateItem(IEnumerable<TreeItem<PropertyDescription>> parents, string propertyName, Type propertyType, object propertyValue) {
            PropertyDescription propertyDescription = new PropertyDescription(propertyName, propertyType, propertyValue);
            return PropertyTreeBuilder.CreateItem(parents, propertyDescription);
        }

        [TestMethod]
        public void PropertyNameIsNull() {
            TreeItem<PropertyDescription> treeItem = CreateItem(null, null, null, null);
                        
            Assert.IsNull(treeItem.Parents);
            Assert.IsNull(treeItem.Children);
            Assert.IsFalse(treeItem.HasChildrenCycle);
            Assert.IsNotNull(treeItem.Value);

            PropertyDescription property = treeItem.Value;
            Assert.IsNull(property.PropertyName);
            Assert.IsNull(property.PropertyType);
            Assert.IsNull(property.PropertyValue);
            Assert.AreEqual(false, property.IsException);
            Assert.AreEqual(false, property.IsNullable);
        }

        [TestMethod]
        public void PropertyTypeIsNull() {
            string propertyName = "Test";
            TreeItem<PropertyDescription> treeItem = CreateItem(null, propertyName, null, null);
                        
            Assert.IsNull(treeItem.Parents);
            Assert.IsNull(treeItem.Children);
            Assert.IsFalse(treeItem.HasChildrenCycle);
            Assert.IsNotNull(treeItem.Value);

            PropertyDescription property = treeItem.Value;
            Assert.AreEqual(propertyName, property.PropertyName);
            Assert.IsNull(property.PropertyType);
            Assert.IsNull(property.PropertyValue);
            Assert.AreEqual(false, property.IsException);
            Assert.AreEqual(false, property.IsNullable);
        }

        [TestMethod]
        public void ObjectValue() {
            IEnumerable<TreeItem<PropertyDescription>> parents = null;
            string propertyName = "Object";
            Type propertyType = typeof(object);
            object propertyValue = new object();

            TreeItem<PropertyDescription> treeItem = CreateItem(parents, propertyName, propertyType, propertyValue);

            Assert.AreEqual(parents, treeItem.Parents);
            Assert.AreEqual(0, treeItem.Children.Count());
            Assert.IsFalse(treeItem.HasChildrenCycle);
            Assert.IsNotNull(treeItem.Value);

            PropertyDescription property = treeItem.Value;
            Assert.AreEqual(propertyName, property.PropertyName);
            Assert.AreEqual(propertyType, property.PropertyType);
            Assert.AreEqual(propertyValue, property.PropertyValue);
            Assert.AreEqual(false, property.IsException);
            Assert.AreEqual(false, property.IsNullable);
        }

        [TestMethod]
        public void DecimalValue() {
            IEnumerable<TreeItem<PropertyDescription>> parents = null;
            string propertyName = "Decimal";
            Type propertyType = typeof(decimal);
            decimal propertyValue = 1;
            
            TreeItem<PropertyDescription> treeItem = CreateItem(parents, propertyName, propertyType, propertyValue);
            
            Assert.AreEqual(parents, treeItem.Parents);
            Assert.AreEqual(0, treeItem.Children.Count());
            Assert.IsFalse(treeItem.HasChildrenCycle);
            Assert.IsNotNull(treeItem.Value);
            
            PropertyDescription property = treeItem.Value;
            Assert.AreEqual(propertyName, property.PropertyName);
            Assert.AreEqual(propertyType, property.PropertyType);
            Assert.AreEqual(propertyValue, property.PropertyValue);
            Assert.AreEqual(false, property.IsException);
            Assert.AreEqual(false, property.IsNullable);
        }

        [TestMethod]
        public void NullableDecimalValue_Null() {
            IEnumerable<TreeItem<PropertyDescription>> parents = null;
            string propertyName = "NullableDecimal";
            Type propertyType = typeof(decimal?);
            decimal? propertyValue = null;

            TreeItem<PropertyDescription> treeItem = CreateItem(parents, propertyName, propertyType, propertyValue);

            Assert.AreEqual(parents, treeItem.Parents);
            Assert.IsNull(treeItem.Children);
            Assert.IsFalse(treeItem.HasChildrenCycle);
            Assert.IsNotNull(treeItem.Value);

            PropertyDescription property = treeItem.Value;
            Assert.AreEqual(propertyName, property.PropertyName);
            Assert.AreEqual(propertyType, property.PropertyType);
            Assert.AreEqual(propertyValue, property.PropertyValue);
            Assert.AreEqual(false, property.IsException);
            Assert.AreEqual(true, property.IsNullable);
        }

        [TestMethod]
        public void IntValue() {
            IEnumerable<TreeItem<PropertyDescription>> parents = null;
            string propertyName = "Int";
            Type propertyType = typeof(int);
            int propertyValue = 1;
            
            TreeItem<PropertyDescription> treeItem = CreateItem(parents, propertyName, propertyType, propertyValue);

            Assert.AreEqual(parents, treeItem.Parents);
            Assert.AreEqual(0, treeItem.Children.Count());
            Assert.IsFalse(treeItem.HasChildrenCycle);
            Assert.IsNotNull(treeItem.Value);

            PropertyDescription property = treeItem.Value;
            Assert.AreEqual(propertyName, property.PropertyName);
            Assert.AreEqual(propertyType, property.PropertyType);
            Assert.AreEqual(propertyValue, property.PropertyValue);
            Assert.AreEqual(false, property.IsException);
            Assert.AreEqual(false, property.IsNullable);
        }

        [TestMethod]
        public void NullableIntValue_Null() {
            IEnumerable<TreeItem<PropertyDescription>> parents = null;
            string propertyName = "Nullable Int";
            Type propertyType = typeof(int?);
            int? propertyValue = null;

            TreeItem<PropertyDescription> treeItem = CreateItem(parents, propertyName, propertyType, propertyValue);

            Assert.AreEqual(parents, treeItem.Parents);
            Assert.IsNull(treeItem.Children);
            Assert.IsFalse(treeItem.HasChildrenCycle);
            Assert.IsNotNull(treeItem.Value);

            PropertyDescription property = treeItem.Value;
            Assert.AreEqual(propertyName, property.PropertyName);
            Assert.AreEqual(propertyType, property.PropertyType);
            Assert.AreEqual(propertyValue, property.PropertyValue);
            Assert.AreEqual(false, property.IsException);
            Assert.AreEqual(true, property.IsNullable);
        }

        [TestMethod]
        public void NullableIntValue_One() {
            IEnumerable<TreeItem<PropertyDescription>> parents = null;
            string propertyName = "Nullable Int";
            Type propertyType = typeof(int?);
            int? propertyValue = 1;

            TreeItem<PropertyDescription> treeItem = CreateItem(parents, propertyName, propertyType, propertyValue);

            Assert.AreEqual(parents, treeItem.Parents);
            Assert.AreEqual(0, treeItem.Children.Count());
            Assert.IsFalse(treeItem.HasChildrenCycle);
            Assert.IsNotNull(treeItem.Value);

            PropertyDescription property = treeItem.Value;
            Assert.AreEqual(propertyName, property.PropertyName);
            Assert.AreEqual(propertyType, property.PropertyType);
            Assert.AreEqual(propertyValue, property.PropertyValue);
            Assert.AreEqual(false, property.IsException);
            Assert.AreEqual(true, property.IsNullable);
        }

        enum TestEnum {
            Value1,
            Value2,
        }

        [TestMethod]
        public void TestEnumValue_Value1() {
            IEnumerable<TreeItem<PropertyDescription>> parents = null;
            string propertyName = "TestEnum";
            Type propertyType = typeof(TestEnum);
            TestEnum propertyValue = TestEnum.Value1;

            TreeItem<PropertyDescription> treeItem = CreateItem(parents, propertyName, propertyType, propertyValue);

            Assert.AreEqual(parents, treeItem.Parents);
            Assert.IsFalse(treeItem.HasChildrenCycle);
            Assert.IsNotNull(treeItem.Value);

            PropertyDescription property = treeItem.Value;
            Assert.AreEqual(propertyName, property.PropertyName);
            Assert.AreEqual(propertyType, property.PropertyType);
            Assert.AreEqual(propertyValue, property.PropertyValue);
            Assert.AreEqual(false, property.IsException);
            Assert.AreEqual(false, property.IsNullable);
        }

        [TestMethod]
        public void NullableEnumValue_Null() {
            IEnumerable<TreeItem<PropertyDescription>> parents = null;
            string propertyName = "Nullable TestEnum";
            Type propertyType = typeof(TestEnum?);
            TestEnum? propertyValue = null;

            TreeItem<PropertyDescription> treeItem = CreateItem(parents, propertyName, propertyType, propertyValue);

            Assert.AreEqual(parents, treeItem.Parents);
            Assert.IsNull(treeItem.Children);
            Assert.IsFalse(treeItem.HasChildrenCycle);
            Assert.IsNotNull(treeItem.Value);

            PropertyDescription property = treeItem.Value;
            Assert.AreEqual(propertyName, property.PropertyName);
            Assert.AreEqual(propertyType, property.PropertyType);
            Assert.AreEqual(propertyValue, property.PropertyValue);
            Assert.AreEqual(false, property.IsException);
            Assert.AreEqual(true, property.IsNullable);
        }

        [TestMethod]
        public void NullableEnumValue_Value1() {
            IEnumerable<TreeItem<PropertyDescription>> parents = null;
            string propertyName = "Nullable TestEnum";
            Type propertyType = typeof(TestEnum?);
            TestEnum? propertyValue = TestEnum.Value1;

            TreeItem<PropertyDescription> treeItem = CreateItem(parents, propertyName, propertyType, propertyValue);

            Assert.AreEqual(parents, treeItem.Parents);
            Assert.AreEqual(0, treeItem.Children.Count());
            Assert.IsFalse(treeItem.HasChildrenCycle);
            Assert.IsNotNull(treeItem.Value);

            PropertyDescription property = treeItem.Value;
            Assert.AreEqual(propertyName, property.PropertyName);
            Assert.AreEqual(propertyType, property.PropertyType);
            Assert.AreEqual(propertyValue, property.PropertyValue);
            Assert.AreEqual(false, property.IsException);
            Assert.AreEqual(true, property.IsNullable);
        }

        struct EmptyTestStruct { }

        [TestMethod]
        public void EmptyTestStructValue() {
            IEnumerable<TreeItem<PropertyDescription>> parents = null;
            string propertyName = "EmptyTestStruct";
            Type propertyType = typeof(EmptyTestStruct);
            EmptyTestStruct propertyValue = new EmptyTestStruct();

            TreeItem<PropertyDescription> treeItem = CreateItem(parents, propertyName, propertyType, propertyValue);

            Assert.AreEqual(parents, treeItem.Parents);
            Assert.AreEqual(0, treeItem.Children.Count());
            Assert.IsFalse(treeItem.HasChildrenCycle);
            Assert.IsNotNull(treeItem.Value);

            PropertyDescription property = treeItem.Value;
            Assert.AreEqual(propertyName, property.PropertyName);
            Assert.AreEqual(propertyType, property.PropertyType);
            Assert.AreEqual(propertyValue, property.PropertyValue);
            Assert.AreEqual(false, property.IsException);
            Assert.AreEqual(false, property.IsNullable);
        }

        [TestMethod]
        public void NullableEmptyTestStructValue_Null() {
            IEnumerable<TreeItem<PropertyDescription>> parents = null;
            string propertyName = "Nullable EmptyTestStruct";
            Type propertyType = typeof(EmptyTestStruct?);
            EmptyTestStruct? propertyValue = null;

            TreeItem<PropertyDescription> treeItem = CreateItem(parents, propertyName, propertyType, propertyValue);

            Assert.AreEqual(parents, treeItem.Parents);
            Assert.IsNull(treeItem.Children);
            Assert.IsFalse(treeItem.HasChildrenCycle);
            Assert.IsNotNull(treeItem.Value);

            PropertyDescription property = treeItem.Value;
            Assert.AreEqual(propertyName, property.PropertyName);
            Assert.AreEqual(propertyType, property.PropertyType);
            Assert.AreEqual(propertyValue, property.PropertyValue);
            Assert.AreEqual(false, property.IsException);
            Assert.AreEqual(true, property.IsNullable);
        }

        [TestMethod]
        public void NullableEmptyTestStructValue_NotNull() {
            IEnumerable<TreeItem<PropertyDescription>> parents = null;
            string propertyName = "Nullable EmptyTestStruct";
            Type propertyType = typeof(EmptyTestStruct?);
            EmptyTestStruct? propertyValue = new EmptyTestStruct();

            TreeItem<PropertyDescription> treeItem = CreateItem(parents, propertyName, propertyType, propertyValue);

            Assert.AreEqual(parents, treeItem.Parents);
            Assert.AreEqual(0, treeItem.Children.Count());
            Assert.IsFalse(treeItem.HasChildrenCycle);
            Assert.IsNotNull(treeItem.Value);

            PropertyDescription property = treeItem.Value;
            Assert.AreEqual(propertyName, property.PropertyName);
            Assert.AreEqual(propertyType, property.PropertyType);
            Assert.AreEqual(propertyValue, property.PropertyValue);
            Assert.AreEqual(false, property.IsException);
            Assert.AreEqual(true, property.IsNullable);
        }

        struct TestStruct {
            object Property0 { get; set; }

            public int Property1 { get; set; }
            public TestEnum Property2 { get; set; }
            public object Property3 { get; set; }
        }

        [TestMethod]
        public void TestStructValue() {
            IEnumerable<TreeItem<PropertyDescription>> parents = null;
            string propertyName = "TestStruct";

            Type propertyType = typeof(TestStruct);
            TestStruct propertyValue = new TestStruct();

            TreeItem<PropertyDescription> treeItem = CreateItem(parents, propertyName, propertyType, propertyValue);
            Assert.AreEqual(parents, treeItem.Parents);
            CheckTestStruct(treeItem, propertyName, propertyType, propertyValue, new List<TreeItem<PropertyDescription>>());
        }

        void CheckTestStruct(TreeItem<PropertyDescription> treeItem, string propertyName, Type propertyType, object propertyValue, IEnumerable<TreeItem<PropertyDescription>> childParrents) {
            Assert.IsFalse(treeItem.HasChildrenCycle);
            Assert.IsNotNull(treeItem.Value);

            PropertyDescription property = treeItem.Value;
            Assert.AreEqual(propertyName, property.PropertyName);
            Assert.AreEqual(propertyType, property.PropertyType);
            Assert.AreEqual(propertyValue, property.PropertyValue);
            Assert.AreEqual(false, property.IsException);
            Assert.AreEqual(false, property.IsNullable);

            Assert.AreEqual(3, treeItem.Children.Count());

            TreeItem<PropertyDescription> childItem1 = treeItem.Children.ElementAt(0);
            Assert.AreEqual(childParrents.Count() + 1, childItem1.Parents.Count());
            for (int i = 0; i < childParrents.Count(); i++)
                Assert.AreEqual(childParrents.ElementAt(i), childItem1.Parents.ElementAt(i));
            Assert.AreEqual(treeItem, childItem1.Parents.ElementAt(childParrents.Count()));
            Assert.IsFalse(childItem1.HasChildrenCycle);
            Assert.IsNotNull(childItem1.Value);

            PropertyDescription property1 = childItem1.Value;
            Assert.AreEqual("Property1", property1.PropertyName);
            Assert.AreEqual(typeof(int), property1.PropertyType);
            Assert.AreEqual(0, property1.PropertyValue);
            Assert.AreEqual(false, property1.IsException);
            Assert.AreEqual(false, property1.IsNullable);

            TreeItem<PropertyDescription> childItem2 = treeItem.Children.ElementAt(1);
            Assert.AreEqual(childParrents.Count() + 1, childItem2.Parents.Count());
            for (int i = 0; i < childParrents.Count(); i++)
                Assert.AreEqual(childParrents.ElementAt(i), childItem2.Parents.ElementAt(i));
            Assert.AreEqual(treeItem, childItem2.Parents.ElementAt(childParrents.Count()));
            Assert.AreEqual(0, childItem2.Children.Count());
            Assert.IsFalse(childItem2.HasChildrenCycle);
            Assert.IsNotNull(childItem2.Value);

            PropertyDescription property2 = childItem2.Value;
            Assert.AreEqual("Property2", property2.PropertyName);
            Assert.AreEqual(typeof(TestEnum), property2.PropertyType);
            Assert.AreEqual(TestEnum.Value1, property2.PropertyValue);
            Assert.AreEqual(false, property2.IsException);
            Assert.AreEqual(false, property2.IsNullable);
            
            TreeItem<PropertyDescription> childItem3 = treeItem.Children.ElementAt(2);
            Assert.AreEqual(childParrents.Count() + 1, childItem3.Parents.Count());
            for (int i = 0; i < childParrents.Count(); i++)
                Assert.AreEqual(childParrents.ElementAt(i), childItem3.Parents.ElementAt(i));
            Assert.AreEqual(treeItem, childItem3.Parents.ElementAt(childParrents.Count()));
            Assert.IsNull(childItem3.Children);
            Assert.IsFalse(childItem3.HasChildrenCycle);
            Assert.IsNotNull(childItem3.Value);

            PropertyDescription property3 = childItem3.Value;
            Assert.AreEqual("Property3", property3.PropertyName);
            Assert.AreEqual(typeof(object), property3.PropertyType);
            Assert.AreEqual(null, property3.PropertyValue);
            Assert.AreEqual(false, property3.IsException);
            Assert.AreEqual(false, property3.IsNullable);
        }

        class TestClass {
            object Property0 { get; set; }

            public int Property1 { get; set; }
            public TestEnum Property2 { get; set; }
            public object Property3 { get; set; }
            public TestStruct Property4 { get; set; }
        }

        [TestMethod]
        public void TestClassIsNull() {
            IEnumerable<TreeItem<PropertyDescription>> parents = null;
            string propertyName = "TestClass";
            Type propertyType = typeof(TestClass);
            TestClass propertyValue = null;

            TreeItem<PropertyDescription> treeItem = CreateItem(parents, propertyName, propertyType, propertyValue);

            Assert.AreEqual(parents, treeItem.Parents);
            Assert.IsNull(treeItem.Children);
            Assert.IsFalse(treeItem.HasChildrenCycle);
            Assert.IsNotNull(treeItem.Value);

            PropertyDescription property = treeItem.Value;
            Assert.AreEqual(propertyName, property.PropertyName);
            Assert.AreEqual(propertyType, property.PropertyType);
            Assert.AreEqual(propertyValue, property.PropertyValue);
            Assert.AreEqual(false, property.IsException);
            Assert.AreEqual(false, property.IsNullable);
        }

        [TestMethod]
        public void TestClass_NotNull() {
            IEnumerable<TreeItem<PropertyDescription>> parents = null;
            string propertyName = "TestClass";
            Type propertyType = typeof(TestClass);
            TestClass propertyValue = new TestClass();

            TreeItem<PropertyDescription> treeItem = CreateItem(parents, propertyName, propertyType, propertyValue);

            Assert.AreEqual(parents, treeItem.Parents);
            Assert.IsNotNull(treeItem.Children);
            Assert.IsFalse(treeItem.HasChildrenCycle);
            Assert.IsNotNull(treeItem.Value);

            PropertyDescription property = treeItem.Value;
            Assert.AreEqual(propertyName, property.PropertyName);
            Assert.AreEqual(propertyType, property.PropertyType);
            Assert.AreEqual(propertyValue, property.PropertyValue);
            Assert.AreEqual(false, property.IsException);
            Assert.AreEqual(false, property.IsNullable);

            Assert.AreEqual(4, treeItem.Children.Count());

            TreeItem<PropertyDescription> childItem1 = treeItem.Children.ElementAt(0);
            Assert.AreEqual(1, childItem1.Parents.Count());
            Assert.AreEqual(treeItem, childItem1.Parents.ElementAt(0));
            Assert.AreEqual(0, childItem1.Children.Count());
            Assert.IsFalse(childItem1.HasChildrenCycle);
            Assert.IsNotNull(childItem1.Value);

            PropertyDescription property1 = childItem1.Value;
            Assert.AreEqual("Property1", property1.PropertyName);
            Assert.AreEqual(typeof(int), property1.PropertyType);
            Assert.AreEqual(0, property1.PropertyValue);
            Assert.AreEqual(false, property1.IsException);
            Assert.AreEqual(false, property1.IsNullable);
            
            TreeItem<PropertyDescription> childItem2 = treeItem.Children.ElementAt(1);
            Assert.AreEqual(1, childItem2.Parents.Count());
            Assert.AreEqual(treeItem, childItem2.Parents.ElementAt(0));
            Assert.AreEqual(0, childItem2.Children.Count());
            Assert.IsFalse(childItem2.HasChildrenCycle);
            Assert.IsNotNull(childItem2.Value);

            PropertyDescription property2 = childItem2.Value;
            Assert.AreEqual("Property2", property2.PropertyName);
            Assert.AreEqual(typeof(TestEnum), property2.PropertyType);
            Assert.AreEqual(TestEnum.Value1, property2.PropertyValue);
            Assert.AreEqual(false, property2.IsException);
            Assert.AreEqual(false, property2.IsNullable);
            
            TreeItem<PropertyDescription> childItem3 = treeItem.Children.ElementAt(2);
            Assert.AreEqual(1, childItem3.Parents.Count());
            Assert.AreEqual(treeItem, childItem3.Parents.ElementAt(0));
            Assert.IsNull(childItem3.Children);
            Assert.IsFalse(childItem3.HasChildrenCycle);
            Assert.IsNotNull(childItem3.Value);

            PropertyDescription property3 = childItem3.Value;
            Assert.AreEqual("Property3", property3.PropertyName);
            Assert.AreEqual(typeof(object), property3.PropertyType);
            Assert.AreEqual(null, property3.PropertyValue);
            Assert.AreEqual(false, property3.IsException);
            Assert.AreEqual(false, property3.IsNullable);
            
            TreeItem<PropertyDescription> childItem4 = treeItem.Children.ElementAt(3);
            Assert.AreEqual(1, childItem4.Parents.Count());
            Assert.AreEqual(treeItem, childItem4.Parents.ElementAt(0));

            List<TreeItem<PropertyDescription>> childItem4Parents = new List<TreeItem<PropertyDescription>>();
            childItem4Parents.Add(treeItem);
            CheckTestStruct(childItem4, "Property4", typeof(TestStruct), default(TestStruct), childItem4Parents);
        }

        class TestClass2 {
            public TestClass2 Property { get; set; }
        }

        [TestMethod]
        public void TestClass2IsNull() {
            IEnumerable<TreeItem<PropertyDescription>> parents = null;
            string propertyName = "TestClass2";
            Type propertyType = typeof(TestClass2);
            TestClass2 propertyValue = null;

            TreeItem<PropertyDescription> treeItem = CreateItem(parents, propertyName, propertyType, propertyValue);

            Assert.AreEqual(parents, treeItem.Parents);
            Assert.IsNull(treeItem.Children);
            Assert.IsFalse(treeItem.HasChildrenCycle);
            Assert.IsNotNull(treeItem.Value);

            PropertyDescription property = treeItem.Value;
            Assert.AreEqual(propertyName, property.PropertyName);
            Assert.AreEqual(propertyType, property.PropertyType);
            Assert.AreEqual(propertyValue, property.PropertyValue);
            Assert.AreEqual(false, property.IsException);
            Assert.AreEqual(false, property.IsNullable);
        }

        [TestMethod]
        public void TestClass2IsNotNull_PropertyIsNull() {
            IEnumerable<TreeItem<PropertyDescription>> parents = null;
            string propertyName = "TestClass2";
            Type propertyType = typeof(TestClass2);
            TestClass2 propertyValue = new TestClass2();

            TreeItem<PropertyDescription> treeItem = CreateItem(parents, propertyName, propertyType, propertyValue);

            Assert.AreEqual(parents, treeItem.Parents);
            Assert.IsFalse(treeItem.HasChildrenCycle);
            Assert.IsNotNull(treeItem.Value);

            PropertyDescription property = treeItem.Value;
            Assert.AreEqual(propertyName, property.PropertyName);
            Assert.AreEqual(propertyType, property.PropertyType);
            Assert.AreEqual(propertyValue, property.PropertyValue);
            Assert.AreEqual(false, property.IsException);
            Assert.AreEqual(false, property.IsNullable);

            Assert.AreEqual(1, treeItem.Children.Count());

            TreeItem<PropertyDescription> childItem1 = treeItem.Children.ElementAt(0);
            Assert.AreEqual(1, childItem1.Parents.Count());
            Assert.AreEqual(treeItem, childItem1.Parents.ElementAt(0));
            Assert.IsNull(childItem1.Children);
            Assert.IsFalse(childItem1.HasChildrenCycle);
            Assert.IsNotNull(childItem1.Value);

            PropertyDescription property1 = childItem1.Value;
            Assert.AreEqual("Property", property1.PropertyName);
            Assert.AreEqual(typeof(TestClass2), property1.PropertyType);
            Assert.AreEqual(null, property1.PropertyValue);
            Assert.AreEqual(false, property1.IsException);
            Assert.AreEqual(false, property1.IsNullable);
        }

        [TestMethod]
        public void TestClass2IsNotNull_PropertyIsNotNull() {
            IEnumerable<TreeItem<PropertyDescription>> parents = null;
            string propertyName = "TestClass2";
            Type propertyType = typeof(TestClass2);
            TestClass2 propertyValue = new TestClass2();
            TestClass2 childPropertyValue = new TestClass2();
            propertyValue.Property = childPropertyValue;

            TreeItem<PropertyDescription> treeItem = CreateItem(parents, propertyName, propertyType, propertyValue);

            Assert.AreEqual(parents, treeItem.Parents);
            Assert.IsFalse(treeItem.HasChildrenCycle);
            Assert.IsNotNull(treeItem.Value);

            PropertyDescription property = treeItem.Value;
            Assert.AreEqual(propertyName, property.PropertyName);
            Assert.AreEqual(propertyType, property.PropertyType);
            Assert.AreEqual(propertyValue, property.PropertyValue);
            Assert.AreEqual(false, property.IsException);
            Assert.AreEqual(false, property.IsNullable);
            
            Assert.AreEqual(1, treeItem.Children.Count());

            TreeItem<PropertyDescription> childItem1 = treeItem.Children.ElementAt(0);
            Assert.AreEqual(1, childItem1.Parents.Count());
            Assert.AreEqual(treeItem, childItem1.Parents.ElementAt(0));
            Assert.IsFalse(childItem1.HasChildrenCycle);
            Assert.IsNotNull(childItem1.Value);

            PropertyDescription property1 = childItem1.Value;
            Assert.AreEqual("Property", property1.PropertyName);
            Assert.AreEqual(typeof(TestClass2), property1.PropertyType);
            Assert.AreEqual(childPropertyValue, property1.PropertyValue);
            Assert.AreEqual(false, property1.IsException);
            Assert.AreEqual(false, property1.IsNullable);

            Assert.AreEqual(1, childItem1.Children.Count());

            TreeItem<PropertyDescription> childChildItem1 = childItem1.Children.ElementAt(0);
            Assert.AreEqual(2, childChildItem1.Parents.Count());
            Assert.AreEqual(treeItem, childChildItem1.Parents.ElementAt(0));
            Assert.AreEqual(childItem1, childChildItem1.Parents.ElementAt(1));
            Assert.IsNull(childChildItem1.Children);
            Assert.IsFalse(childChildItem1.HasChildrenCycle);
            Assert.IsNotNull(childChildItem1.Value);

            PropertyDescription childProperty1 = childChildItem1.Value;
            Assert.AreEqual("Property", childProperty1.PropertyName);
            Assert.AreEqual(typeof(TestClass2), childProperty1.PropertyType);
            Assert.AreEqual(null, childProperty1.PropertyValue);
            Assert.AreEqual(false, childProperty1.IsException);
            Assert.AreEqual(false, childProperty1.IsNullable);
        }

        [TestMethod]
        public void TestClass2IsNotNull_HasChildrenCycle() {
            IEnumerable<TreeItem<PropertyDescription>> parents = null;
            string propertyName = "TestClass2";
            Type propertyType = typeof(TestClass2);
            TestClass2 propertyValue = new TestClass2();
            propertyValue.Property = propertyValue;

            TreeItem<PropertyDescription> treeItem = CreateItem(parents, propertyName, propertyType, propertyValue);

            Assert.AreEqual(parents, treeItem.Parents);
            Assert.IsFalse(treeItem.HasChildrenCycle);
            Assert.IsNotNull(treeItem.Value);

            PropertyDescription property = treeItem.Value;
            Assert.AreEqual(propertyName, property.PropertyName);
            Assert.AreEqual(propertyType, property.PropertyType);
            Assert.AreEqual(propertyValue, property.PropertyValue);
            Assert.AreEqual(false, property.IsException);
            Assert.AreEqual(false, property.IsNullable);

            Assert.AreEqual(1, treeItem.Children.Count());

            TreeItem<PropertyDescription> childItem1 = treeItem.Children.ElementAt(0);
            Assert.AreEqual(1, childItem1.Parents.Count());
            Assert.AreEqual(treeItem, childItem1.Parents.ElementAt(0));
            Assert.IsNull(childItem1.Children);
            Assert.IsTrue(childItem1.HasChildrenCycle);
            Assert.IsNotNull(childItem1.Value);

            PropertyDescription property1 = childItem1.Value;
            Assert.AreEqual("Property", property1.PropertyName);
            Assert.AreEqual(typeof(TestClass2), property1.PropertyType);
            Assert.AreEqual(propertyValue, property1.PropertyValue);
            Assert.AreEqual(false, property1.IsException);
            Assert.AreEqual(false, property1.IsNullable);
        }

        [TestMethod]
        public void StringIsNull() {
            IEnumerable<TreeItem<PropertyDescription>> parents = null;
            string propertyName = "String";
            Type propertyType = typeof(string);
            string propertyValue = null;

            TreeItem<PropertyDescription> treeItem = CreateItem(parents, propertyName, propertyType, propertyValue);

            Assert.AreEqual(parents, treeItem.Parents);
            Assert.IsNull(treeItem.Children);
            Assert.IsFalse(treeItem.HasChildrenCycle);
            Assert.IsNotNull(treeItem.Value);

            PropertyDescription property = treeItem.Value;
            Assert.AreEqual(propertyName, property.PropertyName);
            Assert.AreEqual(propertyType, property.PropertyType);
            Assert.AreEqual(propertyValue, property.PropertyValue);
            Assert.AreEqual(false, property.IsException);
            Assert.AreEqual(false, property.IsNullable);
        }

        [TestMethod]
        public void StringIsEmpty() {
            IEnumerable<TreeItem<PropertyDescription>> parents = null;
            string propertyName = "String";
            Type propertyType = typeof(string);
            string propertyValue = String.Empty;

            TreeItem<PropertyDescription> treeItem = CreateItem(parents, propertyName, propertyType, propertyValue);

            Assert.AreEqual(parents, treeItem.Parents);
            Assert.IsNull(treeItem.Children);
            Assert.IsFalse(treeItem.HasChildrenCycle);
            Assert.IsNotNull(treeItem.Value);

            PropertyDescription property = treeItem.Value;
            Assert.AreEqual(propertyName, property.PropertyName);
            Assert.AreEqual(propertyType, property.PropertyType);
            Assert.AreEqual(propertyValue, property.PropertyValue);
            Assert.AreEqual(false, property.IsException);
            Assert.AreEqual(false, property.IsNullable);
        }

        [TestMethod]
        public void StringIsNotEmpty() {
            IEnumerable<TreeItem<PropertyDescription>> parents = null;
            string propertyName = "String";
            Type propertyType = typeof(string);
            string propertyValue = "testString";

            TreeItem<PropertyDescription> treeItem = CreateItem(parents, propertyName, propertyType, propertyValue);

            Assert.AreEqual(parents, treeItem.Parents);
            Assert.IsNull(treeItem.Children);
            Assert.IsFalse(treeItem.HasChildrenCycle);
            Assert.IsNotNull(treeItem.Value);

            PropertyDescription property = treeItem.Value;
            Assert.AreEqual(propertyName, property.PropertyName);
            Assert.AreEqual(propertyType, property.PropertyType);
            Assert.AreEqual(propertyValue, property.PropertyValue);
            Assert.AreEqual(false, property.IsException);
            Assert.AreEqual(false, property.IsNullable);
        }

        [TestMethod]
        public void ListIntIsNull() {
            IEnumerable<TreeItem<PropertyDescription>> parents = null;
            string propertyName = "List<int>";
            Type propertyType = typeof(List<int>);
            List<int> propertyValue = null;

            TreeItem<PropertyDescription> treeItem = CreateItem(parents, propertyName, propertyType, propertyValue);

            Assert.AreEqual(parents, treeItem.Parents);
            Assert.IsNull(treeItem.Children);
            Assert.IsFalse(treeItem.HasChildrenCycle);
            Assert.IsNotNull(treeItem.Value);

            PropertyDescription property = treeItem.Value;
            Assert.AreEqual(propertyName, property.PropertyName);
            Assert.AreEqual(propertyType, property.PropertyType);
            Assert.AreEqual(propertyValue, property.PropertyValue);
            Assert.AreEqual(false, property.IsException);
            Assert.AreEqual(false, property.IsNullable);
        }

        [TestMethod]
        public void ListIntIsEmpty() {
            IEnumerable<TreeItem<PropertyDescription>> parents = null;
            string propertyName = "List<double>";
            Type propertyType = typeof(List<double>);
            List<double> propertyValue = new List<double>();

            TreeItem<PropertyDescription> treeItem = CreateItem(parents, propertyName, propertyType, propertyValue);

            Assert.AreEqual(parents, treeItem.Parents);
            Assert.IsFalse(treeItem.HasChildrenCycle);
            Assert.IsNotNull(treeItem.Value);

            PropertyDescription property = treeItem.Value;
            Assert.AreEqual(propertyName, property.PropertyName);
            Assert.AreEqual(propertyType, property.PropertyType);
            Assert.AreEqual(propertyValue, property.PropertyValue);
            Assert.AreEqual(false, property.IsException);
            Assert.AreEqual(false, property.IsNullable);

            Assert.AreEqual(2, treeItem.Children.Count());

            TreeItem<PropertyDescription> childItem1 = treeItem.Children.ElementAt(0);
            Assert.AreEqual(1, childItem1.Parents.Count());
            Assert.AreEqual(treeItem, childItem1.Parents.ElementAt(0));
            Assert.AreEqual(0, childItem1.Children.Count());
            Assert.IsFalse(childItem1.HasChildrenCycle);
            Assert.IsNotNull(childItem1.Value);

            PropertyDescription property1 = childItem1.Value;
            Assert.AreEqual("Capacity", property1.PropertyName);
            Assert.AreEqual(typeof(int), property1.PropertyType);
            Assert.AreEqual(0, property1.PropertyValue);
            Assert.AreEqual(false, property1.IsException);
            Assert.AreEqual(false, property1.IsNullable);

            TreeItem<PropertyDescription> childItem2 = treeItem.Children.ElementAt(1);
            Assert.AreEqual(1, childItem2.Parents.Count());
            Assert.AreEqual(treeItem, childItem2.Parents.ElementAt(0));
            Assert.AreEqual(0, childItem2.Children.Count());
            Assert.IsFalse(childItem2.HasChildrenCycle);
            Assert.IsNotNull(childItem2.Value);

            PropertyDescription property2 = childItem2.Value;
            Assert.AreEqual("Count", property2.PropertyName);
            Assert.AreEqual(typeof(int), property2.PropertyType);
            Assert.AreEqual(0, property2.PropertyValue);
            Assert.AreEqual(false, property2.IsException);
            Assert.AreEqual(false, property2.IsNullable);
        }

        [TestMethod]
        public void ListIntIsNotEmpty() {
            IEnumerable<TreeItem<PropertyDescription>> parents = null;
            string propertyName = "List<double>";
            Type propertyType = typeof(List<double>);
            List<double> propertyValue = new List<double>();
            propertyValue.Add(2);

            TreeItem<PropertyDescription> treeItem = CreateItem(parents, propertyName, propertyType, propertyValue);

            Assert.AreEqual(parents, treeItem.Parents);
            Assert.IsFalse(treeItem.HasChildrenCycle);
            Assert.IsNotNull(treeItem.Value);

            PropertyDescription property = treeItem.Value;
            Assert.AreEqual(propertyName, property.PropertyName);
            Assert.AreEqual(propertyType, property.PropertyType);
            Assert.AreEqual(propertyValue, property.PropertyValue);
            Assert.AreEqual(false, property.IsException);
            Assert.AreEqual(false, property.IsNullable);

            Assert.AreEqual(2, treeItem.Children.Count());

            TreeItem<PropertyDescription> childItem1 = treeItem.Children.ElementAt(0);
            Assert.AreEqual(1, childItem1.Parents.Count());
            Assert.AreEqual(treeItem, childItem1.Parents.ElementAt(0));
            Assert.AreEqual(0, childItem1.Children.Count());
            Assert.IsFalse(childItem1.HasChildrenCycle);
            Assert.IsNotNull(childItem1.Value);

            PropertyDescription property1 = childItem1.Value;
            Assert.AreEqual("Capacity", property1.PropertyName);
            Assert.AreEqual(typeof(int), property1.PropertyType);
            Assert.AreEqual(4, property1.PropertyValue);
            Assert.AreEqual(false, property1.IsException);
            Assert.AreEqual(false, property1.IsNullable);

            TreeItem<PropertyDescription> childItem2 = treeItem.Children.ElementAt(1);
            Assert.AreEqual(1, childItem2.Parents.Count());
            Assert.AreEqual(treeItem, childItem2.Parents.ElementAt(0));
            Assert.AreEqual(0, childItem2.Children.Count());
            Assert.IsFalse(childItem2.HasChildrenCycle);
            Assert.IsNotNull(childItem2.Value);

            PropertyDescription property2 = childItem2.Value;
            Assert.AreEqual("Count", property2.PropertyName);
            Assert.AreEqual(typeof(int), property2.PropertyType);
            Assert.AreEqual(1, property2.PropertyValue);
            Assert.AreEqual(false, property2.IsException);
            Assert.AreEqual(false, property2.IsNullable);
        }

        [TestMethod]
        public void DictionaryIsNull() {
            IEnumerable<TreeItem<PropertyDescription>> parents = null;
            string propertyName = "Dictionary<string, double>";
            Type propertyType = typeof(Dictionary<string, double>);
            Dictionary<string, double> propertyValue = null;

            TreeItem<PropertyDescription> treeItem = CreateItem(parents, propertyName, propertyType, propertyValue);

            Assert.AreEqual(parents, treeItem.Parents);
            Assert.IsNull(treeItem.Children);
            Assert.IsFalse(treeItem.HasChildrenCycle);
            Assert.IsNotNull(treeItem.Value);

            PropertyDescription property = treeItem.Value;
            Assert.AreEqual(propertyName, property.PropertyName);
            Assert.AreEqual(propertyType, property.PropertyType);
            Assert.AreEqual(propertyValue, property.PropertyValue);
            Assert.AreEqual(false, property.IsException);
            Assert.AreEqual(false, property.IsNullable);
        }

        [TestMethod]
        public void DictionaryIsEmpty() {
            IEnumerable<TreeItem<PropertyDescription>> parents = null;
            string propertyName = "Dictionary<string, double>";
            Type propertyType = typeof(Dictionary<string, double>);
            Dictionary<string, double> propertyValue = new Dictionary<string, double>();

            TreeItem<PropertyDescription> treeItem = CreateItem(parents, propertyName, propertyType, propertyValue);

            Assert.AreEqual(parents, treeItem.Parents);
            Assert.IsFalse(treeItem.HasChildrenCycle);
            Assert.IsNotNull(treeItem.Value);

            PropertyDescription property = treeItem.Value;
            Assert.AreEqual(propertyName, property.PropertyName);
            Assert.AreEqual(propertyType, property.PropertyType);
            Assert.AreEqual(propertyValue, property.PropertyValue);
            Assert.AreEqual(false, property.IsException);
            Assert.AreEqual(false, property.IsNullable);

            Assert.AreEqual(4, treeItem.Children.Count());

            TreeItem<PropertyDescription> childItem1 = treeItem.Children.ElementAt(0);
            Assert.AreEqual(1, childItem1.Parents.Count());
            Assert.AreEqual(treeItem, childItem1.Parents.ElementAt(0));
            Assert.AreEqual(0, childItem1.Children.Count());
            Assert.IsFalse(childItem1.HasChildrenCycle);
            Assert.IsNotNull(childItem1.Value);

            PropertyDescription property1 = childItem1.Value;
            Assert.AreEqual("Comparer", property1.PropertyName);
            Assert.AreEqual(typeof(IEqualityComparer<string>), property1.PropertyType);
            Assert.AreEqual(propertyValue.Comparer, property1.PropertyValue);
            Assert.AreEqual(false, property1.IsException);
            Assert.AreEqual(false, property1.IsNullable);

            TreeItem<PropertyDescription> childItem2 = treeItem.Children.ElementAt(1);
            Assert.AreEqual(1, childItem2.Parents.Count());
            Assert.AreEqual(treeItem, childItem2.Parents.ElementAt(0));
            Assert.AreEqual(0, childItem2.Children.Count());
            Assert.IsFalse(childItem2.HasChildrenCycle);
            Assert.IsNotNull(childItem2.Value);

            PropertyDescription property2 = childItem2.Value;
            Assert.AreEqual("Count", property2.PropertyName);
            Assert.AreEqual(typeof(int), property2.PropertyType);
            Assert.AreEqual(0, property2.PropertyValue);
            Assert.AreEqual(false, property2.IsException);
            Assert.AreEqual(false, property2.IsNullable);

            TreeItem<PropertyDescription> childItem3 = treeItem.Children.ElementAt(2);
            Assert.AreEqual(1, childItem3.Parents.Count());
            Assert.AreEqual(treeItem, childItem3.Parents.ElementAt(0));
            Assert.IsFalse(childItem3.HasChildrenCycle);
            Assert.IsNotNull(childItem3.Value);

            PropertyDescription property3 = childItem3.Value;
            Assert.AreEqual("Keys", property3.PropertyName);
            Assert.AreEqual(typeof(Dictionary<string, double>.KeyCollection), property3.PropertyType);
            Assert.AreEqual(false, property3.IsException);
            Assert.AreEqual(false, property3.IsNullable);

            Assert.AreEqual(1, childItem3.Children.Count());

            TreeItem<PropertyDescription> childChildItem3 = childItem3.Children.ElementAt(0);
            Assert.AreEqual(2, childChildItem3.Parents.Count());
            Assert.AreEqual(treeItem, childChildItem3.Parents.ElementAt(0));
            Assert.AreEqual(childItem3, childChildItem3.Parents.ElementAt(1));
            Assert.AreEqual(0, childChildItem3.Children.Count());
            Assert.IsFalse(childChildItem3.HasChildrenCycle);
            Assert.IsNotNull(childChildItem3.Value);

            PropertyDescription childProperty3 = childChildItem3.Value;
            Assert.AreEqual("Count", childProperty3.PropertyName);
            Assert.AreEqual(typeof(int), childProperty3.PropertyType);
            Assert.AreEqual(0, childProperty3.PropertyValue);
            Assert.AreEqual(false, childProperty3.IsException);
            Assert.AreEqual(false, childProperty3.IsNullable);

            TreeItem<PropertyDescription> childItem4 = treeItem.Children.ElementAt(3);
            Assert.AreEqual(1, childItem4.Parents.Count());
            Assert.AreEqual(treeItem, childItem4.Parents.ElementAt(0));
            Assert.IsFalse(childItem4.HasChildrenCycle);
            Assert.IsNotNull(childItem4.Value);

            PropertyDescription property4 = childItem4.Value;
            Assert.AreEqual("Values", property4.PropertyName);
            Assert.AreEqual(typeof(Dictionary<string, double>.ValueCollection), property4.PropertyType);
            Assert.AreEqual(false, property4.IsException);
            Assert.AreEqual(false, property4.IsNullable);

            Assert.AreEqual(1, childItem4.Children.Count());

            TreeItem<PropertyDescription> childChildItem4 = childItem4.Children.ElementAt(0);
            Assert.AreEqual(2, childChildItem4.Parents.Count());
            Assert.AreEqual(treeItem, childChildItem4.Parents.ElementAt(0));
            Assert.AreEqual(childItem4, childChildItem4.Parents.ElementAt(1));
            Assert.AreEqual(0, childChildItem4.Children.Count());
            Assert.IsFalse(childChildItem4.HasChildrenCycle);
            Assert.IsNotNull(childChildItem4.Value);

            PropertyDescription childProperty4 = childChildItem4.Value;
            Assert.AreEqual("Count", childProperty4.PropertyName);
            Assert.AreEqual(typeof(int), childProperty4.PropertyType);
            Assert.AreEqual(0, childProperty4.PropertyValue);
            Assert.AreEqual(false, childProperty4.IsException);
            Assert.AreEqual(false, childProperty4.IsNullable);
        }

        [TestMethod]
        public void DictionaryIsNotEmpty() {
            IEnumerable<TreeItem<PropertyDescription>> parents = null;
            string propertyName = "Dictionary<string, double>";
            Type propertyType = typeof(Dictionary<string, double>);
            Dictionary<string, double> propertyValue = new Dictionary<string, double>();
            propertyValue.Add("Test", 2);

            TreeItem<PropertyDescription> treeItem = CreateItem(parents, propertyName, propertyType, propertyValue);

            Assert.AreEqual(parents, treeItem.Parents);
            Assert.IsNotNull(treeItem.Children);
            Assert.IsFalse(treeItem.HasChildrenCycle);
            Assert.IsNotNull(treeItem.Value);

            PropertyDescription property = treeItem.Value;
            Assert.AreEqual(propertyName, property.PropertyName);
            Assert.AreEqual(propertyType, property.PropertyType);
            Assert.AreEqual(propertyValue, property.PropertyValue);
            Assert.AreEqual(false, property.IsException);
            Assert.AreEqual(false, property.IsNullable);

            Assert.AreEqual(4, treeItem.Children.Count());

            TreeItem<PropertyDescription> childItem1 = treeItem.Children.ElementAt(0);
            Assert.AreEqual(1, childItem1.Parents.Count());
            Assert.AreEqual(treeItem, childItem1.Parents.ElementAt(0));
            Assert.AreEqual(0, childItem1.Children.Count());
            Assert.IsFalse(childItem1.HasChildrenCycle);
            Assert.IsNotNull(childItem1.Value);

            PropertyDescription property1 = childItem1.Value;
            Assert.AreEqual("Comparer", property1.PropertyName);
            Assert.AreEqual(typeof(IEqualityComparer<string>), property1.PropertyType);
            Assert.AreEqual(propertyValue.Comparer, property1.PropertyValue);
            Assert.AreEqual(false, property1.IsException);
            Assert.AreEqual(false, property1.IsNullable);

            TreeItem<PropertyDescription> childItem2 = treeItem.Children.ElementAt(1);
            Assert.AreEqual(1, childItem2.Parents.Count());
            Assert.AreEqual(treeItem, childItem2.Parents.ElementAt(0));
            Assert.AreEqual(0, childItem2.Children.Count());
            Assert.IsFalse(childItem2.HasChildrenCycle);
            Assert.IsNotNull(childItem2.Value);

            PropertyDescription property2 = childItem2.Value;
            Assert.AreEqual("Count", property2.PropertyName);
            Assert.AreEqual(typeof(int), property2.PropertyType);
            Assert.AreEqual(1, property2.PropertyValue);
            Assert.AreEqual(false, property2.IsException);
            Assert.AreEqual(false, property2.IsNullable);

            TreeItem<PropertyDescription> childItem3 = treeItem.Children.ElementAt(2);
            Assert.AreEqual(1, childItem3.Parents.Count());
            Assert.AreEqual(treeItem, childItem3.Parents.ElementAt(0));
            Assert.IsFalse(childItem3.HasChildrenCycle);
            Assert.IsNotNull(childItem3.Value);

            PropertyDescription property3 = childItem3.Value;
            Assert.AreEqual("Keys", property3.PropertyName);
            Assert.AreEqual(typeof(Dictionary<string, double>.KeyCollection), property3.PropertyType);
            Assert.AreEqual(false, property3.IsException);
            Assert.AreEqual(false, property3.IsNullable);

            Assert.AreEqual(1, childItem3.Children.Count());

            TreeItem<PropertyDescription> childChildItem3 = childItem3.Children.ElementAt(0);
            Assert.AreEqual(2, childChildItem3.Parents.Count());
            Assert.AreEqual(treeItem, childChildItem3.Parents.ElementAt(0));
            Assert.AreEqual(childItem3, childChildItem3.Parents.ElementAt(1));
            Assert.AreEqual(0, childChildItem3.Children.Count());
            Assert.IsFalse(childChildItem3.HasChildrenCycle);
            Assert.IsNotNull(childChildItem3.Value);

            PropertyDescription childProperty3 = childChildItem3.Value;
            Assert.AreEqual("Count", childProperty3.PropertyName);
            Assert.AreEqual(typeof(int), childProperty3.PropertyType);
            Assert.AreEqual(1, childProperty3.PropertyValue);
            Assert.AreEqual(false, childProperty3.IsException);
            Assert.AreEqual(false, childProperty3.IsNullable);

            TreeItem<PropertyDescription> childItem4 = treeItem.Children.ElementAt(3);
            Assert.AreEqual(1, childItem4.Parents.Count());
            Assert.AreEqual(treeItem, childItem4.Parents.ElementAt(0));
            Assert.IsFalse(childItem4.HasChildrenCycle);
            Assert.IsNotNull(childItem4.Value);

            PropertyDescription property4 = childItem4.Value;
            Assert.AreEqual("Values", property4.PropertyName);
            Assert.AreEqual(typeof(Dictionary<string, double>.ValueCollection), property4.PropertyType);
            Assert.AreEqual(false, property4.IsException);
            Assert.AreEqual(false, property4.IsNullable);

            Assert.AreEqual(1, childItem4.Children.Count());

            TreeItem<PropertyDescription> childChildItem4 = childItem4.Children.ElementAt(0);
            Assert.AreEqual(2, childChildItem4.Parents.Count());
            Assert.AreEqual(treeItem, childChildItem4.Parents.ElementAt(0));
            Assert.AreEqual(childItem4, childChildItem4.Parents.ElementAt(1));
            Assert.AreEqual(0, childChildItem4.Children.Count());
            Assert.IsFalse(childChildItem4.HasChildrenCycle);
            Assert.IsNotNull(childChildItem4.Value);

            PropertyDescription childProperty4 = childChildItem4.Value;
            Assert.AreEqual("Count", childProperty4.PropertyName);
            Assert.AreEqual(typeof(int), childProperty4.PropertyType);
            Assert.AreEqual(1, childProperty4.PropertyValue);
            Assert.AreEqual(false, childProperty4.IsException);
            Assert.AreEqual(false, childProperty4.IsNullable);
        }
    }
}
