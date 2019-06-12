using System;
using System.Collections.Generic;

namespace Reflection.Utils.PropertyTree.Serialization {
    public static class StringExporter {
        public static void Export(StringWriter writer, SerializeContentItem item, int level = 0) {
            if (writer == null || item == null)
                return;
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

        static void ExportHeader(StringWriter writer, IEnumerable<SerializeItem> header, int level) {
            string headerString = IndentBuilder.CreateLevelSpace(level, " ") + SerializeItemsToStringBuilder.Create(header);
            writer.WriteLine(headerString);
        } 
    }
}
