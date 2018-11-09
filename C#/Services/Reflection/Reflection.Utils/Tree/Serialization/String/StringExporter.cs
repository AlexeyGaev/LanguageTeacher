using Reflection.Utils.Tree.Serialization.Localization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Reflection.Utils.PropertyTree.Serialization {
    public static class StringExporter {
        public static void ExportContentItem(StringWriter writer, SerializeContentItem item, int level) {
            ExportHeader(writer, item.Header, level);
            IEnumerable<SerializeContentItemCollection> content = item.Content;
            if (content != null)
                foreach (SerializeContentItemCollection collection in content) 
                    ExportContentCollection(writer, collection, level + 1);
        }

        public static void ExportContentCollection(StringWriter writer, SerializeContentItemCollection collection, int level) {
            ExportHeader(writer, collection.Header, level);
            int count = collection.Count;
            for (int i = 0; i < count; i++) 
                ExportContentItem(writer, collection[i], level + 1);
        }

        public static string CreateLevelSpace(int level, string space) {
            string indent = string.Empty;
            for (int i = 0; i < level; i++)
                indent += space;
            return indent;
        }

        public static string CreateHeaderString(IEnumerable<SerializeItem> header, int level) {
            string result = CreateLevelSpace(level, " ");
            if (header != null) {
                foreach (SerializeItem item in header) {
                    // TODO
                }
            }
            return result;
        }

        public static void ExportHeader(StringWriter writer, IEnumerable<SerializeItem> header, int level) {
            writer.WriteLine(CreateHeaderString(header, level));
        } 

        #region PropertyField
        //public override string ToString() {
        //    string format = LocalizationTable.GetStringById(LocalizationId.Name) + " = {0}, " + LocalizationTable.GetStringById(LocalizationId.Type) + " = {1}";
        //    return String.Format(format, IdToString(), TypeToString());
        //}

        //public string IdToString() {
        //    if (this.id == null)
        //        return LocalizationTable.GetStringById(LocalizationId.Null);
        //    if (this.id is string) {
        //        string stringId = (string)this.id;
        //        if (String.IsNullOrEmpty(stringId))
        //            return LocalizationTable.GetStringById(LocalizationId.Empty);
        //        return stringId;
        //    }
        //    return this.id.ToString();
        //}

        //public string TypeToString() {
        //    if (this.type == null)
        //        return LocalizationTable.GetStringById(LocalizationId.Null);
        //    Type underlyingType = Nullable.GetUnderlyingType(this.type);
        //    if (underlyingType != null)
        //        return LocalizationTable.GetStringById(LocalizationId.Nullable) + " " + underlyingType.Name;
        //    return this.type.Name;
        //} 
        #endregion

        #region Mapping
        //    public override string ToString() {
        //        return String.Format("({0}) : ({1})", this.field.ToString(), this.value.ToString());
        //    }
        #endregion
    }
}
