using System.Collections.Generic;

namespace Translation.Http.Tree {
    public class Tree<T> {
        List<Item<T>> rootItems = new List<Item<T>>();
        public List<Item<T>> RootItems { get { return this.rootItems; } }
    }
}
