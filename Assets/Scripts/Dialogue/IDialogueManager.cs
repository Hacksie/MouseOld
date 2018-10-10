namespace HackedDesign {
    namespace Dialogue {
        public interface IDialogueManager {
            void Initialize (Input.IInputController input);
            Dialogue GetCurrentDialogue ();
            void ShowDialogue (string name);
            void DialogueButton1Event ();
            void DialogueButton2Event ();
            void DialogueButton3Event ();
            void DialogueButton4Event ();
        }
    }
}