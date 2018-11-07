using Reflection.Utils.Tree.Localization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Reflection.Utils.PropertyTree {
    public class PropertyObjectChildren : IEnumerable<PropertyItem>, IEquatable<PropertyObjectChildren> {
        public static bool operator ==(PropertyObjectChildren left, PropertyObjectChildren right) {
            if ((object)left == null)
                return (object)right == null;
            if ((object)right == null)
                return (object)left == null;
            return left.Equals(right);
        }
        public static bool operator !=(PropertyObjectChildren left, PropertyObjectChildren right) {
            if ((object)left == null)
                return (object)right != null;
            if ((object)right == null)
                return (object)left != null;
            return !left.Equals(right);
        }
        
        static PropertyObjectChildren cycle = CreateCycle();
        static PropertyObjectChildren empty = new PropertyObjectChildren(Enumerable.Empty<PropertyItem>());
        static PropertyObjectChildren CreateCycle() {
            PropertyObjectChildren result = new PropertyObjectChildren(Enumerable.Empty<PropertyItem>());
            result.hasCycle = true;
            return result;
        }

        public static PropertyObjectChildren Cycle { get { return cycle; } }
        public static PropertyObjectChildren Empty { get { return empty; } }

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

        public override bool Equals(object obj) {
            return base.Equals(obj);
        }

        public override int GetHashCode() {
            return base.GetHashCode();
        }

        public bool Equals(PropertyObjectChildren other) {
            throw new NotImplementedException();
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
