using System;
using System.Collections.Generic;

namespace Reflection.Utils.PropertyTree {
    public class Filter {
        int levelCount = 1;
        List<Type> stopTypes = new List<Type>();

        public int LevelCount { get { return this.levelCount; } set { this.levelCount = value; } }
        public List<Type> StopTypes { get { return this.stopTypes; } }
        
        public bool CanCreatePropertyItem(object propertyId, Type propertyType, object propertyValue, PropertyCollection parents) {
            int currentLevelCount = parents == null ? 0 : parents.Count;
            return currentLevelCount <= this.levelCount;
        }

        public bool CanCreateChildren(Type propertyType, object propertyValue) {
            return !this.StopTypes.Contains(propertyType);
        }
    }
}