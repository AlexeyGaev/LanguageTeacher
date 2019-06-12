using System;
using System.Collections.Generic;

namespace Reflection.Utils.PropertyTree {
    public class PropertyCollection {
        List<PropertyItem> items = new List<PropertyItem>();

        public int Count { get { return this.items.Count; } }

        public void Add(PropertyItem item) {
            this.items.Add(item);
        }

        public bool ContainsByRefererenceValue(PropertyItem item) {
            int count = this.items.Count;
            if (count == 0)
                return false;
            for (int i = 0; i < count; i++) 
                if (this.items[i].Value.Equals(item.Value))
                    return true;
            return false;
        }

        public bool ContainsByValueType(PropertyItem item) {
            int count = this.items.Count;
            if (count == 0)
                return false;
            for (int i = 0; i < count; i++) 
                if (this.items[i].Type == item.Type && 
                    this.items[i].Id.Equals(item.Id))
                    return true;
            return false;
        }

        public PropertyCollection Clone() {
            PropertyCollection result = new PropertyCollection();
            result.items.AddRange(this.items);
            return result;
        }
    }
}