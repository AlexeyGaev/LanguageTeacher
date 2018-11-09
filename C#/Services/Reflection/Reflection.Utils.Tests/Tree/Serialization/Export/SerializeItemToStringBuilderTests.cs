using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Reflection.Utils.PropertyTree.Serialization.Tests {
    [TestClass]
    public class SerializeItemToStringBuilderTests {
        [TestMethod]
        public void Create() {
            Assert.AreEqual(String.Empty, SerializeItemToStringBuilder.Create(SerializeItem.Empty));
            Assert.AreEqual("Test", SerializeItemToStringBuilder.Create(SerializeItem.CreateOneValue("Test")));
            Assert.AreEqual("Test1=Test2", SerializeItemToStringBuilder.Create(SerializeItem.CreateTwoValues("Test1", "Test2")));
        }
    }
}
