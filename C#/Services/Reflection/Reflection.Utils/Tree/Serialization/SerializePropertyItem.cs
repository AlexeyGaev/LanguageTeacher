using System.Collections.Generic;

namespace Reflection.Utils.PropertyTree {
    public class SerializePropertyItem {
        readonly SerializeInfo field;
        readonly SerializeInfo value;

        public SerializePropertyItem(SerializeInfo field, SerializeInfo value) {
            this.field = field;
            this.value = value;
        }

        public SerializeInfo Field { get { return this.field; } }
        public SerializeInfo Value { get { return this.value; } }

        public SerializePropertyObjectChildren ObjectChildren { get; set; }
        public IEnumerable<SerializePropertyItem> ArrayChildren { get; set; }
    }
}
