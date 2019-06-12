using System;
using System.Collections.Generic;

namespace Reflection.Utils.PropertyTree.Serialization {
    public static class SerializeItemsBuilder {
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

        public static IEnumerable<SerializeItem> CreateCollectionHeader(string name, int count, CycleMode mode) {
            List<SerializeItem> result = new List<SerializeItem>();
            result.Add(SerializeItem.CreateOneValue(name));
            result.Add(SerializeItem.CreateTwoValues(Localization.Count, count.ToString()));
            if (mode == CycleMode.Value)
                result.Add(SerializeItem.CreateOneValue(Localization.ValueCycle));
            if (mode == CycleMode.Reference)
                result.Add(SerializeItem.CreateOneValue(Localization.ReferenceCycle));
            return result;
        }

        public static IEnumerable<SerializeItem> CreatePropertyItemHeader(object propertyId, Type propertyType, object propertyValue) {
            List<SerializeItem> result = new List<SerializeItem>();
            result.Add(SerializeItemBuilder.CreateFieldIdItem(propertyId));
            result.Add(SerializeItemBuilder.CreateTypeItem(propertyType));
            result.Add(SerializeItem.Delimeter);
            result.AddRange(CreateItemsFromPropertyValue(propertyValue));
            return result;
        }
    }
}

