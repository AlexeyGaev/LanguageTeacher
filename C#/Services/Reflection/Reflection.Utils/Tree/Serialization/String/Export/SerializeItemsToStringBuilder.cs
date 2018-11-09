using System;
using System.Collections.Generic;

namespace Reflection.Utils.PropertyTree.Serialization {
    public static class SerializeItemsToStringBuilder {
        public static string Create(IEnumerable<SerializeItem> items) {
            string result = String.Empty;
            if (items == null)
                return result;
            foreach (SerializeItem item in items) {
                string value = SerializeItemToStringBuilder.Create(item);
                if (!String.IsNullOrEmpty(value)) {
                    if (!String.IsNullOrEmpty(result))
                        result += ",";
                    result += value;
                }
            }
            return result;
        }
    }
}
