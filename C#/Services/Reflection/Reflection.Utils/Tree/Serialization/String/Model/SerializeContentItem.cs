using System.Collections.Generic;

namespace Reflection.Utils.PropertyTree.Serialization {
    public class SerializeContentItem {
        readonly IEnumerable<SerializeItem> header;
      
        public SerializeContentItem(IEnumerable<SerializeItem> header) {
            this.header = header;
        }

        public IEnumerable<SerializeItem> Header { get { return this.header; } }
        public IEnumerable<SerializeContentItemCollection> Content { get; set; }
    }
}
