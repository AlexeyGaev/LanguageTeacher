using System;

namespace Reflection.Utils.PropertyTree.Serialization {
    public static class SerializeItemToStringBuilder {
        public static string Create(SerializeItem item) {
            if (item.Mode == SerializeItemMode.OneValue)
                return item.FirstValue;
            else if (item.Mode == SerializeItemMode.TwoValues)
                return item.FirstValue + "=" + item.SecondValue;
            else if (item.Mode == SerializeItemMode.Delimeter)
                return Localization.Delimeter;
            else
                return String.Empty;
        }
    }
}
