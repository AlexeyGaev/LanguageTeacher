using System;

namespace Reflection.Utils.PropertyTree {
    public class PropertyValueEnum : PropertyValueNullable {
        readonly Type enumType;

        public PropertyValueEnum(object value, bool isNullable, Type enumType) 
            : base(value, isNullable) {
            this.enumType = enumType;
        }

        public Type EnumType { get { return this.enumType; } }
        public override bool HasChildren { get { return false; } }
        public override PropertyValueType Type { get { return PropertyValueType.Enum; } }
    }

}
