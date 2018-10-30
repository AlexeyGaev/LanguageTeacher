using System;

namespace Reflection.Utils.PropertyTree {
    public interface IPropertyValue {
        object Value { get; }
        PropertyValueType Type { get; }
        bool HasChildren { get; }
    }
}
