namespace Vocabulary.English {
    public class VerbDetails : IWordDetails {
        public WordType WordType { get { return WordType.Verb; } }
        public VerbPartyType PartyType { get; set; }
        public VerbRoleType RoleType { get; set; }
        public VerbTense Tense { get; set; }
        public VerbParticipleType ParticipleType { get; set; }
        public VerbPluralType PluralType { get; set; }

        // TODO: 
        //public bool IsPerson { get; set; }
        //public bool IsAspect { get; set; }
        //public bool IsModal { get; set; }
        //public bool IsGerund { get; set; }
        //public bool IsParticiple { get; set; }
        //public bool IsVoice { get; set; }
        //public bool IsMode { get; set; }
    }
}
