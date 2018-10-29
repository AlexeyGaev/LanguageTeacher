using System;

namespace Translation.Http.Tree {
    public class PropertyValue {
        public static PropertyValue CreatePrimitive(object value) {
            PropertyValue result = new PropertyValue();
            result.isPrimitive = true;
            result.value = value;
            return result;
        }

        public static PropertyValue CreateStruct(object value) {
            PropertyValue result = new PropertyValue();
            result.isStruct = true;
            result.value = value;
            return result;
        }

        public static PropertyValue CreateString(string value) {
            PropertyValue result = new PropertyValue();
            result.isString = true;
            result.value = value;
            return result;
        }

        public static PropertyValue CreateEnum(object value, Type enumType) {
            PropertyValue result = new PropertyValue();
            result.isEnum = true;
            result.enumType = enumType;
            result.value = value;
            return result;
        }

        public static PropertyValue CreateClass(object value, bool fromParent) {
            PropertyValue result = new PropertyValue();
            result.fromParent = fromParent;
            result.value = value;
            return result;
        }

        public static PropertyValue CreateException(Exception value) {
            PropertyValue result = new PropertyValue();
            result.isException = true;
            result.value = value;
            return result;
        }

        public static PropertyValue CreateUndefined(object value) {
            PropertyValue result = new PropertyValue();
            result.undefined = true;
            result.value = value;
            return result;
        }

        object value;

        Type enumType;

        bool undefined;
        bool isException;
        bool isPrimitive;
        bool isStruct;
        bool isString;
        bool isEnum;

        bool fromParent;

        PropertyValue() { }

        public object Value { get { return this.value; } }
        
        public bool IsPrimitive { get { return this.isPrimitive; } }
        public bool IsStruct { get { return this.isStruct; } }
        public bool IsException { get { return this.isException; } }
        public bool IsString { get { return this.isString; } }
        public bool IsEnum { get { return this.isEnum; } }
        public bool Undefined { get { return this.undefined; } }

        public bool FromParent { get { return this.fromParent; } }

        public Type EnumType { get { return this.enumType; } }

        public bool HasChildren {
            get {
                return
                    !this.undefined &&
                    !this.isPrimitive &&
                    !this.isException &&
                    !this.isEnum &&
                    !this.isString &&
                    !this.fromParent;
            }
        }
    }

    public class PropertyValueUndefined {

    }

    public class PropertyValuePrimitive<T> where T : struct {

    }

    public class PropertyValueStruct<T> where T : struct {

    }

    public class PropertyValueClass {

    }

    public class PropertyValueException {

    }

    public class PropertyValueEnum<T> where T : struct {

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
        NullablePrimitive,
        Enum,
        NullableEnum,
        Struct,
        NullableStruct,
        Exception,
        Class,
        Interface,
        Array,
        Enumerable
    }
}
