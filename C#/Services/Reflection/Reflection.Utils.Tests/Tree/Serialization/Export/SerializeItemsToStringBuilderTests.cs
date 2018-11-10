using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Reflection.Utils.PropertyTree.Serialization.Tests {
    [TestClass]
    public class SerializeItemsToStringBuilderTests {
        [TestMethod]
        public void Create() {
            Assert.AreEqual(String.Empty, SerializeItemsToStringBuilder.Create(null));

            List<SerializeItem> list = new List<SerializeItem>();
            Assert.AreEqual(String.Empty, SerializeItemsToStringBuilder.Create(list));

            list.Add(SerializeItem.Empty);
            Assert.AreEqual(String.Empty, SerializeItemsToStringBuilder.Create(list));

            list.Add(SerializeItem.Empty);
            Assert.AreEqual(String.Empty, SerializeItemsToStringBuilder.Create(list));

            list.Add(SerializeItem.CreateOneValue("Test"));
            Assert.AreEqual("Test", SerializeItemsToStringBuilder.Create(list));

            list.Add(SerializeItem.CreateTwoValues("Test1", "Test2"));
            Assert.AreEqual("Test Test1=Test2", SerializeItemsToStringBuilder.Create(list));

            list.Add(SerializeItem.Empty);
            Assert.AreEqual("Test Test1=Test2", SerializeItemsToStringBuilder.Create(list));
        }
    }
}
