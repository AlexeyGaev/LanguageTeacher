using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Reflection.Utils.PropertyTree.Serialization.Tests {
    [TestClass]
    public class OldTests {
        //[TestMethod]
        //public void CheckToString() {
        //    Mapping emptyMapping = new Mapping(new PropertyField(null, null), new PropertyValue(null));
        //    Assert.AreEqual("(Name = #Null, Type = #Null) : (Value = #Null) (Children: #Null)", new TreeItem<Mapping>(null, emptyMapping).ToString());

        //    Mapping mapping = new Mapping(new PropertyField("Test", typeof(int)), new PropertyValue(1));
        //    TreeItem<Mapping> treeItem = new TreeItem<Mapping>(null, mapping);

        //    string mappingValue = "(Name = Test, Type = Int32) : (Type = Int32, Value = 1)";
        //    Assert.AreEqual(mappingValue + " (Children: #Null)", treeItem.ToString());

        //    treeItem.SetHasChildrenCycle(true);

        //    Assert.AreEqual(mappingValue + " (Children: #Null, HasCycle)", treeItem.ToString());

        //    treeItem.InitChildren();
        //    Assert.AreEqual(mappingValue + " (Children: #Empty)", treeItem.ToString());

        //    treeItem.AddChild(new TreeItem<Mapping>(null, emptyMapping));
        //    Assert.AreEqual(mappingValue + " (Children: 1)", treeItem.ToString());
        //}

        //[TestMethod]
        //public void CheckToString() {
        //    Assert.AreEqual("(Name = Test, Type = Int32) : (Type = Int32, Value = 1)", new Mapping(new PropertyField("Test", typeof(int)), new PropertyValue(1)).ToString());
        //}

        //[TestMethod]
        //public void CheckToString() {
        //    Assert.AreEqual("Name = #Null, Type = #Null", new PropertyField(null, null).ToString());
        //    Assert.AreEqual("Name = #Empty, Type = #Null", new PropertyField(String.Empty, null).ToString());
        //    Assert.AreEqual("Name = Test, Type = #Null", new PropertyField("Test", null).ToString());
        //    Assert.AreEqual("Name = Test, Type = Int32", new PropertyField("Test", typeof(int)).ToString());
        //    Assert.AreEqual("Name = Test, Type = Nullable Int32", new PropertyField("Test", typeof(int?)).ToString());
        //}

        //[TestMethod]
        //public void CheckToString() {
        //    Assert.AreEqual("Value = #Null", PropertyValueStringBuilder.Create(null));
        //    Assert.AreEqual("Type = Int32, Value = 1", PropertyValueStringBuilder.Create(1));
        //    Assert.AreEqual("Type = Object, Value = System.Object", PropertyValueStringBuilder.Create(new object()));
        //}

        //[TestMethod]
        //public void NotChild() {
        //    Mapping emptyMapping = new Mapping(new PropertyField(null, null), new PropertyValue(null));
        //    TreeItem<Mapping> treeItem = new TreeItem<Mapping>(null, emptyMapping);

        //    StringWriter writer = new StringWriter();
        //    TreeExporter<Mapping>.ExportItem(writer, treeItem, 0);

        //    Assert.AreEqual(1, writer.Result.Count());
        //    Assert.AreEqual("(Name = #Null, Type = #Null) : (Value = #Null) (Children: #Null)", writer.Result.ElementAt(0));
        //}

        //[TestMethod]
        //public void OneChild() {
        //    Mapping emptyMapping = new Mapping(new PropertyField(null, null), new PropertyValue(null));
        //    TreeItem<Mapping> treeItem = new TreeItem<Mapping>(null, emptyMapping);
        //    treeItem.InitChildren();
        //    treeItem.AddChild(new TreeItem<Mapping>(null, emptyMapping));

        //    StringWriter writer = new StringWriter();
        //    TreeExporter<Mapping>.ExportItem(writer, treeItem, 0);

        //    Assert.AreEqual(2, writer.Result.Count());
        //    Assert.AreEqual("(Name = #Null, Type = #Null) : (Value = #Null) (Children: 1)", writer.Result.ElementAt(0));
        //    Assert.AreEqual(" (Name = #Null, Type = #Null) : (Value = #Null) (Children: #Null)", writer.Result.ElementAt(1));
        //}
    }
}
