using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Reflection.Utils.PropertyTree {
    public static class PropertyItemBuilder {
        public static PropertyItem Create(PropertyField propertyField, object propertyValue, IEnumerable<PropertyItem> parents = null) {
            PropertyItem result = new PropertyItem(propertyField, propertyValue);
            Type fieldType = propertyField.Type;
            if (fieldType == null || propertyValue == null || fieldType == typeof(string))
                return result;
            if (fieldType.IsPrimitive || fieldType.IsEnum)
                return result;
            Type underlyingType = Nullable.GetUnderlyingType(fieldType);
            if (underlyingType != null && (underlyingType.IsPrimitive || underlyingType.IsEnum))
                return result;
            if (!fieldType.IsArray)  
                CreateObjectChildren(parents, result);
            CreateArrayChildren(parents, result);
            return result;
        }

        static void CreateArrayChildren(IEnumerable<PropertyItem> parents, PropertyItem current) {
            object propertyValue = current.Value;
            if (!(propertyValue is IEnumerable))
                return;
            int index = 0;
            List<PropertyItem> children = new List<PropertyItem>();
            foreach (object item in (IEnumerable)propertyValue) {
                PropertyField propertyField = new PropertyField(index, item.GetType());
                IEnumerable<PropertyItem> childParents = CreateObjectChildParents(parents, current);
                PropertyItem child = Create(propertyField, item, childParents);
                children.Add(child);
                index++;
            }
            current.ArrayChildren = children;
        }

        static void CreateObjectChildren(IEnumerable<PropertyItem> parents, PropertyItem current) {
            if (ShouldCheckChildrenCycle(current) && GetHasChildrenCycle(parents, current)) {
                current.ObjectChildren = PropertyObjectChildren.Cycle;
                return;
            }
            PropertyInfo[] propertyInfos = current.Value.GetType().GetProperties();
            if (propertyInfos == null)
                return;
            List<PropertyItem> children = new List<PropertyItem>();
            foreach (PropertyInfo propertyInfo in propertyInfos) {
                if (propertyInfo.CanRead) {
                    ParameterInfo[] indexParameters = propertyInfo.GetIndexParameters();
                    if (indexParameters.Length == 0) {
                        PropertyField propertyField = new PropertyField(propertyInfo.Name, propertyInfo.PropertyType);
                        object childValue = CreatePropertyValue(propertyInfo, current);
                        IEnumerable<PropertyItem> childParents = CreateObjectChildParents(parents, current);
                        PropertyItem child = Create(propertyField, childValue, childParents);
                        children.Add(child);
                    }
                }
            }
            current.ObjectChildren = new PropertyObjectChildren(children);
        }
        
        static bool ShouldCheckChildrenCycle(PropertyItem item) {
            return Type.GetTypeCode(item.Field.Type) == TypeCode.Object;
        }

        static bool GetHasChildrenCycle(IEnumerable<PropertyItem> parents, PropertyItem current) {
            if (parents == null)
                return false;
            object currentValue = current.Value;
            foreach (PropertyItem parent in parents)
                if (Object.ReferenceEquals(parent.Value, currentValue))
                    return true;
            return false;
        }

        static IEnumerable<PropertyItem> CreateObjectChildParents(IEnumerable<PropertyItem> parents, PropertyItem current) {
            List<PropertyItem> result = new List<PropertyItem>();
            if (parents != null)
                result.AddRange(parents);
            result.Add(current);
            return result;
        }
      
        static object CreatePropertyValue(PropertyInfo propertyInfo, PropertyItem item) {
            try {
                return propertyInfo.GetValue(item.Value);
            } catch (Exception e) {
                return e;
            }
        }
    }
}
