using System;
using System.Collections.Generic;

namespace Reflection.Utils.PropertyTree {
    public class PropertyValueUndefined : PropertyValue {
        public PropertyValueUndefined(object value, IEnumerable<object> parents, IEnumerable<object> children) 
            : base(value, parents, children) {
        }
        public override PropertyValueType Type { get { return PropertyValueType.Undefined; } }
    }
}
