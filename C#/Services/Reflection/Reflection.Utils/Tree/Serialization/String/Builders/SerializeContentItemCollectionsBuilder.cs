using System.Collections.Generic;

namespace Reflection.Utils.PropertyTree.Serialization {
    public static class SerializeContentItemCollectionsBuilder {
        public static IEnumerable<SerializeContentItemCollection> Create(PropertyObjectChildren objectChildren, IEnumerable<PropertyItem> arrayChildren) {
            if (objectChildren == null && arrayChildren == null)
                return null;
            List<SerializeContentItemCollection> result = new List<SerializeContentItemCollection>();
            if (objectChildren != null) {
                if (objectChildren.HasCycle)
                    result.Add(SerializeContentItemCollectionBuilder.CreateCycle(Localization.ObjectChildren));
                else
                    result.Add(SerializeContentItemCollectionBuilder.Create(objectChildren, Localization.ObjectChildren));
            }
            if (arrayChildren != null) 
                result.Add(SerializeContentItemCollectionBuilder.Create(arrayChildren, Localization.ArrayChildren));
            return result;
        }
    }
}

