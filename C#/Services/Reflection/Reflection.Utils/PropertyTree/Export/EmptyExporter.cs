namespace Reflection.Utils.PropertyTree.Export {
    public class EmptyExporter {
        public static string Write(string input, string propertyName) {
            return
                input + propertyName + Localization.Indent + Localization.IsNull +
                Localization.Dot + Localization.NewLine;
        }
    }
}