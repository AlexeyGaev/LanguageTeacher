using Reflection.Utils.Tree.Localization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Reflection.Utils.PropertyTree {
    public class PropertyObjectChildren : IEnumerable<PropertyItem> {
        static PropertyObjectChildren cycle = CreateCycle();
        static PropertyObjectChildren CreateCycle() {
            PropertyObjectChildren result = new PropertyObjectChildren(Enumerable.Empty<PropertyItem>());
            result.hasCycle = true;
            return result;
        }

        public static PropertyObjectChildren Cycle { get { return cycle; } }
        
        readonly IEnumerable<PropertyItem> items;
        bool hasCycle;

        PropertyObjectChildren() { }

        public PropertyObjectChildren(IEnumerable<PropertyItem> items) {
            this.items = items;
            this.hasCycle = false;
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
