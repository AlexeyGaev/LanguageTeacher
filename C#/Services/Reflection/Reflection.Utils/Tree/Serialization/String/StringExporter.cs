using System;
using System.Collections.Generic;

namespace Reflection.Utils.PropertyTree.Serialization {
    public static class StringExporter {
        public static void Export(StringWriter writer, SerializeContentItem item, int level = 0) {
            ExportHeader(writer, item.Header, level);
            IEnumerable<SerializeContentItemCollection> content = item.Content;
            if (content != null)
                foreach (SerializeContentItemCollection collection in content) 
                    ExportContentCollection(writer, collection, level + 1);
        }

        static void ExportContentCollection(StringWriter writer, SerializeContentItemCollection collection, int level) {
            ExportHeader(writer, collection.Header, level);
            int count = collection.Count;
            for (int i = 0; i < count; i++) 
                Export(writer, collection[i], level + 1);
        }

        static string CreateLevelSpace(int level, string space) {
            string indent = string.Empty;
            for (int i = 0; i < level; i++)
                indent += space;
            return indent;
        }

        static string CreateHeaderString(IEnumerable<SerializeItem> header, int level) {
            return CreateLevelSpace(level, " ") + CreateItemsString(header);
        }

        static string CreateItemsString(IEnumerable<SerializeItem> items) {
            string result = String.Empty;
            if (items == null)
                return result;
            foreach (SerializeItem item in items) {
                string value = CreateString(item);
                if (!String.IsNullOrEmpty(value))
                    result += "," + value;
            }
            return result;
        }

        static string CreateString(SerializeItem item) {
            if (item.Mode == SerializeItemMode.OneValue)
                return item.FirstValue;
            else if (item.Mode == SerializeItemMode.TwoValues) 
                return item.FirstValue + "=" + item.SecondValue;
            else
                return String.Empty;
        }

        static void ExportHeader(StringWriter writer, IEnumerable<SerializeItem> header, int level) {
            writer.WriteLine(CreateHeaderString(header, level));
        } 
    }
}
