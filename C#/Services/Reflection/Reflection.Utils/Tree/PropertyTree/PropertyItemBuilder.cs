using System;
using System.Collections.Generic;
using System.Reflection;

namespace Reflection.Utils.PropertyTree {
    public static class PropertyItemBuilder {
        public static PropertyItem Create(IEnumerable<PropertyItem> parents, PropertyField field, object value) {
            PropertyItem result = new PropertyItem(field, value);
            result.ObjectChildren = CreateObjectChildren(parents, result);
            result.ArrayChildren = CreateArrayChildren(result);
            return result;
        }
        
        static PropertyObjectChildren CreateObjectChildren(IEnumerable<PropertyItem> parents, PropertyItem current) {
            if (!CanCreateChildren(current))
                return PropertyObjectChildren.Empty;
            if (ShouldCheckChildrenCycle(current) && GetHasChildrenCycle(parents, current))
                return PropertyObjectChildren.Cycle;
            PropertyInfo[] properties = current.Value.GetType().GetProperties();
            if (properties == null)
                return PropertyObjectChildren.Empty;
            int count = properties.Length;
            if (count == 0)
                return PropertyObjectChildren.Empty;
            List<PropertyItem> children = new List<PropertyItem>();
            foreach (PropertyInfo propertyInfo in current.Value.GetType().GetProperties()) {
                PropertyItem child = Create(CreateObjectChildParents(parents, current), new PropertyField(propertyInfo.Name, propertyInfo.PropertyType), CreatePropertyValue(propertyInfo, current));
                if (CanAddChild(current, child))
                    children.Add(child);
            }
            return new PropertyObjectChildren(children);
        }

        static PropertyArrayChildren CreateArrayChildren(PropertyItem current) {
            return PropertyArrayChildren.Empty;
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
