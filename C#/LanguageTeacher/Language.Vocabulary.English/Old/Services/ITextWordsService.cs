﻿using Utils.Collections;

namespace Language.Vocabulary.Core {
    public interface ITextWordsService {
        IEnumerator<IUnit> GetWords(IUnit text);
    }
}
