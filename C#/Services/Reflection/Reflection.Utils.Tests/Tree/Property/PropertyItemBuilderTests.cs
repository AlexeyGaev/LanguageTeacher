using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Reflection.Utils.PropertyTree.Tests {
    [TestClass]
    public class PropertyItemBuilderTests {
        PropertyItem CreateItem(IEnumerable<PropertyItem> parents, string propertyName, Type propertyType, object propertyValue) {
            return PropertyItemBuilder.Create(parents, new PropertyField(propertyName, propertyType), propertyValue);
        }

        void CheckField(object id, Type type, PropertyField field) {
            Assert.AreEqual(id, field.Id);
            Assert.AreEqual(type, field.Type);
        }
                       
        [TestMethod]
        public void PropertyNameIsNull() {
            PropertyItem item = CreateItem(null, null, null, null);

            Assert.AreEqual(null, item.ObjectChildren);
            Assert.AreEqual(null, item.ArrayChildren);
            CheckField(null, null, item.Field);
            Assert.AreEqual(null, item.Value);
        }

        [TestMethod]
        public void PropertyTypeIsNull() {
            string propertyName = "Test";
            PropertyItem item = CreateItem(null, propertyName, null, null);

            Assert.AreEqual(null, item.ObjectChildren);
            Assert.AreEqual(null, item.ArrayChildren);
            CheckField(propertyName, null, item.Field);
            Assert.AreEqual(null, item.Value);
        }

        [TestMethod]
        public void ObjectValue() {
            string propertyName = "Object";
            Type propertyType = typeof(object);
            object propertyValue = new object();

            PropertyItem item = CreateItem(null, propertyName, propertyType, propertyValue);
            Assert.AreEqual(0, item.ObjectChildren.Count());
            Assert.AreEqual(null, item.ArrayChildren);
            CheckField(propertyName, propertyType, item.Field);
            Assert.AreEqual(propertyValue, item.Value);
        }

        [TestMethod]
        public void DecimalValue() {
            string propertyName = "Decimal";
            Type propertyType = typeof(decimal);
            decimal propertyValue = 1;
            
            PropertyItem item = CreateItem(null, propertyName, propertyType, propertyValue);
            Assert.AreEqual(0, item.ObjectChildren.Count());
            Assert.AreEqual(null, item.ArrayChildren);
            CheckField(propertyName, propertyType, item.Field);
            Assert.AreEqual(propertyValue, item.Value);
        }

        [TestMethod]
        public void NullableDecimalValue_Null() {
            string propertyName = "NullableDecimal";
            Type propertyType = typeof(decimal?);
            decimal? propertyValue = null;

            PropertyItem item = CreateItem(null, propertyName, propertyType, propertyValue);
            Assert.AreEqual(null, item.ObjectChildren);
            Assert.AreEqual(null, item.ArrayChildren);
            CheckField(propertyName, propertyType, item.Field);
            Assert.AreEqual(propertyValue, item.Value);
        }

        [TestMethod]
        public void IntValue() {
            string propertyName = "Int";
            Type propertyType = typeof(int);
            int propertyValue = 1;
            
            PropertyItem item = CreateItem(null, propertyName, propertyType, propertyValue);
            Assert.AreEqual(null, item.ObjectChildren);
            Assert.AreEqual(null, item.ArrayChildren);
            CheckField(propertyName, propertyType, item.Field);
            Assert.AreEqual(propertyValue, item.Value);
        }

        [TestMethod]
        public void NullableIntValue_Null() {
            string propertyName = "Nullable Int";
            Type propertyType = typeof(int?);
            int? propertyValue = null;

            PropertyItem item = CreateItem(null, propertyName, propertyType, propertyValue);
            Assert.AreEqual(null, item.ObjectChildren);
            Assert.AreEqual(null, item.ArrayChildren);
            CheckField(propertyName, propertyType, item.Field);
            Assert.AreEqual(propertyValue, item.Value);
        }

        [TestMethod]
        public void NullableIntValue_One() {
            string propertyName = "Nullable Int";
            Type propertyType = typeof(int?);
            int? propertyValue = 1;

            PropertyItem item = CreateItem(null, propertyName, propertyType, propertyValue);
            Assert.AreEqual(null, item.ObjectChildren);
            Assert.AreEqual(null, item.ArrayChildren);
            CheckField(propertyName, propertyType, item.Field);
            Assert.AreEqual(propertyValue, item.Value);
        }

        enum TestEnum {
            Value1,
            Value2,
        }

        [TestMethod]
        public void TestEnumValue_Value1() {
            string propertyName = "TestEnum";
            Type propertyType = typeof(TestEnum);
            TestEnum propertyValue = TestEnum.Value1;

            PropertyItem item = CreateItem(null, propertyName, propertyType, propertyValue);
            Assert.AreEqual(null, item.ObjectChildren);
            Assert.AreEqual(null, item.ArrayChildren);
            CheckField(propertyName, propertyType, item.Field);
            Assert.AreEqual(propertyValue, item.Value);
        }

        [TestMethod]
        public void NullableEnumValue_Null() {
            string propertyName = "Nullable TestEnum";
            Type propertyType = typeof(TestEnum?);
            TestEnum? propertyValue = null;

            PropertyItem item = CreateItem(null, propertyName, propertyType, propertyValue);
            Assert.AreEqual(null, item.ObjectChildren);
            Assert.AreEqual(null, item.ArrayChildren);
            CheckField(propertyName, propertyType, item.Field);
            Assert.AreEqual(propertyValue, item.Value);
        }

        [TestMethod]
        public void NullableEnumValue_Value1() {
            string propertyName = "Nullable TestEnum";
            Type propertyType = typeof(TestEnum?);
            TestEnum? propertyValue = TestEnum.Value1;

            PropertyItem item = CreateItem(null, propertyName, propertyType, propertyValue);
            Assert.AreEqual(null, item.ObjectChildren);
            Assert.AreEqual(null, item.ArrayChildren);
            CheckField(propertyName, propertyType, item.Field);
            Assert.AreEqual(propertyValue, item.Value);
        }

        struct EmptyTestStruct { }

        [TestMethod]
        public void EmptyTestStructValue() {
            string propertyName = "EmptyTestStruct";
            Type propertyType = typeof(EmptyTestStruct);
            EmptyTestStruct propertyValue = new EmptyTestStruct();

            PropertyItem item = CreateItem(null, propertyName, propertyType, propertyValue);
            Assert.AreEqual(0, item.ObjectChildren.Count());
            Assert.AreEqual(null, item.ArrayChildren);
            CheckField(propertyName, propertyType, item.Field);
            Assert.AreEqual(propertyValue, item.Value);
        }

        [TestMethod]
        public void NullableEmptyTestStructValue_Null() {
            string propertyName = "Nullable EmptyTestStruct";
            Type propertyType = typeof(EmptyTestStruct?);
            EmptyTestStruct? propertyValue = null;

            PropertyItem item = CreateItem(null, propertyName, propertyType, propertyValue);
            Assert.AreEqual(null, item.ObjectChildren);
            Assert.AreEqual(null, item.ArrayChildren);
            CheckField(propertyName, propertyType, item.Field);
            Assert.AreEqual(propertyValue, item.Value);
        }

        [TestMethod]
        public void NullableEmptyTestStructValue_NotNull() {
            string propertyName = "Nullable EmptyTestStruct";
            Type propertyType = typeof(EmptyTestStruct?);
            EmptyTestStruct? propertyValue = new EmptyTestStruct();

            PropertyItem item = CreateItem(null, propertyName, propertyType, propertyValue);
            Assert.AreEqual(0, item.ObjectChildren.Count());
            Assert.AreEqual(null, item.ArrayChildren);
            CheckField(propertyName, propertyType, item.Field);
            Assert.AreEqual(propertyValue, item.Value);
        }

        struct TestStruct {
            object Property0 { get; set; }

            public int Property1 { get; set; }
            public TestEnum Property2 { get; set; }
            public object Property3 { get; set; }
        }

        [TestMethod]
        public void TestStructValue() {
            string propertyName = "TestStruct";
            Type propertyType = typeof(TestStruct);
            TestStruct propertyValue = new TestStruct();

            PropertyItem item = CreateItem(null, propertyName, propertyType, propertyValue);
            CheckTestStruct(item, propertyName, propertyType, propertyValue);
        }

        void CheckTestStruct(PropertyItem item, string propertyName, Type propertyType, object propertyValue) {
            Assert.AreEqual(null, item.ArrayChildren);
            CheckField(propertyName, propertyType, item.Field);
            Assert.AreEqual(propertyValue, item.Value);
                                    
            Assert.AreEqual(3, item.ObjectChildren.Count());

            PropertyItem childItem1 = item.ObjectChildren.ElementAt(0);
            Assert.AreEqual(null, childItem1.ObjectChildren);
            CheckField("Property1", typeof(int), childItem1.Field);
            Assert.AreEqual(0, childItem1.Value);

            PropertyItem childItem2 = item.ObjectChildren.ElementAt(1);
            Assert.AreEqual(null, childItem2.ObjectChildren);
            CheckField("Property2", typeof(TestEnum), childItem2.Field);
            Assert.AreEqual(TestEnum.Value1, childItem2.Value);

            PropertyItem childItem3 = item.ObjectChildren.ElementAt(2);
            Assert.AreEqual(null, childItem3.ObjectChildren);
            CheckField("Property3", typeof(object), childItem3.Field);
            Assert.AreEqual(null, childItem3.Value);
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
            string propertyName = "TestClass";
            Type propertyType = typeof(TestClass);
            TestClass propertyValue = null;

            PropertyItem item = CreateItem(null, propertyName, propertyType, propertyValue);
            Assert.AreEqual(null, item.ObjectChildren);
            Assert.AreEqual(null, item.ArrayChildren);
            CheckField(propertyName, propertyType, item.Field);
            Assert.AreEqual(propertyValue, item.Value);
        }

        [TestMethod]
        public void TestClass_NotNull() {
            string propertyName = "TestClass";
            Type propertyType = typeof(TestClass);
            TestClass propertyValue = new TestClass();

            PropertyItem item = CreateItem(null, propertyName, propertyType, propertyValue);
            CheckField(propertyName, propertyType, item.Field);
            Assert.AreEqual(null, item.ArrayChildren);
            Assert.AreEqual(propertyValue, item.Value);
                        
            Assert.AreEqual(4, item.ObjectChildren.Count());

            PropertyItem childItem1 = item.ObjectChildren.ElementAt(0);
            Assert.AreEqual(null, childItem1.ObjectChildren);
            CheckField("Property1", typeof(int), childItem1.Field);
            Assert.AreEqual(0, childItem1.Value);

            PropertyItem childItem2 = item.ObjectChildren.ElementAt(1);
            Assert.AreEqual(null, childItem2.ObjectChildren);
            CheckField("Property2", typeof(TestEnum), childItem2.Field);
            Assert.AreEqual(TestEnum.Value1, childItem2.Value);

            PropertyItem childItem3 = item.ObjectChildren.ElementAt(2);
            Assert.AreEqual(null, childItem3.ObjectChildren);
            CheckField("Property3", typeof(object), childItem3.Field);
            Assert.AreEqual(null, childItem3.Value);

            PropertyItem childItem4 = item.ObjectChildren.ElementAt(3);
            CheckTestStruct(childItem4, "Property4", typeof(TestStruct), default(TestStruct));
        }

        class TestClass2 {
            public TestClass2 Property { get; set; }
        }

        [TestMethod]
        public void TestClass2IsNull() {
            string propertyName = "TestClass2";
            Type propertyType = typeof(TestClass2);
            TestClass2 propertyValue = null;

            PropertyItem item = CreateItem(null, propertyName, propertyType, propertyValue);
            Assert.AreEqual(null, item.ObjectChildren);
            Assert.AreEqual(null, item.ArrayChildren);
            CheckField(propertyName, propertyType, item.Field);
            Assert.AreEqual(propertyValue, item.Value);
        }

        [TestMethod]
        public void TestClass2IsNotNull_PropertyIsNull() {
            string propertyName = "TestClass2";
            Type propertyType = typeof(TestClass2);
            TestClass2 propertyValue = new TestClass2();

            PropertyItem item = CreateItem(null, propertyName, propertyType, propertyValue);
            CheckField(propertyName, propertyType, item.Field);
            Assert.AreEqual(null, item.ArrayChildren);
            Assert.AreEqual(propertyValue, item.Value);

            Assert.AreEqual(1, item.ObjectChildren.Count());

            PropertyItem childItem1 = item.ObjectChildren.ElementAt(0);
            Assert.AreEqual(null, childItem1.ObjectChildren);
            CheckField("Property", typeof(TestClass2), childItem1.Field);
            Assert.AreEqual(null, childItem1.Value);
        }

        [TestMethod]
        public void TestClass2IsNotNull_PropertyIsNotNull() {
            string propertyName = "TestClass2";
            Type propertyType = typeof(TestClass2);
            TestClass2 propertyValue = new TestClass2();
            TestClass2 childPropertyValue = new TestClass2();
            propertyValue.Property = childPropertyValue;

            PropertyItem item = CreateItem(null, propertyName, propertyType, propertyValue);
            CheckField(propertyName, propertyType, item.Field);
            Assert.AreEqual(null, item.ArrayChildren);
            Assert.AreEqual(propertyValue, item.Value);

            Assert.AreEqual(1, item.ObjectChildren.Count());

            PropertyItem childItem1 = item.ObjectChildren.ElementAt(0);
            CheckField("Property", typeof(TestClass2), childItem1.Field);
            Assert.AreEqual(childPropertyValue, childItem1.Value);
                      
            Assert.AreEqual(1, childItem1.ObjectChildren.Count());

            PropertyItem childChildItem1 = childItem1.ObjectChildren.ElementAt(0);
            Assert.AreEqual(null, childChildItem1.ObjectChildren);
            CheckField("Property", typeof(TestClass2), childChildItem1.Field);
            Assert.AreEqual(null, childChildItem1.Value);
        }

        [TestMethod]
        public void TestClass2IsNotNull_HasChildrenCycle() {
            string propertyName = "TestClass2";
            Type propertyType = typeof(TestClass2);
            TestClass2 propertyValue = new TestClass2();
            propertyValue.Property = propertyValue;

            PropertyItem item = CreateItem(null, propertyName, propertyType, propertyValue);
            CheckField(propertyName, propertyType, item.Field);
            Assert.AreEqual(null, item.ArrayChildren);
            Assert.AreEqual(propertyValue, item.Value);
            
            Assert.AreEqual(1, item.ObjectChildren.Count());

            PropertyItem childItem1 = item.ObjectChildren.ElementAt(0);
            Assert.AreEqual(PropertyObjectChildren.Cycle, childItem1.ObjectChildren);
            CheckField("Property", typeof(TestClass2), childItem1.Field);
            Assert.AreEqual(propertyValue, childItem1.Value);
        }

        [TestMethod]
        public void StringIsNull() {
            string propertyName = "String";
            Type propertyType = typeof(string);
            string propertyValue = null;

            PropertyItem item = CreateItem(null, propertyName, propertyType, propertyValue);
            Assert.AreEqual(null, item.ObjectChildren);
            Assert.AreEqual(null, item.ArrayChildren);
            CheckField(propertyName, propertyType, item.Field);
            Assert.AreEqual(propertyValue, item.Value);
        }

        [TestMethod]
        public void StringIsEmpty() {
            string propertyName = "String";
            Type propertyType = typeof(string);
            string propertyValue = String.Empty;

            PropertyItem item = CreateItem(null, propertyName, propertyType, propertyValue);
            Assert.AreEqual(null, item.ObjectChildren);
            Assert.AreEqual(null, item.ArrayChildren);
            CheckField(propertyName, propertyType, item.Field);
            Assert.AreEqual(propertyValue, item.Value);
        }

        [TestMethod]
        public void StringIsNotEmpty() {
            string propertyName = "String";
            Type propertyType = typeof(string);
            string propertyValue = "testString";

            PropertyItem item = CreateItem(null, propertyName, propertyType, propertyValue);
            Assert.AreEqual(null, item.ObjectChildren);
            Assert.AreEqual(null, item.ArrayChildren);
            CheckField(propertyName, propertyType, item.Field);
            Assert.AreEqual(propertyValue, item.Value);
        }

        [TestMethod]
        public void ListIntIsNull() {
            string propertyName = "List<int>";
            Type propertyType = typeof(List<int>);
            List<int> propertyValue = null;

            PropertyItem item = CreateItem(null, propertyName, propertyType, propertyValue);
            Assert.AreEqual(null, item.ObjectChildren);
            Assert.AreEqual(null, item.ArrayChildren);
            CheckField(propertyName, propertyType, item.Field);
            Assert.AreEqual(propertyValue, item.Value);
        }

        [TestMethod]
        public void ListIntIsEmpty() {
            string propertyName = "List<double>";
            Type propertyType = typeof(List<double>);
            List<double> propertyValue = new List<double>();

            PropertyItem item = CreateItem(null, propertyName, propertyType, propertyValue);
            CheckField(propertyName, propertyType, item.Field);
            Assert.AreEqual(0, item.ArrayChildren.Count());
            Assert.AreEqual(propertyValue, item.Value);

            Assert.AreEqual(2, item.ObjectChildren.Count());

            PropertyItem childItem1 = item.ObjectChildren.ElementAt(0);
            Assert.AreEqual(null, childItem1.ObjectChildren);
            Assert.AreEqual(null, childItem1.ArrayChildren);
            CheckField("Capacity", typeof(int), childItem1.Field);
            Assert.AreEqual(0, childItem1.Value);

            PropertyItem childItem2 = item.ObjectChildren.ElementAt(1);
            Assert.AreEqual(null, childItem2.ObjectChildren);
            Assert.AreEqual(null, childItem2.ArrayChildren);
            CheckField("Count", typeof(int), childItem2.Field);
            Assert.AreEqual(0, childItem2.Value);
        }

        [TestMethod]
        public void ListIntIsNotEmpty() {
            string propertyName = "List<double>";
            Type propertyType = typeof(List<double>);
            List<double> propertyValue = new List<double>();
            propertyValue.Add(5);

            PropertyItem item = CreateItem(null, propertyName, propertyType, propertyValue);
            CheckField(propertyName, propertyType, item.Field);
            Assert.AreEqual(propertyValue, item.Value);
            
            Assert.AreEqual(2, item.ObjectChildren.Count());

            PropertyItem childItem1 = item.ObjectChildren.ElementAt(0);
            Assert.AreEqual(null, childItem1.ObjectChildren);
            Assert.AreEqual(null, childItem1.ArrayChildren);
            CheckField("Capacity", typeof(int), childItem1.Field);
            Assert.AreEqual(4, childItem1.Value);

            PropertyItem childItem2 = item.ObjectChildren.ElementAt(1);
            Assert.AreEqual(null, childItem2.ObjectChildren);
            Assert.AreEqual(null, childItem2.ArrayChildren);
            CheckField("Count", typeof(int), childItem2.Field);
            Assert.AreEqual(1, childItem2.Value);

            Assert.AreEqual(1, item.ArrayChildren.Count());

            PropertyItem arrayItem1 = item.ArrayChildren.ElementAt(0);
            Assert.AreEqual(null, arrayItem1.ObjectChildren);
            Assert.AreEqual(null, arrayItem1.ArrayChildren);
            CheckField(0, typeof(double), arrayItem1.Field);
            Assert.AreEqual((double)5, arrayItem1.Value);
        }

        class TestList : List<char> {
        }

        [TestMethod]
        public void TestListIsNotEmpty() {
            string propertyName = "TestList";
            Type propertyType = typeof(TestList);
            TestList propertyValue = new TestList();
            propertyValue.Add('5');

            PropertyItem item = CreateItem(null, propertyName, propertyType, propertyValue);
            CheckField(propertyName, propertyType, item.Field);
            Assert.AreEqual(propertyValue, item.Value);

            Assert.AreEqual(2, item.ObjectChildren.Count());

            PropertyItem childItem1 = item.ObjectChildren.ElementAt(0);
            Assert.AreEqual(null, childItem1.ObjectChildren);
            Assert.AreEqual(null, childItem1.ArrayChildren);
            CheckField("Capacity", typeof(int), childItem1.Field);
            Assert.AreEqual(4, childItem1.Value);

            PropertyItem childItem2 = item.ObjectChildren.ElementAt(1);
            Assert.AreEqual(null, childItem2.ObjectChildren);
            Assert.AreEqual(null, childItem2.ArrayChildren);
            CheckField("Count", typeof(int), childItem2.Field);
            Assert.AreEqual(1, childItem2.Value);

            Assert.AreEqual(1, item.ArrayChildren.Count());

            PropertyItem arrayItem1 = item.ArrayChildren.ElementAt(0);
            Assert.AreEqual(null, arrayItem1.ObjectChildren);
            Assert.AreEqual(null, arrayItem1.ArrayChildren);
            CheckField(0, typeof(char), arrayItem1.Field);
            Assert.AreEqual('5', arrayItem1.Value);
        }

        [TestMethod]
        public void DictionaryIsNull() {
            string propertyName = "Dictionary<string, double>";
            Type propertyType = typeof(Dictionary<string, double>);
            Dictionary<string, double> propertyValue = null;

            PropertyItem item = CreateItem(null, propertyName, propertyType, propertyValue);
            Assert.AreEqual(null, item.ObjectChildren);
            Assert.AreEqual(null, item.ArrayChildren);
            CheckField(propertyName, propertyType, item.Field);
            Assert.AreEqual(propertyValue, item.Value);
        }

        [TestMethod]
        public void DictionaryIsEmpty() {
            string propertyName = "Dictionary<string, double>";
            Type propertyType = typeof(Dictionary<string, double>);
            Dictionary<string, double> propertyValue = new Dictionary<string, double>();

            PropertyItem item = CreateItem(null, propertyName, propertyType, propertyValue);
            Assert.AreEqual(0, item.ArrayChildren.Count());
            CheckField(propertyName, propertyType, item.Field);
            Assert.AreEqual(propertyValue, item.Value);

            Assert.AreEqual(4, item.ObjectChildren.Count());

            PropertyItem childItem1 = item.ObjectChildren.ElementAt(0);
            Assert.AreEqual(0, childItem1.ObjectChildren.Count());
            Assert.AreEqual(null, childItem1.ArrayChildren);
            CheckField("Comparer", typeof(IEqualityComparer<string>), childItem1.Field);
            Assert.AreEqual(propertyValue.Comparer, childItem1.Value);

            PropertyItem childItem2 = item.ObjectChildren.ElementAt(1);
            Assert.AreEqual(null, childItem2.ObjectChildren);
            Assert.AreEqual(null, childItem2.ArrayChildren);
            CheckField("Count", typeof(int), childItem2.Field);
            Assert.AreEqual(0, childItem2.Value);

            PropertyItem childItem3 = item.ObjectChildren.ElementAt(2);
            Assert.AreEqual(0, childItem3.ArrayChildren.Count());
            CheckField("Keys", typeof(Dictionary<string, double>.KeyCollection), childItem3.Field);
            Assert.AreEqual(propertyValue.Keys, childItem3.Value);
                      
            Assert.AreEqual(1, childItem3.ObjectChildren.Count());

            PropertyItem childChildItem3 = childItem3.ObjectChildren.ElementAt(0);
            Assert.AreEqual(null, childChildItem3.ObjectChildren);
            Assert.AreEqual(null, childChildItem3.ArrayChildren);
            CheckField("Count", typeof(int), childChildItem3.Field);
            Assert.AreEqual(0, childChildItem3.Value);

            PropertyItem childItem4 = item.ObjectChildren.ElementAt(3);
            Assert.AreEqual(0, childItem4.ArrayChildren.Count());
            CheckField("Values", typeof(Dictionary<string, double>.ValueCollection), childItem4.Field);
            Assert.AreEqual(propertyValue.Values, childItem4.Value);
                       
            Assert.AreEqual(1, childItem4.ObjectChildren.Count());

            PropertyItem childChildItem4 = childItem4.ObjectChildren.ElementAt(0);
            Assert.AreEqual(null, childChildItem4.ArrayChildren);
            Assert.AreEqual(null, childChildItem4.ObjectChildren);
            CheckField("Count", typeof(int), childChildItem4.Field);
            Assert.AreEqual(0, childChildItem4.Value);
        }

        [TestMethod]
        public void DictionaryIsNotEmpty() {
            string propertyName = "Dictionary<string, double>";
            Type propertyType = typeof(Dictionary<string, double>);
            Dictionary<string, double> propertyValue = new Dictionary<string, double>();
            propertyValue.Add("Test", 2);

            PropertyItem item = CreateItem(null, propertyName, propertyType, propertyValue);
            CheckField(propertyName, propertyType, item.Field);
            Assert.AreEqual(propertyValue, item.Value);

            Assert.AreEqual(1, item.ArrayChildren.Count());
            PropertyItem arrayItem1 = item.ArrayChildren.ElementAt(0);
            Assert.AreEqual(null, arrayItem1.ArrayChildren);
            CheckField(0, typeof(KeyValuePair<string, double>), arrayItem1.Field);
            Assert.AreEqual(new KeyValuePair<string, double>("Test", 2), arrayItem1.Value);

            Assert.AreEqual(2, arrayItem1.ObjectChildren.Count());

            PropertyItem arrayItem1Child1 = arrayItem1.ObjectChildren.ElementAt(0);
            Assert.AreEqual(null, arrayItem1Child1.ObjectChildren);
            Assert.AreEqual(null, arrayItem1Child1.ArrayChildren);
            CheckField("Key", typeof(string), arrayItem1Child1.Field);
            Assert.AreEqual("Test", arrayItem1Child1.Value);

            PropertyItem arrayItem1Child2 = arrayItem1.ObjectChildren.ElementAt(1);
            Assert.AreEqual(null, arrayItem1Child2.ObjectChildren);
            Assert.AreEqual(null, arrayItem1Child2.ArrayChildren);
            CheckField("Value", typeof(double), arrayItem1Child2.Field);
            Assert.AreEqual((double)2, arrayItem1Child2.Value);
            
            Assert.AreEqual(4, item.ObjectChildren.Count());

            PropertyItem childItem1 = item.ObjectChildren.ElementAt(0);
            Assert.AreEqual(0, childItem1.ObjectChildren.Count());
            Assert.AreEqual(null, childItem1.ArrayChildren);
            CheckField("Comparer", typeof(IEqualityComparer<string>), childItem1.Field);
            Assert.AreEqual(propertyValue.Comparer, childItem1.Value);

            PropertyItem childItem2 = item.ObjectChildren.ElementAt(1);
            Assert.AreEqual(null, childItem2.ObjectChildren);
            Assert.AreEqual(null, childItem2.ArrayChildren);
            CheckField("Count", typeof(int), childItem2.Field);
            Assert.AreEqual(1, childItem2.Value);

            PropertyItem childItem3 = item.ObjectChildren.ElementAt(2);
            CheckField("Keys", typeof(Dictionary<string, double>.KeyCollection), childItem3.Field);
            Assert.AreEqual(propertyValue.Keys, childItem3.Value);

            Assert.AreEqual(1, childItem3.ArrayChildren.Count());

            PropertyItem arrayItemChildItem3 = childItem3.ObjectChildren.ElementAt(0);
            Assert.AreEqual(null, arrayItemChildItem3.ObjectChildren);
            Assert.AreEqual(null, arrayItemChildItem3.ArrayChildren);
            CheckField("Count", typeof(int), arrayItemChildItem3.Field);
            Assert.AreEqual(1, arrayItemChildItem3.Value);

            Assert.AreEqual(1, childItem3.ObjectChildren.Count());

            PropertyItem childChildItem3 = childItem3.ObjectChildren.ElementAt(0);
            Assert.AreEqual(null, childChildItem3.ObjectChildren);
            Assert.AreEqual(null, childChildItem3.ArrayChildren);
            CheckField("Count", typeof(int), childChildItem3.Field);
            Assert.AreEqual(1, childChildItem3.Value);

            PropertyItem childItem4 = item.ObjectChildren.ElementAt(3);
            CheckField("Values", typeof(Dictionary<string, double>.ValueCollection), childItem4.Field);
            Assert.AreEqual(propertyValue.Values, childItem4.Value);

            Assert.AreEqual(1, childItem4.ArrayChildren.Count());
            
            PropertyItem arrayItemChildItem4 = childItem4.ObjectChildren.ElementAt(0);
            Assert.AreEqual(null, arrayItemChildItem4.ObjectChildren);
            Assert.AreEqual(null, arrayItemChildItem4.ArrayChildren);
            CheckField("Count", typeof(int), arrayItemChildItem4.Field);
            Assert.AreEqual(1, arrayItemChildItem4.Value);

            Assert.AreEqual(1, childItem4.ObjectChildren.Count());

            PropertyItem childChildItem4 = childItem4.ObjectChildren.ElementAt(0);
            Assert.AreEqual(null, childChildItem4.ObjectChildren);
            Assert.AreEqual(null, childChildItem4.ArrayChildren);
            CheckField("Count", typeof(int), childChildItem4.Field);
            Assert.AreEqual(1, childChildItem4.Value);
        }

        [TestMethod]
        public void IntArrayIsNull() {
            string propertyName = "Int32[]";
            Type propertyType = typeof(int[]);
            int[] propertyValue = null;

            PropertyItem item = CreateItem(null, propertyName, propertyType, propertyValue);
            Assert.AreEqual(null, item.ObjectChildren);
            Assert.AreEqual(null, item.ArrayChildren);
            CheckField(propertyName, propertyType, item.Field);
            Assert.AreEqual(propertyValue, item.Value);
        }

        [TestMethod]
        public void IntArrayIsEmpty() {
            string propertyName = "Int32[]";
            Type propertyType = typeof(int[]);
            int[] propertyValue = new int[] { };

            PropertyItem item = CreateItem(null, propertyName, propertyType, propertyValue);
            Assert.AreEqual(null, item.ObjectChildren);
            Assert.AreEqual(0, item.ArrayChildren.Count());
            CheckField(propertyName, propertyType, item.Field);
            Assert.AreEqual(propertyValue, item.Value);
        }

        [TestMethod]
        public void IntArrayIsNotNull() {
            string propertyName = "Int32[]";
            Type propertyType = typeof(int[]);
            int[] propertyValue = new int[] { 3, 4 };

            PropertyItem item = CreateItem(null, propertyName, propertyType, propertyValue);
            Assert.AreEqual(null, item.ObjectChildren);
            CheckField(propertyName, propertyType, item.Field);
            Assert.AreEqual(propertyValue, item.Value);

            Assert.AreEqual(2, item.ArrayChildren.Count());

            PropertyItem arrayItem1 = item.ArrayChildren.ElementAt(0);
            Assert.AreEqual(null, arrayItem1.ObjectChildren);
            Assert.AreEqual(null, arrayItem1.ArrayChildren);
            CheckField(0, typeof(int), arrayItem1.Field);
            Assert.AreEqual(3, arrayItem1.Value);

            PropertyItem arrayItem2 = item.ArrayChildren.ElementAt(1);
            Assert.AreEqual(null, arrayItem2.ObjectChildren);
            Assert.AreEqual(null, arrayItem2.ArrayChildren);
            CheckField(1, typeof(int), arrayItem2.Field);
            Assert.AreEqual(4, arrayItem2.Value);

        }
    }
}
