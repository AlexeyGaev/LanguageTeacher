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
            Action<TreeItem<T>> addChildren) {

            TreeItem<T> result = new TreeItem<T>(parents, value);
            if (!canCreateChildren(result))
                return result;
            if (shouldCheckChildrenCycle(result)) {
                bool hasChildrenCycle = getHasChildrenCycle(result);
                if (getHasChildrenCycle(result)) {
                    result.SetHasChildrenCycle(true);
                    return result;
                }
            }
            result.InitChildren();
            addChildren(result);
            return result;
        }
    }
}
