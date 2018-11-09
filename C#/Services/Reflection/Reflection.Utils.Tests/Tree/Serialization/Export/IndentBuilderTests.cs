using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Reflection.Utils.PropertyTree.Serialization.Tests {
    [TestClass]
    public class IndentBuilderTests {
        [TestMethod]
        public void CreateLevelSpace() {
            Assert.AreEqual("  ", IndentBuilder.CreateLevelSpace(2, " "));
            Assert.AreEqual(string.Empty, IndentBuilder.CreateLevelSpace(0, " "));
            Assert.AreEqual(string.Empty, IndentBuilder.CreateLevelSpace(-2, " "));
        }
    }
}
