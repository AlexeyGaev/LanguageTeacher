using System;

namespace Reflection.Utils.PropertyTree {
    public class PropertyValueClass : PropertyValue {
        bool hasChildren;

        public PropertyValueClass(object value, bool hasChildren) 
            : base(value) {
            this.hasChildren = hasChildren;
        }

        public override bool HasChildren { get { return this.hasChildren; } }
        public override PropertyValueType Type { get { return PropertyValueType.Class; } }
    }
}
