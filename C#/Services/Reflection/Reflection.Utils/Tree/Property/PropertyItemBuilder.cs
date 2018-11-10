using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Reflection.Utils.PropertyTree {
    public static class PropertyItemBuilder {
        public static PropertyItem Create(PropertyField propertyField, object propertyValue, IEnumerable<object> parents = null) {
            PropertyItem result = new PropertyItem(propertyField, propertyValue);
            Type type = propertyField.Type;
            if (type == null || propertyValue == null || type == typeof(string))
                return result;
            Type underlyingType = Nullable.GetUnderlyingType(type);
            if (underlyingType != null)
                type = underlyingType;
            if (type.IsPrimitive || type.IsEnum)
                return result;
            TypeCode typeCode = Type.GetTypeCode(type);
            if (typeCode != TypeCode.DateTime && typeCode != TypeCode.Object)
                return result;
            if (!type.IsArray)
                result.ObjectChildren = CreateObjectChildren(parents, propertyValue, type.GetProperties());
            result.ArrayChildren = CreateArrayChildren(parents, propertyValue as IEnumerable);
            return result;
        }

        static IEnumerable<PropertyItem> CreateArrayChildren(IEnumerable<object> parents, IEnumerable enumerable) {
            if (enumerable == null)
                return null;
            int index = 0;
            List<PropertyItem> children = new List<PropertyItem>();
            foreach (object item in enumerable) {
                PropertyField propertyField = new PropertyField(index, item.GetType());
                IEnumerable<object> childParents = CreateObjectChildParents(parents, enumerable);
                PropertyItem child = Create(propertyField, item, childParents);
                children.Add(child);
                index++;
            }
            return children;
        }

        static PropertyObjectChildren CreateObjectChildren(IEnumerable<object> parents, object currentValue, PropertyInfo[] propertyInfos) {
            if (GetHasChildrenCycle(parents, currentValue)) 
                return PropertyObjectChildren.Cycle;
            if (propertyInfos == null)
                return null;
            List<PropertyItem> children = new List<PropertyItem>();
            foreach (PropertyInfo propertyInfo in propertyInfos) {
                if (propertyInfo.CanRead) {
                    ParameterInfo[] indexParameters = propertyInfo.GetIndexParameters();
                    if (indexParameters.Length == 0) {
                        PropertyField propertyField = new PropertyField(propertyInfo.Name, propertyInfo.PropertyType);
                        object childValue = CreatePropertyValue(propertyInfo, currentValue);
                        IEnumerable<object> childParents = CreateObjectChildParents(parents, currentValue);
                        PropertyItem child = Create(propertyField, childValue, childParents);
                        children.Add(child);
                    }
                }
            }
            return new PropertyObjectChildren(children);
        }
        
        static bool GetHasChildrenCycle(IEnumerable<object> parents, object currentValue) {
            if (parents == null)
                return false;
            foreach (object parent in parents) {
                Type currentType = currentValue.GetType();
                if (currentType.IsValueType) {
                    if (parent.GetType() == currentType)
                        return true;
                }
                else if (Object.ReferenceEquals(parent, currentValue))
                    return true;
            }
            return false;
        }

        static IEnumerable<object> CreateObjectChildParents(IEnumerable<object> parents, object value) {
            List<object> result = new List<object>();
            if (parents != null)
                result.AddRange(parents);
            result.Add(value);
            return result;
        }
      
        static object CreatePropertyValue(PropertyInfo propertyInfo, object value) {
            try {
                return propertyInfo.GetValue(value);
            } catch (Exception e) {
                return e;
            }
        }
    }
}
