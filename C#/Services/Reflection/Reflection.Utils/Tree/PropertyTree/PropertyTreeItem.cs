using Reflection.Utils.Tree.Localization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Reflection.Utils.PropertyTree {
    public class PropertyTreeItem {
        readonly IEnumerable<PropertyTreeItem> objectParents;
        readonly PropertyField field;
        readonly object value;
        readonly List<PropertyTreeItem> objectChildren;
        readonly List<PropertyTreeItem> arrayChildren;

        bool hasObjectCycle;
        bool hasArrayChildren;

        public PropertyTreeItem(IEnumerable<PropertyTreeItem> objectParents, PropertyField field, object value) {
            this.objectParents = objectParents;
            this.value = value;
            this.field = field;
            this.objectChildren = new List<PropertyTreeItem>();
            this.arrayChildren = new List<PropertyTreeItem>();
            this.hasObjectCycle = false;
            this.hasArrayChildren = false;
        }
        
        public PropertyField Field { get { return this.field; } }
        public object Value { get { return this.value; } }
        public bool HasObjectCycle { get { return this.hasObjectCycle; } }
        public bool HasArrayChildren { get { return this.hasArrayChildren; } }
        
        public IEnumerable<PropertyTreeItem> ObjectParents { get { return this.objectParents; } }
        public IEnumerable<PropertyTreeItem> ObjectChildren { get { return this.objectChildren; } }
        public IEnumerable<PropertyTreeItem> ArrayChildren { get { return this.arrayChildren; } }

        public void AddObjectChild(PropertyTreeItem objectChild) {
            this.objectChildren.Add(objectChild);
        }

        public void AddArrayChild(PropertyTreeItem arrayChild) {
            this.arrayChildren.Add(arrayChild);
        }

        public void SetHasObjectCycle(bool value) {
            if (this.hasObjectCycle == value)
                return;
            this.hasObjectCycle = value;
            if (value)
                this.objectChildren.Clear();
        }

        public void SetHasArrayChildren(bool value) {
            if (this.hasArrayChildren == value)
                return;
            this.hasArrayChildren = value;
            if (!value)
                this.arrayChildren.Clear();
        }

        public override string ToString() {
            return Value.ToString() + " " + GetChildrenStringInfo();
        }

        string GetChildrenStringInfo() {
            string result = String.Format(LocalizationTable.GetStringById(LocalizationId.ObjectChildren) + ": {0}", this.objectChildren.Count());
            if (this.hasObjectCycle)
                result += ", " + LocalizationTable.GetStringById(LocalizationId.HasObjectChildrenCycle);
            return result;
        }
    }
}
