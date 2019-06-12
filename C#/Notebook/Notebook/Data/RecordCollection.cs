using System.Collections.Generic;

namespace Notebook.Core.Data {
    public class RecordCollection {
        readonly List<Record> items;
       
        public Record this[int index] { get { return this.items[index]; } }
        public int Count { get { return this.items.Count; } }

        public void Add(Record item) {
            // TODO
        }
        public void AddRange(IEnumerable<Record> items) {
            // TODO
        }
        public void Remove(Record item) {
            // TODO
        }
        public void Clear() {
            // TODO
        }
    }
}
