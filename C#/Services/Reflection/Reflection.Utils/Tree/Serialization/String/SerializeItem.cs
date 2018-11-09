namespace Reflection.Utils.PropertyTree.Selialization {
    public struct SerializeItem {
        string name;
        string value;
        SerializeItemMode mode;

        public SerializeItem(string name, string value, SerializeItemMode mode) {
            this.name = name;
            this.value = value;
            this.mode = mode;
        }

        public string Name { get { return this.name; } }
        public string Value { get { return this.value; } }

        public SerializeItemMode Mode { get { return this.mode; } }
    }
}
