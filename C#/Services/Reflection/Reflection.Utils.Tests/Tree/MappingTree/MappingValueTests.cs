using Microsoft.VisualStudio.TestTools.UnitTesting;
using Reflection.Utils.Tree.MappingTree;
using System;

namespace Reflection.Utils.Tests {
    [TestClass]
    public class MappingValueTests {
        [TestMethod]
        public void CheckEquals() {
            MappingValue value1 = new MappingValue(1);
#pragma warning disable CS1718 // Comparison made to same variable
            Assert.IsTrue(value1 == value1);
#pragma warning restore CS1718 // Comparison made to same variable
            Assert.IsTrue(value1.Equals(value1));
            
            Assert.IsFalse(value1 == null);
            Assert.IsFalse(null == value1);
            Assert.IsFalse(value1.Equals(null));

            MappingValue value2 = new MappingValue(1);
            Assert.IsTrue(value1 == value2);
            Assert.IsTrue(value2 == value1);
            Assert.IsTrue(value1.Equals(value2));
            Assert.IsTrue(value2.Equals(value1));

            MappingValue value3 = new MappingValue(2);
            Assert.IsFalse(value1 == value3);
            Assert.IsFalse(value3 == value1);
            Assert.IsFalse(value1.Equals(value3));
            Assert.IsFalse(value3.Equals(value1));
        }

        [TestMethod]
        public void CheckHashCode() {
            Assert.AreEqual(0, new MappingValue(null).GetHashCode());
            Assert.AreNotEqual(0, new MappingValue(1).GetHashCode());
        }

        [TestMethod]
        public void CheckToString() {
            Assert.AreEqual("Value = #Null", new MappingValue(null).ToString());
            Assert.AreEqual("Type = Int32, Value = 1", new MappingValue(1).ToString());
            Assert.AreEqual("Type = DateTime, Value = 01.01.0001 0:00:00", new MappingValue(new DateTime()).ToString());
            Assert.AreEqual("Type = Object, Value = System.Object", new MappingValue(new object()).ToString());
        }
    }
}
