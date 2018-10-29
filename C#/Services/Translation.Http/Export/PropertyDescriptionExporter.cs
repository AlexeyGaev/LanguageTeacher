using System;
using Translation.Http.PropertyTree;

namespace Translation.Http {
    public static class PropertyDescriptionExporter {
        public static string Write(string input, int indentLevel, PropertyDescription property, out bool writeChildren) {
            string result = input;
            string propertyStringValue = String.Empty;
            propertyStringValue = PropertyValueExporter.Write(propertyStringValue, property.PropertyValue);
            if (propertyStringValue == null) {
                result = WriteNullValueLine(result, indentLevel, property.PropertyName, property.PropertyType.Name);
                writeChildren = false;
            } else {
                result = WriteLine(result, indentLevel, property.PropertyName, property.PropertyType.Name, propertyStringValue);
                writeChildren = true;
            }
            return result;
        }

        public static string WriteLine(string input, int indentLevel, string propertyName, string propertyType, string propertyValue) {
            return
                input + IndentBuilder.Create(indentLevel) +
                propertyName + Localization.StartBlock + propertyType + Localization.EndBlock + Localization.Colon + " " + propertyValue +
                Localization.Dot + Localization.NewLine;
        }

        public static string WriteNullValueLine(string input, int indentLevel, string propertyName, string propertyType) {
            return
                input + IndentBuilder.Create(indentLevel) +
                propertyName + Localization.StartBlock + propertyType + Localization.EndBlock + " " + Localization.IsNull +
                Localization.Dot + Localization.NewLine;
        }
    }
}