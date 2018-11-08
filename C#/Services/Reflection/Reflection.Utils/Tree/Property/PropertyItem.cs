using System.Collections.Generic;

namespace Reflection.Utils.PropertyTree {
    public class PropertyItem {
        readonly PropertyField field;
        readonly object value;

        public PropertyItem(PropertyField field, object value) {
            this.value = value;
            this.field = field;
        }
        
        public PropertyField Field { get { return this.field; } }
        public object Value { get { return this.value; } }
                
        public PropertyObjectChildren ObjectChildren { get; set; }
        public IEnumerable<PropertyItem> ArrayChildren { get; set; }
    }
}
