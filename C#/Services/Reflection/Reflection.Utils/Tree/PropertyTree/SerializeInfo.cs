namespace Reflection.Utils.PropertyTree {
    public struct SerializeInfo {
        string name;
        string value;

        public SerializeInfo(string name, string value) {
            this.name = name;
            this.value = value;
        }

        public string Name { get { return this.name; } }
        public string Value { get { return this.value; } }
    }
}
