using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Reflection.Utils.PropertyTree.Tests {
    [TestClass]
    public class PropertyItemBuilderTests {
        enum TestEnum {
            Value1,
            Value2,
        }

        struct TestStruct {
            object Property0 { get; set; }

            public int Property1 { get; set; }
            public TestEnum Property2 { get; set; }
            public object Property3 { get; set; }

            public void CheckChildren(PropertyObjectChildren children) {
                Assert.AreEqual(3, children.Count());

                PropertyItem item1 = children.ElementAt(0);
                Assert.AreEqual(null, item1.ObjectChildren);
                Assert.AreEqual("Property1", item1.Id);
                Assert.AreEqual(typeof(int), item1.Type);
                Assert.AreEqual(Property1, item1.Value);

                PropertyItem item2 = children.ElementAt(1);
                Assert.AreEqual(null, item2.ObjectChildren);
                Assert.AreEqual("Property2", item2.Id);
                Assert.AreEqual(typeof(TestEnum), item2.Type);
                Assert.AreEqual(Property2, item2.Value);

                PropertyItem item3 = children.ElementAt(2);
                Assert.AreEqual(null, item3.ObjectChildren);
                Assert.AreEqual("Property3", item3.Id);
                Assert.AreEqual(typeof(object), item3.Type);
                Assert.AreEqual(Property3, item3.Value);
            }
        }

        class TestClass {
            object Property0 { get; set; }

            public int Property1 { get; set; }
            public TestEnum Property2 { get; set; }
            public object Property3 { get; set; }
            public TestStruct Property4 { get; set; }

            public void CheckChildren(PropertyObjectChildren children) {
                Assert.AreEqual(4, children.Count());

                PropertyItem item1 = children.ElementAt(0);
                Assert.AreEqual(null, item1.ObjectChildren);
                Assert.AreEqual("Property1", item1.Id);
                Assert.AreEqual(typeof(int), item1.Type);
                Assert.AreEqual(Property1, item1.Value);

                PropertyItem item2 = children.ElementAt(1);
                Assert.AreEqual(null, item2.ObjectChildren);
                Assert.AreEqual("Property2", item2.Id);
                Assert.AreEqual(typeof(TestEnum), item2.Type);
                Assert.AreEqual(Property2, item2.Value);

                PropertyItem item3 = children.ElementAt(2);
                Assert.AreEqual(null, item3.ObjectChildren);
                Assert.AreEqual("Property3", item3.Id);
                Assert.AreEqual(typeof(object), item3.Type);
                Assert.AreEqual(Property3, item3.Value);

                PropertyItem item4 = children.ElementAt(3);
                Assert.AreEqual(null, item4.ObjectChildren);
                Assert.AreEqual(null, item4.ArrayChildren);
                Assert.AreEqual("Property4", item4.Id);
                Assert.AreEqual(typeof(TestStruct), item4.Type);
                Assert.AreEqual(Property4, item4.Value);
            }
        }

        class TestClass2 {
            public TestClass2 Property { get; set; }

            public void CheckChildren(PropertyObjectChildren children) {
                Assert.AreEqual(1, children.Count());

                PropertyItem item1 = children.ElementAt(0);
                Assert.AreEqual("Property", item1.Id);
                Assert.AreEqual(typeof(TestClass2), item1.Type);

                if (Property == null) {
                    Assert.AreEqual(null, item1.Value);
                    Assert.AreEqual(null, item1.ObjectChildren);
                } else {
                    Assert.AreEqual(Property, item1.Value);
                    Property.CheckChildren(item1.ObjectChildren);
                }
            }
        }

        class TestList : List<char> { }

        struct EmptyTestStruct { }

        PropertyItem CreateItem(string propertyName, Type propertyType, object propertyValue) {
            return PropertyItemBuilder.Create(propertyName, propertyType, propertyValue, null, null);
        }
                                       
        [TestMethod]
        public void PropertyNameIsNull() {
            PropertyItem item = CreateItem(null, null, null);

            Assert.AreEqual(null, item.ObjectChildren);
            Assert.AreEqual(null, item.ArrayChildren);
            Assert.AreEqual(null, item.Id);
            Assert.AreEqual(null, item.Type);
            Assert.AreEqual(null, item.Value);
        }

        [TestMethod]
        public void PropertyTypeIsNull() {
            string propertyName = "Test";
            PropertyItem item = CreateItem(propertyName, null, null);

            Assert.AreEqual(null, item.ObjectChildren);
            Assert.AreEqual(null, item.ArrayChildren);
            Assert.AreEqual(propertyName, item.Id);
            Assert.AreEqual(null, item.Type);
            Assert.AreEqual(null, item.Value);
        }

        [TestMethod]
        public void ObjectValue() {
            string propertyName = "Object";
            Type propertyType = typeof(object);
            object propertyValue = new object();

            PropertyItem item = CreateItem(propertyName, propertyType, propertyValue);
            Assert.AreEqual(0, item.ObjectChildren.Count());
            Assert.AreEqual(null, item.ArrayChildren);
            Assert.AreEqual(propertyName, item.Id);
            Assert.AreEqual(propertyType, item.Type);
            Assert.AreEqual(propertyValue, item.Value);
        }

        [TestMethod]
        public void DateTimeValue() {
            string propertyName = "DateTime";
            Type propertyType = typeof(DateTime);
            DateTime propertyValue = new DateTime(2018, 1, 1);

            PropertyItem item = CreateItem(propertyName, propertyType, propertyValue);
            Assert.AreEqual(16, item.ObjectChildren.Count());
            Assert.AreEqual(null, item.ArrayChildren);
            Assert.AreEqual(propertyName, item.Id);
            Assert.AreEqual(propertyType, item.Type);
            Assert.AreEqual(propertyValue, item.Value);

            PropertyItem child1 = item.ObjectChildren.ElementAt(0);
            Assert.AreEqual(0, child1.ObjectChildren.Count());
            Assert.AreEqual(CycleMode.Value, child1.ObjectChildren.CycleMode);
            Assert.AreEqual(null, child1.ArrayChildren);
            Assert.AreEqual("UtcNow", child1.Id);
            Assert.AreEqual(propertyType, child1.Type);
            
            // TODO

            PropertyItem child2 = item.ObjectChildren.ElementAt(1);

            // TODO

            PropertyItem child3 = item.ObjectChildren.ElementAt(2);

            // TODO

            PropertyItem child4 = item.ObjectChildren.ElementAt(3);


            
        }

        [TestMethod]
        public void DecimalValue() {
            string propertyName = "Decimal";
            Type propertyType = typeof(decimal);
            decimal propertyValue = 1;
            
            PropertyItem item = CreateItem(propertyName, propertyType, propertyValue);
            Assert.AreEqual(null, item.ObjectChildren);
            Assert.AreEqual(null, item.ArrayChildren);
            Assert.AreEqual(propertyName, item.Id);
            Assert.AreEqual(propertyType, item.Type);
            Assert.AreEqual(propertyValue, item.Value);
        }

        [TestMethod]
        public void NullableDecimalValue_Null() {
            string propertyName = "NullableDecimal";
            Type propertyType = typeof(decimal?);
            decimal? propertyValue = null;

            PropertyItem item = CreateItem(propertyName, propertyType, propertyValue);
            Assert.AreEqual(null, item.ObjectChildren);
            Assert.AreEqual(null, item.ArrayChildren);
            Assert.AreEqual(propertyName, item.Id);
            Assert.AreEqual(propertyType, item.Type);
            Assert.AreEqual(propertyValue, item.Value);
        }

        [TestMethod]
        public void IntValue() {
            string propertyName = "Int";
            Type propertyType = typeof(int);
            int propertyValue = 1;
            
            PropertyItem item = CreateItem(propertyName, propertyType, propertyValue);
            Assert.AreEqual(null, item.ObjectChildren);
            Assert.AreEqual(null, item.ArrayChildren);
            Assert.AreEqual(propertyName, item.Id);
            Assert.AreEqual(propertyType, item.Type);
            Assert.AreEqual(propertyValue, item.Value);
        }

        [TestMethod]
        public void NullableIntValue_Null() {
            string propertyName = "Nullable Int";
            Type propertyType = typeof(int?);
            int? propertyValue = null;

            PropertyItem item = CreateItem(propertyName, propertyType, propertyValue);
            Assert.AreEqual(null, item.ObjectChildren);
            Assert.AreEqual(null, item.ArrayChildren);
            Assert.AreEqual(propertyName, item.Id);
            Assert.AreEqual(propertyType, item.Type);
            Assert.AreEqual(propertyValue, item.Value);
        }

        [TestMethod]
        public void NullableIntValue_One() {
            string propertyName = "Nullable Int";
            Type propertyType = typeof(int?);
            int? propertyValue = 1;

            PropertyItem item = CreateItem(propertyName, propertyType, propertyValue);
            Assert.AreEqual(2, item.ObjectChildren.Count());
            Assert.AreEqual(null, item.ArrayChildren);
            Assert.AreEqual(propertyName, item.Id);
            Assert.AreEqual(propertyType, item.Type);
            Assert.AreEqual(propertyValue, item.Value);

            PropertyItem child1 = item.ObjectChildren.ElementAt(0);
            Assert.AreEqual(null, child1.ObjectChildren);
            Assert.AreEqual(null, child1.ArrayChildren);
            Assert.AreEqual("HasValue", child1.Id);
            Assert.AreEqual(typeof(bool), child1.Type);
            Assert.AreEqual(true, child1.Value);

            PropertyItem child2 = item.ObjectChildren.ElementAt(1);
            Assert.AreEqual(null, child1.ObjectChildren);
            Assert.AreEqual(null, child2.ArrayChildren);
            Assert.AreEqual("Value", child2.Id);
            Assert.AreEqual(typeof(int), child2.Type);
            Assert.AreEqual(propertyValue, child2.Value);
        }

        [TestMethod]
        public void TestEnumValue_Value1() {
            string propertyName = "TestEnum";
            Type propertyType = typeof(TestEnum);
            TestEnum propertyValue = TestEnum.Value1;

            PropertyItem item = CreateItem(propertyName, propertyType, propertyValue);
            Assert.AreEqual(null, item.ObjectChildren);
            Assert.AreEqual(null, item.ArrayChildren);
            Assert.AreEqual(propertyName, item.Id);
            Assert.AreEqual(propertyType, item.Type);
            Assert.AreEqual(propertyValue, item.Value);
        }

        [TestMethod]
        public void NullableEnumValue_Null() {
            string propertyName = "Nullable TestEnum";
            Type propertyType = typeof(TestEnum?);
            TestEnum? propertyValue = null;

            PropertyItem item = CreateItem(propertyName, propertyType, propertyValue);
            Assert.AreEqual(null, item.ObjectChildren);
            Assert.AreEqual(null, item.ArrayChildren);
            Assert.AreEqual(propertyName, item.Id);
            Assert.AreEqual(propertyType, item.Type);
            Assert.AreEqual(propertyValue, item.Value);
        }

        [TestMethod]
        public void NullableEnumValue_Value1() {
            string propertyName = "Nullable TestEnum";
            Type propertyType = typeof(TestEnum?);
            TestEnum? propertyValue = TestEnum.Value1;

            PropertyItem item = CreateItem(propertyName, propertyType, propertyValue);
            Assert.AreEqual(2, item.ObjectChildren.Count());
            Assert.AreEqual(null, item.ArrayChildren);
            Assert.AreEqual(propertyName, item.Id);
            Assert.AreEqual(propertyType, item.Type);
            Assert.AreEqual(propertyValue, item.Value);

            PropertyItem child1 = item.ObjectChildren.ElementAt(0);
            Assert.AreEqual(null, child1.ObjectChildren);
            Assert.AreEqual(null, child1.ArrayChildren);
            Assert.AreEqual("HasValue", child1.Id);
            Assert.AreEqual(typeof(bool), child1.Type);
            Assert.AreEqual(true, child1.Value);

            PropertyItem child2 = item.ObjectChildren.ElementAt(1);
            Assert.AreEqual(null, child1.ObjectChildren);
            Assert.AreEqual(null, child2.ArrayChildren);
            Assert.AreEqual("Value", child2.Id);
            Assert.AreEqual(typeof(TestEnum), child2.Type);
            Assert.AreEqual(propertyValue, child2.Value);
        }


        [TestMethod]
        public void EmptyTestStructValue() {
            string propertyName = "EmptyTestStruct";
            Type propertyType = typeof(EmptyTestStruct);
            EmptyTestStruct propertyValue = new EmptyTestStruct();

            PropertyItem item = CreateItem(propertyName, propertyType, propertyValue);
            Assert.AreEqual(null, item.ObjectChildren);
            Assert.AreEqual(null, item.ArrayChildren);
            Assert.AreEqual(propertyName, item.Id);
            Assert.AreEqual(propertyType, item.Type);
            Assert.AreEqual(propertyValue, item.Value);
        }

        [TestMethod]
        public void NullableEmptyTestStructValue_Null() {
            string propertyName = "Nullable EmptyTestStruct";
            Type propertyType = typeof(EmptyTestStruct?);
            EmptyTestStruct? propertyValue = null;

            PropertyItem item = CreateItem(propertyName, propertyType, propertyValue);
            Assert.AreEqual(null, item.ObjectChildren);
            Assert.AreEqual(null, item.ArrayChildren);
            Assert.AreEqual(propertyName, item.Id);
            Assert.AreEqual(propertyType, item.Type);
            Assert.AreEqual(propertyValue, item.Value);
        }

        [TestMethod]
        public void NullableEmptyTestStructValue_NotNull() {
            string propertyName = "Nullable EmptyTestStruct";
            Type propertyType = typeof(EmptyTestStruct?);
            EmptyTestStruct? propertyValue = new EmptyTestStruct();

            PropertyItem item = CreateItem(propertyName, propertyType, propertyValue);
            Assert.AreEqual(2, item.ObjectChildren.Count());
            Assert.AreEqual(null, item.ArrayChildren);
            Assert.AreEqual(propertyName, item.Id);
            Assert.AreEqual(propertyType, item.Type);
            Assert.AreEqual(propertyValue, item.Value);

            PropertyItem child1 = item.ObjectChildren.ElementAt(0);
            Assert.AreEqual(null, child1.ObjectChildren);
            Assert.AreEqual(null, child1.ArrayChildren);
            Assert.AreEqual("HasValue", child1.Id);
            Assert.AreEqual(typeof(bool), child1.Type);
            Assert.AreEqual(true, child1.Value);

            PropertyItem child2 = item.ObjectChildren.ElementAt(1);
            Assert.AreEqual(null, child2.ObjectChildren);
            Assert.AreEqual(null, child2.ArrayChildren);
            Assert.AreEqual("Value", child2.Id);
            Assert.AreEqual(typeof(EmptyTestStruct), child2.Type);
            Assert.AreEqual(propertyValue, child2.Value);
        }

        [TestMethod]
        public void TestStructValue_Default() {
            string propertyName = "TestStruct";
            Type propertyType = typeof(TestStruct);
            TestStruct propertyValue = new TestStruct();

            PropertyItem item = CreateItem(propertyName, propertyType, propertyValue);
            Assert.AreEqual(null, item.ObjectChildren);
            Assert.AreEqual(null, item.ArrayChildren);
            Assert.AreEqual(propertyName, item.Id);
            Assert.AreEqual(propertyType, item.Type);
            Assert.AreEqual(propertyValue, item.Value);
        }

        [TestMethod]
        public void TestStructValue_NotDefault() {
            string propertyName = "TestStruct";
            Type propertyType = typeof(TestStruct);
            TestStruct propertyValue = new TestStruct();
            propertyValue.Property1 = 1;

            PropertyItem item = CreateItem(propertyName, propertyType, propertyValue);
            Assert.AreEqual(null, item.ArrayChildren);
            Assert.AreEqual(propertyName, item.Id);
            Assert.AreEqual(propertyType, item.Type);
            Assert.AreEqual(propertyValue, item.Value);

            propertyValue.CheckChildren(item.ObjectChildren);
        }

        [TestMethod]
        public void TestClassIsNull() {
            string propertyName = "TestClass";
            Type propertyType = typeof(TestClass);
            TestClass propertyValue = null;

            PropertyItem item = CreateItem(propertyName, propertyType, propertyValue);
            Assert.AreEqual(null, item.ObjectChildren);
            Assert.AreEqual(null, item.ArrayChildren);
            Assert.AreEqual(propertyName, item.Id);
            Assert.AreEqual(propertyType, item.Type);
            Assert.AreEqual(propertyValue, item.Value);
        }

        [TestMethod]
        public void TestClass_NotNull() {
            string propertyName = "TestClass";
            Type propertyType = typeof(TestClass);
            TestClass propertyValue = new TestClass();

            PropertyItem item = CreateItem(propertyName, propertyType, propertyValue);
            Assert.AreEqual(propertyName, item.Id);
            Assert.AreEqual(propertyType, item.Type);
            Assert.AreEqual(null, item.ArrayChildren);
            Assert.AreEqual(propertyValue, item.Value);

            propertyValue.CheckChildren(item.ObjectChildren);
        }

        [TestMethod]
        public void TestClass2IsNull() {
            string propertyName = "TestClass2";
            Type propertyType = typeof(TestClass2);
            TestClass2 propertyValue = null;

            PropertyItem item = CreateItem(propertyName, propertyType, propertyValue);
            Assert.AreEqual(null, item.ObjectChildren);
            Assert.AreEqual(null, item.ArrayChildren);
            Assert.AreEqual(propertyName, item.Id);
            Assert.AreEqual(propertyType, item.Type);
            Assert.AreEqual(propertyValue, item.Value);
        }

        [TestMethod]
        public void TestClass2IsNotNull_PropertyIsNull() {
            string propertyName = "TestClass2";
            Type propertyType = typeof(TestClass2);
            TestClass2 propertyValue = new TestClass2();

            PropertyItem item = CreateItem(propertyName, propertyType, propertyValue);
            Assert.AreEqual(propertyName, item.Id);
            Assert.AreEqual(propertyType, item.Type);
            Assert.AreEqual(null, item.ArrayChildren);
            Assert.AreEqual(propertyValue, item.Value);

            propertyValue.CheckChildren(item.ObjectChildren);
        }

        [TestMethod]
        public void TestClass2IsNotNull_PropertyIsNotNull() {
            string propertyName = "TestClass2";
            Type propertyType = typeof(TestClass2);
            TestClass2 propertyValue = new TestClass2();
            TestClass2 childPropertyValue = new TestClass2();
            propertyValue.Property = childPropertyValue;

            PropertyItem item = CreateItem(propertyName, propertyType, propertyValue);
            Assert.AreEqual(propertyName, item.Id);
            Assert.AreEqual(propertyType, item.Type);
            Assert.AreEqual(null, item.ArrayChildren);
            Assert.AreEqual(propertyValue, item.Value);

            propertyValue.CheckChildren(item.ObjectChildren);
        }

        [TestMethod]
        public void TestClass2IsNotNull_HasChildrenCycle() {
            string propertyName = "TestClass2";
            Type propertyType = typeof(TestClass2);
            TestClass2 propertyValue = new TestClass2();
            propertyValue.Property = propertyValue;

            PropertyItem item = CreateItem(propertyName, propertyType, propertyValue);
            Assert.AreEqual(propertyName, item.Id);
            Assert.AreEqual(propertyType, item.Type);
            Assert.AreEqual(null, item.ArrayChildren);
            Assert.AreEqual(propertyValue, item.Value);
            
            Assert.AreEqual(1, item.ObjectChildren.Count());

            PropertyItem childItem1 = item.ObjectChildren.ElementAt(0);
            Assert.AreEqual(PropertyObjectChildren.ReferenceCycle, childItem1.ObjectChildren);
            Assert.AreEqual("Property", childItem1.Id);
            Assert.AreEqual(typeof(TestClass2), childItem1.Type);
            Assert.AreEqual(propertyValue, childItem1.Value);
        }

        [TestMethod]
        public void StringIsNull() {
            string propertyName = "String";
            Type propertyType = typeof(string);
            string propertyValue = null;

            PropertyItem item = CreateItem(propertyName, propertyType, propertyValue);
            Assert.AreEqual(null, item.ObjectChildren);
            Assert.AreEqual(null, item.ArrayChildren);
            Assert.AreEqual(propertyName, item.Id);
            Assert.AreEqual(propertyType, item.Type);
            Assert.AreEqual(propertyValue, item.Value);
        }

        [TestMethod]
        public void StringIsEmpty() {
            string propertyName = "String";
            Type propertyType = typeof(string);
            string propertyValue = String.Empty;

            PropertyItem item = CreateItem(propertyName, propertyType, propertyValue);
            Assert.AreEqual(null, item.ObjectChildren);
            Assert.AreEqual(null, item.ArrayChildren);
            Assert.AreEqual(propertyName, item.Id);
            Assert.AreEqual(propertyType, item.Type);
            Assert.AreEqual(propertyValue, item.Value);
        }

        [TestMethod]
        public void StringIsNotEmpty() {
            string propertyName = "String";
            Type propertyType = typeof(string);
            string propertyValue = "testString";

            PropertyItem item = CreateItem(propertyName, propertyType, propertyValue);
            Assert.AreEqual(null, item.ObjectChildren);
            Assert.AreEqual(null, item.ArrayChildren);
            Assert.AreEqual(propertyName, item.Id);
            Assert.AreEqual(propertyType, item.Type);
            Assert.AreEqual(propertyValue, item.Value);
        }

        [TestMethod]
        public void ListIntIsNull() {
            string propertyName = "List<int>";
            Type propertyType = typeof(List<int>);
            List<int> propertyValue = null;

            PropertyItem item = CreateItem(propertyName, propertyType, propertyValue);
            Assert.AreEqual(null, item.ObjectChildren);
            Assert.AreEqual(null, item.ArrayChildren);
            Assert.AreEqual(propertyName, item.Id);
            Assert.AreEqual(propertyType, item.Type);
            Assert.AreEqual(propertyValue, item.Value);
        }

        [TestMethod]
        public void ListIntIsEmpty() {
            string propertyName = "List<double>";
            Type propertyType = typeof(List<double>);
            List<double> propertyValue = new List<double>();

            PropertyItem item = CreateItem(propertyName, propertyType, propertyValue);
            Assert.AreEqual(propertyName, item.Id);
            Assert.AreEqual(propertyType, item.Type);
            Assert.AreEqual(0, item.ArrayChildren.Count());
            Assert.AreEqual(propertyValue, item.Value);

            Assert.AreEqual(2, item.ObjectChildren.Count());

            PropertyItem childItem1 = item.ObjectChildren.ElementAt(0);
            Assert.AreEqual(null, childItem1.ObjectChildren);
            Assert.AreEqual(null, childItem1.ArrayChildren);
            Assert.AreEqual("Capacity", childItem1.Id);
            Assert.AreEqual(typeof(int), childItem1.Type);
            Assert.AreEqual(0, childItem1.Value);

            PropertyItem childItem2 = item.ObjectChildren.ElementAt(1);
            Assert.AreEqual(null, childItem2.ObjectChildren);
            Assert.AreEqual(null, childItem2.ArrayChildren);
            Assert.AreEqual("Count", childItem2.Id);
            Assert.AreEqual(typeof(int), childItem2.Type);
            Assert.AreEqual(0, childItem2.Value);
        }

        [TestMethod]
        public void ListIntIsNotEmpty() {
            string propertyName = "List<double>";
            Type propertyType = typeof(List<double>);
            List<double> propertyValue = new List<double>();
            propertyValue.Add(5);

            PropertyItem item = CreateItem(propertyName, propertyType, propertyValue);
            Assert.AreEqual(propertyName, item.Id);
            Assert.AreEqual(propertyType, item.Type);
            Assert.AreEqual(propertyValue, item.Value);
            
            Assert.AreEqual(2, item.ObjectChildren.Count());

            PropertyItem childItem1 = item.ObjectChildren.ElementAt(0);
            Assert.AreEqual(null, childItem1.ObjectChildren);
            Assert.AreEqual(null, childItem1.ArrayChildren);
            Assert.AreEqual("Capacity", childItem1.Id);
            Assert.AreEqual(typeof(int), childItem1.Type);
            Assert.AreEqual(4, childItem1.Value);

            PropertyItem childItem2 = item.ObjectChildren.ElementAt(1);
            Assert.AreEqual(null, childItem2.ObjectChildren);
            Assert.AreEqual(null, childItem2.ArrayChildren);
            Assert.AreEqual("Count", childItem2.Id);
            Assert.AreEqual(typeof(int), childItem2.Type);
            Assert.AreEqual(1, childItem2.Value);

            Assert.AreEqual(1, item.ArrayChildren.Count());

            PropertyItem arrayItem1 = item.ArrayChildren.ElementAt(0);
            Assert.AreEqual(null, arrayItem1.ObjectChildren);
            Assert.AreEqual(null, arrayItem1.ArrayChildren);
            Assert.AreEqual(0, arrayItem1.Id);
            Assert.AreEqual(typeof(double), arrayItem1.Type);
            Assert.AreEqual((double)5, arrayItem1.Value);
        }

        [TestMethod]
        public void TestListIsNotEmpty() {
            string propertyName = "TestList";
            Type propertyType = typeof(TestList);
            TestList propertyValue = new TestList();
            propertyValue.Add('5');

            PropertyItem item = CreateItem(propertyName, propertyType, propertyValue);
            Assert.AreEqual(propertyName, item.Id);
            Assert.AreEqual(propertyType, item.Type);
            Assert.AreEqual(propertyValue, item.Value);

            Assert.AreEqual(2, item.ObjectChildren.Count());

            PropertyItem childItem1 = item.ObjectChildren.ElementAt(0);
            Assert.AreEqual(null, childItem1.ObjectChildren);
            Assert.AreEqual(null, childItem1.ArrayChildren);
            Assert.AreEqual("Capacity", childItem1.Id);
            Assert.AreEqual(typeof(int), childItem1.Type);
            Assert.AreEqual(4, childItem1.Value);

            PropertyItem childItem2 = item.ObjectChildren.ElementAt(1);
            Assert.AreEqual(null, childItem2.ObjectChildren);
            Assert.AreEqual(null, childItem2.ArrayChildren);
            Assert.AreEqual("Count", childItem2.Id);
            Assert.AreEqual(typeof(int), childItem2.Type);
            Assert.AreEqual(1, childItem2.Value);

            Assert.AreEqual(1, item.ArrayChildren.Count());

            PropertyItem arrayItem1 = item.ArrayChildren.ElementAt(0);
            Assert.AreEqual(null, arrayItem1.ObjectChildren);
            Assert.AreEqual(null, arrayItem1.ArrayChildren);
            Assert.AreEqual(0, arrayItem1.Id);
            Assert.AreEqual(typeof(char), arrayItem1.Type);
            Assert.AreEqual('5', arrayItem1.Value);
        }

        [TestMethod]
        public void DictionaryIsNull() {
            string propertyName = "Dictionary<string, double>";
            Type propertyType = typeof(Dictionary<string, double>);
            Dictionary<string, double> propertyValue = null;

            PropertyItem item = CreateItem(propertyName, propertyType, propertyValue);
            Assert.AreEqual(null, item.ObjectChildren);
            Assert.AreEqual(null, item.ArrayChildren);
            Assert.AreEqual(propertyName, item.Id);
            Assert.AreEqual(propertyType, item.Type);
            Assert.AreEqual(propertyValue, item.Value);
        }

        [TestMethod]
        public void DictionaryIsEmpty() {
            string propertyName = "Dictionary<string, double>";
            Type propertyType = typeof(Dictionary<string, double>);
            Dictionary<string, double> propertyValue = new Dictionary<string, double>();

            PropertyItem item = CreateItem(propertyName, propertyType, propertyValue);
            Assert.AreEqual(0, item.ArrayChildren.Count());
            Assert.AreEqual(propertyName, item.Id);
            Assert.AreEqual(propertyType, item.Type);
            Assert.AreEqual(propertyValue, item.Value);

            Assert.AreEqual(4, item.ObjectChildren.Count());

            PropertyItem childItem1 = item.ObjectChildren.ElementAt(0);
            Assert.AreEqual(0, childItem1.ObjectChildren.Count());
            Assert.AreEqual(null, childItem1.ArrayChildren);
            Assert.AreEqual("Comparer", childItem1.Id);
            Assert.AreEqual(typeof(IEqualityComparer<string>), childItem1.Type);
            Assert.AreEqual(propertyValue.Comparer, childItem1.Value);

            PropertyItem childItem2 = item.ObjectChildren.ElementAt(1);
            Assert.AreEqual(null, childItem2.ObjectChildren);
            Assert.AreEqual(null, childItem2.ArrayChildren);
            Assert.AreEqual("Count", childItem2.Id);
            Assert.AreEqual(typeof(int), childItem2.Type);
            Assert.AreEqual(0, childItem2.Value);

            PropertyItem childItem3 = item.ObjectChildren.ElementAt(2);
            Assert.AreEqual(0, childItem3.ArrayChildren.Count());
            Assert.AreEqual("Keys", childItem3.Id);
            Assert.AreEqual(typeof(Dictionary<string, double>.KeyCollection), childItem3.Type);
            Assert.AreEqual(propertyValue.Keys, childItem3.Value);
                                  
            Assert.AreEqual(1, childItem3.ObjectChildren.Count());

            PropertyItem childChildItem3 = childItem3.ObjectChildren.ElementAt(0);
            Assert.AreEqual(null, childChildItem3.ObjectChildren);
            Assert.AreEqual(null, childChildItem3.ArrayChildren);
            Assert.AreEqual("Count", childChildItem3.Id);
            Assert.AreEqual(typeof(int), childChildItem3.Type);
            Assert.AreEqual(0, childChildItem3.Value);

            PropertyItem childItem4 = item.ObjectChildren.ElementAt(3);
            Assert.AreEqual(0, childItem4.ArrayChildren.Count());
            Assert.AreEqual("Values", childItem4.Id);
            Assert.AreEqual(typeof(Dictionary<string, double>.ValueCollection), childItem4.Type);
            Assert.AreEqual(propertyValue.Values, childItem4.Value);
                       
            Assert.AreEqual(1, childItem4.ObjectChildren.Count());

            PropertyItem childChildItem4 = childItem4.ObjectChildren.ElementAt(0);
            Assert.AreEqual(null, childChildItem4.ArrayChildren);
            Assert.AreEqual(null, childChildItem4.ObjectChildren);
            Assert.AreEqual("Count", childChildItem4.Id);
            Assert.AreEqual(typeof(int), childChildItem4.Type);
            Assert.AreEqual(0, childChildItem4.Value);
        }

        [TestMethod]
        public void DictionaryIsNotEmpty() {
            string propertyName = "Dictionary<string, double>";
            Type propertyType = typeof(Dictionary<string, double>);
            Dictionary<string, double> propertyValue = new Dictionary<string, double>();
            propertyValue.Add("Test", 2);

            PropertyItem item = CreateItem(propertyName, propertyType, propertyValue);
            Assert.AreEqual(propertyName, item.Id);
            Assert.AreEqual(propertyType, item.Type);
            Assert.AreEqual(propertyValue, item.Value);

            Assert.AreEqual(1, item.ArrayChildren.Count());
            PropertyItem arrayItem1 = item.ArrayChildren.ElementAt(0);
            Assert.AreEqual(null, arrayItem1.ArrayChildren);
            Assert.AreEqual(0, arrayItem1.Id);
            Assert.AreEqual(typeof(KeyValuePair<string, double>), arrayItem1.Type);
            Assert.AreEqual(new KeyValuePair<string, double>("Test", 2), arrayItem1.Value);

            Assert.AreEqual(2, arrayItem1.ObjectChildren.Count());

            PropertyItem arrayItem1Child1 = arrayItem1.ObjectChildren.ElementAt(0);
            Assert.AreEqual(null, arrayItem1Child1.ObjectChildren);
            Assert.AreEqual(null, arrayItem1Child1.ArrayChildren);
            Assert.AreEqual("Key", arrayItem1Child1.Id);
            Assert.AreEqual(typeof(string), arrayItem1Child1.Type);
            Assert.AreEqual("Test", arrayItem1Child1.Value);

            PropertyItem arrayItem1Child2 = arrayItem1.ObjectChildren.ElementAt(1);
            Assert.AreEqual(null, arrayItem1Child2.ObjectChildren);
            Assert.AreEqual(null, arrayItem1Child2.ArrayChildren);
            Assert.AreEqual("Value", arrayItem1Child2.Id);
            Assert.AreEqual(typeof(double), arrayItem1Child2.Type);
            Assert.AreEqual((double)2, arrayItem1Child2.Value);
            
            Assert.AreEqual(4, item.ObjectChildren.Count());

            PropertyItem childItem1 = item.ObjectChildren.ElementAt(0);
            Assert.AreEqual(0, childItem1.ObjectChildren.Count());
            Assert.AreEqual(null, childItem1.ArrayChildren);
            Assert.AreEqual("Comparer", childItem1.Id);
            Assert.AreEqual(typeof(IEqualityComparer<string>), childItem1.Type);
            Assert.AreEqual(propertyValue.Comparer, childItem1.Value);

            PropertyItem childItem2 = item.ObjectChildren.ElementAt(1);
            Assert.AreEqual(null, childItem2.ObjectChildren);
            Assert.AreEqual(null, childItem2.ArrayChildren);
            Assert.AreEqual("Count", childItem2.Id);
            Assert.AreEqual(typeof(int), childItem2.Type);
            Assert.AreEqual(1, childItem2.Value);

            PropertyItem childItem3 = item.ObjectChildren.ElementAt(2);
            Assert.AreEqual("Keys", childItem3.Id);
            Assert.AreEqual(typeof(Dictionary<string, double>.KeyCollection), childItem3.Type);
            Assert.AreEqual(propertyValue.Keys, childItem3.Value);

            Assert.AreEqual(1, childItem3.ArrayChildren.Count());

            PropertyItem arrayItemChildItem3 = childItem3.ObjectChildren.ElementAt(0);
            Assert.AreEqual(null, arrayItemChildItem3.ObjectChildren);
            Assert.AreEqual(null, arrayItemChildItem3.ArrayChildren);
            Assert.AreEqual("Count", arrayItemChildItem3.Id);
            Assert.AreEqual(typeof(int), arrayItemChildItem3.Type);
            Assert.AreEqual(1, arrayItemChildItem3.Value);

            Assert.AreEqual(1, childItem3.ObjectChildren.Count());

            PropertyItem childChildItem3 = childItem3.ObjectChildren.ElementAt(0);
            Assert.AreEqual(null, childChildItem3.ObjectChildren);
            Assert.AreEqual(null, childChildItem3.ArrayChildren);
            Assert.AreEqual("Count", childChildItem3.Id);
            Assert.AreEqual(typeof(int), childChildItem3.Type);
            Assert.AreEqual(1, childChildItem3.Value);

            PropertyItem childItem4 = item.ObjectChildren.ElementAt(3);
            Assert.AreEqual("Values", childItem4.Id);
            Assert.AreEqual(typeof(Dictionary<string, double>.ValueCollection), childItem4.Type);
            Assert.AreEqual(propertyValue.Values, childItem4.Value);

            Assert.AreEqual(1, childItem4.ArrayChildren.Count());
            
            PropertyItem arrayItemChildItem4 = childItem4.ObjectChildren.ElementAt(0);
            Assert.AreEqual(null, arrayItemChildItem4.ObjectChildren);
            Assert.AreEqual(null, arrayItemChildItem4.ArrayChildren);
            Assert.AreEqual("Count", arrayItemChildItem4.Id);
            Assert.AreEqual(typeof(int), arrayItemChildItem4.Type);
            Assert.AreEqual(1, arrayItemChildItem4.Value);

            Assert.AreEqual(1, childItem4.ObjectChildren.Count());

            PropertyItem childChildItem4 = childItem4.ObjectChildren.ElementAt(0);
            Assert.AreEqual(null, childChildItem4.ObjectChildren);
            Assert.AreEqual(null, childChildItem4.ArrayChildren);
            Assert.AreEqual("Count", childChildItem4.Id);
            Assert.AreEqual(typeof(int), childChildItem4.Type);
            Assert.AreEqual(1, childChildItem4.Value);
        }

        [TestMethod]
        public void IntArrayIsNull() {
            string propertyName = "Int32[]";
            Type propertyType = typeof(int[]);
            int[] propertyValue = null;

            PropertyItem item = CreateItem(propertyName, propertyType, propertyValue);
            Assert.AreEqual(null, item.ObjectChildren);
            Assert.AreEqual(null, item.ArrayChildren);
            Assert.AreEqual(propertyName, item.Id);
            Assert.AreEqual(propertyType, item.Type);
            Assert.AreEqual(propertyValue, item.Value);
        }

        [TestMethod]
        public void IntArrayIsEmpty() {
            string propertyName = "Int32[]";
            Type propertyType = typeof(int[]);
            int[] propertyValue = new int[] { };

            PropertyItem item = CreateItem(propertyName, propertyType, propertyValue);
            Assert.AreEqual(null, item.ObjectChildren);
            Assert.AreEqual(0, item.ArrayChildren.Count());
            Assert.AreEqual(propertyName, item.Id);
            Assert.AreEqual(propertyType, item.Type);
            Assert.AreEqual(propertyValue, item.Value);
        }

        [TestMethod]
        public void IntArrayIsNotNull() {
            string propertyName = "Int32[]";
            Type propertyType = typeof(int[]);
            int[] propertyValue = new int[] { 3, 4 };

            PropertyItem item = CreateItem(propertyName, propertyType, propertyValue);
            Assert.AreEqual(null, item.ObjectChildren);
            Assert.AreEqual(propertyName, item.Id);
            Assert.AreEqual(propertyType, item.Type);
            Assert.AreEqual(propertyValue, item.Value);

            Assert.AreEqual(2, item.ArrayChildren.Count());

            PropertyItem arrayItem1 = item.ArrayChildren.ElementAt(0);
            Assert.AreEqual(null, arrayItem1.ObjectChildren);
            Assert.AreEqual(null, arrayItem1.ArrayChildren);
            Assert.AreEqual(0, arrayItem1.Id);
            Assert.AreEqual(typeof(int), arrayItem1.Type);
            Assert.AreEqual(3, arrayItem1.Value);

            PropertyItem arrayItem2 = item.ArrayChildren.ElementAt(1);
            Assert.AreEqual(null, arrayItem2.ObjectChildren);
            Assert.AreEqual(null, arrayItem2.ArrayChildren);
            Assert.AreEqual(1, arrayItem2.Id);
            Assert.AreEqual(typeof(int), arrayItem2.Type);
            Assert.AreEqual(4, arrayItem2.Value);
        }
    }
}
