using Reflection.Utils.Tree;
using Reflection.Utils.Tree.Export;
using System;

namespace Reflection.Utils.PropertyTree.Export {
    public static class PropertyTreeExporter {
        public static void Export(StringWriter writer, Tree<PropertyDescription> tree, ExportStringFormat format) {
            TreeExporter<StringWriter, PropertyDescription, ExportStringFormat>.Export(
                writer,
                tree,
                format, 
                (w, i, f) => ExportRootProperty(w, i, f),
                (w, i, f, l) => ExportChildProperty(w, i, f, l),
                t => t.PropertyValue.Value != null
                );
        }

        static void ExportChildProperty(StringWriter writer, PropertyDescription property, ExportStringFormat format, int level) {
            ExportPropertyName(writer, property, level, format);
            ExportPropertyValue(writer, property.PropertyValue, format);
        }

        static void ExportRootProperty(StringWriter writer, PropertyDescription property, ExportStringFormat format) {
            ExportPropertyName(writer, property, 0, format);
        }

        static void ExportPropertyName(StringWriter writer, PropertyDescription property, int level, ExportStringFormat format) {
            writer.WriteLine(
                format.CreateLevelIndent(level) +
                String.Format(format.PropertyNameFormat, property.PropertyName, property.PropertyType.Name));
        }

        static void ExportPropertyValue(StringWriter writer, PropertyValue propertyValue, ExportStringFormat format) {
            object value = propertyValue.Value;
            PropertyValueType type = propertyValue.Type;
            switch (type) {
                case PropertyValueType.Primitive:
                    ExportPrimitiveValue(writer, type, value, ((PropertyValueNullable)propertyValue).IsNullable, format); break;
                case PropertyValueType.Struct:
                    ExportStructValue(writer, type, value, ((PropertyValueNullable)propertyValue).IsNullable, format); break;
                case PropertyValueType.Enum:
                    ExportEnumValue(writer, type, value, ((PropertyValueNullable)propertyValue).IsNullable, ((PropertyValueEnum)propertyValue).EnumType, format); break;
                case PropertyValueType.String:
                    ExportStringValue(writer, type, (string)value, format); break;

                case PropertyValueType.Undefined:
                case PropertyValueType.Exception:
                case PropertyValueType.Class: ExportReferenceValue(writer, type, value, format); break;
            }
        }
        static void ExportReferenceValue(StringWriter writer, PropertyValueType type, object value, ExportStringFormat format) {
            string exportedValue;
            if (value == null)
                exportedValue = String.Format(format.PropertyValueNullFormat, GetValueTypeString(type));
            else
                exportedValue = String.Format(format.PropertyValueFormat, value.ToString(), GetValueTypeString(type));
            writer.Write(exportedValue);
        }
        static void ExportPrimitiveValue(StringWriter writer, PropertyValueType type, object value, bool isNullable, ExportStringFormat format) {
            string exportedValue;
            if (isNullable) {
                if (value == null)
                    exportedValue = String.Format(format.PropertyValueNullFormat, "Nullable " + GetValueTypeString(type));
                else
                    exportedValue = String.Format(format.PropertyValueFormat, value.ToString(), "Nullable " + GetValueTypeString(type));
            } else
                exportedValue = String.Format(format.PropertyValueFormat, value.ToString(), GetValueTypeString(type));
            writer.Write(exportedValue);
        }
        static void ExportStructValue(StringWriter writer, PropertyValueType type, object value, bool isNullable, ExportStringFormat format) {
            string exportedValue = String.Format(format.PropertyValueFormat, value.ToString(), GetValueTypeString(type));
            writer.Write(exportedValue);
        }
        static void ExportStringValue(StringWriter writer, PropertyValueType type, string value, ExportStringFormat format) {
            string exportedValue;
            if (value == null) 
                exportedValue = String.Format(format.PropertyValueNullFormat, GetValueTypeString(type));
            else if (value == String.Empty)
                exportedValue = String.Format(format.PropertyValueEmptyFormat, GetValueTypeString(type));
            else
                exportedValue = String.Format(format.PropertyValueFormat, value, GetValueTypeString(type));
            writer.Write(exportedValue);
        }
        static void ExportEnumValue(StringWriter writer, PropertyValueType type, object value, bool isNullable, Type enumType, ExportStringFormat format) {
            string exportedValue;
            if (isNullable) {
                if (value == null)
                    exportedValue = String.Format(format.PropertyValueNullFormat, "Nullable " + GetValueTypeString(type));
                else
                    exportedValue = String.Format(format.PropertyValueFormat, Enum.GetName(enumType, value), "Nullable " + GetValueTypeString(type));
            } else
                exportedValue = String.Format(format.PropertyValueFormat, Enum.GetName(enumType, value), GetValueTypeString(type));
            writer.Write(exportedValue);
        }
        static string GetValueTypeString(PropertyValueType type) {
            return Enum.GetName(typeof(PropertyValueType), type);
        }
    }
}