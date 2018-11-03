using Microsoft.VisualStudio.TestTools.UnitTesting;
using Reflection.Utils.PropertyTree;
using Reflection.Utils.Tree;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Reflection.Utils.Tests {
    [TestClass]
    public class PropertyTreeBuilderTests {
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

        [TestMethod]
        public void DecimalValue() {
            IEnumerable<TreeItem<PropertyDescription>> parents = null;
            string propertyName = "Decimal";
            Type propertyType = typeof(decimal);
            object propertyOwner = null;
            decimal propertyValue = 1;
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
            Assert.AreEqual(PropertyValueType.Struct, property.PropertyValueType);
        }

        [TestMethod]
        public void NullableDecimalValue_Null() {
            IEnumerable<TreeItem<PropertyDescription>> parents = null;
            string propertyName = "NullableDecimal";
            Type propertyType = typeof(decimal?);
            object propertyOwner = null;
            decimal? propertyValue = null;
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
            Assert.AreEqual(PropertyValueType.Struct | PropertyValueType.Nullable, property.PropertyValueType);
        }

        [TestMethod]
        public void IntValue() {
            IEnumerable<TreeItem<PropertyDescription>> parents = null;
            string propertyName = "Int";
            Type propertyType = typeof(int);
            object propertyOwner = null;
            int propertyValue = 1;
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
            Assert.AreEqual(PropertyValueType.Primitive, property.PropertyValueType);
        }

        [TestMethod]
        public void NullableIntValue_Null() {
            IEnumerable<TreeItem<PropertyDescription>> parents = null;
            string propertyName = "Nullable Int";
            Type propertyType = typeof(int?);
            object propertyOwner = null;
            int? propertyValue = null;
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
            Assert.AreEqual(PropertyValueType.Primitive | PropertyValueType.Nullable, property.PropertyValueType);
        }

        [TestMethod]
        public void NullableIntValue_One() {
            IEnumerable<TreeItem<PropertyDescription>> parents = null;
            string propertyName = "Nullable Int";
            Type propertyType = typeof(int?);
            object propertyOwner = null;
            int? propertyValue = 1;
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
            Assert.AreEqual(PropertyValueType.Primitive | PropertyValueType.Nullable, property.PropertyValueType);
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
            object propertyOwner = null;
            TestEnum propertyValue = TestEnum.Value1;
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
            Assert.AreEqual(PropertyValueType.Enum, property.PropertyValueType);
        }

        [TestMethod]
        public void NullableEnumValue_Null() {
            IEnumerable<TreeItem<PropertyDescription>> parents = null;
            string propertyName = "Nullable TestEnum";
            Type propertyType = typeof(TestEnum?);
            object propertyOwner = null;
            TestEnum? propertyValue = null;
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
            Assert.AreEqual(PropertyValueType.Enum | PropertyValueType.Nullable, property.PropertyValueType);
        }

        [TestMethod]
        public void NullableEnumValue_Value1() {
            IEnumerable<TreeItem<PropertyDescription>> parents = null;
            string propertyName = "Nullable TestEnum";
            Type propertyType = typeof(TestEnum?);
            object propertyOwner = null;
            TestEnum? propertyValue = TestEnum.Value1;
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
            Assert.AreEqual(PropertyValueType.Enum | PropertyValueType.Nullable, property.PropertyValueType);
        }

        struct EmptyTestStruct { }

        [TestMethod]
        public void EmptyTestStructValue() {
            IEnumerable<TreeItem<PropertyDescription>> parents = null;
            string propertyName = "EmptyTestStruct";
            Type propertyType = typeof(EmptyTestStruct);
            object propertyOwner = null;
            EmptyTestStruct propertyValue = new EmptyTestStruct();
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
            Assert.AreEqual(PropertyValueType.Struct, property.PropertyValueType);
        }

        [TestMethod]
        public void NullableEmptyTestStructValue_Null() {
            IEnumerable<TreeItem<PropertyDescription>> parents = null;
            string propertyName = "Nullable EmptyTestStruct";
            Type propertyType = typeof(EmptyTestStruct?);
            object propertyOwner = null;
            EmptyTestStruct? propertyValue = null;
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
            Assert.AreEqual(PropertyValueType.Struct | PropertyValueType.Nullable, property.PropertyValueType);
        }

        [TestMethod]
        public void NullableEmptyTestStructValue_NotNull() {
            IEnumerable<TreeItem<PropertyDescription>> parents = null;
            string propertyName = "Nullable EmptyTestStruct";
            Type propertyType = typeof(EmptyTestStruct?);
            object propertyOwner = null;
            EmptyTestStruct? propertyValue = new EmptyTestStruct();
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
            Assert.AreEqual(PropertyValueType.Struct | PropertyValueType.Nullable, property.PropertyValueType);
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
            object propertyOwner = null;
            TestStruct propertyValue = new TestStruct();
            bool hasException = false;

            TreeItem<PropertyDescription> treeItem = CreateItem(parents, propertyName, propertyType, propertyOwner, propertyValue, hasException);
            Assert.AreEqual(parents, treeItem.Parents);
            CheckTestStruct(treeItem, propertyName, propertyType, propertyOwner, propertyValue, new List<TreeItem<PropertyDescription>>());
        }

        void CheckTestStruct(TreeItem<PropertyDescription> treeItem, string propertyName, Type propertyType, object propertyOwner, object propertyValue, IEnumerable<TreeItem<PropertyDescription>> childParrents) {
            Assert.IsNotNull(treeItem.Children);
            Assert.IsNotNull(treeItem.Value);

            PropertyDescription property = treeItem.Value;
            Assert.AreEqual(propertyName, property.PropertyName);
            Assert.AreEqual(propertyType, property.PropertyType);
            Assert.AreEqual(propertyOwner, property.PropertyOwner);
            Assert.AreEqual(propertyValue, property.PropertyValue);
            Assert.AreEqual(PropertyValueType.Struct, property.PropertyValueType);

            Assert.AreEqual(3, treeItem.Children.Count());

            TreeItem<PropertyDescription> childItem1 = treeItem.Children.ElementAt(0);
            Assert.AreEqual(childParrents.Count() + 1, childItem1.Parents.Count());
            for (int i = 0; i < childParrents.Count(); i++)
                Assert.AreEqual(childParrents.ElementAt(i), childItem1.Parents.ElementAt(i));
            Assert.AreEqual(treeItem, childItem1.Parents.ElementAt(childParrents.Count()));
            Assert.IsNull(childItem1.Children);
            Assert.IsNotNull(childItem1.Value);

            PropertyDescription property1 = childItem1.Value;
            Assert.AreEqual("Property1", property1.PropertyName);
            Assert.AreEqual(typeof(int), property1.PropertyType);
            Assert.AreEqual(propertyValue, property1.PropertyOwner);
            Assert.AreEqual(0, property1.PropertyValue);
            Assert.AreEqual(PropertyValueType.Primitive, property1.PropertyValueType);

            TreeItem<PropertyDescription> childItem2 = treeItem.Children.ElementAt(1);
            Assert.AreEqual(childParrents.Count() + 1, childItem2.Parents.Count());
            for (int i = 0; i < childParrents.Count(); i++)
                Assert.AreEqual(childParrents.ElementAt(i), childItem2.Parents.ElementAt(i));
            Assert.AreEqual(treeItem, childItem2.Parents.ElementAt(childParrents.Count()));

            Assert.IsNull(childItem2.Children);
            Assert.IsNotNull(childItem2.Value);

            PropertyDescription property2 = childItem2.Value;
            Assert.AreEqual("Property2", property2.PropertyName);
            Assert.AreEqual(typeof(TestEnum), property2.PropertyType);
            Assert.AreEqual(propertyValue, property2.PropertyOwner);
            Assert.AreEqual(TestEnum.Value1, property2.PropertyValue);
            Assert.AreEqual(PropertyValueType.Enum, property2.PropertyValueType);

            TreeItem<PropertyDescription> childItem3 = treeItem.Children.ElementAt(2);
            Assert.AreEqual(childParrents.Count() + 1, childItem3.Parents.Count());
            for (int i = 0; i < childParrents.Count(); i++)
                Assert.AreEqual(childParrents.ElementAt(i), childItem3.Parents.ElementAt(i));
            Assert.AreEqual(treeItem, childItem3.Parents.ElementAt(childParrents.Count()));

            Assert.IsNull(childItem3.Children);
            Assert.IsNotNull(childItem3.Value);

            PropertyDescription property3 = childItem3.Value;
            Assert.AreEqual("Property3", property3.PropertyName);
            Assert.AreEqual(typeof(object), property3.PropertyType);
            Assert.AreEqual(propertyValue, property3.PropertyOwner);
            Assert.AreEqual(null, property3.PropertyValue);
            Assert.AreEqual(PropertyValueType.Class, property3.PropertyValueType);
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
            object propertyOwner = null;
            TestClass propertyValue = null;
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

        [TestMethod]
        public void TestClass_NotNull() {
            IEnumerable<TreeItem<PropertyDescription>> parents = null;
            string propertyName = "TestClass";
            Type propertyType = typeof(TestClass);
            object propertyOwner = null;
            TestClass propertyValue = new TestClass();
            bool hasException = false;

            TreeItem<PropertyDescription> treeItem = CreateItem(parents, propertyName, propertyType, propertyOwner, propertyValue, hasException);

            Assert.AreEqual(parents, treeItem.Parents);
            Assert.IsNotNull(treeItem.Children);
            Assert.IsNotNull(treeItem.Value);

            PropertyDescription property = treeItem.Value;
            Assert.AreEqual(propertyName, property.PropertyName);
            Assert.AreEqual(propertyType, property.PropertyType);
            Assert.AreEqual(propertyOwner, property.PropertyOwner);
            Assert.AreEqual(propertyValue, property.PropertyValue);
            Assert.AreEqual(PropertyValueType.Class, property.PropertyValueType);

            Assert.AreEqual(4, treeItem.Children.Count());

            TreeItem<PropertyDescription> childItem1 = treeItem.Children.ElementAt(0);
            Assert.AreEqual(1, childItem1.Parents.Count());
            Assert.AreEqual(treeItem, childItem1.Parents.ElementAt(0));
            Assert.IsNull(childItem1.Children);
            Assert.IsNotNull(childItem1.Value);

            PropertyDescription property1 = childItem1.Value;
            Assert.AreEqual("Property1", property1.PropertyName);
            Assert.AreEqual(typeof(int), property1.PropertyType);
            Assert.AreEqual(propertyValue, property1.PropertyOwner);
            Assert.AreEqual(0, property1.PropertyValue);
            Assert.AreEqual(PropertyValueType.Primitive, property1.PropertyValueType);

            TreeItem<PropertyDescription> childItem2 = treeItem.Children.ElementAt(1);
            Assert.AreEqual(1, childItem2.Parents.Count());
            Assert.AreEqual(treeItem, childItem2.Parents.ElementAt(0));
            Assert.IsNull(childItem2.Children);
            Assert.IsNotNull(childItem2.Value);

            PropertyDescription property2 = childItem2.Value;
            Assert.AreEqual("Property2", property2.PropertyName);
            Assert.AreEqual(typeof(TestEnum), property2.PropertyType);
            Assert.AreEqual(propertyValue, property2.PropertyOwner);
            Assert.AreEqual(TestEnum.Value1, property2.PropertyValue);
            Assert.AreEqual(PropertyValueType.Enum, property2.PropertyValueType);

            TreeItem<PropertyDescription> childItem3 = treeItem.Children.ElementAt(2);
            Assert.AreEqual(1, childItem3.Parents.Count());
            Assert.AreEqual(treeItem, childItem3.Parents.ElementAt(0));
            Assert.IsNull(childItem3.Children);
            Assert.IsNotNull(childItem3.Value);

            PropertyDescription property3 = childItem3.Value;
            Assert.AreEqual("Property3", property3.PropertyName);
            Assert.AreEqual(typeof(object), property3.PropertyType);
            Assert.AreEqual(propertyValue, property3.PropertyOwner);
            Assert.AreEqual(null, property3.PropertyValue);
            Assert.AreEqual(PropertyValueType.Class, property3.PropertyValueType);

            TreeItem<PropertyDescription> childItem4 = treeItem.Children.ElementAt(3);
            Assert.AreEqual(1, childItem4.Parents.Count());
            Assert.AreEqual(treeItem, childItem4.Parents.ElementAt(0));

            List<TreeItem<PropertyDescription>> childItem4Parents = new List<TreeItem<PropertyDescription>>();
            childItem4Parents.Add(treeItem);
            CheckTestStruct(childItem4, "Property4", typeof(TestStruct), propertyValue, default(TestStruct), childItem4Parents);
        }

        class TestClass2 {
            public TestClass2 Property { get; set; }
        }

        [TestMethod]
        public void TestClass2IsNull() {
            IEnumerable<TreeItem<PropertyDescription>> parents = null;
            string propertyName = "TestClass2";
            Type propertyType = typeof(TestClass2);
            object propertyOwner = null;
            TestClass2 propertyValue = null;
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

        [TestMethod]
        public void TestClass2IsNotNull_PropertyIsNull() {
            IEnumerable<TreeItem<PropertyDescription>> parents = null;
            string propertyName = "TestClass2";
            Type propertyType = typeof(TestClass2);
            object propertyOwner = null;
            TestClass2 propertyValue = new TestClass2();
            bool hasException = false;

            TreeItem<PropertyDescription> treeItem = CreateItem(parents, propertyName, propertyType, propertyOwner, propertyValue, hasException);

            Assert.AreEqual(parents, treeItem.Parents);
            Assert.IsNotNull(treeItem.Children);
            Assert.IsNotNull(treeItem.Value);

            PropertyDescription property = treeItem.Value;
            Assert.AreEqual(propertyName, property.PropertyName);
            Assert.AreEqual(propertyType, property.PropertyType);
            Assert.AreEqual(propertyOwner, property.PropertyOwner);
            Assert.AreEqual(propertyValue, property.PropertyValue);
            Assert.AreEqual(PropertyValueType.Class, property.PropertyValueType);

            Assert.AreEqual(1, treeItem.Children.Count());

            TreeItem<PropertyDescription> childItem1 = treeItem.Children.ElementAt(0);
            Assert.AreEqual(1, childItem1.Parents.Count());
            Assert.AreEqual(treeItem, childItem1.Parents.ElementAt(0));
            Assert.IsNull(childItem1.Children);
            Assert.IsNotNull(childItem1.Value);

            PropertyDescription property1 = childItem1.Value;
            Assert.AreEqual("Property", property1.PropertyName);
            Assert.AreEqual(typeof(TestClass2), property1.PropertyType);
            Assert.AreEqual(propertyValue, property1.PropertyOwner);
            Assert.AreEqual(null, property1.PropertyValue);
            Assert.AreEqual(PropertyValueType.Class, property1.PropertyValueType);
        }

        [TestMethod]
        public void TestClass2IsNotNull_PropertyIsNotNull() {
            IEnumerable<TreeItem<PropertyDescription>> parents = null;
            string propertyName = "TestClass2";
            Type propertyType = typeof(TestClass2);
            object propertyOwner = null;
            TestClass2 propertyValue = new TestClass2();
            TestClass2 childPropertyValue = new TestClass2();
            propertyValue.Property = childPropertyValue;
            bool hasException = false;

            TreeItem<PropertyDescription> treeItem = CreateItem(parents, propertyName, propertyType, propertyOwner, propertyValue, hasException);

            Assert.AreEqual(parents, treeItem.Parents);
            Assert.IsNotNull(treeItem.Children);
            Assert.IsNotNull(treeItem.Value);

            PropertyDescription property = treeItem.Value;
            Assert.AreEqual(propertyName, property.PropertyName);
            Assert.AreEqual(propertyType, property.PropertyType);
            Assert.AreEqual(propertyOwner, property.PropertyOwner);
            Assert.AreEqual(propertyValue, property.PropertyValue);
            Assert.AreEqual(PropertyValueType.Class, property.PropertyValueType);

            Assert.AreEqual(1, treeItem.Children.Count());

            TreeItem<PropertyDescription> childItem1 = treeItem.Children.ElementAt(0);
            Assert.AreEqual(1, childItem1.Parents.Count());
            Assert.AreEqual(treeItem, childItem1.Parents.ElementAt(0));
            Assert.IsNotNull(childItem1.Children);
            Assert.IsNotNull(childItem1.Value);

            PropertyDescription property1 = childItem1.Value;
            Assert.AreEqual("Property", property1.PropertyName);
            Assert.AreEqual(typeof(TestClass2), property1.PropertyType);
            Assert.AreEqual(propertyValue, property1.PropertyOwner);
            Assert.AreEqual(childPropertyValue, property1.PropertyValue);
            Assert.AreEqual(PropertyValueType.Class, property1.PropertyValueType);

            Assert.AreEqual(1, childItem1.Children.Count());

            TreeItem<PropertyDescription> childChildItem1 = childItem1.Children.ElementAt(0);
            Assert.AreEqual(2, childChildItem1.Parents.Count());
            Assert.AreEqual(treeItem, childChildItem1.Parents.ElementAt(0));
            Assert.AreEqual(childItem1, childChildItem1.Parents.ElementAt(1));
            Assert.IsNull(childChildItem1.Children);
            Assert.IsNotNull(childChildItem1.Value);

            PropertyDescription childProperty1 = childChildItem1.Value;
            Assert.AreEqual("Property", childProperty1.PropertyName);
            Assert.AreEqual(typeof(TestClass2), childProperty1.PropertyType);
            Assert.AreEqual(childPropertyValue, childProperty1.PropertyOwner);
            Assert.AreEqual(null, childProperty1.PropertyValue);
            Assert.AreEqual(PropertyValueType.Class, childProperty1.PropertyValueType);
        }

        [TestMethod]
        public void TestClass2IsNotNull_PropertyIsTheSame() {
            IEnumerable<TreeItem<PropertyDescription>> parents = null;
            string propertyName = "TestClass2";
            Type propertyType = typeof(TestClass2);
            object propertyOwner = null;
            TestClass2 propertyValue = new TestClass2();
            propertyValue.Property = propertyValue;
            bool hasException = false;

            TreeItem<PropertyDescription> treeItem = CreateItem(parents, propertyName, propertyType, propertyOwner, propertyValue, hasException);

            Assert.AreEqual(parents, treeItem.Parents);
            Assert.IsNotNull(treeItem.Children);
            Assert.IsNotNull(treeItem.Value);

            PropertyDescription property = treeItem.Value;
            Assert.AreEqual(propertyName, property.PropertyName);
            Assert.AreEqual(propertyType, property.PropertyType);
            Assert.AreEqual(propertyOwner, property.PropertyOwner);
            Assert.AreEqual(propertyValue, property.PropertyValue);
            Assert.AreEqual(PropertyValueType.Class, property.PropertyValueType);

            Assert.AreEqual(1, treeItem.Children.Count());

            TreeItem<PropertyDescription> childItem1 = treeItem.Children.ElementAt(0);
            Assert.AreEqual(1, childItem1.Parents.Count());
            Assert.AreEqual(treeItem, childItem1.Parents.ElementAt(0));
            Assert.IsNull(childItem1.Children);
            Assert.IsNotNull(childItem1.Value);

            PropertyDescription property1 = childItem1.Value;
            Assert.AreEqual("Property", property1.PropertyName);
            Assert.AreEqual(typeof(TestClass2), property1.PropertyType);
            Assert.AreEqual(propertyValue, property1.PropertyOwner);
            Assert.AreEqual(propertyValue, property1.PropertyValue);
            Assert.AreEqual(PropertyValueType.Class, property1.PropertyValueType);
        }
    }

}
