using System;
using System.Collections.Generic;

namespace Reflection.Utils.PropertyTree {
    public class PropertyItem {
        readonly object id;
        readonly Type type;
        readonly object value;

        public PropertyItem(object id, Type type, object value) {
            this.id = id;
            this.type = type;
            this.value = value;
        }
        
        public object Id { get { return this.id; } }
        public Type Type { get { return this.type; } }
        public object Value { get { return this.value; } }
                
        public PropertyObjectChildren ObjectChildren { get; set; }
        public IEnumerable<PropertyItem> ArrayChildren { get; set; }
    }
}
