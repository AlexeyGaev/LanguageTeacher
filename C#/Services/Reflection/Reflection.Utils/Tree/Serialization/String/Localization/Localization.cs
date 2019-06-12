namespace Reflection.Utils.PropertyTree.Serialization {
    public static class Localization {
        public static string IdName { get { return LocalizationTable.GetStringById(LocalizationId.Id); } }
        public static string TypeName { get { return LocalizationTable.GetStringById(LocalizationId.Type); } }
        public static string ValueName { get { return LocalizationTable.GetStringById(LocalizationId.Value); } }
        public static string NullValue { get { return LocalizationTable.GetStringById(LocalizationId.Null); } }
        public static string EmptyValue { get { return LocalizationTable.GetStringById(LocalizationId.Empty); } }
        public static string Delimeter { get { return LocalizationTable.GetStringById(LocalizationId.Delimeter); } }
        public static string ObjectChildren { get { return LocalizationTable.GetStringById(LocalizationId.ObjectChildren); } }
        public static string ArrayChildren { get { return LocalizationTable.GetStringById(LocalizationId.ArrayChildren); } }
        public static string Count { get { return LocalizationTable.GetStringById(LocalizationId.Count); } }
        public static string Nullable { get { return LocalizationTable.GetStringById(LocalizationId.Nullable); } }
        public static string Exception { get { return LocalizationTable.GetStringById(LocalizationId.Exception); } }
        public static string ValueCycle { get { return LocalizationTable.GetStringById(LocalizationId.ValueCycle); } }
        public static string ReferenceCycle { get { return LocalizationTable.GetStringById(LocalizationId.ReferenceCycle); } }
    }
}