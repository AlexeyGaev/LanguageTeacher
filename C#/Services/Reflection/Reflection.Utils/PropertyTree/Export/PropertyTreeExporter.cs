using Reflection.Utils.Tree;
using Reflection.Utils.Tree.Export;
using System;

namespace Reflection.Utils.PropertyTree.Export {
    public static class PropertyTreeExporter {
        public static void Export(StringWriter writer, Tree<PropertyDescription> tree) {
            TreeExporter<StringWriter, PropertyDescription>.Export(
                writer,
                tree,
                (w, r) => ExportRootProperty(w, r),
                (w, t, l) => ExportChildProperty(w, t, l),
                t => t.PropertyValue.Value != null
                );
        }

        static void ExportChildProperty(StringWriter writer, PropertyDescription property, int level) {
            writer.WriteLine(
                IndentBuilder.Create(level) +
                property.PropertyName +
                Localization.StartBlock + property.PropertyType.Name + Localization.EndBlock +
                Localization.Colon + Localization.Indent);
            ExportPropertyValue(writer, property.PropertyValue);
        }

        static void ExportRootProperty(StringWriter writer, PropertyDescription property) {
            writer.WriteLine(
                IndentBuilder.Create(0) +
                property.PropertyName + Localization.Indent +
                Localization.StartBlock + property.PropertyType.Name + Localization.EndBlock + 
                Localization.Colon);
        }

        static void ExportPropertyValue(StringWriter writer, IPropertyValue propertyValue) {
            object value = propertyValue.Value;
            PropertyValueType type = propertyValue.Type;
            switch (type) {
                case PropertyValueType.Primitive:
                case PropertyValueType.String:
                case PropertyValueType.Struct: ExportStringValue(writer, type, value.ToString()); break;
                case PropertyValueType.Enum: ExportStringValue(writer, type, Enum.GetName(((PropertyValueEnum)propertyValue).EnumType, value)); break;

                case PropertyValueType.Undefined:
                case PropertyValueType.Exception:
                case PropertyValueType.Class: ExportValue(writer, type, value); break;
            }
        }

        static void ExportValue(StringWriter writer, PropertyValueType type, object value) {
            if (value == null)
                ExportNullValue(writer, type);
            else
                ExportStringValue(writer, type, value.ToString());
        }

        static void ExportNullValue(StringWriter writer, PropertyValueType type) {
            writer.Write(Localization.IsNull + Localization.Indent + Localization.StartBlock + GetValueTypeString(type) + Localization.EndBlock);
        }

        static void ExportStringValue(StringWriter writer, PropertyValueType type, string value) {
            writer.Write(value + Localization.Indent + Localization.StartBlock + GetValueTypeString(type) + Localization.EndBlock);
        }

        static string GetValueTypeString(PropertyValueType type) {
            return Enum.GetName(typeof(PropertyValueType), type);
        }
    }
}