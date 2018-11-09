using System.Collections.Generic;

namespace Reflection.Utils.PropertyTree.Serialization {
    public class SerializeContentItemCollection {
        readonly IEnumerable<SerializeItem> header;
        readonly List<SerializeContentItem> items;

        public SerializeContentItemCollection(IEnumerable<SerializeItem> header) {
            this.header = header;
            this.items = new List<SerializeContentItem>();
        }

        public SerializeContentItem this[int index] { get { return this.items[index]; } }
        public IEnumerable<SerializeItem> Header { get { return this.header; } }
        public int Count { get { return this.items.Count; } }

        public void Add(SerializeContentItem item) {
            this.items.Add(item);
        }
    }
}
