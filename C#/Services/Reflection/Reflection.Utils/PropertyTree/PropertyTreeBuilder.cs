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
            result.Children = CreateChildren(parents, result, propertyValueType, propertyValue);
            return result;
        }
        
        static IEnumerable<TreeItem<PropertyDescription>> CreateChildren(IEnumerable<TreeItem<PropertyDescription>> parents, TreeItem<PropertyDescription> current, PropertyValueType type, object value) {
            if (!CanCreateChildren(parents, type, value))
                return null;
            PropertyInfo[] propertyInfos = value.GetType().GetProperties();
            int count = propertyInfos.Length;
            if (count <= 0)
                return null;
            List<TreeItem<PropertyDescription>> result = new List<TreeItem<PropertyDescription>>();
            for (int i = 0; i < count; i++) 
                result.Add(CreateChild(CreateChildParents(parents, current), propertyInfos[i], value));
            return result;
        }

        static IEnumerable<TreeItem<PropertyDescription>> CreateChildParents(IEnumerable<TreeItem<PropertyDescription>> parents, TreeItem<PropertyDescription> current) {
            List<TreeItem<PropertyDescription>> result = new List<TreeItem<PropertyDescription>>();
            if (parents != null)
                result.AddRange(parents);
            result.Add(current);
            return result;
        }

        static TreeItem<PropertyDescription> CreateChild(IEnumerable<TreeItem<PropertyDescription>> parents, PropertyInfo propertyInfo, object owner) {
            string propertyName = propertyInfo.Name;
            Type propertyType = propertyInfo.PropertyType;
            try {
                object childPropertyValue = propertyInfo.GetValue(owner);
                return CreateItem(parents, propertyName, propertyType, owner, childPropertyValue, false);
            } catch (Exception e) {
                return CreateItem(parents, propertyName, propertyType, owner, e, true);
            }
        }

        static bool ContainsDescriptionValue(IEnumerable<TreeItem<PropertyDescription>> parents, object propertyValue) {
            foreach (TreeItem<PropertyDescription> parent in parents) 
                if (Object.ReferenceEquals(parent.Value.PropertyValue, propertyValue))
                    return true;
            return false;
        }

        static PropertyValueType CreatePropertyValueType(Type type, bool hasException) {
            Type underlyingType = Nullable.GetUnderlyingType(type);
            if (underlyingType == null) 
                return CreatePropertyValueTypeCore(type, false, hasException);
            return CreatePropertyValueTypeCore(underlyingType, true, hasException);
        }

        static PropertyValueType CreatePropertyValueTypeCore(Type type, bool isNullable, bool hasException) {
            if (type == typeof(string))
                return GetPropertyValueType(PropertyValueType.String, isNullable, hasException);
            if (type.IsPrimitive)
                return GetPropertyValueType(PropertyValueType.Primitive, isNullable, hasException);
            if (type.IsEnum)
                return GetPropertyValueType(PropertyValueType.Enum, isNullable, hasException);
            if (type.IsClass)
                return GetPropertyValueType(PropertyValueType.Class, isNullable, hasException);
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

        static bool CanCreateChildren(IEnumerable<TreeItem<PropertyDescription>> parents, PropertyValueType propertyValueType, object propertyValue) {
            return
                 propertyValue != null && 
                !propertyValueType.HasFlag(PropertyValueType.Exception) &&
                (propertyValueType.HasFlag(PropertyValueType.Class) ||
                 propertyValueType.HasFlag(PropertyValueType.Struct)) &&
                (parents == null || 
                !propertyValueType.HasFlag(PropertyValueType.Class) || 
                !ContainsDescriptionValue(parents, propertyValue));
        }
    }
}
