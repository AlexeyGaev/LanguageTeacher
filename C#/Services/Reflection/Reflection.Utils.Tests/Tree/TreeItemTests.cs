using Microsoft.VisualStudio.TestTools.UnitTesting;
using Reflection.Utils.Tree;
using Reflection.Utils.Tree.MappingTree;

namespace Reflection.Utils.Tests {
    [TestClass]
    public class TreeItemTests {
        [TestMethod]
        public void CheckToString() {
            Mapping emptyMapping = new Mapping(new MappingField(null, null), new MappingValue(null));
            Assert.AreEqual("(Name = #Null, Type = #Null) : (Value = #Null) (Children: #Null)", new TreeItem<Mapping>(null, emptyMapping).ToString());

            Mapping mapping = new Mapping(new MappingField("Test", typeof(int)), new MappingValue(1));
            TreeItem<Mapping> treeItem = new TreeItem<Mapping>(null, mapping);

            string mappingValue = "(Name = Test, Type = Int32) : (Type = Int32, Value = 1)";
            Assert.AreEqual(mappingValue + " (Children: #Null)", treeItem.ToString());

            treeItem.SetHasChildrenCycle(true);

            Assert.AreEqual(mappingValue + " (Children: #Null, HasCycle)", treeItem.ToString());

            treeItem.InitChildren();
            Assert.AreEqual(mappingValue + " (Children: #Empty)", treeItem.ToString());

            treeItem.AddChild(new TreeItem<Mapping>(null, emptyMapping));
            Assert.AreEqual(mappingValue + " (Children: 1)", treeItem.ToString());
        }
    }
}
