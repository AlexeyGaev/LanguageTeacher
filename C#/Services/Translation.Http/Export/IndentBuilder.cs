using System;
using Translation.Http.Tree;

namespace Translation.Http {
    public static class IndentBuilder {
        public static string Create(int level) {
            string indent = string.Empty;
            for (int i = 0; i < level; i++)
                indent += Localization.Indent;
            return indent;
        }
    }
}