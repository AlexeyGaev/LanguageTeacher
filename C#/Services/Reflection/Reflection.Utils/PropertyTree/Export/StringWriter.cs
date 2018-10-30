using System.Collections.Generic;

namespace Reflection.Utils.PropertyTree.Export {
    public class StringWriter {
        List<string> result;

        public IEnumerable<string> Result { get { return this.result; } }

        public void Write(string value) {
            if (this.result == null) {
                Create();
                Add(value);
                return;
            }
            this.result[this.result.Count - 1] += value;
        }

        public void WriteLine(string value) {
            if (result == null)
                Create();
            Add(value);
        }

        void Create() {
            result = new List<string>();
        }

        void Add(string value) {
            result.Add(value);
        }
    }
}