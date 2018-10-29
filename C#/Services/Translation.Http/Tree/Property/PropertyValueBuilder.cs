using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Translation.Http.Tree {
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
        public static PropertyValue Create(Tree<PropertyDescription> tree, Type type, object value) {
            if (type.IsPrimitive)
                return PropertyValue.CreatePrimitive(value);
            if (type.IsEnum)
                return PropertyValue.CreateEnum(value, type);
            if (type == typeof(string))
                return PropertyValue.CreateString((string)value);
            if (type.IsClass)
                return PropertyValue.CreateClass(value, tree == null ? false : ContainsByOwner(tree, value));
            // TODO : StackOverflow
            //if (propertyType.IsValueType)
            //    return PropertyValue.CreateStruct(value); 
            return PropertyValue.CreateUndefined(value);
        }

        public static PropertyValue Create(Tree<PropertyDescription> tree, object owner, PropertyInfo propertyInfo) {
            try {
                object value = propertyInfo.GetValue(owner);
                return Create(tree, propertyInfo.PropertyType, value);
            } catch (Exception e) {
                return PropertyValue.CreateException(e);
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
