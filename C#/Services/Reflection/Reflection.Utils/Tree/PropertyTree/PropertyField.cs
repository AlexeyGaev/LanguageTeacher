using Reflection.Utils.Tree.Localization;
using System;

namespace Reflection.Utils.PropertyTree {
    public struct PropertyField : IEquatable<PropertyField> {
        public static bool operator ==(PropertyField left, PropertyField right) {
            return left.Equals(right);
        }
        public static bool operator !=(PropertyField left, PropertyField right) {
            return !left.Equals(right);
        }

        string name;
        Type type;

        public PropertyField(string name, Type type) {
            this.name = name;
            this.type = type;
        }

        public string Name { get { return this.name; } }
        public Type Type { get { return this.type; } }

        public bool IsNullable { get { return this.type != null && Nullable.GetUnderlyingType(this.type) != null; } }

        public bool Equals(PropertyField other) {
            return
                String.Compare(this.name, other.name, StringComparison.InvariantCulture) == 0 &&
                this.type == other.type;
        }

        public override bool Equals(object obj) {
            return obj is PropertyField && Equals((PropertyField)obj);
        }

        public override int GetHashCode() {
            int result = 0;
            if (this.name != null)
                result ^= this.name.GetHashCode();
            if (this.type != null)
                result ^= this.type.GetHashCode();
            return result;
        }

        public override string ToString() {
            string format = LocalizationTable.GetStringById(LocalizationId.Name) + " = {0}, " + LocalizationTable.GetStringById(LocalizationId.Type) + " = {1}";
            return String.Format(format, NameToString(), TypeToString());
        }

        public string NameToString() {
            if (this.name == null)
                return LocalizationTable.GetStringById(LocalizationId.Null);
            if (String.IsNullOrEmpty(this.name))
                return LocalizationTable.GetStringById(LocalizationId.Empty);
            return this.name;
        }

        public string TypeToString() {
            if (this.type == null)
                return LocalizationTable.GetStringById(LocalizationId.Null);
            Type underlyingType = Nullable.GetUnderlyingType(this.type);
            if (underlyingType != null)
                return LocalizationTable.GetStringById(LocalizationId.Nullable) + " " + underlyingType.Name;
            return this.type.Name;
        } 
    }


}
