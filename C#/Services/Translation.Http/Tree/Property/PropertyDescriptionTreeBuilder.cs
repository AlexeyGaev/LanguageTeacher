using System.Collections.Generic;

namespace Translation.Http.Tree {
    public static class PropertyDescriptionTreeBuilder {
        public static Tree<PropertyDescription> Create(object source, string name) {
            Tree<PropertyDescription> tree = new Tree<PropertyDescription>();
            if (source == null)
                return tree;
            PropertyValue propertyValue = PropertyValueBuilder.Create(null, source.GetType(), source);
            PropertyDescription propertyDescription = new PropertyDescription(null, name, source.GetType(), propertyValue);
            tree.RootItem = new TreeItem<PropertyDescription>(propertyDescription);
            foreach (PropertyDescription value in CreateValueItems(tree, source)) {
                TreeItem<PropertyDescription> item = new TreeItem<PropertyDescription>(value);
                AddItems(tree, tree.RootItem.Children, GetChildOwner(value));
            }
            return tree;
        }

        static void AddItems(Tree<PropertyDescription> tree, List<TreeItem<PropertyDescription>> items, object owner) {
            if (owner == null)
                return;
            foreach (PropertyDescription value in CreateValueItems(tree, owner)) {
                TreeItem<PropertyDescription> item = new TreeItem<PropertyDescription>(value);
                items.Add(item);
                if (CanAddChildren(value)) 
                    AddItems(tree, item.Children, GetChildOwner(value));
            }
        }

        static object GetChildOwner(PropertyDescription value) {
            return value.PropertyValue.Value;
        }

        static bool CanAddChildren(PropertyDescription value) {
            return value.PropertyValue.HasChildren;
        }

        static IEnumerable<PropertyDescription> CreateValueItems(Tree<PropertyDescription> tree, object owner) {
            return PropertyDescriptionsBuilder.Create(tree, owner);
        }
    }
}
