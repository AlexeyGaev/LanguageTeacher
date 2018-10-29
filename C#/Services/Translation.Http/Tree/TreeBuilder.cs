using System;
using System.Collections.Generic;

namespace Translation.Http.Tree {
    public static class TreeBuilder<T> {
        public static Tree<T> Create(object source, 
            Func<object, T> createRootValue, 
            Func<Tree<T>, object, IEnumerable<T>> createValueItems, 
            Func<T, bool> canAddChildren, 
            Func<T, object> getChildOwner) {

            Tree<T> tree = new Tree<T>();
            if (source == null)
                return tree;

            tree.RootItem = new TreeItem<T>(createRootValue(source));
            AddItems(tree, tree.RootItem.Children, source, (t, o) => createValueItems(t, o), i => canAddChildren(i), i => getChildOwner(i));
            return tree;
        }

        static void AddItems(Tree<T> tree, List<TreeItem<T>> items, object owner, Func<Tree<T>, object, IEnumerable<T>> createValueItems, Func<T, bool> canAddChildren, Func<T, object> getChildOwner) {
            if (owner == null)
                return;
            foreach (T value in createValueItems(tree, owner)) {
                TreeItem<T> item = new TreeItem<T>(value);
                items.Add(item);
                if (canAddChildren(value))
                    AddItems(tree, item.Children, getChildOwner(value), (t, o) => createValueItems(t, o), i => canAddChildren(i), i => getChildOwner(i));
            }
        }
    }
}
