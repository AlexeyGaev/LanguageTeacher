using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using Translation.Http.Tree;

namespace Translation.Http {
    public static class PropertyTreeExporter {
        public static string Convert(HttpContext context, string propertyName) {
            int indentLevel = 0;
            if (context == null)
                return WriteNullValueLine(String.Empty, indentLevel, propertyName);

            Tree<PropertyDescription> tree = PropertyDescriptionTreeBuilder.Create(context, propertyName);
            string result = String.Empty;
            TreeItem<PropertyDescription> rootItem = tree.RootItem;
            result = WriteLine(result, indentLevel, rootItem.Value.PropertyName, rootItem.Value.PropertyType.Name);
            result = Write(result, indentLevel + 1, rootItem.Children);
            return result;
        }

        static string WriteLine(string input, int indentLevel, string propertyName, string propertyType) {
            return
                input + IndentBuilder.Create(indentLevel) +
                propertyName + Localization.StartBlock + propertyType + Localization.EndBlock + Localization.Colon +
                Localization.NewLine;
        }

        static string WriteNullValueLine(string input, int indentLevel, string propertyName) {
            return
                input + IndentBuilder.Create(indentLevel) + propertyName + Localization.Indent + Localization.IsNull +
                Localization.Dot + Localization.NewLine;
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