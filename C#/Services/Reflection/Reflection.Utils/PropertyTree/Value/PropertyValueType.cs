using System;

namespace Reflection.Utils.PropertyTree {
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
