namespace Reflection.Utils.PropertyTree.Export {
    public class ExportStringFormat {
        string propertyNameFormat = "{0} ({1}) = ";
        string propertyValueFormat = "{0} ({1})";
        string propertyValueNullFormat = Localization.GetStringById(LocalizationId.Null) + " ({1})";
        string propertyValueEmptyFormat = Localization.GetStringById(LocalizationId.Empty) + " ({1})";
        string space = " ";

        public string PropertyNameFormat { get { return this.propertyNameFormat; } }
        public string PropertyValueFormat { get { return this.propertyValueFormat; } }
        public string PropertyValueNullFormat { get { return this.propertyValueNullFormat; } }
        public string PropertyValueEmptyFormat { get { return this.propertyValueEmptyFormat; } }
        public string Space { get { return this.space; } }

        public string CreateLevelIndent(int level) {
            return IndentBuilder.CreateLevelSpace(level, this.space);
        }
    }
}