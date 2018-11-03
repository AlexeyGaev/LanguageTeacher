using System;
using System.Collections.Generic;
using System.Reflection;
using Reflection.Utils.Tree;

namespace Reflection.Utils.PropertyTree {
   

    //public static class PropertyValueBuilder {
        

        //static bool ContainsByOwner(Tree<PropertyDescription> tree, object owner) {
        //    return ContainsByOwnerCore(tree.RootItem.Children, owner);
        //}

        //static bool ContainsByOwnerCore(List<TreeItem<PropertyDescription>> items, object owner) {
        //    if (owner == null || owner.GetType().IsValueType)
        //        return false;
        //    foreach (TreeItem<PropertyDescription> item in items) {
        //        PropertyDescription parentProperty = item.Value;
        //        object parentOwner = parentProperty.Owner;
        //        Type parentType = parentOwner.GetType();
        //        if (parentType.IsValueType)
        //            continue;
        //        if (IsEqualOwners(parentOwner, owner))
        //            return true;
        //        if (ContainsByOwnerCore(item.Children, owner))
        //            return true;
        //    }
        //    return false;
        //}

        //static bool IsEqualOwners(object parent, object owner) {
        //    return Object.ReferenceEquals(parent, owner);
        //}
    //}

    //public class TreeWalker<T> {
    //    readonly TreeItem<T> rootItem;
    //    public PropertyTreeWalker(TreeItem<T> rootItem) {
    //        this.rootItem = rootItem;
    //    }

    //    public bool NextChild() {
    //        if ()
    //    }

    //}
}
