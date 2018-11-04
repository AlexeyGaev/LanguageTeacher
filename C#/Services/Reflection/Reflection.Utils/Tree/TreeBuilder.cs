using System;
using System.Collections.Generic;

namespace Reflection.Utils.Tree {
    public static class TreeBuilder<T> {
        public static TreeItem<T> Create(
            IEnumerable<TreeItem<T>> parents,
            T value,
            Func<TreeItem<T>, bool> canCreateChildren,
            Func<TreeItem<T>, bool> shouldCheckChildrenCycle,
            Func<TreeItem<T>, bool> getHasChildrenCycle,
            Func<TreeItem<T>, IEnumerable<TreeItem<T>>> createChildren) {

            TreeItem<T> result = new TreeItem<T>();
            result.Parents = parents;
            result.Value = value;
            if (!canCreateChildren(result))
                return result;
            if (shouldCheckChildrenCycle(result)) {
                bool hasChildrenCycle = getHasChildrenCycle(result);
                result.HasChildrenCycle = hasChildrenCycle;
                if (hasChildrenCycle)
                    return result;
            }
            result.Children = createChildren(result);
            return result;
        }
    }
}
