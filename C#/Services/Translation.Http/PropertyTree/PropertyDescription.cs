using System;

namespace Translation.Http.PropertyTree {
    public class PropertyDescription {
        readonly object owner;
        readonly string propertyName;
        readonly Type propertyType;
        readonly IPropertyValue propertyValue;

        public PropertyDescription(object owner, string propertyName, Type propertyType, IPropertyValue propertyValue) {
            this.owner = owner;
            this.propertyName = propertyName;
            this.propertyType = propertyType;
            this.propertyValue = propertyValue;
        }

        public object Owner { get { return this.owner; } }
        public string PropertyName { get { return this.propertyName; } }
        public Type PropertyType { get { return this.propertyType; } }
        public IPropertyValue PropertyValue { get { return this.propertyValue; } }
    }
}
