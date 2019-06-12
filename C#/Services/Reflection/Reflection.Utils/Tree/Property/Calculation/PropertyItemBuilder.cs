using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Reflection.Utils.PropertyTree {
    public static class PropertyItemBuilder {
        public static PropertyItem Create(object propertyId, Type propertyType, object propertyValue, PropertyCollection parents, Filter filter) {
            if (filter != null && !filter.CanCreatePropertyItem(propertyId, propertyType, propertyValue, parents))
                return null;
            PropertyItem result = new PropertyItem(propertyId, propertyType, propertyValue);
            if (filter != null && !filter.CanCreateChildren(propertyType, propertyValue))
                return result;
            if (!CanCreateChildren(propertyType, propertyValue))
                return result;
            if (!propertyType.IsArray)
                result.ObjectChildren = CreateObjectChildren(result, propertyType.GetProperties(), parents, filter);
            if (propertyValue is IEnumerable)
                result.ArrayChildren = CreateArrayChildren(result, parents, filter);
            return result;
        }

        static bool CanCreateChildren(Type propertyType, object propertyValue) {
            return 
                propertyType != null && !propertyType.IsPrimitive && !propertyType.IsEnum && 
                propertyType != typeof(string) && propertyValue != null &&
                (Nullable.GetUnderlyingType(propertyType) != null || 
                (CanCreateChildren(Type.GetTypeCode(propertyType)) &&
                (!propertyType.IsValueType ||
                !propertyValue.Equals(Activator.CreateInstance(propertyType)))));
        }

        static bool CanCreateChildren(TypeCode typeCode) {
            return
                typeCode == TypeCode.DateTime ||
                typeCode == TypeCode.Object;
        }

        static IEnumerable<PropertyItem> CreateArrayChildren(PropertyItem propertyItem, PropertyCollection parents, Filter filter) {
            int index = 0;
            List<PropertyItem> children = new List<PropertyItem>();
            foreach (object item in (IEnumerable)propertyItem.Value) {
                PropertyCollection childParents = CreateObjectChildParents(parents, propertyItem);
                PropertyItem child = Create(index, item.GetType(), item, childParents, filter);
                if (child == null)
                    break;
                children.Add(child);
                index++;
            }
            return children;
        }

        static PropertyObjectChildren CreateObjectChildren(PropertyItem currentItem, PropertyInfo[] propertyInfos, PropertyCollection parents, Filter filter) {
            if (parents != null) {
                Type type = currentItem.Value.GetType();
                if (type.IsValueType) {
                    if (parents.ContainsByValueType(currentItem))
                        return PropertyObjectChildren.ValueCycle;
                }
                else if (parents.ContainsByRefererenceValue(currentItem))
                    return PropertyObjectChildren.ReferenceCycle;
            }
            List<PropertyItem> children = new List<PropertyItem>();
            foreach (PropertyInfo propertyInfo in propertyInfos) {
                if (propertyInfo.CanRead) {
                    ParameterInfo[] indexParameters = propertyInfo.GetIndexParameters();
                    if (indexParameters.Length == 0) {
                        object childValue = CreatePropertyValue(propertyInfo, currentItem.Value);
                        PropertyCollection childParents = CreateObjectChildParents(parents, currentItem);
                        PropertyItem child = Create(propertyInfo.Name, propertyInfo.PropertyType, childValue, childParents, filter);
                        if (child == null)
                            break;
                        children.Add(child);
                    }
                }
            }
            return new PropertyObjectChildren(children);
        }
        
        static PropertyCollection CreateObjectChildParents(PropertyCollection parents, PropertyItem item) {
            PropertyCollection result = parents == null ? new PropertyCollection() : parents.Clone();
            result.Add(item);
            return result;
        }
      
        static object CreatePropertyValue(PropertyInfo propertyInfo, object value) {
            try {
                return propertyInfo.GetValue(value);
            } catch (Exception e) {
                return e;
            }
        }
    }

    public class ObjectInfo {

    }
}
