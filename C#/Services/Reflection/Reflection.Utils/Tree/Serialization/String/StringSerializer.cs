using System;
using System.Collections.Generic;
using System.Linq;

namespace Reflection.Utils.PropertyTree.Serialization {
    public static class StringSerializer {
        public static SerializeContentItem Serialize(PropertyItem propertyItem) {
            List<SerializeItem> header = new List<SerializeItem>();
            header.AddRange(Serialize(propertyItem.Field));
            header.Add(SerializeItem.CreateOneValue(Localization.Delimeter));
            header.AddRange(Serialize(propertyItem.Value));
            SerializeContentItem result = new SerializeContentItem(header);
            PropertyObjectChildren objectChildren = propertyItem.ObjectChildren;
            IEnumerable<PropertyItem> arrayChildren = propertyItem.ArrayChildren;
            if (objectChildren == null && arrayChildren == null)
                return result;
            List<SerializeContentItemCollection> content = new List<SerializeContentItemCollection>();
            if (objectChildren != null)
                content.Add(Serialize(objectChildren));
            if (arrayChildren != null)
                content.Add(Serialize(arrayChildren, Localization.ArrayChildren));
            result.Content = content;
            return result;
        }

        static IEnumerable<SerializeItem> Serialize(PropertyField propertyField) {
            List<SerializeItem> result = new List<SerializeItem>();
            result.Add(CreateIdValue(propertyField.Id));
            result.Add(CreateTypeValue(propertyField.Type));
            return result;
        }

        static SerializeItem CreateIdValue(object id) {
            string idName = Localization.IdName;
            string idValue = ObjectToString(id);
            return SerializeItem.CreateTwoValues(idName, idValue);
        }

        static SerializeItem CreateTypeValue(Type type) {
            string typeName = Localization.TypeName;
            string typeValue = CreateTypeValueCore(type);
            return SerializeItem.CreateTwoValues(typeName, typeValue);
        }

        static string CreateTypeValueCore(Type type) {
            if (type == null)
                return Localization.NullValue;
            Type underlyingType = Nullable.GetUnderlyingType(type);
            if (underlyingType != null)
                return Localization.Nullable + " " + ObjectToString(underlyingType.Name);
            return ObjectToString(type.Name);
        }

        static SerializeContentItemCollection Serialize(PropertyObjectChildren objectChildren) {
            if (objectChildren.HasCycle) 
                return new SerializeContentItemCollection(CreateCycleHeader(objectChildren.Count()));
            return Serialize(objectChildren, Localization.ObjectChildren);
        }

        static IEnumerable<SerializeItem> CreateCycleHeader(int count) {
            List<SerializeItem> result = new List<SerializeItem>();
            result.Add(SerializeItem.CreateOneValue(Localization.ObjectChildren));
            result.Add(SerializeItem.CreateTwoValues(Localization.Count, count.ToString()));
            result.Add(SerializeItem.CreateOneValue(Localization.HasCycle));
            return result;
        }

        static IEnumerable<SerializeItem> CreateCountHeader(string name, int count) {
            List<SerializeItem> result = new List<SerializeItem>();
            result.Add(SerializeItem.CreateOneValue(name));
            result.Add(SerializeItem.CreateTwoValues(Localization.Count, count.ToString()));
            return result;
        }

        static SerializeContentItemCollection Serialize(IEnumerable<PropertyItem> children, string name) {
            SerializeContentItemCollection result = new SerializeContentItemCollection(CreateCountHeader(name, children.Count()));
            foreach (PropertyItem item in children)
                result.Add(Serialize(item));
            return result;
        }

        static IEnumerable<SerializeItem> Serialize(object propertyValue) {
            if (propertyValue == null)
                return CreateNullItems();

            Type type = propertyValue.GetType();
            List<SerializeItem> items = new List<SerializeItem>();
            items.Add(CreateStringValue(propertyValue, type));
            items.Add(CreateTypeValue(type));
            return items;
        }

        static SerializeItem CreateStringValue(object value, Type type) {
            string value1 = Localization.ValueName;
            string value2 = CreateStringValueCore(value, type);
            return SerializeItem.CreateTwoValues(value1, value2);
        }

        static IEnumerable<SerializeItem> CreateNullItems() {
            List<SerializeItem> nullItems = new List<SerializeItem>();
            string value1 = Localization.ValueName;
            string value2 = Localization.NullValue;
            SerializeItem item = SerializeItem.CreateTwoValues(value1, value2);
            nullItems.Add(item);
            return nullItems;
        }

        static string CreateStringValueCore(object value, Type type) {
            if (value is Exception)
                return Localization.Exception;
            Type underlyingType = Nullable.GetUnderlyingType(type);
            if (underlyingType != null)
                type = underlyingType;
            TypeCode typeCode = Type.GetTypeCode(type);
            if (typeCode == TypeCode.DBNull || typeCode == TypeCode.Empty)
                return Localization.NullValue;
            if (typeCode == TypeCode.String) 
                return CreateStringValue((string)value);
            return value.ToString();
        }

        static string CreateStringValue(string value) {
            return String.IsNullOrEmpty(value) ? Localization.EmptyValue : value;
        }

        static string ObjectToString(object value) {
            if (value == null)
                return Localization.NullValue;
            return value is string ? CreateStringValue((string)value) : value.ToString();
        }
    }
}

