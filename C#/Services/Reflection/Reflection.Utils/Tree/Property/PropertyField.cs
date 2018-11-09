using System;

namespace Reflection.Utils.PropertyTree {
    public struct PropertyField : IEquatable<PropertyField> {
        public static bool operator ==(PropertyField left, PropertyField right) {
            return left.Equals(right);
        }
        public static bool operator !=(PropertyField left, PropertyField right) {
            return !left.Equals(right);
        }

        object id;
        Type type;

        public PropertyField(object id, Type type) {
            this.id = id;
            this.type = type;
        }

        public object Id { get { return this.id; } }
        public Type Type { get { return this.type; } }

        public bool IsNullable { get { return this.type != null && Nullable.GetUnderlyingType(this.type) != null; } }

        public bool Equals(PropertyField other) {
            if ((object)other.id == null)
                return (object)this.id == null && this.type == other.type;
            else if ((object)this.id == null)
                return (object)other.id == null && this.type == other.type;
            return 
                this.id.Equals(other.id) &&
                this.type == other.type;
        }

        public override bool Equals(object obj) {
            return obj is PropertyField && Equals((PropertyField)obj);
        }

        public override int GetHashCode() {
            int result = 0;
            if (this.id != null)
                result ^= this.id.GetHashCode();
            if (this.type != null)
                result ^= this.type.GetHashCode();
            return result;
        }
    }
}
