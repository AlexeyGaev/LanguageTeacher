using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using Translation.Http.Tree;

namespace Translation.Http {
    public static class PropertyTreeExporter {
        public static string Convert(HttpContext value) {
            string propertyName = "HttpContext";
            string propertyType = value.GetType().Name;
            int indentLevel = 0;
            if (value == null)
                return PropertyDescriptionExporter.WriteNullValueLine(String.Empty, indentLevel, propertyName, propertyType);

            string result = String.Empty;
            result = PropertyDescriptionExporter.WriteLine(result, indentLevel, propertyName, propertyType);
            Tree<PropertyDescription> tree = PropertyDescriptionTreeBuilder.Create(value);
            result = Write(result, indentLevel + 1, tree.RootItems);
            return result;
        }

        static string Write(string result, int indentLevel, List<Item<PropertyDescription>> items) {
            foreach (Item<PropertyDescription> item in items) {
                bool writeChildren;
                result = PropertyDescriptionExporter.Write(result, indentLevel, item.Value, out writeChildren);
                if (writeChildren)
                    result = Write(result, indentLevel + 1, item.Children);
            }
            return result;
        }
    }
}