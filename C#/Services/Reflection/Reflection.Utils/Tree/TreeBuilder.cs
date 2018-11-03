using System;
using System.Collections.Generic;

namespace Reflection.Utils.Tree {
    //public static class TreeBuilder<T> {
    //    public static TreeItem<T> Create(
    //        //IEnumerable<TreeItem<T>> parents, 
    //        //object source, 
    //        //Func<IEnumerable<TreeItem<T>>, object, T> createRootValue, 
    //        //Func<IEnumerable<TreeItem<T>>, object, IEnumerable<T>> createValueItems, 
    //        //Func<IEnumerable<TreeItem<T>>, T, bool> canAddChildren, 
    //        //Func<T, object> getChildOwner) {
                       
    //        //T rootValue = createRootValue(parents, source);
    //        //if (canAddChildren(parents, rootValue)) {

    //        //}

    //        //IEnumerable<TreeItem<T>> children = Create
    //        //TreeItem <T> item = new TreeItem<T>(createRootValue(parents, source), parents);
    //        //AddItems(parents, tree.RootItem.Children, source, (p, o) => createValueItems(p, o), i => canAddChildren(i), i => getChildOwner(i));
    //        //return tree;
    //    }

    //    static void AddItems(IEnumerable<TreeItem<T>> parents, List<TreeItem<T>> items, object owner, Func<IEnumerable<TreeItem<T>>, object, IEnumerable<T>> createValueItems, Func<T, bool> canAddChildren, Func<T, object> getChildOwner) {
    //        if (owner == null)
    //            return;
    //        foreach (T value in createValueItems(parents, owner)) {
    //            if (canAddChildren(value)) 
    //                AddItems(CloneParents(parents), item.Children, getChildOwner(value), (t, o) => createValueItems(t, o), i => canAddChildren(i), i => getChildOwner(i));

    //            TreeItem<T> item = new TreeItem<T>(value, parents);
    //            items.Add(item);
    //        }
    //    }

    //    static List<TreeItem<T>> CloneParents(IEnumerable<TreeItem<T>> parents) {
    //        return new List<TreeItem<T>>(parents);
    //    }
    //}
}
