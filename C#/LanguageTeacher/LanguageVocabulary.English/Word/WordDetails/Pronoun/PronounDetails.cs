namespace Vocabulary.English {
    public class PronounDetails : IWordDetails {
        public WordType WordType { get { return WordType.Pronoun; } }
        public VerbPartyType PartyType { get; set; }
        public bool IsPlural { get; set; }
        public PronounType PronounType { get; set; }
    }
}
