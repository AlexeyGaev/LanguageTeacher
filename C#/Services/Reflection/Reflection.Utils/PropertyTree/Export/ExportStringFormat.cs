namespace Reflection.Utils.PropertyTree.Export {
    public class ExportStringFormat {
        string propertyNameFormat = "{0} ({1}) = ";

        string propertyValueFormat = "{0} ({1})";
        string propertyNullableValueFormat = "{0} (" + Localization.GetStringById(LocalizationId.Nullable) + " {1})";
        
        string propertyValueFormatWithException = Localization.GetStringById(LocalizationId.Exception) + " {0} ({1})";
        string propertyNullableValueFormatWithException = Localization.GetStringById(LocalizationId.Exception) + " {0} (" + Localization.GetStringById(LocalizationId.Nullable) + " {1})";

        string propertyValueNullFormat = Localization.GetStringById(LocalizationId.Null) + " ({0})";
        string propertyNullableValueNullFormat = Localization.GetStringById(LocalizationId.Null) + " (" + Localization.GetStringById(LocalizationId.Nullable) +" {0})";

        string propertyValueEmptyFormat = Localization.GetStringById(LocalizationId.Empty) + " ({1})";
        string space = " ";

        public string PropertyNameFormat { get { return this.propertyNameFormat; } }

        public string PropertyValueFormat { get { return this.propertyValueFormat; } }
        public string PropertyNullableValueFormat { get { return this.propertyNullableValueFormat; } }

        public string PropertyValueFormatWithException { get { return this.propertyValueFormatWithException; } }
        public string PropertyNullableValueFormatWithException { get { return this.propertyNullableValueFormatWithException; } }

        public string PropertyValueNullFormat { get { return this.propertyValueNullFormat; } }
        public string PropertyNullableValueNullFormat { get { return this.propertyNullableValueNullFormat; } }

        public string PropertyValueEmptyFormat { get { return this.propertyValueEmptyFormat; } }
        public string Space { get { return this.space; } }

        public string CreateLevelIndent(int level) {
            return IndentBuilder.CreateLevelSpace(level, this.space);
        }
    }
}