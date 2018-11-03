using System.Collections.Generic;

namespace Reflection.Utils.PropertyTree.Export {
    public static class Localization {
        static Dictionary<LocalizationId, string> innerTable;

        static Localization() {
            innerTable = new Dictionary<LocalizationId, string>();
            innerTable.Add(LocalizationId.Null, "Null");
            innerTable.Add(LocalizationId.Empty, "Empty");
            innerTable.Add(LocalizationId.Nullable, "Nullable");
            innerTable.Add(LocalizationId.Exception, "Exception");
        }

        public static string GetStringById(LocalizationId id) {
            return innerTable[id];
        }
    } 
}