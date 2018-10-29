using System;
using Translation.Http.Tree;

namespace Translation.Http {
    public static class PropertyValueExporter {
        public static string Write(string input, PropertyValue propertyValue) {
            if (propertyValue.Undefined)
                return input + "Undefined";
            if (propertyValue.IsException)
                return Write(input, "Exception", propertyValue.Value.GetType().Name);
            if (propertyValue.IsPrimitive)
                return Write(input, "Primitive", propertyValue.Value.ToString());
            if (propertyValue.IsString)
                return Write(input, "String", (string)propertyValue.Value);
            if (propertyValue.IsStruct)
                return Write(input, "Struct", propertyValue.ToString());
            if (propertyValue.IsEnum)
                return Write(input, "Enum", Enum.GetName(propertyValue.EnumType, propertyValue.Value));
            if (propertyValue.Value != null)
                return Write(input, "Class", propertyValue.Value.ToString());
            return null;
        }

        static string Write(string input, string valueType, string valueString) {
            return input + valueType + Localization.Indent + Localization.Dash + Localization.Indent + valueString;
        }
    }
}