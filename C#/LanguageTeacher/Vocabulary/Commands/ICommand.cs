namespace Vocabulary.Commands {
    public interface ICommand {
        bool CanExecute { get; }
        void Execute();
    }
}
