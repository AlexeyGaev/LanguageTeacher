using System;

namespace Reflection.Utils.PropertyTree {
    public class PropertyValueStruct : PropertyValueNullable {
        public PropertyValueStruct(object value, bool isNullable) 
            : base(value, isNullable) {
        }

        public override bool HasChildren { get { return false; } }
        public override PropertyValueType Type { get { return PropertyValueType.Struct; } }
    }

}
