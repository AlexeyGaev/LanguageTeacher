using System;
using System.Reflection;

namespace Translation.Http.Tree {
    public class PropertyDescription {
        readonly object owner;
        readonly PropertyInfo propertyInfo;
        readonly PropertyValue propertyValue;

        public PropertyDescription(object owner, PropertyInfo propertyInfo, PropertyValue propertyValue) {
            this.owner = owner;
            this.propertyInfo = propertyInfo;
            this.propertyValue = propertyValue;
        }

        public object Owner { get { return this.owner; } }
        public PropertyInfo PropertyInfo { get { return this.propertyInfo; } }
        public PropertyValue PropertyValue { get { return this.propertyValue; } }

        public bool HasChildren { get { return this.propertyValue.HasChildren; } }
    }
}
