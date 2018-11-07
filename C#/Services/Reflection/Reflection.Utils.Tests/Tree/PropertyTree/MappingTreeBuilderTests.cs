using Microsoft.VisualStudio.TestTools.UnitTesting;
using Reflection.Utils.Tree;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Reflection.Utils.PropertyTree.Tests {
    [TestClass]
    public class PropertyTreeBuilderTests {
        PropertyTreeItem CreateItem(IEnumerable<PropertyTreeItem> parents, string propertyName, Type propertyType, object propertyValue) {
            return PropertyTreeBuilder.CreateItem(parents, new PropertyField(propertyName, propertyType), propertyValue);
        }

        void CheckField(string name, Type type, PropertyField field) {
            Assert.AreEqual(name, field.Name);
            Assert.AreEqual(type, field.Type);
        }
        
        [TestMethod]
        public void PropertyNameIsNull() {
            PropertyTreeItem item = CreateItem(null, null, null, null);
                        
            Assert.IsNull(item.ObjectParents);
            Assert.IsNull(item.ObjectChildren);
            Assert.IsFalse(item.HasObjectCycle);
            CheckField(null, null, item.Field);
            Assert.AreEqual(null, item.Value);
        }

        [TestMethod]
        public void PropertyTypeIsNull() {
            string propertyName = "Test";
            PropertyTreeItem treeItem = CreateItem(null, propertyName, null, null);

            Assert.IsNull(treeItem.ObjectParents);
            Assert.IsNull(treeItem.ObjectChildren);
            Assert.IsFalse(treeItem.HasObjectCycle);
            Assert.IsFalse(treeItem.HasArrayChildren);

            CheckTreeItem(propertyName, null, null, treeItem);
        }

        [TestMethod]
        public void ObjectValue() {
            IEnumerable<TreeItem<Mapping>> parents = null;
            string propertyName = "Object";
            Type propertyType = typeof(object);
            object propertyValue = new object();

            TreeItem<Mapping> treeItem = CreateItem(parents, propertyName, propertyType, propertyValue);

            Assert.AreEqual(parents, treeItem.Parents);
            Assert.AreEqual(0, treeItem.Children.Count());
            Assert.IsFalse(treeItem.HasChildrenCycle);
           
            CheckMapping(propertyName, propertyType, propertyValue, treeItem.Value);
        }

        [TestMethod]
        public void DecimalValue() {
            IEnumerable<TreeItem<Mapping>> parents = null;
            string propertyName = "Decimal";
            Type propertyType = typeof(decimal);
            decimal propertyValue = 1;
            
            TreeItem<Mapping> treeItem = CreateItem(parents, propertyName, propertyType, propertyValue);
            
            Assert.AreEqual(parents, treeItem.Parents);
            Assert.AreEqual(0, treeItem.Children.Count());
            Assert.IsFalse(treeItem.HasChildrenCycle);

            CheckMapping(propertyName, propertyType, propertyValue, treeItem.Value);
        }

        [TestMethod]
        public void NullableDecimalValue_Null() {
            IEnumerable<TreeItem<Mapping>> parents = null;
            string propertyName = "NullableDecimal";
            Type propertyType = typeof(decimal?);
            decimal? propertyValue = null;

            TreeItem<Mapping> treeItem = CreateItem(parents, propertyName, propertyType, propertyValue);

            Assert.AreEqual(parents, treeItem.Parents);
            Assert.IsNull(treeItem.Children);
            Assert.IsFalse(treeItem.HasChildrenCycle);

            CheckMapping(propertyName, propertyType, propertyValue, treeItem.Value);
        }

        [TestMethod]
        public void IntValue() {
            IEnumerable<TreeItem<Mapping>> parents = null;
            string propertyName = "Int";
            Type propertyType = typeof(int);
            int propertyValue = 1;
            
            TreeItem<Mapping> treeItem = CreateItem(parents, propertyName, propertyType, propertyValue);

            Assert.AreEqual(parents, treeItem.Parents);
            Assert.AreEqual(0, treeItem.Children.Count());
            Assert.IsFalse(treeItem.HasChildrenCycle);
            Assert.IsNotNull(treeItem.Value);

            CheckMapping(propertyName, propertyType, propertyValue, treeItem.Value);
        }

        [TestMethod]
        public void NullableIntValue_Null() {
            IEnumerable<TreeItem<Mapping>> parents = null;
            string propertyName = "Nullable Int";
            Type propertyType = typeof(int?);
            int? propertyValue = null;

            TreeItem<Mapping> treeItem = CreateItem(parents, propertyName, propertyType, propertyValue);

            Assert.AreEqual(parents, treeItem.Parents);
            Assert.IsNull(treeItem.Children);
            Assert.IsFalse(treeItem.HasChildrenCycle);

            CheckMapping(propertyName, propertyType, propertyValue, treeItem.Value);
        }

        [TestMethod]
        public void NullableIntValue_One() {
            IEnumerable<TreeItem<Mapping>> parents = null;
            string propertyName = "Nullable Int";
            Type propertyType = typeof(int?);
            int? propertyValue = 1;

            TreeItem<Mapping> treeItem = CreateItem(parents, propertyName, propertyType, propertyValue);

            Assert.AreEqual(parents, treeItem.Parents);
            Assert.AreEqual(0, treeItem.Children.Count());
            Assert.IsFalse(treeItem.HasChildrenCycle);

            CheckMapping(propertyName, propertyType, propertyValue, treeItem.Value);
        }

        enum TestEnum {
            Value1,
            Value2,
        }

        [TestMethod]
        public void TestEnumValue_Value1() {
            IEnumerable<TreeItem<Mapping>> parents = null;
            string propertyName = "TestEnum";
            Type propertyType = typeof(TestEnum);
            TestEnum propertyValue = TestEnum.Value1;

            TreeItem<Mapping> treeItem = CreateItem(parents, propertyName, propertyType, propertyValue);

            Assert.AreEqual(parents, treeItem.Parents);
            Assert.IsFalse(treeItem.HasChildrenCycle);
            Assert.IsNotNull(treeItem.Value);

            CheckMapping(propertyName, propertyType, propertyValue, treeItem.Value);
        }

        [TestMethod]
        public void NullableEnumValue_Null() {
            IEnumerable<TreeItem<Mapping>> parents = null;
            string propertyName = "Nullable TestEnum";
            Type propertyType = typeof(TestEnum?);
            TestEnum? propertyValue = null;

            TreeItem<Mapping> treeItem = CreateItem(parents, propertyName, propertyType, propertyValue);

            Assert.AreEqual(parents, treeItem.Parents);
            Assert.IsNull(treeItem.Children);
            Assert.IsFalse(treeItem.HasChildrenCycle);

            CheckMapping(propertyName, propertyType, propertyValue, treeItem.Value);
        }

        [TestMethod]
        public void NullableEnumValue_Value1() {
            IEnumerable<TreeItem<Mapping>> parents = null;
            string propertyName = "Nullable TestEnum";
            Type propertyType = typeof(TestEnum?);
            TestEnum? propertyValue = TestEnum.Value1;

            TreeItem<Mapping> treeItem = CreateItem(parents, propertyName, propertyType, propertyValue);

            Assert.AreEqual(parents, treeItem.Parents);
            Assert.AreEqual(0, treeItem.Children.Count());
            Assert.IsFalse(treeItem.HasChildrenCycle);

            CheckMapping(propertyName, propertyType, propertyValue, treeItem.Value);
        }

        struct EmptyTestStruct { }

        [TestMethod]
        public void EmptyTestStructValue() {
            IEnumerable<TreeItem<Mapping>> parents = null;
            string propertyName = "EmptyTestStruct";
            Type propertyType = typeof(EmptyTestStruct);
            EmptyTestStruct propertyValue = new EmptyTestStruct();

            TreeItem<Mapping> treeItem = CreateItem(parents, propertyName, propertyType, propertyValue);

            Assert.AreEqual(parents, treeItem.Parents);
            Assert.AreEqual(0, treeItem.Children.Count());
            Assert.IsFalse(treeItem.HasChildrenCycle);

            CheckMapping(propertyName, propertyType, propertyValue, treeItem.Value);
        }

        [TestMethod]
        public void NullableEmptyTestStructValue_Null() {
            IEnumerable<TreeItem<Mapping>> parents = null;
            string propertyName = "Nullable EmptyTestStruct";
            Type propertyType = typeof(EmptyTestStruct?);
            EmptyTestStruct? propertyValue = null;

            TreeItem<Mapping> treeItem = CreateItem(parents, propertyName, propertyType, propertyValue);

            Assert.AreEqual(parents, treeItem.Parents);
            Assert.IsNull(treeItem.Children);
            Assert.IsFalse(treeItem.HasChildrenCycle);

            CheckMapping(propertyName, propertyType, propertyValue, treeItem.Value);
        }

        [TestMethod]
        public void NullableEmptyTestStructValue_NotNull() {
            IEnumerable<TreeItem<Mapping>> parents = null;
            string propertyName = "Nullable EmptyTestStruct";
            Type propertyType = typeof(EmptyTestStruct?);
            EmptyTestStruct? propertyValue = new EmptyTestStruct();

            TreeItem<Mapping> treeItem = CreateItem(parents, propertyName, propertyType, propertyValue);

            Assert.AreEqual(parents, treeItem.Parents);
            Assert.AreEqual(0, treeItem.Children.Count());
            Assert.IsFalse(treeItem.HasChildrenCycle);

            CheckMapping(propertyName, propertyType, propertyValue, treeItem.Value);
        }

        struct TestStruct {
            object Property0 { get; set; }

            public int Property1 { get; set; }
            public TestEnum Property2 { get; set; }
            public object Property3 { get; set; }
        }

        [TestMethod]
        public void TestStructValue() {
            IEnumerable<TreeItem<Mapping>> parents = null;
            string propertyName = "TestStruct";

            Type propertyType = typeof(TestStruct);
            TestStruct propertyValue = new TestStruct();

            TreeItem<Mapping> treeItem = CreateItem(parents, propertyName, propertyType, propertyValue);
            Assert.AreEqual(parents, treeItem.Parents);
            CheckTestStruct(treeItem, propertyName, propertyType, propertyValue, new List<TreeItem<Mapping>>());
        }

        void CheckTestStruct(TreeItem<Mapping> treeItem, string propertyName, Type propertyType, object propertyValue, IEnumerable<TreeItem<Mapping>> childParrents) {
            Assert.IsFalse(treeItem.HasChildrenCycle);
            Assert.IsNotNull(treeItem.Value);

            CheckMapping(propertyName, propertyType, propertyValue, treeItem.Value);
                        
            Assert.AreEqual(3, treeItem.Children.Count());

            TreeItem<Mapping> childItem1 = treeItem.Children.ElementAt(0);
            Assert.AreEqual(childParrents.Count() + 1, childItem1.Parents.Count());
            for (int i = 0; i < childParrents.Count(); i++)
                Assert.AreEqual(childParrents.ElementAt(i), childItem1.Parents.ElementAt(i));
            Assert.AreEqual(treeItem, childItem1.Parents.ElementAt(childParrents.Count()));
            Assert.IsFalse(childItem1.HasChildrenCycle);
           
            CheckMapping("Property1", typeof(int), 0, childItem1.Value);
            
            TreeItem<Mapping> childItem2 = treeItem.Children.ElementAt(1);
            Assert.AreEqual(childParrents.Count() + 1, childItem2.Parents.Count());
            for (int i = 0; i < childParrents.Count(); i++)
                Assert.AreEqual(childParrents.ElementAt(i), childItem2.Parents.ElementAt(i));
            Assert.AreEqual(treeItem, childItem2.Parents.ElementAt(childParrents.Count()));
            Assert.AreEqual(0, childItem2.Children.Count());
            Assert.IsFalse(childItem2.HasChildrenCycle);
           
            CheckMapping("Property2", typeof(TestEnum), TestEnum.Value1, childItem2.Value);
                                   
            TreeItem<Mapping> childItem3 = treeItem.Children.ElementAt(2);
            Assert.AreEqual(childParrents.Count() + 1, childItem3.Parents.Count());
            for (int i = 0; i < childParrents.Count(); i++)
                Assert.AreEqual(childParrents.ElementAt(i), childItem3.Parents.ElementAt(i));
            Assert.AreEqual(treeItem, childItem3.Parents.ElementAt(childParrents.Count()));
            Assert.IsNull(childItem3.Children);
            Assert.IsFalse(childItem3.HasChildrenCycle);

            CheckMapping("Property3", typeof(object), null, childItem3.Value);
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
            IEnumerable<TreeItem<Mapping>> parents = null;
            string propertyName = "TestClass";
            Type propertyType = typeof(TestClass);
            TestClass propertyValue = null;

            TreeItem<Mapping> treeItem = CreateItem(parents, propertyName, propertyType, propertyValue);

            Assert.AreEqual(parents, treeItem.Parents);
            Assert.IsNull(treeItem.Children);
            Assert.IsFalse(treeItem.HasChildrenCycle);

            CheckMapping(propertyName, propertyType, propertyValue, treeItem.Value);
        }

        [TestMethod]
        public void TestClass_NotNull() {
            IEnumerable<TreeItem<Mapping>> parents = null;
            string propertyName = "TestClass";
            Type propertyType = typeof(TestClass);
            TestClass propertyValue = new TestClass();

            TreeItem<Mapping> treeItem = CreateItem(parents, propertyName, propertyType, propertyValue);

            Assert.AreEqual(parents, treeItem.Parents);
            Assert.IsNotNull(treeItem.Children);
            Assert.IsFalse(treeItem.HasChildrenCycle);
          
            CheckMapping(propertyName, propertyType, propertyValue, treeItem.Value);

            Assert.AreEqual(4, treeItem.Children.Count());

            TreeItem<Mapping> childItem1 = treeItem.Children.ElementAt(0);
            Assert.AreEqual(1, childItem1.Parents.Count());
            Assert.AreEqual(treeItem, childItem1.Parents.ElementAt(0));
            Assert.AreEqual(0, childItem1.Children.Count());
            Assert.IsFalse(childItem1.HasChildrenCycle);

            CheckMapping("Property1", typeof(int), 0, childItem1.Value);
            
            TreeItem<Mapping> childItem2 = treeItem.Children.ElementAt(1);
            Assert.AreEqual(1, childItem2.Parents.Count());
            Assert.AreEqual(treeItem, childItem2.Parents.ElementAt(0));
            Assert.AreEqual(0, childItem2.Children.Count());
            Assert.IsFalse(childItem2.HasChildrenCycle);
       
            CheckMapping("Property2", typeof(TestEnum), TestEnum.Value1, childItem2.Value);
            
            TreeItem<Mapping> childItem3 = treeItem.Children.ElementAt(2);
            Assert.AreEqual(1, childItem3.Parents.Count());
            Assert.AreEqual(treeItem, childItem3.Parents.ElementAt(0));
            Assert.IsNull(childItem3.Children);
            Assert.IsFalse(childItem3.HasChildrenCycle);
         
            CheckMapping("Property3", typeof(object), null, childItem3.Value);
            
            TreeItem<Mapping> childItem4 = treeItem.Children.ElementAt(3);
            Assert.AreEqual(1, childItem4.Parents.Count());
            Assert.AreEqual(treeItem, childItem4.Parents.ElementAt(0));

            List<TreeItem<Mapping>> childItem4Parents = new List<TreeItem<Mapping>>();
            childItem4Parents.Add(treeItem);
            CheckTestStruct(childItem4, "Property4", typeof(TestStruct), default(TestStruct), childItem4Parents);
        }

        class TestClass2 {
            public TestClass2 Property { get; set; }
        }

        [TestMethod]
        public void TestClass2IsNull() {
            IEnumerable<TreeItem<Mapping>> parents = null;
            string propertyName = "TestClass2";
            Type propertyType = typeof(TestClass2);
            TestClass2 propertyValue = null;

            TreeItem<Mapping> treeItem = CreateItem(parents, propertyName, propertyType, propertyValue);

            Assert.AreEqual(parents, treeItem.Parents);
            Assert.IsNull(treeItem.Children);
            Assert.IsFalse(treeItem.HasChildrenCycle);

            CheckMapping(propertyName, propertyType, propertyValue, treeItem.Value);
        }

        [TestMethod]
        public void TestClass2IsNotNull_PropertyIsNull() {
            IEnumerable<TreeItem<Mapping>> parents = null;
            string propertyName = "TestClass2";
            Type propertyType = typeof(TestClass2);
            TestClass2 propertyValue = new TestClass2();

            TreeItem<Mapping> treeItem = CreateItem(parents, propertyName, propertyType, propertyValue);

            Assert.AreEqual(parents, treeItem.Parents);
            Assert.IsFalse(treeItem.HasChildrenCycle);
            Assert.IsNotNull(treeItem.Value);

            CheckMapping(propertyName, propertyType, propertyValue, treeItem.Value);

            Assert.AreEqual(1, treeItem.Children.Count());

            TreeItem<Mapping> childItem1 = treeItem.Children.ElementAt(0);
            Assert.AreEqual(1, childItem1.Parents.Count());
            Assert.AreEqual(treeItem, childItem1.Parents.ElementAt(0));
            Assert.IsNull(childItem1.Children);
            Assert.IsFalse(childItem1.HasChildrenCycle);
            Assert.IsNotNull(childItem1.Value);

            CheckMapping("Property", typeof(TestClass2), null, childItem1.Value);
        }

        [TestMethod]
        public void TestClass2IsNotNull_PropertyIsNotNull() {
            IEnumerable<TreeItem<Mapping>> parents = null;
            string propertyName = "TestClass2";
            Type propertyType = typeof(TestClass2);
            TestClass2 propertyValue = new TestClass2();
            TestClass2 childPropertyValue = new TestClass2();
            propertyValue.Property = childPropertyValue;

            TreeItem<Mapping> treeItem = CreateItem(parents, propertyName, propertyType, propertyValue);

            Assert.AreEqual(parents, treeItem.Parents);
            Assert.IsFalse(treeItem.HasChildrenCycle);
           
            CheckMapping(propertyName, propertyType, propertyValue, treeItem.Value);

            Assert.AreEqual(1, treeItem.Children.Count());

            TreeItem<Mapping> childItem1 = treeItem.Children.ElementAt(0);
            Assert.AreEqual(1, childItem1.Parents.Count());
            Assert.AreEqual(treeItem, childItem1.Parents.ElementAt(0));
            Assert.IsFalse(childItem1.HasChildrenCycle);
           
            CheckMapping("Property", typeof(TestClass2), childPropertyValue, childItem1.Value);

            Assert.AreEqual(1, childItem1.Children.Count());

            TreeItem<Mapping> childChildItem1 = childItem1.Children.ElementAt(0);
            Assert.AreEqual(2, childChildItem1.Parents.Count());
            Assert.AreEqual(treeItem, childChildItem1.Parents.ElementAt(0));
            Assert.AreEqual(childItem1, childChildItem1.Parents.ElementAt(1));
            Assert.IsNull(childChildItem1.Children);
            Assert.IsFalse(childChildItem1.HasChildrenCycle);

            CheckMapping("Property", typeof(TestClass2), null, childChildItem1.Value);
        }

        [TestMethod]
        public void TestClass2IsNotNull_HasChildrenCycle() {
            IEnumerable<TreeItem<Mapping>> parents = null;
            string propertyName = "TestClass2";
            Type propertyType = typeof(TestClass2);
            TestClass2 propertyValue = new TestClass2();
            propertyValue.Property = propertyValue;

            TreeItem<Mapping> treeItem = CreateItem(parents, propertyName, propertyType, propertyValue);

            Assert.AreEqual(parents, treeItem.Parents);
            Assert.IsFalse(treeItem.HasChildrenCycle);

            CheckMapping(propertyName, propertyType, propertyValue, treeItem.Value);

            Assert.AreEqual(1, treeItem.Children.Count());

            TreeItem<Mapping> childItem1 = treeItem.Children.ElementAt(0);
            Assert.AreEqual(1, childItem1.Parents.Count());
            Assert.AreEqual(treeItem, childItem1.Parents.ElementAt(0));
            Assert.IsNull(childItem1.Children);
            Assert.IsTrue(childItem1.HasChildrenCycle);

            CheckMapping("Property", typeof(TestClass2), propertyValue, childItem1.Value);
        }

        [TestMethod]
        public void StringIsNull() {
            IEnumerable<TreeItem<Mapping>> parents = null;
            string propertyName = "String";
            Type propertyType = typeof(string);
            string propertyValue = null;

            TreeItem<Mapping> treeItem = CreateItem(parents, propertyName, propertyType, propertyValue);

            Assert.AreEqual(parents, treeItem.Parents);
            Assert.IsNull(treeItem.Children);
            Assert.IsFalse(treeItem.HasChildrenCycle);

            CheckMapping(propertyName, propertyType, propertyValue, treeItem.Value);
        }

        [TestMethod]
        public void StringIsEmpty() {
            IEnumerable<TreeItem<Mapping>> parents = null;
            string propertyName = "String";
            Type propertyType = typeof(string);
            string propertyValue = String.Empty;

            TreeItem<Mapping> treeItem = CreateItem(parents, propertyName, propertyType, propertyValue);

            Assert.AreEqual(parents, treeItem.Parents);
            Assert.IsNull(treeItem.Children);
            Assert.IsFalse(treeItem.HasChildrenCycle);
            Assert.IsNotNull(treeItem.Value);

            CheckMapping(propertyName, propertyType, propertyValue, treeItem.Value);
        }

        [TestMethod]
        public void StringIsNotEmpty() {
            IEnumerable<TreeItem<Mapping>> parents = null;
            string propertyName = "String";
            Type propertyType = typeof(string);
            string propertyValue = "testString";

            TreeItem<Mapping> treeItem = CreateItem(parents, propertyName, propertyType, propertyValue);

            Assert.AreEqual(parents, treeItem.Parents);
            Assert.IsNull(treeItem.Children);
            Assert.IsFalse(treeItem.HasChildrenCycle);
            Assert.IsNotNull(treeItem.Value);

            CheckMapping(propertyName, propertyType, propertyValue, treeItem.Value);
        }

        [TestMethod]
        public void ListIntIsNull() {
            IEnumerable<TreeItem<Mapping>> parents = null;
            string propertyName = "List<int>";
            Type propertyType = typeof(List<int>);
            List<int> propertyValue = null;

            TreeItem<Mapping> treeItem = CreateItem(parents, propertyName, propertyType, propertyValue);

            Assert.AreEqual(parents, treeItem.Parents);
            Assert.IsNull(treeItem.Children);
            Assert.IsFalse(treeItem.HasChildrenCycle);
            Assert.IsNotNull(treeItem.Value);

            CheckMapping(propertyName, propertyType, propertyValue, treeItem.Value);
        }

        [TestMethod]
        public void ListIntIsEmpty() {
            IEnumerable<TreeItem<Mapping>> parents = null;
            string propertyName = "List<double>";
            Type propertyType = typeof(List<double>);
            List<double> propertyValue = new List<double>();

            TreeItem<Mapping> treeItem = CreateItem(parents, propertyName, propertyType, propertyValue);

            Assert.AreEqual(parents, treeItem.Parents);
            Assert.IsFalse(treeItem.HasChildrenCycle);
           
            CheckMapping(propertyName, propertyType, propertyValue, treeItem.Value);

            Assert.AreEqual(2, treeItem.Children.Count());

            TreeItem<Mapping> childItem1 = treeItem.Children.ElementAt(0);
            Assert.AreEqual(1, childItem1.Parents.Count());
            Assert.AreEqual(treeItem, childItem1.Parents.ElementAt(0));
            Assert.AreEqual(0, childItem1.Children.Count());
            Assert.IsFalse(childItem1.HasChildrenCycle);

            CheckMapping("Capacity", typeof(int), 0, childItem1.Value);
                        
            TreeItem<Mapping> childItem2 = treeItem.Children.ElementAt(1);
            Assert.AreEqual(1, childItem2.Parents.Count());
            Assert.AreEqual(treeItem, childItem2.Parents.ElementAt(0));
            Assert.AreEqual(0, childItem2.Children.Count());
            Assert.IsFalse(childItem2.HasChildrenCycle);

            CheckMapping("Count", typeof(int), 0, childItem2.Value);
        }

        [TestMethod]
        public void ListIntIsNotEmpty() {
            IEnumerable<TreeItem<Mapping>> parents = null;
            string propertyName = "List<double>";
            Type propertyType = typeof(List<double>);
            List<double> propertyValue = new List<double>();
            propertyValue.Add(2);

            TreeItem<Mapping> treeItem = CreateItem(parents, propertyName, propertyType, propertyValue);

            Assert.AreEqual(parents, treeItem.Parents);
            Assert.IsFalse(treeItem.HasChildrenCycle);
         
            CheckMapping(propertyName, propertyType, propertyValue, treeItem.Value);

            Assert.AreEqual(2, treeItem.Children.Count());

            TreeItem<Mapping> childItem1 = treeItem.Children.ElementAt(0);
            Assert.AreEqual(1, childItem1.Parents.Count());
            Assert.AreEqual(treeItem, childItem1.Parents.ElementAt(0));
            Assert.AreEqual(0, childItem1.Children.Count());
            Assert.IsFalse(childItem1.HasChildrenCycle);
        
            CheckMapping("Capacity", typeof(int), 4, childItem1.Value);

            TreeItem<Mapping> childItem2 = treeItem.Children.ElementAt(1);
            Assert.AreEqual(1, childItem2.Parents.Count());
            Assert.AreEqual(treeItem, childItem2.Parents.ElementAt(0));
            Assert.AreEqual(0, childItem2.Children.Count());
            Assert.IsFalse(childItem2.HasChildrenCycle);

            CheckMapping("Count", typeof(int), 1, childItem2.Value);
        }

        [TestMethod]
        public void DictionaryIsNull() {
            IEnumerable<TreeItem<Mapping>> parents = null;
            string propertyName = "Dictionary<string, double>";
            Type propertyType = typeof(Dictionary<string, double>);
            Dictionary<string, double> propertyValue = null;

            TreeItem<Mapping> treeItem = CreateItem(parents, propertyName, propertyType, propertyValue);

            Assert.AreEqual(parents, treeItem.Parents);
            Assert.IsNull(treeItem.Children);
            Assert.IsFalse(treeItem.HasChildrenCycle);
           
            CheckMapping(propertyName, propertyType, propertyValue, treeItem.Value);
        }

        [TestMethod]
        public void DictionaryIsEmpty() {
            IEnumerable<TreeItem<Mapping>> parents = null;
            string propertyName = "Dictionary<string, double>";
            Type propertyType = typeof(Dictionary<string, double>);
            Dictionary<string, double> propertyValue = new Dictionary<string, double>();

            TreeItem<Mapping> treeItem = CreateItem(parents, propertyName, propertyType, propertyValue);

            Assert.AreEqual(parents, treeItem.Parents);
            Assert.IsFalse(treeItem.HasChildrenCycle);
          
            CheckMapping(propertyName, propertyType, propertyValue, treeItem.Value);

            Assert.AreEqual(4, treeItem.Children.Count());

            TreeItem<Mapping> childItem1 = treeItem.Children.ElementAt(0);
            Assert.AreEqual(1, childItem1.Parents.Count());
            Assert.AreEqual(treeItem, childItem1.Parents.ElementAt(0));
            Assert.AreEqual(0, childItem1.Children.Count());
            Assert.IsFalse(childItem1.HasChildrenCycle);
           
            CheckMapping("Comparer", typeof(IEqualityComparer<string>), propertyValue.Comparer, childItem1.Value);
            
            TreeItem<Mapping> childItem2 = treeItem.Children.ElementAt(1);
            Assert.AreEqual(1, childItem2.Parents.Count());
            Assert.AreEqual(treeItem, childItem2.Parents.ElementAt(0));
            Assert.AreEqual(0, childItem2.Children.Count());
            Assert.IsFalse(childItem2.HasChildrenCycle);

            CheckMapping("Count", typeof(int), 0, childItem2.Value);

            TreeItem<Mapping> childItem3 = treeItem.Children.ElementAt(2);
            Assert.AreEqual(1, childItem3.Parents.Count());
            Assert.AreEqual(treeItem, childItem3.Parents.ElementAt(0));
            Assert.IsFalse(childItem3.HasChildrenCycle);

            CheckMapping("Keys", typeof(Dictionary<string, double>.KeyCollection), propertyValue.Keys, childItem3.Value);
                      
            Assert.AreEqual(1, childItem3.Children.Count());

            TreeItem<Mapping> childChildItem3 = childItem3.Children.ElementAt(0);
            Assert.AreEqual(2, childChildItem3.Parents.Count());
            Assert.AreEqual(treeItem, childChildItem3.Parents.ElementAt(0));
            Assert.AreEqual(childItem3, childChildItem3.Parents.ElementAt(1));
            Assert.AreEqual(0, childChildItem3.Children.Count());
            Assert.IsFalse(childChildItem3.HasChildrenCycle);

            CheckMapping("Count", typeof(int), 0, childChildItem3.Value);

            TreeItem<Mapping> childItem4 = treeItem.Children.ElementAt(3);
            Assert.AreEqual(1, childItem4.Parents.Count());
            Assert.AreEqual(treeItem, childItem4.Parents.ElementAt(0));
            Assert.IsFalse(childItem4.HasChildrenCycle);
          
            CheckMapping("Values", typeof(Dictionary<string, double>.ValueCollection), propertyValue.Values, childItem4.Value);
                       
            Assert.AreEqual(1, childItem4.Children.Count());

            TreeItem<Mapping> childChildItem4 = childItem4.Children.ElementAt(0);
            Assert.AreEqual(2, childChildItem4.Parents.Count());
            Assert.AreEqual(treeItem, childChildItem4.Parents.ElementAt(0));
            Assert.AreEqual(childItem4, childChildItem4.Parents.ElementAt(1));
            Assert.AreEqual(0, childChildItem4.Children.Count());
            Assert.IsFalse(childChildItem4.HasChildrenCycle);

            CheckMapping("Count", typeof(int), 0, childChildItem4.Value);
        }

        [TestMethod]
        public void DictionaryIsNotEmpty() {
            IEnumerable<TreeItem<Mapping>> parents = null;
            string propertyName = "Dictionary<string, double>";
            Type propertyType = typeof(Dictionary<string, double>);
            Dictionary<string, double> propertyValue = new Dictionary<string, double>();
            propertyValue.Add("Test", 2);

            TreeItem<Mapping> treeItem = CreateItem(parents, propertyName, propertyType, propertyValue);

            Assert.AreEqual(parents, treeItem.Parents);
            Assert.IsNotNull(treeItem.Children);
            Assert.IsFalse(treeItem.HasChildrenCycle);

            Assert.AreEqual(parents, treeItem.Parents);
            Assert.IsFalse(treeItem.HasChildrenCycle);

            CheckMapping(propertyName, propertyType, propertyValue, treeItem.Value);

            Assert.AreEqual(4, treeItem.Children.Count());

            TreeItem<Mapping> childItem1 = treeItem.Children.ElementAt(0);
            Assert.AreEqual(1, childItem1.Parents.Count());
            Assert.AreEqual(treeItem, childItem1.Parents.ElementAt(0));
            Assert.AreEqual(0, childItem1.Children.Count());
            Assert.IsFalse(childItem1.HasChildrenCycle);

            CheckMapping("Comparer", typeof(IEqualityComparer<string>), propertyValue.Comparer, childItem1.Value);

            TreeItem<Mapping> childItem2 = treeItem.Children.ElementAt(1);
            Assert.AreEqual(1, childItem2.Parents.Count());
            Assert.AreEqual(treeItem, childItem2.Parents.ElementAt(0));
            Assert.AreEqual(0, childItem2.Children.Count());
            Assert.IsFalse(childItem2.HasChildrenCycle);

            CheckMapping("Count", typeof(int), 1, childItem2.Value);

            TreeItem<Mapping> childItem3 = treeItem.Children.ElementAt(2);
            Assert.AreEqual(1, childItem3.Parents.Count());
            Assert.AreEqual(treeItem, childItem3.Parents.ElementAt(0));
            Assert.IsFalse(childItem3.HasChildrenCycle);

            CheckMapping("Keys", typeof(Dictionary<string, double>.KeyCollection), propertyValue.Keys, childItem3.Value);

            Assert.AreEqual(1, childItem3.Children.Count());

            TreeItem<Mapping> childChildItem3 = childItem3.Children.ElementAt(0);
            Assert.AreEqual(2, childChildItem3.Parents.Count());
            Assert.AreEqual(treeItem, childChildItem3.Parents.ElementAt(0));
            Assert.AreEqual(childItem3, childChildItem3.Parents.ElementAt(1));
            Assert.AreEqual(0, childChildItem3.Children.Count());
            Assert.IsFalse(childChildItem3.HasChildrenCycle);

            CheckMapping("Count", typeof(int), 1, childChildItem3.Value);

            TreeItem<Mapping> childItem4 = treeItem.Children.ElementAt(3);
            Assert.AreEqual(1, childItem4.Parents.Count());
            Assert.AreEqual(treeItem, childItem4.Parents.ElementAt(0));
            Assert.IsFalse(childItem4.HasChildrenCycle);

            CheckMapping("Values", typeof(Dictionary<string, double>.ValueCollection), propertyValue.Values, childItem4.Value);

            Assert.AreEqual(1, childItem4.Children.Count());

            TreeItem<Mapping> childChildItem4 = childItem4.Children.ElementAt(0);
            Assert.AreEqual(2, childChildItem4.Parents.Count());
            Assert.AreEqual(treeItem, childChildItem4.Parents.ElementAt(0));
            Assert.AreEqual(childItem4, childChildItem4.Parents.ElementAt(1));
            Assert.AreEqual(0, childChildItem4.Children.Count());
            Assert.IsFalse(childChildItem4.HasChildrenCycle);

            CheckMapping("Count", typeof(int), 1, childChildItem4.Value);
        }
    }
}
