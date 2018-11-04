using Microsoft.VisualStudio.TestTools.UnitTesting;
using Reflection.Utils.PropertyTree;
using Reflection.Utils.Tree;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Reflection.Utils.Tests {
    [TestClass]
    public class PropertyDescriptionTests {
        [TestMethod]
        public void CheckEquals() {
            PropertyDescription property1 = new PropertyDescription("Test", typeof(int), null);
            Assert.IsTrue(property1 == property1);
            Assert.IsTrue(property1.Equals(property1));
            
            Assert.IsFalse(property1 == null);
            Assert.IsFalse(null == property1);
            Assert.IsFalse(property1.Equals(null));

            PropertyDescription property2 = new PropertyDescription("Test", typeof(int), null);
            Assert.IsTrue(property1 == property2);
            Assert.IsTrue(property2 == property1);
            Assert.IsTrue(property1.Equals(property2));
            Assert.IsTrue(property2.Equals(property1));

            PropertyDescription property3 = new PropertyDescription("Test", typeof(int), 1);
            Assert.IsFalse(property1 == property3);
            Assert.IsFalse(property3 == property1);
            Assert.IsFalse(property1.Equals(property3));
            Assert.IsFalse(property3.Equals(property1));

            PropertyDescription property4 = new PropertyDescription("Test", null, 1);
            Assert.IsFalse(property1 == property4);
            Assert.IsFalse(property4 == property1);
            Assert.IsFalse(property1.Equals(property4));
            Assert.IsFalse(property4.Equals(property1));

            PropertyDescription property5 = new PropertyDescription(null, null, 1);
            Assert.IsFalse(property4 == property5);
            Assert.IsFalse(property5 == property4);
            Assert.IsFalse(property5.Equals(property4));
            Assert.IsFalse(property4.Equals(property5));
        }
        [TestMethod]
        public void CheckHashCode() {
            Assert.AreEqual(0, new PropertyDescription(null, null, null).GetHashCode());
            Assert.AreNotEqual(0, new PropertyDescription(String.Empty, null, null).GetHashCode());
            Assert.AreNotEqual(0, new PropertyDescription("Test", typeof(int), null).GetHashCode());
            Assert.AreNotEqual(0, new PropertyDescription("Test", typeof(int), 1).GetHashCode());
        }
    }
}
