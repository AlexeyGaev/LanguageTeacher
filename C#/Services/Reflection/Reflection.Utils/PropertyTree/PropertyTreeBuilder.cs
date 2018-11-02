using System;
using System.Collections.Generic;
using System.Linq;
using Reflection.Utils.Tree;

namespace Reflection.Utils.PropertyTree {
    public static class PropertyTreeBuilder {
        public static Tree<PropertyDescription> Create(object source, string sourceName, Type sourceType) {
            return TreeBuilder<PropertyDescription>.Create(
                source,
                s => CreateRootValue(s, sourceName, sourceType, parents.Clone()),
                (t, o) => CreateDescriptions(t, o, parents.Clone()),
                p => p.PropertyValue.HasChildren,
                p => p.PropertyValue.Value);
        }

        static PropertyDescription CreateRootValue(object propertyValue, string propertyName, Type propertyType) {
            return new PropertyDescription(null, propertyName, propertyType, PropertyValueBuilder.CreateCore(null, propertyType, propertyValue, parents));
        }

        static IEnumerable<PropertyDescription> CreateDescriptions(Tree<PropertyDescription> tree, object owner, ParentValues parents) {
            return owner.GetType().GetProperties().Select(p => new PropertyDescription(owner, p.Name, p.PropertyType, PropertyValueBuilder.Create(tree, owner, p)));
        }
    }
}
