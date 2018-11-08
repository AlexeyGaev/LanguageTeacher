using Reflection.Utils.Tree.Localization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Reflection.Utils.PropertyTree {
    public class PropertyArrayChildren : IEnumerable<PropertyItem> {
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

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}
