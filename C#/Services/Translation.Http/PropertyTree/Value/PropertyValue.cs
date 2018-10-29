using System;

namespace Translation.Http.PropertyTree {
    public interface IPropertyValue {
        object Value { get; }
        PropertyValueType Type { get; }
        bool HasChildren { get; }
    }

    public class PropertyValueUndefined : IPropertyValue {
        readonly object value;
        public PropertyValueUndefined(object value) {
            this.value = value;
        }
        public object Value { get { return this.value; } }
        public PropertyValueType Type { get { return PropertyValueType.Undefined; } }
        public bool HasChildren { get { return false; } }
    }

    public class PropertyValuePrimitive : IPropertyValue {
        object value;
        bool isNullable;

        public PropertyValuePrimitive(object value, bool isNullable) {
            this.value = value;
            this.isNullable = isNullable;
        }

        public object Value { get { return this.value; } }
        public bool IsNullable { get { return this.isNullable; } }
        public bool HasChildren { get { return false; } }
        public PropertyValueType Type { get { return PropertyValueType.Primitive; } }
    }

    public class PropertyValueStruct : IPropertyValue {
        object value;
        bool isNullable;

        public PropertyValueStruct(object value, bool isNullable) {
            this.value = value;
            this.isNullable = isNullable;
        }

        public object Value { get { return this.value; } }
        public bool IsNullable { get { return this.isNullable; } }
        public bool HasChildren { get { return false; } }
        public PropertyValueType Type { get { return PropertyValueType.Struct; } }
    }

    public class PropertyValueString : IPropertyValue {
        string value;
        
        public PropertyValueString(string value) {
            this.value = value;
        }

        public object Value { get { return this.value; } }
        public bool HasChildren { get { return false; } }
        public PropertyValueType Type { get { return PropertyValueType.String; } }
    }

    public class PropertyValueClass : IPropertyValue {
        object value;
        bool hasChildren;

        public PropertyValueClass(object value, bool hasChildren) {
            this.value = value;
            this.hasChildren = hasChildren;
        }

        public object Value { get { return this.value; } }
        public bool HasChildren { get { return this.hasChildren; } }
        public PropertyValueType Type { get { return PropertyValueType.Class; } }
    }

    public class PropertyValueException : IPropertyValue {
        Exception value;

        public PropertyValueException(Exception value) {
            this.value = value;
        }

        public object Value { get { return this.value; } }
        public bool HasChildren { get { return false; } }
        public PropertyValueType Type { get { return PropertyValueType.Exception; } }
    }

    public class PropertyValueEnum : IPropertyValue {
        readonly Type enumType;
        readonly object value;

        bool isNullable;

        public PropertyValueEnum(object value, Type enumType, bool isNullable)  {
            this.value = value;
            this.enumType = enumType;
            this.isNullable = isNullable;
        }

        public object Value { get { return this.value; } }
        public Type EnumType { get { return this.enumType; } }
        public bool HasChildren { get { return false; } }
        public PropertyValueType Type { get { return PropertyValueType.Enum; } }
    }

    public enum PrimitiveValueType {
        Boolean,
        Byte,
        SByte,
        Int16,
        UInt16,
        Int32,
        UInt32,
        Int64,
        UInt64,
        IntPtr,
        UIntPtr,
        Char,
        Double,
        Single
    }

    public enum PropertyValueType {
        Undefined,
        Primitive,
        Enum,
        Struct,
        String,
        Exception,
        Class,
        Interface,
        Array,
        Enumerable
    }
}
