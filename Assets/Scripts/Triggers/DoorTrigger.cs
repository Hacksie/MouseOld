using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign
{
    namespace Triggers
    {
        public class DoorTrigger : BaseTrigger
        {
            public Collider2D doorCollider;
            public Animator animator;
            public bool animate = true;
            

            bool open = false;

            public override void Initialize()
            {
                base.Initialize();
                Debug.Log("Door: Initialize door trigger");
                base.Activate();

                animator = GetComponent<Animator>();
            }

            // Update is called once per frame
            public override void UpdateTrigger(Input.IInputController inputController)
            {
                if (animate && animator != null)
                    animator.SetBool("open", open || overloaded);

                base.UpdateTrigger(inputController);
            }

            public override void Invoke(UnityEngine.GameObject source)
            {
                open = true;
                base.Invoke(source);
            }

            public override void Hack(UnityEngine.GameObject source)
            {
                open = true;
                base.Hack(source);
            }

            public override void Bug(UnityEngine.GameObject source)
            {
                open = true;
                base.Bug(source);
            }

            public override void Overload(UnityEngine.GameObject source)
            {
                open = true;
                base.Overload(source);
            }

            public override void Leave(UnityEngine.GameObject source)
            {
                open = false;
                base.Leave(source);
            }
        }
    }
}