﻿using System;

namespace Reflection.Utils.PropertyTree {
    public class PropertyValueUndefined : PropertyValue {
        public PropertyValueUndefined(object value, ParentValues parents) 
            : base(value, parents) {
        }
        public override bool HasChildren { get { return false; } }
        public override PropertyValueType Type { get { return PropertyValueType.Undefined; } }
    }
}
