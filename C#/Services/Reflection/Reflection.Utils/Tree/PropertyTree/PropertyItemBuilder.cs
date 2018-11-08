using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Reflection.Utils.PropertyTree {
    public static class PropertyItemBuilder {
        public static PropertyItem Create(IEnumerable<PropertyItem> parents, string fieldName, Type fieldType, object propertyValue) {
            PropertyItem result = CreatePrimitiveItem(fieldName, fieldType, propertyValue);
            if (fieldType == null || propertyValue == null || fieldType == typeof(string))
                return result;
            if (fieldType.IsPrimitive || fieldType.IsEnum)
                return result;
            Type underlyingType = Nullable.GetUnderlyingType(fieldType);
            if (underlyingType != null && (underlyingType.IsPrimitive || underlyingType.IsEnum))
                return result;
            if (fieldType.IsArray) {
                result.ArrayChildren = CreateArrayChildren(parents, result);
                return result;
            }
            if (ShouldCheckChildrenCycle(result) && GetHasChildrenCycle(parents, result)) {
                result.ObjectChildren = PropertyObjectChildren.Cycle;
                return result;
            }
            PropertyInfo[] properties = result.Value.GetType().GetProperties();
            if (properties == null)
                return result;
            result.ObjectChildren = CreateObjectChildren(parents, result, properties);
            result.ArrayChildren = CreateArrayChildren(parents, result);
            return result;
        }

        static IEnumerable<PropertyItem> CreateArrayChildren(IEnumerable<PropertyItem> parents, PropertyItem current) {
            if (!(current.Value is IEnumerable))
                return null;

            string arrayFieldNamePrefix = "Item";
            int index = 0;
            List<PropertyItem> items = new List<PropertyItem>();
            foreach (object item in (IEnumerable)current.Value) {
                IEnumerable<PropertyItem> childParents = CreateObjectChildParents(parents, current);
                string arrayFieldName = arrayFieldNamePrefix + index.ToString();
                items.Add(Create(childParents, arrayFieldName, item.GetType(), item));
                index++;
            }
            return items;
        }

        static PropertyItem CreatePrimitiveItem(string fieldName, Type fieldType, object propertyValue) {
            return new PropertyItem(new PropertyField(fieldName, fieldType), propertyValue);
        }

        static PropertyObjectChildren CreateObjectChildren(IEnumerable<PropertyItem> parents, PropertyItem current, PropertyInfo[] infos) {
            List<PropertyItem> children = new List<PropertyItem>();
            foreach (PropertyInfo propertyInfo in infos) {
                if (propertyInfo.CanRead) {
                    IEnumerable<PropertyItem> childParents = CreateObjectChildParents(parents, current);
                    ParameterInfo[] indexParameters = propertyInfo.GetIndexParameters();
                    if (indexParameters.Length == 0) {
                        object childValue = CreatePropertyValue(propertyInfo, current);
                        PropertyItem child = Create(childParents, propertyInfo.Name, propertyInfo.PropertyType, childValue);
                        children.Add(child);
                    }
                    //else {
                    //    //foreach (ParameterInfo indexParameter in indexParameters) {
                    //    //    string memberName = indexParameter.Member.Name;
                    //    //    Type parameterType = indexParameter.ParameterType;
                    //    //    string parameterName = indexParameter.Name;
                    //    //    indexParameter.Member.MemberType
                    //    //    Type reflectedType = indexParameter.Member.ReflectedType;
                    //    //    Type type = indexParameter.GetType();
                    //    //    MemberInfo member = indexParameter.Member;
                    //    //}
                    //}
                }
            }
            return new PropertyObjectChildren(children);
        }

        //static PropertyArrayChildren CreateArrayChildren(PropertyItem current) {
        //    object value = current.Value;
        //    if (value == null || value is string)
        //        return null;
                       
        //    IEnumerable enumerable = value as IEnumerable;
        //    if (enumerable == null)
        //        return null;
            
        //    List<PropertyItem> arrayItems = new List<PropertyItem>();
        //    PropertyField field = new PropertyField();
        //    foreach (object arrayValue in enumerable) {
        //        PropertyItem item = Create(null, field, arrayValue);
        //        arrayItems.Add(item);
        //    }
        //    return new PropertyArrayChildren(arrayItems);
        //}

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
        
        //static bool CanAddChild(PropertyItem child) {
        //    return !(child.Value is TargetParameterCountException);
        //}

        static object CreatePropertyValue(PropertyInfo propertyInfo, PropertyItem item) {
            try {
                return propertyInfo.GetValue(item.Value);
            } catch (Exception e) {
                return e;
            }
        }
    }
}
