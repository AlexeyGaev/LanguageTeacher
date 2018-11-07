using System.Collections.Generic;

namespace Reflection.Utils.PropertyTree {
    public class SerializePropertyTreeItem {
        readonly SerializeInfo field;
        readonly SerializeInfo value;

        public SerializePropertyTreeItem(SerializeInfo field, SerializeInfo value) {
            this.field = field;
            this.value = value;
        }

        public SerializeInfo Field { get { return this.field; } }
        public SerializeInfo Value { get { return this.value; } }

        public bool HasObjectCycle { get; set; }
        public bool HasArrayChildren { get; set; }
        public IEnumerable<SerializePropertyTreeItem> ObjectChildren { get; set; }
        public IEnumerable<SerializePropertyTreeItem> ArrayChildren { get; set; }
    }
}
