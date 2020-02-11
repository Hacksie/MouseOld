namespace HackedDesign {
    namespace Triggers {
        public interface ITrigger {
            void Initialize (Input.IInputController inputController);
            void Invoke ();
            void Overload();
            void Hack();
            void Keycard();
            void Leave();
            void UpdateTrigger();
            void Activate ();
            void Deactivate ();
        }
    }
}