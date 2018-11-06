namespace Reflection.Utils.Tree.Export {
    public static class TreeExporter<T> {
        public static void ExportItem(StringWriter writer, TreeItem<T> item, int level) {
            writer.WriteLine(IndentBuilder.CreateLevelSpace(level, " ") + item.ToString());
            if (item.Children != null)
                foreach (TreeItem<T> child in item.Children)
                    ExportItem(writer, child, level + 1);
        }
    }
}