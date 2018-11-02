using System;
using System.Collections.Generic;
using System.Reflection;
using Reflection.Utils.Tree;

namespace Reflection.Utils.PropertyTree {
    // TODO:
    //IsPrimitive: Boolean, Byte, SByte, Int16, UInt16, Int32, UInt32, Int64, UInt64, IntPtr, UIntPtr, Char, Double, Single.
    //struct
    //IsEnum
    //Nullable: struct, IsPrimitive, IsEnum
    //IsValueType: IsPrimitive, struct, IsEnum
    //IsGenericType
    //string 
    //IsArray
    //IEnumerable

    //[Test]
    //public void Test() {
    //    var t = typeof(int?);
    //    var t1 = typeof(int);

    //    Assert.That(Nullable.GetUnderlyingType(t) == typeof(int));
    //    Assert.That(Nullable.GetUnderlyingType(t1) == null);
    //}

    public static class PropertyValueBuilder {
        public static PropertyValue Create(Tree<PropertyDescription> tree, object owner, PropertyInfo propertyInfo, ParentValues parents) {
            try {
                object value = propertyInfo.GetValue(owner);
                return CreateCore(tree, propertyInfo.PropertyType, value, parents);
            } catch (Exception e) {
                return new PropertyValueException(e, parents);
            }
        }

        static bool IsValidClass(object value) {
            if (value == null)
                return false;
            Type valueType = value.GetType();
            if (valueType.IsValueType)
                return false;
            return true;
        }

        public static PropertyValue CreateCore(Tree<PropertyDescription> tree, Type type, object value, ParentValues parents) {
            if (type == null)
                return null;
            if (type == typeof(string))
                return new PropertyValueString((string)value, parents);
            if (type.IsClass) 
                return new PropertyValueClass(value, parents);
                
            Type underlyingType = Nullable.GetUnderlyingType(type);
            if (underlyingType != null) 
                return CreateValueBased(value, underlyingType, true, parents);
            if (type.IsValueType) 
                return CreateValueBased(value, type, false, parents);
            return new PropertyValueUndefined(value, parents);
        }

        static PropertyValueNullable CreateValueBased(object value, Type type, bool isNullable, ParentValues parents) {
            if (type.IsPrimitive)
                return new PropertyValuePrimitive(value, isNullable, parents);
            if (type.IsEnum)
                return new PropertyValueEnum(value, isNullable, type, parents);
            return new PropertyValueStruct(value, isNullable, parents);
        }

        static bool ContainsByOwner(Tree<PropertyDescription> tree, object owner) {
            return ContainsByOwnerCore(tree.RootItem.Children, owner);
        }

        static bool ContainsByOwnerCore(List<TreeItem<PropertyDescription>> items, object owner) {
            if (owner == null || owner.GetType().IsValueType)
                return false;
            foreach (TreeItem<PropertyDescription> item in items) {
                PropertyDescription parentProperty = item.Value;
                object parentOwner = parentProperty.Owner;
                Type parentType = parentOwner.GetType();
                if (parentType.IsValueType)
                    continue;
                if (IsEqualOwners(parentOwner, owner))
                    return true;
                if (ContainsByOwnerCore(item.Children, owner))
                    return true;
            }
            return false;
        }

        static bool IsEqualOwners(object parent, object owner) {
            return Object.ReferenceEquals(parent, owner);
        }
    }

    //public class TreeWalker<T> {
    //    readonly TreeItem<T> rootItem;
    //    public PropertyTreeWalker(TreeItem<T> rootItem) {
    //        this.rootItem = rootItem;
    //    }

    //    public bool NextChild() {
    //        if ()
    //    }

    //}
}
