using System;
using System.Reflection;

namespace Translation.Http.Tree {
    public class PropertyDescription {
        readonly object owner;
        readonly string propertyName;
        readonly Type propertyType;
        readonly PropertyValue propertyValue;

        public PropertyDescription(object owner, string propertyName, Type propertyType, PropertyValue propertyValue) {
            this.owner = owner;
            this.propertyName = propertyName;
            this.propertyType = propertyType;
            this.propertyValue = propertyValue;
        }

        public object Owner { get { return this.owner; } }
        public string PropertyName { get { return this.propertyName; } }
        public Type PropertyType { get { return this.propertyType; } }
        public PropertyValue PropertyValue { get { return this.propertyValue; } }
    }
}
