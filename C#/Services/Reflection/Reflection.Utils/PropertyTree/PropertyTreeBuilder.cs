using System;
using System.Collections.Generic;
using System.Reflection;
using Reflection.Utils.Tree;

namespace Reflection.Utils.PropertyTree {
    public static class PropertyTreeBuilder {
        public static TreeItem<PropertyDescription> CreateItem(IEnumerable<TreeItem<PropertyDescription>> parents, PropertyDescription value) {
            return TreeBuilder<PropertyDescription>.Create(
                parents,
                value,
                t => CanCreateChildren(t),
                t => ShouldCheckChildrenCycle(t),
                t => GetHasChildrenCycle(t),
                t => CreateChildren(t));
        }
        
        static IEnumerable<TreeItem<PropertyDescription>> CreateChildren(TreeItem<PropertyDescription> current) {
            List<TreeItem<PropertyDescription>> result = new List<TreeItem<PropertyDescription>>();
            foreach(PropertyInfo propertyInfo in current.Value.PropertyValue.GetType().GetProperties()) { 
                TreeItem<PropertyDescription> child = CreateItem(CreateChildParents(current), CreateChildValue(current, propertyInfo));
                if (CanAddChild(current, child))
                    result.Add(child);
            }
            return result;
        }

        static bool CanCreateChildren(TreeItem<PropertyDescription> item) {
            PropertyDescription value = item.Value;
            return 
                value.PropertyValue != null && 
                !value.IsException && 
                Type.GetTypeCode(value.PropertyType) != TypeCode.String;
        }

        static bool ShouldCheckChildrenCycle(TreeItem<PropertyDescription> item) {
            return Type.GetTypeCode(item.Value.PropertyType) == TypeCode.Object;
        }

        static bool GetHasChildrenCycle(TreeItem<PropertyDescription> current) {
            if (current.Parents == null)
                return false;
            object currentValue = current.Value.PropertyValue;
            foreach (TreeItem<PropertyDescription> parent in current.Parents)
                if (Object.ReferenceEquals(parent.Value.PropertyValue, currentValue))
                    return true;
            return false;
        }

        #region internal
        static IEnumerable<TreeItem<PropertyDescription>> CreateChildParents(TreeItem<PropertyDescription> current) {
            List<TreeItem<PropertyDescription>> result = new List<TreeItem<PropertyDescription>>();
            if (current.Parents != null)
                result.AddRange(current.Parents);
            result.Add(current);
            return result;
        }
        static PropertyDescription CreateChildValue(TreeItem<PropertyDescription> current, PropertyInfo propertyInfo) {
            return new PropertyDescription(propertyInfo.Name, propertyInfo.PropertyType, CreatePropertyValue(propertyInfo, current.Value.PropertyValue));
        }

        static bool CanAddChild(TreeItem<PropertyDescription> current, TreeItem<PropertyDescription> child) {
            return !(child.Value.PropertyValue is TargetParameterCountException);
        }

        static object CreatePropertyValue(PropertyInfo propertyInfo, object owner) {
            try {
                return propertyInfo.GetValue(owner);
            } catch (Exception e) {
                return e;
            }
        }
        #endregion 
    }
}
