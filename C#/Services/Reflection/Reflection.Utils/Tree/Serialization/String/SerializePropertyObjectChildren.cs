using System.Collections.Generic;

namespace Reflection.Utils.PropertyTree.Selialization {
    public class SerializePropertyItemCollection {
        readonly SerializeItem header;
        readonly List<SerializePropertyItem> items;

        public SerializePropertyItemCollection(SerializeItem header) {
            this.header = header;
            this.items = new List<SerializePropertyItem>();
        }

        public SerializePropertyItem this[int index] { get { return this.items[index]; } }
        public SerializeItem Header { get { return this.header; } }
        public int Count { get { return this.items.Count; } }

        public void Add(SerializePropertyItem item) {
            this.items.Add(item);
        }
    }
}
