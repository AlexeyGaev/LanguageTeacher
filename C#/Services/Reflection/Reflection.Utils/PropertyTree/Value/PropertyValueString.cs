using System;

namespace Reflection.Utils.PropertyTree {
    public class PropertyValueString : PropertyValue {
        public PropertyValueString(string value, ParentValues parents) 
            : base(value, parents) {
        }

        public override bool HasChildren { get { return false; } }
        public override PropertyValueType Type { get { return PropertyValueType.String; } }
    }
}
