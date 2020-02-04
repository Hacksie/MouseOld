namespace HackedDesign {
    namespace Triggers {
        public interface ITrigger {
            void Initialize (Input.IInputController inputController);
            void Invoke ();
            void Overload();
            void Leave();
            void UpdateTrigger();
            void Activate ();
            void Deactivate ();
        }
    }
}