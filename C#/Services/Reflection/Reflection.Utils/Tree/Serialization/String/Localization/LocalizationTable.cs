using System.Collections.Generic;

namespace Reflection.Utils.PropertyTree.Serialization {
    public static class LocalizationTable {
        static Dictionary<LocalizationId, string> innerTable;

        static LocalizationTable() {
            innerTable = new Dictionary<LocalizationId, string>();
            innerTable.Add(LocalizationId.Id, "Id");
            innerTable.Add(LocalizationId.Type, "Type");
            innerTable.Add(LocalizationId.Value, "Value");
            innerTable.Add(LocalizationId.Count, "Count");
          
            innerTable.Add(LocalizationId.ObjectChildren, "ObjectChildren");
            innerTable.Add(LocalizationId.ArrayChildren, "ArrayChildren");
            innerTable.Add(LocalizationId.Null, "#Null");
            innerTable.Add(LocalizationId.Empty, "#Empty");
            innerTable.Add(LocalizationId.Exception, "Exception");
            innerTable.Add(LocalizationId.Nullable, "Nullable");
            innerTable.Add(LocalizationId.Delimeter, "|");

            innerTable.Add(LocalizationId.ValueCycle, "ValueCycle");
            innerTable.Add(LocalizationId.ReferenceCycle, "ReferenceCycle");
        }

        public static string GetStringById(LocalizationId id) {
            return innerTable[id];
        }
    } 
}