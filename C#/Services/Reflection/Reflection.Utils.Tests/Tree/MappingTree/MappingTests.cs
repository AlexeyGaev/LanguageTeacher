using Microsoft.VisualStudio.TestTools.UnitTesting;
using Reflection.Utils.Tree.MappingTree;
using System;

namespace Reflection.Utils.Tests {
    [TestClass]
    public class MappingTests {
        [TestMethod]
        public void CheckEquals() {
            Mapping property1 = new Mapping(new MappingField("Test", typeof(int)), new MappingValue(1));
#pragma warning disable CS1718 // Comparison made to same variable
            Assert.IsTrue(property1 == property1);
#pragma warning restore CS1718 // Comparison made to same variable
            Assert.IsTrue(property1.Equals(property1));

            Assert.IsFalse(property1 == null);
            Assert.IsFalse(null == property1);
            Assert.IsFalse(property1.Equals(null));

            Mapping property2 = new Mapping(new MappingField("Test", typeof(int)), new MappingValue(1));
            Assert.IsTrue(property1 == property2);
            Assert.IsTrue(property2 == property1);
            Assert.IsTrue(property1.Equals(property2));
            Assert.IsTrue(property2.Equals(property1));

            Mapping property3 = new Mapping(new MappingField("Test", typeof(int)), new MappingValue(2));
            Assert.IsFalse(property1 == property3);
            Assert.IsFalse(property3 == property1);
            Assert.IsFalse(property1.Equals(property3));
            Assert.IsFalse(property3.Equals(property1));
        }

        [TestMethod]
        public void CheckHashCode() {
            Assert.AreNotEqual(0, new Mapping(new MappingField("Test", typeof(int)), new MappingValue(1)).GetHashCode());
        }

        [TestMethod]
        public void CheckToString() {
            Assert.AreEqual("(Name = Test, Type = Int32) : (Type = Int32, Value = 1)", new Mapping(new MappingField("Test", typeof(int)), new MappingValue(1)).ToString());
        }
    }
}
