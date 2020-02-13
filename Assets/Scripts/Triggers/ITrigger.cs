
namespace HackedDesign {
    namespace Triggers {
        public interface ITrigger {
            void Initialize (Input.IInputController inputController);
            void Invoke (UnityEngine.GameObject source);
            void Overload(UnityEngine.GameObject source);
            void Hack(UnityEngine.GameObject source);
            //void Keycard(UnityEngine.GameObject source);
            void Bug(UnityEngine.GameObject source);
            void Leave(UnityEngine.GameObject source);
            void UpdateTrigger();
            void Activate ();
            void Deactivate ();
        }
    }
}