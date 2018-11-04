using Reflection.Utils.Tree;
using Reflection.Utils.Tree.Export;
using System;

namespace Reflection.Utils.PropertyTree.Export {
    // TODO:
    //IsPrimitive: Boolean, Byte, SByte, Int16, UInt16, Int32, UInt32, Int64, UInt64, IntPtr, UIntPtr, Char, Double, Single.
    //struct
    //IsEnum
    //Nullable: struct, IsPrimitive, IsEnum
    //IsValueType: IsPrimitive, struct, IsEnum
    //IsGenericType
    //string 
    //IsArray
    //IEnumerable

    //public static class PropertyTreeExporter {
    //    public static void Export(StringWriter writer, Tree<PropertyDescription> tree, ExportStringFormat format) {
    //        TreeExporter<StringWriter, PropertyDescription, ExportStringFormat>.Export(
    //            writer,
    //            tree,
    //            format, 
    //            (w, i, f) => ExportRootProperty(w, i, f),
    //            (w, i, f, l) => ExportChildProperty(w, i, f, l),
    //            t => t.PropertyValue != null
    //            );
    //    }

    //    static void ExportChildProperty(StringWriter writer, PropertyDescription property, ExportStringFormat format, int level) {
    //        ExportPropertyName(writer, property, level, format);
    //        ExportPropertyValue(writer, property, format);
    //    }

    //    static void ExportRootProperty(StringWriter writer, PropertyDescription property, ExportStringFormat format) {
    //        ExportPropertyName(writer, property, 0, format);
    //    }

    //    static void ExportPropertyName(StringWriter writer, PropertyDescription property, int level, ExportStringFormat format) {
    //        writer.WriteLine(
    //            format.CreateLevelIndent(level) +
    //            String.Format(format.PropertyNameFormat, property.PropertyName, property.PropertyType.Name));
    //    }

    //    static void ExportPropertyValue(StringWriter writer, PropertyDescription property, ExportStringFormat format) {
    //        object value = property.PropertyValue;
    //        PropertyValueType type = property.PropertyValueType;
    //        bool isNullable = type.HasFlag(PropertyValueType.Nullable);
    //        bool hasException = type.HasFlag(PropertyValueType.Exception);
    //        if (type.HasFlag(PropertyValueType.Primitive)) {
    //            ExportPrimitiveValue(writer, value, isNullable, hasException, format);
    //        }

    //        switch (type) {
    //            case PropertyValueType.Primitive:
    //                ExportPrimitiveValue(writer, type, value, ((PropertyValueNullable)propertyValue).IsNullable, format); break;
    //            case PropertyValueType.Struct:
    //                ExportStructValue(writer, type, value, ((PropertyValueNullable)propertyValue).IsNullable, format); break;
    //            case PropertyValueType.Enum:
    //                ExportEnumValue(writer, type, value, ((PropertyValueNullable)propertyValue).IsNullable, ((PropertyValueEnum)propertyValue).EnumType, format); break;
    //            case PropertyValueType.String:
    //                ExportStringValue(writer, type, (string)value, format); break;

    //            case PropertyValueType.Undefined:
    //            case PropertyValueType.Exception:
    //            case PropertyValueType.Class: ExportReferenceValue(writer, type, value, format); break;
    //        }
    //    }
    //    static void ExportReferenceValue(StringWriter writer, PropertyValueType type, object value, ExportStringFormat format) {
    //        string exportedValue;
    //        if (value == null)
    //            exportedValue = String.Format(format.PropertyValueNullFormat, GetValueTypeString(type));
    //        else
    //            exportedValue = String.Format(format.PropertyValueFormat, value.ToString(), GetValueTypeString(type));
    //        writer.Write(exportedValue);
    //    }
    //    static void ExportPrimitiveValue(StringWriter writer, object value, bool isNullable, bool hasException, ExportStringFormat format) {
    //        string exportedValue;
    //        if (isNullable) {
    //            if (value == null)
    //                exportedValue = String.Format(format.PropertyNullableValueNullFormat,  GetValueTypeString(PropertyValueType.Primitive));
    //            else if (hasException)
    //                exportedValue = String.Format(format.PropertyNullableValueFormatWithException, value.ToString() + GetValueTypeString(PropertyValueType.Primitive));
    //            else
    //                exportedValue = String.Format(format.PropertyNullableValueFormat, value.ToString(), GetValueTypeString(PropertyValueType.Primitive));
    //        } else if (hasException)
    //            exportedValue = String.Format(format.PropertyValueFormatWithException, value.ToString() + GetValueTypeString(PropertyValueType.Primitive));
    //        else
    //            exportedValue = String.Format(format.PropertyValueFormat, value.ToString(), GetValueTypeString(PropertyValueType.Primitive));
    //        writer.Write(exportedValue);
    //    }
    //    static void ExportStructValue(StringWriter writer, PropertyValueType type, object value, bool isNullable, ExportStringFormat format) {
    //        string exportedValue = String.Format(format.PropertyValueFormat, value.ToString(), GetValueTypeString(type));
    //        writer.Write(exportedValue);
    //    }
    //    static void ExportStringValue(StringWriter writer, PropertyValueType type, string value, ExportStringFormat format) {
    //        string exportedValue;
    //        if (value == null) 
    //            exportedValue = String.Format(format.PropertyValueNullFormat, GetValueTypeString(type));
    //        else if (value == String.Empty)
    //            exportedValue = String.Format(format.PropertyValueEmptyFormat, GetValueTypeString(type));
    //        else
    //            exportedValue = String.Format(format.PropertyValueFormat, value, GetValueTypeString(type));
    //        writer.Write(exportedValue);
    //    }
    //    static void ExportEnumValue(StringWriter writer, PropertyValueType type, object value, bool isNullable, Type enumType, ExportStringFormat format) {
    //        string exportedValue;
    //        if (isNullable) {
    //            if (value == null)
    //                exportedValue = String.Format(format.PropertyValueNullFormat, "Nullable " + GetValueTypeString(type));
    //            else
    //                exportedValue = String.Format(format.PropertyValueFormat, Enum.GetName(enumType, value), "Nullable " + GetValueTypeString(type));
    //        } else
    //            exportedValue = String.Format(format.PropertyValueFormat, Enum.GetName(enumType, value), GetValueTypeString(type));
    //        writer.Write(exportedValue);
    //    }
    //    static string GetValueTypeString(PropertyValueType type) {
    //        return Enum.GetName(typeof(PropertyValueType), type);
    //    }
    //}
}