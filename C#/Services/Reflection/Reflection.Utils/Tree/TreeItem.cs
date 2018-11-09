using Reflection.Utils.Tree.Localization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Reflection.Utils.Tree {
    public class TreeItem<T> {
        readonly IEnumerable<TreeItem<T>> parents;
        readonly T value;
        bool hasChildrenCycle;
        IList<TreeItem<T>> children;

        public TreeItem(IEnumerable<TreeItem<T>> parents, T value) {
            this.parents = parents;
            this.value = value;
        }

        public IEnumerable<TreeItem<T>> Parents { get { return this.parents; } }
        public T Value { get { return this.value; } }

        public IEnumerable<TreeItem<T>> Children { get { return this.children; } }
        public bool HasChildrenCycle { get { return this.hasChildrenCycle; } }

        public void InitChildren() {
            this.children = new List<TreeItem<T>>();
            this.hasChildrenCycle = false;
        }

        public void AddChild(TreeItem<T> child) {
            this.children.Add(child);
        }

        public void SetHasChildrenCycle(bool value) {
            if (this.hasChildrenCycle == value)
                return;
            this.hasChildrenCycle = value;
            if (value)
                this.children = null;
        } 

        //public override string ToString() {
        //    return Value.ToString() + " " + GetChildrenStringInfo();
        //}

        //string GetChildrenStringInfo() {
        //    if (Children == null) {
        //        if (HasChildrenCycle)
        //            return
        //                "(" + LocalizationTable.GetStringById(LocalizationId.ObjectChildren) + ": " +
        //                LocalizationTable.GetStringById(LocalizationId.Null) + ", " +
        //                LocalizationTable.GetStringById(LocalizationId.HasObjectChildrenCycle) + ")";
        //        else
        //            return
        //                "(" + LocalizationTable.GetStringById(LocalizationId.ObjectChildren) + ": " +
        //                LocalizationTable.GetStringById(LocalizationId.Null) + ")";
        //    }
        //    int childrenCount = Children.Count();
        //    if (childrenCount == 0)
        //        return 
        //            "(" +  LocalizationTable.GetStringById(LocalizationId.ObjectChildren) + ": " +
        //            LocalizationTable.GetStringById(LocalizationId.Empty) + ")";
        //    return String.Format("(" + LocalizationTable.GetStringById(LocalizationId.ObjectChildren) + ": {0})", childrenCount);
        //}
    }
}
