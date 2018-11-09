using System.Collections.Generic;

namespace Reflection.Utils.PropertyTree.Selialization {
    public class SerializePropertyItem {
        readonly IEnumerable<SerializeItem> header;
      
        public SerializePropertyItem(IEnumerable<SerializeItem> header) {
            this.header = header;
        }

        public IEnumerable<SerializeItem> Header { get { return this.header; } }
        public IEnumerable<SerializePropertyItemCollection> Content { get; set; }
    }
}
