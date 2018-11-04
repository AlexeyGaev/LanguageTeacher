using System;

namespace Reflection.Utils.PropertyTree {
    public struct PropertyDescription : IEquatable<PropertyDescription> {
        public static bool operator ==(PropertyDescription left, PropertyDescription right) {
            return left.Equals(right);
        }
        public static bool operator !=(PropertyDescription left, PropertyDescription right) {
            return !left.Equals(right);
        }

        string propertyName;
        Type propertyType;
        object propertyValue;

        public PropertyDescription(string propertyName, Type propertyType, object propertyValue) {
            this.propertyName = propertyName;
            this.propertyType = propertyType;
            this.propertyValue = propertyValue;
        }

        public string PropertyName { get { return this.propertyName; } }
        public Type PropertyType { get { return this.propertyType; } }
        public object PropertyValue { get { return this.propertyValue; } }
        public bool IsNullable { get { return PropertyType != null && Nullable.GetUnderlyingType(PropertyType) != null; } }
        public bool IsException { get { return PropertyValue != null && PropertyValue is Exception; } }
        public bool IsArray { get { return PropertyType != null && PropertyType.IsArray; } }
        
        public bool Equals(PropertyDescription other) {
            if ((object)other == null ||
                String.Compare(this.propertyName, other.propertyName, StringComparison.InvariantCulture) != 0 ||
                this.propertyType != other.propertyType)
                return false;
            if (this.propertyValue == null)
                return other.propertyValue == null;
            return this.propertyValue.Equals(other.propertyValue);
        }

        public override bool Equals(object obj) {
            return obj is PropertyDescription && Equals((PropertyDescription)obj);
        }

        public override int GetHashCode() {
            int result = 0;
            if (this.propertyName != null)
                result ^= this.propertyName.GetHashCode();
            if (this.propertyType != null)
                result ^= this.propertyType.GetHashCode();
            if (this.propertyValue != null)
                result ^= this.propertyValue.GetHashCode();
            return result;
        }

        public override string ToString() {
            return String.Format("{0} ({1}): {2}", PropertyName, PropertyType == null ? "Null" : PropertyType.Name, PropertyValue == null ? "None" : PropertyValue.ToString());
        }
    }
}
