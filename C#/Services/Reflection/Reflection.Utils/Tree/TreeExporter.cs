using System;
using System.Collections.Generic;

namespace Reflection.Utils.Tree.Export {
    public static class TreeExporter<Writer, ItemValue, Format> {
        public static void Export(Writer writer, Tree<ItemValue> tree, Format format, Action<Writer, ItemValue, Format> exportRoot, Action<Writer, ItemValue, Format, int> exportChild, Func<ItemValue, bool> canExportChild) {
            TreeItem<ItemValue> rootItem = tree.RootItem;
            exportRoot(writer, rootItem.Value, format);
            ExportCore(writer, rootItem.Children, format, 1, (w, i, f, l) => exportChild(w, i, f, l), i => canExportChild(i));
        }

        static void ExportCore(Writer writer, IEnumerable<TreeItem<ItemValue>> items, Format format, int level, Action<Writer, ItemValue, Format, int> exportChild, Func<ItemValue, bool> canExportChild) {
            foreach (TreeItem<ItemValue> item in items) {
                exportChild(writer, item.Value, format, level);
                if (canExportChild(item.Value))
                    ExportCore(writer, item.Children, format, level + 1, (w, i, f, l) => exportChild(w, i, f, l), i => canExportChild(i));
            }
        }
    }
}