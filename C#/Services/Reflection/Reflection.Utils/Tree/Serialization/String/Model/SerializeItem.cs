namespace Reflection.Utils.PropertyTree.Serialization {
    public struct SerializeItem {
        static SerializeItem empty = CreateEmpty();
        public static SerializeItem Empty { get { return empty; } }
        public static SerializeItem CreateOneValue(string value) {
            SerializeItem result = new SerializeItem();
            result.firstValue = value;
            result.secondValue = value;
            result.mode = SerializeItemMode.OneValue;
            return result;
        }
        public static SerializeItem CreateTwoValues(string value1, string value2) {
            SerializeItem result = new SerializeItem();
            result.firstValue = value1;
            result.secondValue = value2;
            result.mode = SerializeItemMode.TwoValues;
            return result;
        }

        static SerializeItem CreateEmpty() {
            SerializeItem result = new SerializeItem();
            result.mode = SerializeItemMode.Empty;
            return result;
        }

        string firstValue;
        string secondValue;
        SerializeItemMode mode;
      
        public string FirstValue { get { return this.firstValue; } }
        public string SecondValue { get { return this.secondValue; } }
        public SerializeItemMode Mode { get { return this.mode; } }
    }
}
