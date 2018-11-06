using Reflection.Utils.Tree.Localization;
using System;

namespace Reflection.Utils.Tree.MappingTree {
    public struct MappingValue : IEquatable<MappingValue> {
        public static bool operator ==(MappingValue left, MappingValue right) {
            return left.Equals(right);
        }
        public static bool operator !=(MappingValue left, MappingValue right) {
            return !left.Equals(right);
        }

        object value;
       
        public MappingValue(object value) {
            this.value = value;
        }
               
        public object Value { get { return this.value; } }

        public bool Equals(MappingValue other) {
            if (this.value == null)
                return other.value == null;
            return this.value.Equals(other.value);
        }

        public override bool Equals(object obj) {
            return obj is MappingValue && Equals((MappingValue)obj);
        }

        public override int GetHashCode() {
            int result = 0;
            if (this.value != null)
                result ^= this.value.GetHashCode();
            return result;
        }

        public override string ToString() {
            if (this.value == null)
                return LocalizationTable.GetStringById(LocalizationId.Value) + " = " + LocalizationTable.GetStringById(LocalizationId.Null);
            string format = LocalizationTable.GetStringById(LocalizationId.Type) + " = {0}, " + LocalizationTable.GetStringById(LocalizationId.Value) + " = {1}";

            Type type = this.Value.GetType();
            Type underlyingType = Nullable.GetUnderlyingType(type);
            if (underlyingType != null)
                type = underlyingType;
                       
            return String.Format(format, type.Name, ValueToString(type));
        }
                
        string ValueToString(Type type) {
            if (this.value is Exception)
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
