using System;

namespace Reflection.Utils.PropertyTree {
    public class PropertyValuePrimitive : PropertyValueNullable {
        public PropertyValuePrimitive(object value, bool isNullable) 
            : base(value, isNullable) {
        }
        public override bool HasChildren { get { return false; } }
        public override PropertyValueType Type { get { return PropertyValueType.Primitive; } }
    }
}
