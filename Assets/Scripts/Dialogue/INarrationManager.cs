namespace HackedDesign {
    namespace Dialogue {
        public interface INarrationManager {
            void Initialize (Input.IInputController input);
            Narration GetCurrentNarration ();
            void ShowNarration(string name);
            void NarrationButtonEvent();
        }
    }
}