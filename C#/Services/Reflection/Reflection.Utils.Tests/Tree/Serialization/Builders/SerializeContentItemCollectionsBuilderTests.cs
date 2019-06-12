using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Reflection.Utils.PropertyTree.Serialization.Tests {
    [TestClass]
    public class SerializeContentItemCollectionsBuilderTests {
        [TestMethod]
        public void Create_NullNull() {
            Assert.AreEqual(null, SerializeContentItemCollectionsBuilder.Create(null, null));
        }

        [TestMethod]
        public void Create_ObjectChildrenCycle() {
            IEnumerable<SerializeContentItemCollection> collections = SerializeContentItemCollectionsBuilder.Create(PropertyObjectChildren.ReferenceCycle, null);
            Assert.AreEqual(1, collections.Count());
            SerializeContentItemCollectionBuilderTests.CheckCycle(collections.ElementAt(0), Localization.ObjectChildren, Localization.ReferenceCycle);
        }

        [TestMethod]
        public void Create_ObjectChildren() {
            IEnumerable<SerializeContentItemCollection> collections = SerializeContentItemCollectionsBuilder.Create(new PropertyObjectChildren(Enumerable.Empty<PropertyItem>()), null);
            Assert.AreEqual(1, collections.Count());
            SerializeContentItemCollectionBuilderTests.CheckEmpty(collections.ElementAt(0), Localization.ObjectChildren);
        }

        [TestMethod]
        public void Create_ArrayChildren() {
            IEnumerable<SerializeContentItemCollection> collections = SerializeContentItemCollectionsBuilder.Create(null, Enumerable.Empty<PropertyItem>());
            Assert.AreEqual(1, collections.Count());
            SerializeContentItemCollectionBuilderTests.CheckEmpty(collections.ElementAt(0), Localization.ArrayChildren);
        }

        [TestMethod]
        public void Create_ObjectAndArrayChildren() {
            IEnumerable<SerializeContentItemCollection> collections = SerializeContentItemCollectionsBuilder.Create(new PropertyObjectChildren(Enumerable.Empty<PropertyItem>()), Enumerable.Empty<PropertyItem>());
            Assert.AreEqual(2, collections.Count());
            SerializeContentItemCollectionBuilderTests.CheckEmpty(collections.ElementAt(0), Localization.ObjectChildren);
            SerializeContentItemCollectionBuilderTests.CheckEmpty(collections.ElementAt(1), Localization.ArrayChildren);
        }
    }
}
