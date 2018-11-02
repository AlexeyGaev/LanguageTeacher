using System;
using System.Collections.Generic;

namespace Reflection.Utils.PropertyTree {
    public class PropertyValueException : PropertyValue {
        public PropertyValueException(Exception value, IEnumerable<object> parents, IEnumerable<object> children) 
            : base(value, parents, children) {
        }

        public override PropertyValueType Type { get { return PropertyValueType.Exception; } }
    }
}
