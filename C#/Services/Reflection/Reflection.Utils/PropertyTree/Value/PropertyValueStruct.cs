using System;
using System.Collections.Generic;

namespace Reflection.Utils.PropertyTree {
    public class PropertyValueStruct : PropertyValueNullable {
        public PropertyValueStruct(object value, bool isNullable, IEnumerable<object> parents, IEnumerable<object> children) 
            : base(value, isNullable, parents, children) {
        }

        public override PropertyValueType Type { get { return PropertyValueType.Struct; } }
    }

}
