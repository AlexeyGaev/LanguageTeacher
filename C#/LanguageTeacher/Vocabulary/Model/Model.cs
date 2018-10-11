using Vocabulary.Commands;
namespace Vocabulary.Model {
    public class Model {
        public Unit[] Words { get; set; }
        public Unit[] Suggestions { get; set; }
        public Unit[] StableExpressions { get; set; }
        
        public ICommand Translation { get; set; }


    }
}
