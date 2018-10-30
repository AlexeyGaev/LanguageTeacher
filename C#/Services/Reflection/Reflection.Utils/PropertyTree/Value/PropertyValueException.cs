using System;

namespace Reflection.Utils.PropertyTree {
    public class PropertyValueException : PropertyValue {
        public PropertyValueException(Exception value) 
            : base(value) {
        }

        public override bool HasChildren { get { return false; } }
        public override PropertyValueType Type { get { return PropertyValueType.Exception; } }
    }
}
