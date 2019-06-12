using System.Collections.Generic;

namespace Reflection.Utils.PropertyTree.Serialization {
    public static class SerializeContentItemBuilder {
        public static SerializeContentItem Create(PropertyItem propertyItem) {
            IEnumerable<SerializeItem> header = SerializeItemsBuilder.CreatePropertyItemHeader(propertyItem.Id, propertyItem.Type, propertyItem.Value);
            SerializeContentItem result = new SerializeContentItem(header);
            result.Content = SerializeContentItemCollectionsBuilder.Create(propertyItem.ObjectChildren, propertyItem.ArrayChildren);
            return result;
        }
    }
}

