using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Reflection.Utils.PropertyTree.Tests {
    [TestClass]
    public class PropertyFieldTests {
        [TestMethod]
        public void CheckEquals() {
            PropertyField field1 = new PropertyField("Test", typeof(int));
#pragma warning disable CS1718 // Comparison made to same variable
            Assert.IsTrue(field1 == field1);
#pragma warning restore CS1718 // Comparison made to same variable
            Assert.IsTrue(field1.Equals(field1));

            Assert.IsFalse(field1 == null);
            Assert.IsFalse(null == field1);
            Assert.IsFalse(field1.Equals(null));

            PropertyField field2 = new PropertyField("Test", typeof(int));
            Assert.IsTrue(field1 == field2);
            Assert.IsTrue(field2 == field1);
            Assert.IsTrue(field1.Equals(field2));
            Assert.IsTrue(field2.Equals(field1));

            PropertyField field3 = new PropertyField("Test", typeof(double));
            Assert.IsFalse(field1 == field3);
            Assert.IsFalse(field3 == field1);
            Assert.IsFalse(field1.Equals(field3));
            Assert.IsFalse(field3.Equals(field1));

            PropertyField field4 = new PropertyField("Test", null);
            Assert.IsFalse(field1 == field4);
            Assert.IsFalse(field4 == field1);
            Assert.IsFalse(field1.Equals(field4));
            Assert.IsFalse(field4.Equals(field1));

            PropertyField field5 = new PropertyField("Test1", null);
            Assert.IsFalse(field4 == field5);
            Assert.IsFalse(field5 == field4);
            Assert.IsFalse(field4.Equals(field5));
            Assert.IsFalse(field5.Equals(field4));

            PropertyField field6 = new PropertyField(null, null);
            Assert.IsFalse(field1 == field6);
            Assert.IsFalse(field6 == field1);
            Assert.IsFalse(field1.Equals(field6));
            Assert.IsFalse(field6.Equals(field1));
        }

        [TestMethod]
        public void CheckHashCode() {
            Assert.AreEqual(0, new PropertyField(null, null).GetHashCode());
            Assert.AreNotEqual(0, new PropertyField(String.Empty, null).GetHashCode());
            Assert.AreNotEqual(0, new PropertyField("Test", typeof(int)).GetHashCode());
        }
    }
}
