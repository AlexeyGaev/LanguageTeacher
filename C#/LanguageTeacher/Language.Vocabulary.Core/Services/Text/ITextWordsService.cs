﻿using Utils.Collection;

namespace Language.Vocabulary.Core {
    public interface ITextWordsService {
        ICollection<IContextUnit> GetWords(IUnit text);
    }
}