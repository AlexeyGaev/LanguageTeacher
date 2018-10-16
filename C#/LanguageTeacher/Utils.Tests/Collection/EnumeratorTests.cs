using NUnit.Framework;
using Utils.Collection;

namespace Utils.Collection.Tests {
    [TestFixture]
    public class EnumeratorTests {
        [Test]
        public void Empty() {
            Enumerator<int> enumerator = new Enumerator<int>(new int[0], 0);
            Assert.IsFalse(enumerator.MoveNext());
            Assert.AreEqual(0, enumerator.Current);
        }

        [Test]
        public void NotEmpty() {
            Enumerator<int> enumerator = new Enumerator<int>(new int[3] { 1, 2, 3}, 2);
            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual(1, enumerator.Current);
            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual(2, enumerator.Current);
            Assert.IsFalse(enumerator.MoveNext());
            Assert.AreEqual(0, enumerator.Current);
        }
    }
}
