using Reflection.Utils.Tree.Localization;
using System;

namespace Reflection.Utils.PropertyTree {
    public static class PropertyFieldStringBuilder {

    }


    public static class PropertyValueStringBuilder {
        public static string Create(object value) {
            if (value == null)
                return LocalizationTable.GetStringById(LocalizationId.Value) + " = " + LocalizationTable.GetStringById(LocalizationId.Null);
            string format = LocalizationTable.GetStringById(LocalizationId.Type) + " = {0}, " + LocalizationTable.GetStringById(LocalizationId.Value) + " = {1}";
            Type type = value.GetType();
            Type underlyingType = Nullable.GetUnderlyingType(type);
            if (underlyingType != null)
                type = underlyingType;

            return String.Format(format, type.Name, ValueToString(value, type));
        }

        static string ValueToString(object value, Type type) {
            if (value is Exception)
                return LocalizationTable.GetStringById(LocalizationId.Exception);
            TypeCode typeCode = Type.GetTypeCode(type);
            if (typeCode == TypeCode.DBNull || typeCode == TypeCode.Empty)
                return LocalizationTable.GetStringById(LocalizationId.Null);
            if (typeCode == TypeCode.String) {
                string stringValue = (string)value;
                return String.IsNullOrEmpty(stringValue) ? LocalizationTable.GetStringById(LocalizationId.Empty) : stringValue;
            }
            return value.ToString();
        }
    }
}
