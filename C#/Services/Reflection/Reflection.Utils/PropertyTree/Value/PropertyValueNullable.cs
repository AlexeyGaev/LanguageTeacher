namespace Reflection.Utils.PropertyTree {
    public abstract class PropertyValueNullable : PropertyValue {
        bool isNullable;

        protected PropertyValueNullable(object value, bool isNullable)
            : base(value) {
            this.isNullable = isNullable;
        }

        public bool IsNullable { get { return this.isNullable; } }
    }
}
