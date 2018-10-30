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
        public static IPropertyValue Create(Tree<PropertyDescription> tree, Type type, object value) {
            if (type == typeof(string))
                return new PropertyValueString((string)value);
            if (type.IsValueType) {
                if (type.IsEnum) 
                    return new PropertyValueEnum(value, type, Enum.GetUnderlyingType(type) == type);
                if (type.IsPrimitive)
                    return new PropertyValuePrimitive(value, Nullable.GetUnderlyingType(type) == type);
                return new PropertyValueStruct(value, Nullable.GetUnderlyingType(type) == type); 
            }
            if (type.IsClass)
                return new PropertyValueClass(value, tree == null ? false : !ContainsByOwner(tree, value));
           
            return new PropertyValueUndefined(value);
        }

        public static IPropertyValue Create(Tree<PropertyDescription> tree, object owner, PropertyInfo propertyInfo) {
            try {
                object value = propertyInfo.GetValue(owner);
                return Create(tree, propertyInfo.PropertyType, value);
            } catch (Exception e) {
                return new PropertyValueException(e);
            }
        }

        static bool ContainsByOwner(Tree<PropertyDescription> tree, object owner) {
            if (owner.GetType().IsValueType)
                return false;
            return ContainsByOwnerCore(tree.RootItem.Children, owner);
        }

        static bool ContainsByOwnerCore(List<TreeItem<PropertyDescription>> items, object owner) {
            if (owner.GetType().IsValueType)
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
}
