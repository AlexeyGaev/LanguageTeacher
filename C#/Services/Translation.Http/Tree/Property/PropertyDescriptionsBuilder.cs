using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Translation.Http.Tree {
    public static class PropertyDescriptionsBuilder {
        public static IEnumerable<PropertyDescription> Create(Tree<PropertyDescription> tree, object owner) {
            return owner.GetType().GetProperties().Select(p => new PropertyDescription(owner, p, PropertyValueBuilder.Create(tree, owner, p)));
        }
    }
}
