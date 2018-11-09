using Reflection.Utils.Tree.Serialization.Localization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Reflection.Utils.PropertyTree.Serialization {
    public static class StringSerializer {
        public static IEnumerable<SerializeItem> Serialize(PropertyField propertyField) {
            List<SerializeItem> result = new List<SerializeItem>();
            object id = propertyField.Id;
            string idName = LocalizationTable.GetStringById(LocalizationId.Id);
            string idValue = id == null ? null : ObjectToString(idName);
            result.Add(SerializeItem.CreateTwoValues(idName, idValue));
            Type type = propertyField.Type;
            string typeName = LocalizationTable.GetStringById(LocalizationId.Type);
            string typeValue = type == null ? null : type.Name;
            result.Add(SerializeItem.CreateTwoValues(typeName, idValue));
            return result;
        }

        static string ObjectToString(object value) {
            return value is string ? (string)value : value.ToString();
        }

        public static SerializeContentItem Serialize(PropertyItem propertyItem) {
            List<SerializeItem> header = new List<SerializeItem>();
            header.AddRange(Serialize(propertyItem.Field));
            header.AddRange(Serialize(propertyItem.Value));
            SerializeContentItem result = new SerializeContentItem(header);
            List<SerializeContentItemCollection> content = new List<SerializeContentItemCollection>();
            SerializeContentItemCollection objectChildren = Serialize(propertyItem.ObjectChildren);
            if (objectChildren != null)
                content.Add(objectChildren);
            SerializeContentItemCollection arrayChildren = Serialize(propertyItem.ArrayChildren);
            if (arrayChildren != null)
                content.Add(arrayChildren);
            result.Content = content;
            return result;
        }

        public static SerializeContentItemCollection Serialize(PropertyObjectChildren objectChildren) {
            if (objectChildren == null)
                return null;
            if (objectChildren.HasCycle) 
                return new SerializeContentItemCollection(CreateCycleHeader(objectChildren.Count()));
            SerializeContentItemCollection result = new SerializeContentItemCollection(CreateCountHeader(LocalizationTable.GetStringById(LocalizationId.ObjectChildren), objectChildren.Count()));
            foreach (PropertyItem item in objectChildren) 
                result.Add(Serialize(item));
            return result;
        }

        static IEnumerable<SerializeItem> CreateCycleHeader(int count) {
            List<SerializeItem> result = new List<SerializeItem>();
            result.Add(SerializeItem.CreateOneValue(LocalizationTable.GetStringById(LocalizationId.ObjectChildren)));
            result.Add(SerializeItem.CreateTwoValues(LocalizationTable.GetStringById(LocalizationId.Count), count.ToString()));
            result.Add(SerializeItem.CreateOneValue(LocalizationTable.GetStringById(LocalizationId.HasCycle)));
            return result;
        }

        static IEnumerable<SerializeItem> CreateCountHeader(string name, int count) {
            List<SerializeItem> result = new List<SerializeItem>();
            result.Add(SerializeItem.CreateOneValue(name));
            result.Add(SerializeItem.CreateTwoValues(LocalizationTable.GetStringById(LocalizationId.Count), count.ToString()));
            return result;
        }

        public static SerializeContentItemCollection Serialize(IEnumerable<PropertyItem> arrayChildren) {
            if (arrayChildren == null)
                return null;
            SerializeContentItemCollection result = new SerializeContentItemCollection(CreateCountHeader(LocalizationTable.GetStringById(LocalizationId.ArrayChildren), arrayChildren.Count()));
            foreach (PropertyItem item in arrayChildren)
                result.Add(Serialize(item));
            return result;
        }

        public static IEnumerable<SerializeItem> Serialize(object propertyValue) {
            if (propertyValue == null)
                return CreateNullItems();

            Type type = propertyValue.GetType();
            List<SerializeItem> items = new List<SerializeItem>();
            string type1 = LocalizationTable.GetStringById(LocalizationId.Type);
            string type2 = type.Name;
            SerializeItem typeItem = SerializeItem.CreateTwoValues(type1, type2);
            items.Add(typeItem);
            string value1 = LocalizationTable.GetStringById(LocalizationId.Value);
            string value2 = ValueToString(propertyValue, type);
            SerializeItem valueItem = SerializeItem.CreateTwoValues(value1, value2);
            items.Add(valueItem);
            return items;
        }

        static IEnumerable<SerializeItem> CreateNullItems() {
            List<SerializeItem> nullItems = new List<SerializeItem>();
            string value1 = LocalizationTable.GetStringById(LocalizationId.Value);
            string value2 = LocalizationTable.GetStringById(LocalizationId.Null);
            SerializeItem item = SerializeItem.CreateTwoValues(value1, value2);
            nullItems.Add(item);
            return nullItems;
        }

        static string ValueToString(object value, Type type) {
            if (value is Exception)
                return LocalizationTable.GetStringById(LocalizationId.Exception);
            Type underlyingType = Nullable.GetUnderlyingType(type);
            if (underlyingType != null)
                type = underlyingType;
            TypeCode typeCode = Type.GetTypeCode(type);
            if (typeCode == TypeCode.DBNull || typeCode == TypeCode.Empty)
                return LocalizationTable.GetStringById(LocalizationId.Null);
            if (typeCode == TypeCode.String) {
                string stringValue = (string)value;
                return String.IsNullOrEmpty(stringValue) ? LocalizationTable.GetStringById(LocalizationId.Empty) : stringValue;
            }
            return value.ToString();
        }
    }
}
