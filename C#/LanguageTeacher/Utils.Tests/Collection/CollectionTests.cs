﻿using NUnit.Framework;
using Utils.Collection;

namespace Utils.Collection.Tests {
    [TestFixture]
    public class CollectionTests {
        [Test]
        public void Default() {
            Collection<int> collection = new Collection<int>();
            Assert.AreEqual(0, collection.Count);
        }

        [Test]
        public void GetEmptyEnumerator() {
            Collection<int> collection = new Collection<int>();
            IEnumerator<int> enumerator = collection.GetEnumerator();
            Assert.IsFalse(enumerator.MoveNext());
            Assert.AreEqual(0, enumerator.Current);
        }

        [Test]
        public void AddOneItem() {
            Collection<int> collection = new Collection<int>();
            collection.Add(2);

            Assert.AreEqual(1, collection.Count);
            IEnumerator<int> enumerator = collection.GetEnumerator();
            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual(2, enumerator.Current);
            Assert.IsFalse(enumerator.MoveNext());
            Assert.AreEqual(0, enumerator.Current);
        }

        [Test]
        public void Clear() {
            Collection<int> collection = new Collection<int>();
            collection.Add(2);
            collection.Clear();

            Assert.AreEqual(0, collection.Count);
        }
                
        [Test]
        public void IncreaseCapacity() {
            Collection<int> collection = new Collection<int>();
            collection.Add(2);
            collection.Add(3);

            Assert.AreEqual(2, collection.Count);
            collection.Add(4);
            Assert.AreEqual(3, collection.Count);
            IEnumerator<int> enumerator = collection.GetEnumerator();
            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual(2, enumerator.Current);
            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual(3, enumerator.Current);
            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual(4, enumerator.Current);
            Assert.IsFalse(enumerator.MoveNext());
            Assert.AreEqual(0, enumerator.Current);
        }
    }
}
