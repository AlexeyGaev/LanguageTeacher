using System;
using System.Collections;

namespace Reflection.Utils.PropertyTree {
    public class PropertyDescription : IFormattable, IEquatable<PropertyDescription> {
        public static bool operator ==(PropertyDescription left, PropertyDescription right) {
            if ((object)left == null)
                return (object)right == null;
            if ((object)right == null)
                return (object)left == null;
            return left.Equals(right);
        }
        public static bool operator !=(PropertyDescription left, PropertyDescription right) {
            if ((object)left == null)
                return (object)right != null;
            if ((object)right == null)
                return (object)left != null;
            return !left.Equals(right);
        }

        readonly string propertyName;
        readonly Type propertyType;
        readonly object propertyValue;

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
            return
                (object)other != null &&
                String.Compare(this.propertyName, other.propertyName, StringComparison.InvariantCulture) == 0 &&
                this.propertyType.Equals(other.propertyType) &&
                this.propertyValue.Equals(other.propertyValue);
        }

        public override bool Equals(object obj) {
            return obj is PropertyDescription && Equals((PropertyDescription)obj);
        }

        public override int GetHashCode() {
            return
                this.propertyName.GetHashCode() ^
                this.propertyType.GetHashCode() ^
                this.propertyValue.GetHashCode();
        }

        public override string ToString() {
            return String.Format("{0} : {1}" , PropertyName, PropertyValue == null ? "None" : PropertyValue);
        }

        public string ToString(string format, IFormatProvider formatProvider) {
            throw new NotImplementedException();
        }
    }
}
