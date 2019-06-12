using System.Collections.Generic;

namespace Notebook.Core.Data {
    public class AccountCollection {
        readonly List<Account> items;
       
        public Account this[int index] { get { return this.items[index]; } }
        public int Count { get { return this.items.Count; } }

        public void Add(Account item) {
            // TODO
        }
        public void AddRange(IEnumerable<Account> items) {
            // TODO
        }
        public void Remove(Account item) {
            // TODO
        }
        public void Clear() {
            // TODO
        }
    }
}
