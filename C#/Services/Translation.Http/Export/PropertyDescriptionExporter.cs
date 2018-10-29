using System;
using Translation.Http.Tree;

namespace Translation.Http {
    public static class PropertyDescriptionExporter {
        public static string Write(string input, int indentLevel, PropertyDescription property, out bool writeChildren) {
            string result = input;
            string propertyName = property.PropertyInfo.Name;
            string propertyType = property.PropertyInfo.PropertyType.Name;
            string propertyStringValue = String.Empty;
            propertyStringValue = PropertyValueExporter.Write(propertyStringValue, property.PropertyValue);
            if (propertyStringValue == null) {
                result = WriteNullValueLine(result, indentLevel, propertyName, propertyType);
                writeChildren = false;
            } else {
                result = WriteLine(result, indentLevel, propertyName, propertyType, propertyStringValue);
                writeChildren = true;
            }
            return result;
        }
        
        public static string WriteLine(string input, int indentLevel, string propertyName, string propertyType) {
            return
                input + CreateIndent(indentLevel) +
                propertyName + Localization.StartBlock + propertyType + Localization.EndBlock + Localization.Colon +
                Localization.NewLine;
        }

        public static string WriteLine(string input, int indentLevel, string propertyName, string propertyType, string propertyValue) {
            return
                input + CreateIndent(indentLevel) +
                propertyName + Localization.StartBlock + propertyType + Localization.EndBlock + Localization.Colon + " " + propertyValue +
                Localization.Dot + Localization.NewLine;
        }

        public static string WriteNullValueLine(string input, int indentLevel, string propertyName, string propertyType) {
            return
                input + CreateIndent(indentLevel) +
                propertyName + Localization.StartBlock + propertyType + Localization.EndBlock + " " + Localization.IsNull +
                Localization.Dot + Localization.NewLine;
        }

        static string CreateIndent(int indentLevel) {
            string indent = string.Empty;
            for (int i = 0; i < indentLevel; i++)
                indent += Localization.Indent;
            return indent;
        }
    }
}