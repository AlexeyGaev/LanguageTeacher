namespace Reflection.Utils.PropertyTree.Serialization {
    public static class IndentBuilder {
        public static string CreateLevelSpace(int level, string space) {
            string indent = string.Empty;
            for (int i = 0; i < level; i++)
                indent += space;
            return indent;
        }
    }
}
