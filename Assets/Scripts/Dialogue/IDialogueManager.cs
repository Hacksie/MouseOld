namespace HackedDesign {
    namespace Dialogue {
        public interface IDialogueManager {
            void Initialize (Input.IInputController input);
            Dialogue GetCurrentDialogue ();
            void ShowDialogue (string name);
            void DialogueButtonEvent ();
        }
    }
}