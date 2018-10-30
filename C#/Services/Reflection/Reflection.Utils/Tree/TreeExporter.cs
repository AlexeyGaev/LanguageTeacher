using System;
using System.Collections.Generic;

namespace Reflection.Utils.Tree.Export {
    public static class TreeExporter<W, T> {
        public static void Export(W writer, Tree<T> tree, Action<W, T> exportRoot, Action<W, T, int> exportChild, Func<T, bool> canExportChild) {
            TreeItem<T> rootItem = tree.RootItem;
            exportRoot(writer, rootItem.Value);
            ExportCore(writer, 1, rootItem.Children, (w, t, i) => exportChild(w, t, i), t => canExportChild(t));
        }

        static void ExportCore(W writer, int indentLevel, List<TreeItem<T>> items, Action<W, T, int> exportChild, Func<T, bool> canExportChild) {
            foreach (TreeItem<T> item in items) {
                exportChild(writer, item.Value, indentLevel);
                if (canExportChild(item.Value))
                    ExportCore(writer, indentLevel + 1, item.Children, (w, t, i) => exportChild(w, t, i), t => canExportChild(t));
            }
        }
    }
}