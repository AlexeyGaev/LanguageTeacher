using System;
using System.Collections.Generic;

namespace Reflection.Utils.PropertyTree {
    public class PropertyDescription {
        readonly string propertyName;
       
        public PropertyDescription(string propertyName) {
            this.propertyName = propertyName;
        }
        
        public string PropertyName { get { return this.propertyName; } }
                
        public object PropertyOwner { get; set; }
        public Type PropertyType { get; set; }
        public PropertyValueType PropertyValueType { get; set; }
        public object PropertyValue { get; set; }
    }
}
