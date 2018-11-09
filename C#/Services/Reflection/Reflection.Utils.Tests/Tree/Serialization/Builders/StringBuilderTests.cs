using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Reflection.Utils.PropertyTree.Serialization.Tests {
    [TestClass]
    public class StringBuilderTests {
        [TestMethod]
        public void CreateStringFromObject() {
            Assert.AreEqual(Localization.NullValue, StringBuilder.CreateStringFromObject(null));
            Assert.AreEqual(Localization.EmptyValue, StringBuilder.CreateStringFromObject(String.Empty));
            Assert.AreEqual("Test", StringBuilder.CreateStringFromObject("Test"));
            Assert.AreEqual("System.Object", StringBuilder.CreateStringFromObject(new object()));
            Assert.AreEqual("1", StringBuilder.CreateStringFromObject(1));
        }

        [TestMethod]
        public void CreateStringFromValue() {
            Assert.AreEqual(Localization.NullValue, StringBuilder.CreateStringFromValue(null));
            Assert.AreEqual(Localization.Exception, StringBuilder.CreateStringFromValue(new Exception()));
            Assert.AreEqual(Localization.EmptyValue, StringBuilder.CreateStringFromValue(String.Empty));
            Assert.AreEqual("Test", StringBuilder.CreateStringFromValue("Test"));
            Assert.AreEqual("System.Object", StringBuilder.CreateStringFromValue(new object()));
            Assert.AreEqual("1", StringBuilder.CreateStringFromValue(1));
            Assert.AreEqual("1", StringBuilder.CreateStringFromValue((int?)1));
        }

        [TestMethod]
        public void CreateString() {
            Assert.AreEqual(Localization.NullValue, StringBuilder.CreateString(null));
            Assert.AreEqual(Localization.EmptyValue, StringBuilder.CreateString(String.Empty));
            Assert.AreEqual("Test", StringBuilder.CreateString("Test"));
        }

        [TestMethod]
        public void CreateStringFromType() {
            Assert.AreEqual(Localization.NullValue, StringBuilder.CreateStringFromType(null));
            Assert.AreEqual(typeof(int).Name, StringBuilder.CreateStringFromType(typeof(int)));
            Assert.AreEqual(Localization.Nullable + " " + typeof(int).Name, StringBuilder.CreateStringFromType(typeof(int?)));
        }
    }
}
