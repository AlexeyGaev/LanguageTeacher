using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using Translation.Http.Tree;

namespace Translation.Http.PropertyTree {
    public class EmptyExporter {
        public static string Write(string input, string propertyName) {
            return
                input + propertyName + Localization.Indent + Localization.IsNull +
                Localization.Dot + Localization.NewLine;
        }
    }

    public static class PropertyTreeExporter {
        public static string Write(string input, Tree<PropertyDescription> tree) {
            TreeItem<PropertyDescription> rootItem = tree.RootItem;
            input = WriteLine(input, 0, rootItem.Value.PropertyName, rootItem.Value.PropertyType.Name);
            input = Write(input, 1, rootItem.Children);
            return input;
        }

        static string WriteLine(string input, int indentLevel, string propertyName, string propertyType) {
            return
                input + IndentBuilder.Create(indentLevel) +
                propertyName + Localization.StartBlock + propertyType + Localization.EndBlock + Localization.Colon +
                Localization.NewLine;
        }

        

        static string Write(string result, int indentLevel, List<TreeItem<PropertyDescription>> items) {
            foreach (TreeItem<PropertyDescription> item in items) {
                bool writeChildren;
                result = PropertyDescriptionExporter.Write(result, indentLevel, item.Value, out writeChildren);
                if (writeChildren)
                    result = Write(result, indentLevel + 1, item.Children);
            }
            return result;
        }
    }
}