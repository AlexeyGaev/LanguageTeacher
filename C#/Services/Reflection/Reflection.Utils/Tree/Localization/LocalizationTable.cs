using System.Collections.Generic;

namespace Reflection.Utils.Tree.Localization {
    public static class LocalizationTable {
        static Dictionary<LocalizationId, string> innerTable;

        static LocalizationTable() {
            innerTable = new Dictionary<LocalizationId, string>();
            innerTable.Add(LocalizationId.Null, "#Null");
            innerTable.Add(LocalizationId.Empty, "#Empty");
            innerTable.Add(LocalizationId.Nullable, "Nullable");
            innerTable.Add(LocalizationId.Exception, "Exception");
            innerTable.Add(LocalizationId.Children, "Children");
            innerTable.Add(LocalizationId.HasChildrenCycle, "HasCycle");
            innerTable.Add(LocalizationId.Name, "Name");
            innerTable.Add(LocalizationId.Type, "Type");
            innerTable.Add(LocalizationId.TypeCode, "TypeCode");
            innerTable.Add(LocalizationId.Value, "Value");
        }

        public static string GetStringById(LocalizationId id) {
            return innerTable[id];
        }
    } 
}