using System;

namespace Reflection.Utils.PropertyTree.Serialization {
    public static class StringBuilder {
        public static string CreateStringFromObject(object value) {
            if (value == null)
                return Localization.NullValue;
            return value is string ? CreateString((string)value) : value.ToString();
        }

        public static string CreateStringFromValue(object propertyValue) {
            if (propertyValue == null)
                return Localization.NullValue;
            if (propertyValue is Exception)
                return Localization.Exception;
            Type propertyType = propertyValue.GetType();
            Type underlyingType = Nullable.GetUnderlyingType(propertyType);
            if (underlyingType != null)
                propertyType = underlyingType;
            TypeCode typeCode = Type.GetTypeCode(propertyType);
            if (typeCode == TypeCode.DBNull || typeCode == TypeCode.Empty)
                return Localization.NullValue;
            if (typeCode == TypeCode.String)
                return CreateString((string)propertyValue);
            return propertyValue.ToString();
        }

        public static string CreateStringFromType(Type type) {
            if (type == null)
                return Localization.NullValue;
            Type underlyingType = Nullable.GetUnderlyingType(type);
            if (underlyingType != null)
                return Localization.Nullable + " " + CreateString(underlyingType.Name);
            return CreateString(type.Name);
        }

        public static string CreateString(string value) {
            return value == null ? Localization.NullValue : (String.IsNullOrEmpty(value) ? Localization.EmptyValue : value);
        }
    }
}

