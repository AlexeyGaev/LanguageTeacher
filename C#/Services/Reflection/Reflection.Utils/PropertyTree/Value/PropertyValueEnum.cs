using System;
using System.Collections.Generic;

namespace Reflection.Utils.PropertyTree {
    public class PropertyValueEnum : PropertyValueNullable {
        readonly Type enumType;

        public PropertyValueEnum(object value, bool isNullable, Type enumType, IEnumerable<object> parents, IEnumerable<object> children) 
            : base(value, isNullable, parents, children) {
            this.enumType = enumType;
        }

        public Type EnumType { get { return this.enumType; } }
        public override PropertyValueType Type { get { return PropertyValueType.Enum; } }
    }

}
