using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Reflection.Utils.PropertyTree.Selialization.Tests {
    [TestClass]
    public class PropertyValueStringBuilderTests {
        [TestMethod]
        public void CheckToString() {
            Assert.AreEqual("Value = #Null", PropertyValueStringBuilder.Create(null));
            Assert.AreEqual("Type = Int32, Value = 1", PropertyValueStringBuilder.Create(1));
            Assert.AreEqual("Type = Object, Value = System.Object", PropertyValueStringBuilder.Create(new object()));
        }
    }
}
