using System.Collections.Generic;

namespace Reflection.Utils.PropertyTree.Serialization {
    public static class SerializeItemsBuilder {
        public static IEnumerable<SerializeItem> CreateItemsFromPropertyField(PropertyField propertyField) {
            List<SerializeItem> result = new List<SerializeItem>();
            result.Add(SerializeItemBuilder.CreateFieldIdItem(propertyField.Id));
            result.Add(SerializeItemBuilder.CreateTypeItem(propertyField.Type));
            return result;
        }

        public static IEnumerable<SerializeItem> CreateItemsFromPropertyValue(object propertyValue) {
            List<SerializeItem> result = new List<SerializeItem>();
            if (propertyValue == null) {
                result.Add(SerializeItemBuilder.CreateNullValueItem());
                return result;
            }

            result.Add(SerializeItemBuilder.CreateValueItem(propertyValue));
            result.Add(SerializeItemBuilder.CreateTypeItem(propertyValue.GetType()));
            return result;
        }

        public static IEnumerable<SerializeItem> CreateCollectionCountHeader(string name, int count) {
            List<SerializeItem> result = new List<SerializeItem>();
            result.Add(SerializeItem.CreateOneValue(name));
            result.Add(SerializeItem.CreateTwoValues(Localization.Count, count.ToString()));
            return result;
        }

        public static IEnumerable<SerializeItem> CreateCollectionCycleHeader(string name) {
            List<SerializeItem> result = new List<SerializeItem>();
            result.Add(SerializeItem.CreateOneValue(name));
            result.Add(SerializeItem.CreateTwoValues(Localization.Count, "0"));
            result.Add(SerializeItem.CreateOneValue(Localization.HasCycle));
            return result;
        }

        public static IEnumerable<SerializeItem> CreatePropertyItemHeader(PropertyField propertyField, object propertyValue) {
            List<SerializeItem> result = new List<SerializeItem>();
            result.AddRange(SerializeItemsBuilder.CreateItemsFromPropertyField(propertyField));
            result.Add(SerializeItem.CreateOneValue(Localization.Delimeter));
            result.AddRange(SerializeItemsBuilder.CreateItemsFromPropertyValue(propertyValue));
            return result;
        }
    }
}

