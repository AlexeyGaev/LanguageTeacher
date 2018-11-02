using System;

namespace Reflection.Utils.PropertyTree {
    public class PropertyValueStruct : PropertyValueNullable {
        public PropertyValueStruct(object value, bool isNullable, ParentValues parents) 
            : base(value, isNullable, parents) {
        }

        public override bool HasChildren { get { return false; } }
        public override PropertyValueType Type { get { return PropertyValueType.Struct; } }
    }

}
