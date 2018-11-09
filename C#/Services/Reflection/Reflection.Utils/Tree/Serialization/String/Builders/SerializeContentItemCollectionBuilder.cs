using System.Collections.Generic;
using System.Linq;

namespace Reflection.Utils.PropertyTree.Serialization {
    public static class SerializeContentItemCollectionBuilder {
        public static SerializeContentItemCollection CreateCycle(string name) {
            IEnumerable<SerializeItem> header = SerializeItemsBuilder.CreateCollectionCycleHeader(name);
            return new SerializeContentItemCollection(header);
        }

        public static SerializeContentItemCollection Create(IEnumerable<PropertyItem> children, string name) {
            IEnumerable<SerializeItem> header = SerializeItemsBuilder.CreateCollectionCountHeader(name, children.Count());
            SerializeContentItemCollection result = new SerializeContentItemCollection(header);
            foreach (PropertyItem item in children)
                result.Add(SerializeContentItemBuilder.Create(item));
            return result;
        }
    }
}

