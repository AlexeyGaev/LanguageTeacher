using Reflection.Utils.Tree.Localization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Reflection.Utils.PropertyTree {
    public class SerializePropertyObjectChildren : IEnumerable<SerializePropertyItem> {
        static SerializePropertyObjectChildren cycle = new SerializePropertyObjectChildren(Enumerable.Empty<SerializePropertyItem>(), true);
        public static SerializePropertyObjectChildren Cycle { get { return cycle; } }
        
        readonly IEnumerable<SerializePropertyItem> items;
        bool hasCycle;

        SerializePropertyObjectChildren(IEnumerable<SerializePropertyItem> items, bool hasCycle) {
            this.items = items; 
            this.hasCycle = hasCycle;
        }

        public SerializePropertyObjectChildren(IEnumerable<SerializePropertyItem> items) 
            : this(items, false) {
            this.items = items;
            this.hasCycle = false;
        }
                
        public bool HasCycle { get { return this.hasCycle; } }

        public IEnumerator<SerializePropertyItem> GetEnumerator() {
            return this.items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public override string ToString() {
            string result = String.Format(LocalizationTable.GetStringById(LocalizationId.ObjectChildren) + ": {0}", this.items.Count());
            if (this.hasCycle)
                result += ", " + LocalizationTable.GetStringById(LocalizationId.HasObjectChildrenCycle);
            return result;
        }
    }
}
