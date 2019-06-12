using System.Collections.Generic;

namespace Reflection.Utils.PropertyTree.Serialization {
    public static class SerializeContentItemCollectionsBuilder {
        public static IEnumerable<SerializeContentItemCollection> Create(PropertyObjectChildren objectChildren, IEnumerable<PropertyItem> arrayChildren) {
            if (objectChildren == null && arrayChildren == null)
                return null;
            List<SerializeContentItemCollection> result = new List<SerializeContentItemCollection>();
            if (objectChildren != null) 
                result.Add(SerializeContentItemCollectionBuilder.Create(objectChildren, Localization.ObjectChildren, objectChildren.CycleMode));
            if (arrayChildren != null) 
                result.Add(SerializeContentItemCollectionBuilder.Create(arrayChildren, Localization.ArrayChildren, CycleMode.None));
            return result;
        }
    }
}

