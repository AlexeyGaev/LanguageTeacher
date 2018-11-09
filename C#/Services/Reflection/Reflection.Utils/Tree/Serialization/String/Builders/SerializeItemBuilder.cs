using System;

namespace Reflection.Utils.PropertyTree.Serialization {
    public static class SerializeItemBuilder {
        public static SerializeItem CreateFieldIdItem(object fieldId) {
            string idName = Localization.IdName;
            string idValue = StringBuilder.CreateStringFromObject(fieldId);
            return SerializeItem.CreateTwoValues(idName, idValue);
        }

        public static SerializeItem CreateTypeItem(Type type) {
            string typeName = Localization.TypeName;
            string typeValue = StringBuilder.CreateStringFromType(type);
            return SerializeItem.CreateTwoValues(typeName, typeValue);
        }

        public static SerializeItem CreateNullValueItem() {
            string value1 = Localization.ValueName;
            string value2 = Localization.NullValue;
            return SerializeItem.CreateTwoValues(value1, value2);
        }

        public static SerializeItem CreateValueItem(object value) {
            string value1 = Localization.ValueName;
            string value2 = StringBuilder.CreateStringFromValue(value);
            return SerializeItem.CreateTwoValues(value1, value2);
        }
    }
}

