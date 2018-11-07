using Reflection.Utils.Tree.Localization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Reflection.Utils.PropertyTree {
    public class PropertyObjectChildren : IEnumerable<PropertyItem> {
        static PropertyObjectChildren cycle = CreateCycle();
        public static PropertyObjectChildren Cycle { get { return cycle; } }
        static PropertyObjectChildren CreateCycle() {
            PropertyObjectChildren result = new PropertyObjectChildren();
            result.hasCycle = true;
            return result;
        }

        readonly List<PropertyItem> items;
        bool hasCycle;

        public PropertyObjectChildren() {
            this.items = new List<PropertyItem>();
            this.hasCycle = false;
        }

        public void Add(PropertyItem item) {
            if (!this.hasCycle)
                items.Add(item);
        }
        
        public bool HasCycle { get { return this.hasCycle; } }

        public IEnumerator<PropertyItem> GetEnumerator() {
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
