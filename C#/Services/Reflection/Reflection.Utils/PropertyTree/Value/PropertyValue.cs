using System.Collections.Generic;

namespace Reflection.Utils.PropertyTree {
    public abstract class PropertyValue {
        readonly object value;
        readonly IEnumerable<object> parents;

        protected PropertyValue(object value, IEnumerable<object> parents, IEnumerable<object> children) {
            this.value = value;
            this.parents = parents;
        }

        public object Value { get { return this.value; } }
        public bool HasParents { get { return this.parents != null; } }
        public ParentValues Parents { get { return this.parents; } }

        public abstract PropertyValueType Type { get; }
        public abstract bool HasChildren { get; }
    }
}
