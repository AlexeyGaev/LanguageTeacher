using System;
using System.Collections.Generic;

namespace Reflection.Utils.PropertyTree {
    public class PropertyValueString : PropertyValue {
        public PropertyValueString(string value, IEnumerable<object> parents, IEnumerable<object> children) 
            : base(value, parents, children) {
        }

        public override PropertyValueType Type { get { return PropertyValueType.String; } }
    }
}
