using Reflection.Utils.PropertyTree.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Reflection.Utils.PropertyTree {
    public static class PropertyStringSerializer {
        public static string Serialize(string name, Type type, object value) {
            StringWriter writer = new StringWriter();
            PropertyItem propertyItem = PropertyItemBuilder.Create(new PropertyField(name, type), value);
            SerializeContentItem serializeContentItem = SerializeContentItemBuilder.Create(propertyItem);
            StringExporter.Export(writer, serializeContentItem);
            IEnumerable<string> strings = writer.Result;
            if (strings == null)
                return string.Empty;
            string result = string.Empty;
            foreach (string str in strings) {
                if (!String.IsNullOrEmpty(str)) {
                    if (!String.IsNullOrEmpty(result))
                        result += "\n";
                    result += str;
                }
            }
            return result;
        }
    }
}
