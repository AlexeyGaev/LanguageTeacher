using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Reflection.Utils.PropertyTree.Tests {
    [TestClass]
    public class MappingValueTests {
        [TestMethod]
        public void CheckEquals() {
            PropertyValue value1 = new PropertyValue(1);
#pragma warning disable CS1718 // Comparison made to same variable
            Assert.IsTrue(value1 == value1);
#pragma warning restore CS1718 // Comparison made to same variable
            Assert.IsTrue(value1.Equals(value1));
            
            Assert.IsFalse(value1 == null);
            Assert.IsFalse(null == value1);
            Assert.IsFalse(value1.Equals(null));

            PropertyValue value2 = new PropertyValue(1);
            Assert.IsTrue(value1 == value2);
            Assert.IsTrue(value2 == value1);
            Assert.IsTrue(value1.Equals(value2));
            Assert.IsTrue(value2.Equals(value1));

            PropertyValue value3 = new PropertyValue(2);
            Assert.IsFalse(value1 == value3);
            Assert.IsFalse(value3 == value1);
            Assert.IsFalse(value1.Equals(value3));
            Assert.IsFalse(value3.Equals(value1));
        }

        [TestMethod]
        public void CheckHashCode() {
            Assert.AreEqual(0, new PropertyValue(null).GetHashCode());
            Assert.AreNotEqual(0, new PropertyValue(1).GetHashCode());
        }

        //[TestMethod]
        //public void CheckToString() {
        //    Assert.AreEqual("Value = #Null", new PropertyValue(null).ToString());
        //    Assert.AreEqual("Type = Int32, Value = 1", new PropertyValue(1).ToString());
        //    Assert.AreEqual("Type = DateTime, Value = 01.01.0001 0:00:00", new PropertyValue(new DateTime()).ToString());
        //    Assert.AreEqual("Type = Object, Value = System.Object", new PropertyValue(new object()).ToString());
        //}
    }
}
