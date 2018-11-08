using System;

namespace Reflection.Utils.PropertyTree {
    //public struct Mapping : IEquatable<Mapping> {
    //    public static bool operator ==(Mapping left, Mapping right) {
    //        return left.Equals(right);
    //    }
    //    public static bool operator !=(Mapping left, Mapping right) {
    //        return !left.Equals(right);
    //    }

    //    PropertyField field;
    //    PropertyValue value;

    //    public Mapping(PropertyField field, PropertyValue value) {
    //        this.field = field;
    //        this.value = value;
    //    }

    //    public PropertyField Field { get { return this.field; } }
    //    public PropertyValue Value { get { return this.value; } }
        
    //    public bool Equals(Mapping other) {
    //        return
    //            this.field == other.field &&
    //            this.value == other.value;
    //    }

    //    public override bool Equals(object obj) {
    //        return obj is Mapping && Equals((Mapping)obj);
    //    }

    //    public override int GetHashCode() {
    //        return this.field.GetHashCode() ^ this.value.GetHashCode();
    //    }

    //    public override string ToString() {
    //        return String.Format("({0}) : ({1})", this.field.ToString(), this.value.ToString());
    //    }
    //}
}
