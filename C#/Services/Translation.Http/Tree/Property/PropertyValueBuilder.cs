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

    public static class PropertyValueBuilder {
        public static PropertyValue Create(Tree<PropertyDescription> tree, object owner, PropertyInfo propertyInfo) {
            try {
                object value = propertyInfo.GetValue(owner);
                Type propertyType = propertyInfo.PropertyType;
                if (propertyType.IsPrimitive)
                    return PropertyValue.CreatePrimitive(value);
                if (propertyType.IsEnum) 
                    return PropertyValue.CreateEnum(value, propertyType);
                if (propertyType == typeof(string))
                    return PropertyValue.CreateString((string)value);
                if (propertyType.IsClass) 
                    return PropertyValue.CreateClass(value, ContainsByOwner(tree, value));
                // TODO : StackOverflow
                //if (propertyType.IsValueType)
                //    return PropertyValue.CreateStruct(value); 
                return PropertyValue.CreateUndefined(value);
            } catch (Exception e) {
                return PropertyValue.CreateException(e);
            }
        }

        static bool ContainsByOwner(Tree<PropertyDescription> tree, object owner) {
            if (owner.GetType().IsValueType)
                return false;
            return ContainsByOwnerCore(tree.RootItems, owner);
        }

        static bool ContainsByOwnerCore(List<Item<PropertyDescription>> items, object owner) {
            if (owner.GetType().IsValueType)
                return false;
            foreach (Item<PropertyDescription> item in items) {
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
