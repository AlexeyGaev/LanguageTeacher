using System.Collections.Generic;
using System.Linq;

namespace Reflection.Utils.PropertyTree.Serialization {
    public static class SerializeContentItemCollectionBuilder {
        public static SerializeContentItemCollection Create(IEnumerable<PropertyItem> children, string name, CycleMode mode) {
            IEnumerable<SerializeItem> header = SerializeItemsBuilder.CreateCollectionHeader(name, children.Count(), mode);
            SerializeContentItemCollection result = new SerializeContentItemCollection(header);
            foreach (PropertyItem item in children)
                result.Add(SerializeContentItemBuilder.Create(item));
            return result;
        }
    }
}

