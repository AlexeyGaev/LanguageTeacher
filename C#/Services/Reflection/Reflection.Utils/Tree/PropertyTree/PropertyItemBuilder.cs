using System;
using System.Collections.Generic;
using System.Reflection;

namespace Reflection.Utils.PropertyTree {
    public static class PropertyItemBuilder {
        public static PropertyItem Create(IEnumerable<PropertyItem> parents, PropertyField field, object value) {
            PropertyItem result = new PropertyItem(field, value);
            // TODO : array
            if (!CanCreateChildren(result))
                return result;
            if (ShouldCheckChildrenCycle(result)) {
                if (GetHasChildrenCycle(parents, result)) {
                    result.ObjectChildren = PropertyObjectChildren.Cycle;
                    return result;
                }
            }
            result.ObjectChildren = CreateChildren(parents, result);
            return result;
        }
        
        static PropertyObjectChildren CreateChildren(IEnumerable<PropertyItem> parents, PropertyItem current) {
            PropertyObjectChildren result = new PropertyObjectChildren();
            foreach (PropertyInfo propertyInfo in current.Value.GetType().GetProperties()) {
                PropertyItem child = Create(CreateObjectChildParents(parents, current), new PropertyField(propertyInfo.Name, propertyInfo.PropertyType), CreatePropertyValue(propertyInfo, current));
                if (CanAddChild(current, child))
                    result.Add(child);
            }
            return result;
        }

        static bool CanCreateChildren(PropertyItem item) {
            object value = item.Value;
            return
                value != null && 
                !(value is Exception) && 
                Type.GetTypeCode(item.Field.Type) != TypeCode.String;
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
        
        static bool CanAddChild(PropertyItem current, PropertyItem child) {
            return !(child.Value is TargetParameterCountException);
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
