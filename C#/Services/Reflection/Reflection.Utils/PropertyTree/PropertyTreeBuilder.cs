using System;
using System.Collections.Generic;
using System.Linq;
using Reflection.Utils.Tree;

namespace Reflection.Utils.PropertyTree {
    public static class PropertyTreeBuilder {
        public static Tree<PropertyDescription> Create(object source, string name) {
            return TreeBuilder<PropertyDescription>.Create(
                source,
                s => CreateRootValue(s, name),
                (t, o) => CreateDescriptions(t, o),
                p => p.PropertyValue.HasChildren,
                p => p.PropertyValue.Value);
        }

        static PropertyDescription CreateRootValue(object source, string name) {
            Type ownerType = source.GetType();
            return new PropertyDescription(null, name, ownerType, PropertyValueBuilder.Create(null, ownerType, source));
        }

        static IEnumerable<PropertyDescription> CreateDescriptions(Tree<PropertyDescription> tree, object owner) {
            return owner.GetType().GetProperties().Select(p => new PropertyDescription(owner, p.Name, p.PropertyType, PropertyValueBuilder.Create(tree, owner, p)));
        }
    }
}
