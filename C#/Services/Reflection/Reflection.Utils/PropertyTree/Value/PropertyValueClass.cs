using System;

namespace Reflection.Utils.PropertyTree {
    public class PropertyValueClass : PropertyValue {
        public PropertyValueClass(object value, ParentValues parents) 
            : base(value, parents) {
        }

        public override bool HasChildren { get { return HasParents && Parents.Contains(Value); } }
        public override PropertyValueType Type { get { return PropertyValueType.Class; } }
    }
}
