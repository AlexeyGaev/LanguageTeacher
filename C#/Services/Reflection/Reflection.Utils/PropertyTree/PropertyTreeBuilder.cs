using System;
using System.Collections.Generic;
using System.Linq;
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

    public static class PropertyTreeBuilder {
        public static TreeItem<PropertyDescription> CreateItem(IEnumerable<TreeItem<PropertyDescription>> parents, string propertyName, Type propertyType, object propertyOwner, object propertyValue, bool hasException) {
            TreeItem<PropertyDescription> result = new TreeItem<PropertyDescription>();
            result.Parents = parents;
            if (propertyName == null)
                return result;
            
            PropertyDescription description = new PropertyDescription(propertyName);
            result.Value = description;
            description.PropertyOwner = propertyOwner;
            description.PropertyValue = propertyValue;
            if (propertyType == null)
                return result;
            description.PropertyType = propertyType;
            PropertyValueType propertyValueType = CreatePropertyValueType(propertyType, hasException);
            description.PropertyValueType = propertyValueType;
            if (!CanCreateChildren(propertyValueType, propertyValue))
                return result;
            if (propertyValueType.HasFlag(PropertyValueType.Class) && ContainsDescriptionValue(parents, propertyValue))
                return result;

            PropertyInfo[] propertyInfos = propertyValue.GetType().GetProperties();
            int count = propertyInfos.Length;
            if (count <= 0)
                return result;
                       
            List<TreeItem<PropertyDescription>> children = new List<TreeItem<PropertyDescription>>();
            for (int i = 0; i < count; i++) {
                PropertyInfo propertyInfo = propertyInfos[i];
                List<TreeItem<PropertyDescription>> childPropertyParents = new List<TreeItem<PropertyDescription>>();
                childPropertyParents.AddRange(parents);
                childPropertyParents.Add(result);
                string childPropertyName = propertyInfo.Name;
                Type childPropertyType = propertyInfo.PropertyType;
                object childPropertyOwner = propertyValue;
                try {
                    object childPropertyValue = propertyInfo.GetValue(propertyValue);
                    children.Add(CreateItem(childPropertyParents, childPropertyName, childPropertyType, childPropertyOwner, childPropertyValue, false));
                } catch (Exception e) {
                    children.Add(CreateItem(childPropertyParents, childPropertyName, childPropertyType, childPropertyOwner, e, true));
                }
            }

            result.Children = children;
            return result;
        }

        static bool ContainsDescriptionValue(IEnumerable<TreeItem<PropertyDescription>> parents, object propertyValue) {
            foreach (TreeItem<PropertyDescription> parent in parents) {
                if (Object.ReferenceEquals(parent.Value.PropertyValue, propertyValue))
                    return true;
            }
            return false;
        }

        static PropertyValueType CreatePropertyValueType(Type type, bool hasException) {
            bool isNullable = Nullable.GetUnderlyingType(type) != null;
            if (type == typeof(string))
                return GetPropertyValueType(PropertyValueType.String, isNullable, hasException);
            if (type.IsClass)
                return GetPropertyValueType(PropertyValueType.Class, isNullable, hasException);
            if (type.IsPrimitive)
                return GetPropertyValueType(PropertyValueType.Primitive, isNullable, hasException);
            if (type.IsEnum)
                return GetPropertyValueType(PropertyValueType.Enum, isNullable, hasException);
            if (type.IsValueType)
                return GetPropertyValueType(PropertyValueType.Struct, isNullable, hasException);
            return GetPropertyValueType(PropertyValueType.Undefined, isNullable, hasException);
        }


        static PropertyValueType GetPropertyValueType(PropertyValueType underlyingType, bool isNullable, bool hasException) {
            PropertyValueType result = underlyingType;
            if (isNullable)
                result |= PropertyValueType.Nullable;
            if (hasException)
                result |= PropertyValueType.Exception;
            return result;
        }

        static bool CanCreateChildren(PropertyValueType propertyValueType, object propertyValue) {
            return
                propertyValue != null && 
                !propertyValueType.HasFlag(PropertyValueType.Exception) &&
                (propertyValueType.HasFlag(PropertyValueType.Class) ||
                 propertyValueType.HasFlag(PropertyValueType.Struct));
        }
    }
}
