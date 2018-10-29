using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Translation.Http.Tree {
    public static class PropertyDescriptionTreeBuilder {
        public static Tree<PropertyDescription> Create(object source) {
            Tree<PropertyDescription> tree = new Tree<PropertyDescription>();
            AddItems(tree, tree.RootItems, source);
            return tree;
        }

        static void AddItems(Tree<PropertyDescription> tree, List<Item<PropertyDescription>> items, object owner) {
            if (owner == null)
                return;
            foreach (PropertyDescription value in CreateValueItems(tree, owner)) {
                Item<PropertyDescription> item = new Item<PropertyDescription>(value);
                items.Add(item);
                if (CanAddChildren(value)) 
                    AddItems(tree, item.Children, GetChildOwner(value));
            }
        }

        static object GetChildOwner(PropertyDescription value) {
            return value.PropertyValue.Value;
        }

        static bool CanAddChildren(PropertyDescription value) {
            return value.HasChildren;
        }

        static IEnumerable<PropertyDescription> CreateValueItems(Tree<PropertyDescription> tree, object owner) {
            return PropertyDescriptionsBuilder.Create(tree, owner);
        }
    }
}
