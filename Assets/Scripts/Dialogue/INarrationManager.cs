namespace HackedDesign {
    namespace Dialogue {
        public interface INarrationManager {
            void Initialize ();
            Narration GetCurrentNarration ();
            void ShowNarration(string name);
            void NarrationButtonEvent();
        }
    }
}