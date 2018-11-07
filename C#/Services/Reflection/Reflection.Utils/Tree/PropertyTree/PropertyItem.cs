using Reflection.Utils.Tree.Localization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Reflection.Utils.PropertyTree {
    public class PropertyItem : IEquatable<PropertyItem> {
        public static bool operator ==(PropertyItem left, PropertyItem right) {
            return left.Equals(right);
        }
        public static bool operator !=(PropertyItem left, PropertyItem right) {
            return !left.Equals(right);
        }

        readonly PropertyField field;
        readonly object value;
 
        public PropertyItem(PropertyField field, object value) {
            this.value = value;
            this.field = field;
        }
        
        public PropertyField Field { get { return this.field; } }
        public object Value { get { return this.value; } }
                
        public PropertyObjectChildren ObjectChildren { get; set; }
        public IEnumerable<PropertyItem> ArrayChildren { get; set; }
                
        public bool Equals(PropertyItem other) {
            return
                this.field == other.field &&
                this.value == other.value;
        }

        public override bool Equals(object obj) {
            return obj is PropertyItem && Equals((PropertyItem)obj);
        }

        public override int GetHashCode() {
            return this.field.GetHashCode() ^ this.value.GetHashCode();
        }

        public override string ToString() {
            return Value.ToString() + " " + ObjectChildren.ToString();
        }
    }
}
