namespace Reflection.Utils.PropertyTree {
    public abstract class PropertyValueNullable : PropertyValue {
        bool isNullable;

        protected PropertyValueNullable(object value, bool isNullable, ParentValues values)
            : base(value, values) {
            this.isNullable = isNullable;
        }

        public bool IsNullable { get { return this.isNullable; } }
    }
}
