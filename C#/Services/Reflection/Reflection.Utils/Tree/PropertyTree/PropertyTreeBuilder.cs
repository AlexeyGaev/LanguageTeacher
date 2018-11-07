using Reflection.Utils.Tree;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Reflection.Utils.PropertyTree {
    public static class PropertyTreeBuilder {
        public static PropertyTreeItem CreateItem(IEnumerable<PropertyTreeItem> parents, PropertyField field, object value) {
            PropertyTreeItem result = new PropertyTreeItem(parents, field, value);
            if (!CanCreateChildren(result))
                return result;
            if (ShouldCheckChildrenCycle(result)) {
                if (GetHasChildrenCycle(result)) {
                    result.SetHasObjectCycle(true);
                    return result;
                }
            }
            AddChildren(result);
            return result;
        }
        
        static void AddChildren(PropertyTreeItem current) {
            foreach (PropertyInfo propertyInfo in current.Value.GetType().GetProperties()) {
                PropertyField childField = new PropertyField(propertyInfo.Name, propertyInfo.PropertyType);
                PropertyTreeItem child = CreateItem(CreateObjectChildParents(current), childField, CreatePropertyValue(propertyInfo, current));
                if (CanAddChild(current, child))
                    current.AddObjectChild(child);
            }
        }

        static bool CanCreateChildren(PropertyTreeItem item) {
            object value = item.Value;
            return
                value != null && 
                !(value is Exception) && 
                Type.GetTypeCode(item.Field.Type) != TypeCode.String;
        }

        static bool ShouldCheckChildrenCycle(PropertyTreeItem item) {
            return Type.GetTypeCode(item.Field.Type) == TypeCode.Object;
        }

        static bool GetHasChildrenCycle(PropertyTreeItem current) {
            if (current.ObjectParents == null)
                return false;
            object currentValue = current.Value;
            foreach (PropertyTreeItem parent in current.ObjectParents)
                if (Object.ReferenceEquals(parent.Value, currentValue))
                    return true;
            return false;
        }

        #region internal
        static IEnumerable<PropertyTreeItem> CreateObjectChildParents(PropertyTreeItem current) {
            List<PropertyTreeItem> result = new List<PropertyTreeItem>();
            if (current.ObjectParents != null)
                result.AddRange(current.ObjectParents);
            result.Add(current);
            return result;
        }
        static Mapping CreateChildValue(PropertyTreeItem current, PropertyInfo propertyInfo) {
            return new Mapping(new PropertyField(propertyInfo.Name, propertyInfo.PropertyType), new PropertyValue(CreatePropertyValue(propertyInfo, current)));
        }
        
        static bool CanAddChild(PropertyTreeItem current, PropertyTreeItem child) {
            return !(child.Value is TargetParameterCountException);
        }

        static object CreatePropertyValue(PropertyInfo propertyInfo, PropertyTreeItem item) {
            try {
                return propertyInfo.GetValue(item.Value);
            } catch (Exception e) {
                return e;
            }
        }
        #endregion 
    }
}
