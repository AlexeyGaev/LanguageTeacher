using Reflection.Utils.Tree.Localization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Reflection.Utils.PropertyTree {
    public class PropertyArrayChildren : IEnumerable<PropertyItem>, IEquatable<PropertyArrayChildren> {
        public static bool operator ==(PropertyArrayChildren left, PropertyArrayChildren right) {
            if ((object)left == null)
                return (object)right == null;
            if ((object)right == null)
                return (object)left == null;
            return left.Equals(right);
        }
        public static bool operator !=(PropertyArrayChildren left, PropertyArrayChildren right) {
            if ((object)left == null)
                return (object)right != null;
            if ((object)right == null)
                return (object)left != null;
            return !left.Equals(right);
        }
        
        static PropertyArrayChildren empty = new PropertyArrayChildren(Enumerable.Empty<PropertyItem>());

        public static PropertyArrayChildren Empty { get { return empty; } }

        readonly IEnumerable<PropertyItem> items;
      
        PropertyArrayChildren() { }

        public PropertyArrayChildren(IEnumerable<PropertyItem> items) {
            this.items = items;
        }
            
        public IEnumerator<PropertyItem> GetEnumerator() {
            return this.items.GetEnumerator();
        }

        public override bool Equals(object obj) {
            return base.Equals(obj);
        }

        public override int GetHashCode() {
            return base.GetHashCode();
        }

        public bool Equals(PropertyArrayChildren other) {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public override string ToString() {
            return base.ToString();
        }
    }
}
