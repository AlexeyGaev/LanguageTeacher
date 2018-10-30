namespace Reflection.Utils.PropertyTree {
    public abstract class PropertyValue {
        readonly object value;

        protected PropertyValue(object value) {
            this.value = value;
        }

        public object Value { get { return this.value; } }

        public abstract PropertyValueType Type { get; }
        public abstract bool HasChildren { get; }
    }
}
