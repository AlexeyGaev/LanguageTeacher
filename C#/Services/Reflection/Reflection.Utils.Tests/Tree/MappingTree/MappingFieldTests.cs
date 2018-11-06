using Microsoft.VisualStudio.TestTools.UnitTesting;
using Reflection.Utils.Tree.MappingTree;
using System;

namespace Reflection.Utils.Tests {
    [TestClass]
    public class MappingFieldTests {
        [TestMethod]
        public void CheckEquals() {
            MappingField field1 = new MappingField("Test", typeof(int));
#pragma warning disable CS1718 // Comparison made to same variable
            Assert.IsTrue(field1 == field1);
#pragma warning restore CS1718 // Comparison made to same variable
            Assert.IsTrue(field1.Equals(field1));
            
            Assert.IsFalse(field1 == null);
            Assert.IsFalse(null == field1);
            Assert.IsFalse(field1.Equals(null));

            MappingField field2 = new MappingField("Test", typeof(int));
            Assert.IsTrue(field1 == field2);
            Assert.IsTrue(field2 == field1);
            Assert.IsTrue(field1.Equals(field2));
            Assert.IsTrue(field2.Equals(field1));
            
            MappingField field3 = new MappingField("Test", typeof(double));
            Assert.IsFalse(field1 == field3);
            Assert.IsFalse(field3 == field1);
            Assert.IsFalse(field1.Equals(field3));
            Assert.IsFalse(field3.Equals(field1));

            MappingField field4 = new MappingField("Test", null);
            Assert.IsFalse(field1 == field4);
            Assert.IsFalse(field4 == field1);
            Assert.IsFalse(field1.Equals(field4));
            Assert.IsFalse(field4.Equals(field1));

            MappingField field5 = new MappingField("Test1", null);
            Assert.IsFalse(field4 == field5);
            Assert.IsFalse(field5 == field4);
            Assert.IsFalse(field4.Equals(field5));
            Assert.IsFalse(field5.Equals(field4));

            MappingField field6 = new MappingField(null, null);
            Assert.IsFalse(field1 == field6);
            Assert.IsFalse(field6 == field1);
            Assert.IsFalse(field1.Equals(field6));
            Assert.IsFalse(field6.Equals(field1));
        }

        [TestMethod]
        public void CheckHashCode() {
            Assert.AreEqual(0, new MappingField(null, null).GetHashCode());
            Assert.AreNotEqual(0, new MappingField(String.Empty, null).GetHashCode());
            Assert.AreNotEqual(0, new MappingField("Test", typeof(int)).GetHashCode());
        }

        [TestMethod]
        public void CheckToString() {
            Assert.AreEqual("Name = #Null, Type = #Null", new MappingField(null, null).ToString());
            Assert.AreEqual("Name = #Empty, Type = #Null", new MappingField(String.Empty, null).ToString());
            Assert.AreEqual("Name = Test, Type = #Null", new MappingField("Test", null).ToString());
            Assert.AreEqual("Name = Test, Type = Int32", new MappingField("Test", typeof(int)).ToString());
            Assert.AreEqual("Name = Test, Type = Nullable Int32", new MappingField("Test", typeof(int?)).ToString());
        }
    }
}
