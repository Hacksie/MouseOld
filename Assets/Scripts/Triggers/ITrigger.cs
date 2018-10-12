namespace HackedDesign {
    namespace Triggers {
        public interface ITrigger {
            void Initialize (Input.IInputController inputController);
            void UpdateTrigger ();
            void Invoke ();
        }
    }
}