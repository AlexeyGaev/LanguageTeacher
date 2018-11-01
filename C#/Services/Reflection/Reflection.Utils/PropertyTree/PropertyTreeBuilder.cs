using System;
using System.Collections.Generic;
using System.Linq;
using Reflection.Utils.Tree;

namespace Reflection.Utils.PropertyTree {
    public static class PropertyTreeBuilder {
        public static Tree<PropertyDescription> Create(object source, string sourceName, Type sourceType) {
            return TreeBuilder<PropertyDescription>.Create(
                source,
                s => CreateRootValue(s, sourceName, sourceType),
                (t, o) => CreateDescriptions(t, o),
                p => p.PropertyValue.HasChildren,
                p => p.PropertyValue.Value);
        }

        static PropertyDescription CreateRootValue(object propertyValue, string propertyName, Type propertyType) {
            return new PropertyDescription(null, propertyName, propertyType, PropertyValueBuilder.CreateCore(null, propertyType, propertyValue));
        }

        static IEnumerable<PropertyDescription> CreateDescriptions(Tree<PropertyDescription> tree, object owner) {
            return owner.GetType().GetProperties().Select(p => new PropertyDescription(owner, p.Name, p.PropertyType, PropertyValueBuilder.Create(tree, owner, p)));
        }
    }
}
