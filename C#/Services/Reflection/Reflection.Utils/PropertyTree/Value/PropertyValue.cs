using System.Collections.Generic;

namespace Reflection.Utils.PropertyTree {
    public abstract class PropertyValue {
        readonly object value;
        readonly IEnumerable<object> parents;
        readonly IEnumerable<object> children;

        protected PropertyValue(object value, IEnumerable<object> parents, IEnumerable<object> children) {
            this.value = value;
            this.parents = parents;
            this.children = children;
        }

        public object Value { get { return this.value; } }
        public bool HasParents { get { return this.parents != null; } }
        public bool HasChildren { get { return this.children != null; } }
        
        public abstract PropertyValueType Type { get; }
    }
}
