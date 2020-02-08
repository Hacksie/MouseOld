using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign
{
    namespace Triggers
    {
        public class DoorTrigger : BaseTrigger
        {


            public bool levelEntry = false;
            //public GameObject doorObject;
            public Collider2D doorCollider;
            public Animator animator;
            public bool overloaded = false;

            bool open = false;
            bool unlocked = false;


            public override void Initialize(Input.IInputController inputController)
            {
                base.Initialize(inputController);
                Debug.Log("Door: Initialize door trigger");
                base.Activate();

                animator = GetComponent<Animator>();
                allowRepeatTriggers = true;

            }

            // Update is called once per frame
            public override void UpdateTrigger()
            {
                if (animator != null)
                    animator.SetBool("open", open || overloaded);
            }

            public override void Invoke()
            {
                open = true;
                base.Invoke();
            }

            public override void Leave()
            {
                open = false;
                base.Leave();
            }

            protected override void OnTriggerStay2D(Collider2D other)
            {
                if (!enabled || overloaded)
                {
                    return;
                }

                if (requireInteraction && inputController == null)
                {
                    Debug.LogError(this.name + ": trigger has no inputController");
                    return;
                }

                if (other.tag == TagManager.PLAYER && !open && (!hasBeenTriggered || (allowRepeatTriggers && hasBeenTriggered)))
                {
                    if (sprite != null && !sprite.gameObject.activeInHierarchy)
                    {
                        sprite.gameObject.SetActive(true);
                    }

                    if (!requireInteraction)
                    {
                        hasBeenTriggered = true;
                        unlocked = true;
                        Invoke();
                    }

                    if (levelEntry)
                    {
                        if (requireInteraction && (unlocked && inputController.InteractButtonUp()))
                        {
                            hasBeenTriggered = true;
                            unlocked = true;
                            Invoke();
                        }
                        else if (requireInteraction && CoreGame.Instance.State.currentLevel.timer.running && inputController.InteractButtonUp())
                        {
                            // If the timer is running, the entry must have been overloaded
                            hasBeenTriggered = true;
                            overloaded = true;
                            unlocked = true;
                            Invoke();
                        }
                        else if (!overloaded && requireInteraction && inputController.OverloadButtonUp() && CoreGame.Instance.State.player.CanOverload())
                        {
                            overloaded = true;
                            unlocked = true;
                            hasBeenTriggered = true;
                            CoreGame.Instance.State.player.ConsumeOverload();
                            Overload();
                        } 
                    }
                    else
                    {
                        if (requireInteraction && (unlocked && inputController.InteractButtonUp()))
                        {
                            hasBeenTriggered = true;
                            Invoke();
                        }
                        else if (requireInteraction && (!unlocked && inputController.InteractButtonUp() && CoreGame.Instance.State.player.CanKeycard()))
                        {
                            unlocked = true;
                            hasBeenTriggered = true;
                            CoreGame.Instance.State.player.ConsumeKeycard();
                            Invoke();
                        }
                        else if (requireInteraction && inputController.OverloadButtonUp() && CoreGame.Instance.State.player.CanOverload())
                        {
                            overloaded = true;
                            unlocked = true;
                            hasBeenTriggered = true;
                            CoreGame.Instance.State.player.ConsumeOverload();
                            Invoke();
                        }
                    }


                }
                if (other.tag == TagManager.NPC && !open && allowNPCAutoInteraction)
                {
                    Invoke();
                }
            }

            protected override void OnTriggerExit2D(Collider2D other)
            {
                if (!enabled)
                {
                    return;
                }

                if (other.tag == TagManager.PLAYER)
                {
                    if (sprite != null && sprite.gameObject.activeInHierarchy)
                    {
                        sprite.gameObject.SetActive(false);
                    }

                    if (hasBeenTriggered && !hasBeenLeft)
                    {
                        Leave();
                        if (!allowRepeatTriggers)
                        {
                            hasBeenLeft = true;
                        }
                    }
                }
                if (other.tag == TagManager.NPC && allowNPCAutoInteraction)
                {
                    Leave();
                }
            }
        }
    }
}