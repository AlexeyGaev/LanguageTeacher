using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Language.Common.Utils {
    [TestClass]
    public class EnumeratorTests {
        [TestMethod]
        public void Empty() {
            Enumerator<int> enumerator = new Enumerator<int>(new int[0], 0);
            Assert.IsFalse(enumerator.MoveNext());
            Assert.AreEqual(0, enumerator.Current);
        }

        [TestMethod]
        public void NotEmpty() {
            Enumerator<int> enumerator = new Enumerator<int>(new int[3] { 1, 2, 3}, 2);
            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual(1, enumerator.Current);
            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual(2, enumerator.Current);
            Assert.IsFalse(enumerator.MoveNext());
            Assert.AreEqual(0, enumerator.Current);
        }

        [TestMethod]
        public void Reset() {
            Enumerator<int> enumerator = new Enumerator<int>(new int[3] { 1, 2, 3 }, 2);
            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual(1, enumerator.Current);
            Assert.IsTrue(enumerator.MoveNext());
            enumerator.Reset();
            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual(1, enumerator.Current);
            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual(2, enumerator.Current);
            Assert.IsFalse(enumerator.MoveNext());
            Assert.AreEqual(0, enumerator.Current);
        }
    }
}
