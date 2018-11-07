using Microsoft.VisualStudio.TestTools.UnitTesting;
using Reflection.Utils.Tree;
using Reflection.Utils.Tree.Export;
using System.Linq;

namespace Reflection.Utils.PropertyTree.Tests {
    [TestClass]
    public class TreeExporterTests {
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
