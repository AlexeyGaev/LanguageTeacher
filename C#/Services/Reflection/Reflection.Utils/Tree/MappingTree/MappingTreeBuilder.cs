using System;
using System.Collections.Generic;
using System.Reflection;

namespace Reflection.Utils.Tree.MappingTree {
    public static class MappingTreeBuilder {
        public static TreeItem<Mapping> CreateItem(IEnumerable<TreeItem<Mapping>> parents, Mapping value) {
            return TreeBuilder<Mapping>.Create(
                parents,
                value,
                t => CanCreateChildren(t),
                t => ShouldCheckChildrenCycle(t),
                t => GetHasChildrenCycle(t),
                t => AddChildren(t));
        }
        
        static void AddChildren(TreeItem<Mapping> current) {
            foreach (PropertyInfo propertyInfo in current.Value.Value.Value.GetType().GetProperties()) { 
                TreeItem<Mapping> child = CreateItem(CreateChildParents(current), CreateChildValue(current, propertyInfo));
                if (CanAddChild(current, child))
                    current.AddChild(child);
            }
        }

        static bool CanCreateChildren(TreeItem<Mapping> item) {
            Mapping value = item.Value;
            return 
                value.Value.Value != null && 
                !(value.Value.Value is Exception) && 
                Type.GetTypeCode(value.Field.Type) != TypeCode.String;
        }

        static bool ShouldCheckChildrenCycle(TreeItem<Mapping> item) {
            return Type.GetTypeCode(item.Value.Field.Type) == TypeCode.Object;
        }

        static bool GetHasChildrenCycle(TreeItem<Mapping> current) {
            if (current.Parents == null)
                return false;
            object currentValue = current.Value.Value.Value;
            foreach (TreeItem<Mapping> parent in current.Parents)
                if (Object.ReferenceEquals(parent.Value.Value.Value, currentValue))
                    return true;
            return false;
        }

        #region internal
        static IEnumerable<TreeItem<Mapping>> CreateChildParents(TreeItem<Mapping> current) {
            List<TreeItem<Mapping>> result = new List<TreeItem<Mapping>>();
            if (current.Parents != null)
                result.AddRange(current.Parents);
            result.Add(current);
            return result;
        }
        static Mapping CreateChildValue(TreeItem<Mapping> current, PropertyInfo propertyInfo) {
            return new Mapping(new MappingField(propertyInfo.Name, propertyInfo.PropertyType), new MappingValue(CreatePropertyValue(propertyInfo, current)));
        }
        
        static bool CanAddChild(TreeItem<Mapping> current, TreeItem<Mapping> child) {
            return !(child.Value.Value.Value is TargetParameterCountException);
        }

        static object CreatePropertyValue(PropertyInfo propertyInfo, TreeItem<Mapping> mapping) {
            try {
                return propertyInfo.GetValue(mapping.Value.Value.Value);
            } catch (Exception e) {
                return e;
            }
        }
        #endregion 
    }
}
