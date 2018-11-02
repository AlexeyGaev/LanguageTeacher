using System.Collections.Generic;

namespace Reflection.Utils.PropertyTree {
    public abstract class PropertyValueNullable : PropertyValue {
        bool isNullable;

        protected PropertyValueNullable(object value, bool isNullable, IEnumerable<object> parents, IEnumerable<object> children)
            : base(value, parents, children) {
            this.isNullable = isNullable;
        }

        public bool IsNullable { get { return this.isNullable; } }
    }
}
