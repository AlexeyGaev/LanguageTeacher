namespace Reflection.Utils.PropertyTree.Serialization {
    public struct SerializeItem {
        static SerializeItem empty = CreateEmpty();
        public static SerializeItem Empty { get { return empty; } }
        public static SerializeItem CreateOneValue(string value) {
            SerializeItem result = new SerializeItem();
            result.valueFirst = value;
            result.valueSecond = value;
            result.mode = SerializeItemMode.OneValue;
            return result;
        }
        public static SerializeItem CreateTwoValues(string value1, string value2) {
            SerializeItem result = new SerializeItem();
            result.valueFirst = value1;
            result.valueSecond = value2;
            result.mode = SerializeItemMode.TwoValues;
            return result;
        }

        static SerializeItem CreateEmpty() {
            SerializeItem result = new SerializeItem();
            result.mode = SerializeItemMode.Empty;
            return result;
        }

        string valueFirst;
        string valueSecond;
        SerializeItemMode mode;
      
        public string ValueFirst { get { return this.valueFirst; } }
        public string ValueSecond { get { return this.valueSecond; } }
        public SerializeItemMode Mode { get { return this.mode; } }
    }
}
