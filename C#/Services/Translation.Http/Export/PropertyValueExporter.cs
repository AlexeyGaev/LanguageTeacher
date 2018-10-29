using System;
using Translation.Http.PropertyTree;

namespace Translation.Http {
    public static class PropertyValueExporter {
        public static string Write(string input, IPropertyValue propertyValue) {
            if (propertyValue.Type == PropertyValueType.Undefined)
                return input + "Undefined";
            if (propertyValue.Type == PropertyValueType.Exception)
                return Write(input, "Exception", propertyValue.Value.GetType().Name);
            if (propertyValue.Type == PropertyValueType.Primitive)
                return Write(input, "Primitive", propertyValue.Value.ToString());
            if (propertyValue.Type == PropertyValueType.String)
                return Write(input, "String", propertyValue.Value == null ? String.Empty : propertyValue.Value.ToString());
            if (propertyValue.Type == PropertyValueType.Struct)
                return Write(input, "Struct", propertyValue.Value == null ? String.Empty : propertyValue.Value.ToString());
            if (propertyValue.Type == PropertyValueType.Enum)
                return Write(input, "Enum", Enum.GetName(((PropertyValueEnum)propertyValue).EnumType, propertyValue.Value));
            if (propertyValue.Type == PropertyValueType.Class)
                return Write(input, "Class", propertyValue.Value == null ? String.Empty : propertyValue.Value.ToString());
            return "------Nothing-------";
        }

        static string Write(string input, string valueType, string valueString) {
            return input + valueType + Localization.Indent + Localization.Dash + Localization.Indent + valueString;
        }
    }
}