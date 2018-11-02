using System;
using System.Collections.Generic;

namespace Reflection.Utils.PropertyTree {
    public class PropertyValuePrimitive : PropertyValueNullable {
        public PropertyValuePrimitive(object value, bool isNullable, IEnumerable<object> parents, IEnumerable<object> children) 
            : base(value, isNullable, parents, children) {
        }
        public override PropertyValueType Type { get { return PropertyValueType.Primitive; } }
    }
}
