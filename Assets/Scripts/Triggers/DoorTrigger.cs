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

            public override void Initialize(Input.IInputController inputController)
            {
                base.Initialize(inputController);
                Debug.Log("Door: Initialize door trigger");
                base.Activate();

                animator = GetComponent<Animator>();
            }

            // Update is called once per frame
            public override void UpdateTrigger()
            {
                if (animate && animator != null)
                    animator.SetBool("open", open || overloaded);
            }

            public override void Invoke()
            {
                open = true;
                base.Invoke();
            }

            public override void Hack()
            {
                open = true;
                base.Hack();
            }

            public override void Bug()
            {
                open = true;
                base.Bug();
            }

            public override void Overload()
            {
                open = true;
                base.Overload();
            }

            public override void Leave()
            {
                open = false;
                base.Leave();
            }
        }
    }
}