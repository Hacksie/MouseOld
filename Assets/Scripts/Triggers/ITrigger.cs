
namespace HackedDesign {
    namespace Triggers {
        public interface ITrigger {
            void Initialize ();
            void Invoke (UnityEngine.GameObject source);
            void Overload(UnityEngine.GameObject source);
            void Hack(UnityEngine.GameObject source);
            void Bug(UnityEngine.GameObject source);
            void Leave(UnityEngine.GameObject source);
            void UpdateTrigger(Input.IInputController inputController);
            void Activate ();
            void Deactivate ();
        }
    }
}