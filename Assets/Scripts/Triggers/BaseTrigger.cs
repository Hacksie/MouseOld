using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HackedDesign
{
    namespace Triggers
    {
        public class BaseTrigger : MonoBehaviour, ITrigger
        {

            protected Input.IInputController inputController;
            protected new Collider2D collider;
            public new bool enabled = true;

            [Header("Sprites")]
            public SpriteRenderer sprite;
            public SpriteRenderer interactSprite;
            public SpriteRenderer hackSprite;
            //public SpriteRenderer keycardSprite;
            public SpriteRenderer overloadSprite;
            public SpriteRenderer bugSprite;


            [Header("Trigger settings")]
            public bool requireInteraction = false;
            public bool requireHack = false;
            //public bool requireKeycard = false;
            public bool requireOverload = false;
            public bool requireBug = false;
            public bool allowOverload = false;
            public bool allowRepeatInteractions = false;
            public bool allowNPCAutoInteraction = false;



            [Header("Trigger actions")]
            public string triggerAction;
            public string bugAction;
            public string hackAction;
            //public string keycardAction;
            public string overloadAction;
            public string leaveAction;

            [Header("Trigger state")]
            public bool triggered = false;
            public bool npcTriggered = false;
            public bool overloaded = false;
            public bool hacked = false;
            public bool bugged = false;

            public void Start()
            {
                if (this.tag != TagManager.TRIGGER)
                {
                    Debug.LogError(this.name + ": trigger is not tagged: " + this.name);
                }

                if (sprite != null && sprite.gameObject.activeInHierarchy)
                {
                    sprite.gameObject.SetActive(false);
                }
            }

            public virtual void Initialize(Input.IInputController inputController)
            {
                this.inputController = inputController;
                triggered = false;
                npcTriggered = false;
                collider = GetComponent<Collider2D>();
                if (enabled)
                {
                    Activate();
                }
                else
                {
                    Deactivate();
                }

            }

            public void Activate()
            {
                if (collider != null)
                {
                    collider.enabled = true;
                }
            }

            public void Deactivate()
            {
                if (collider != null)
                {
                    collider.enabled = false;
                }
            }

            public virtual void UpdateTrigger()
            {

            }

            public virtual void Invoke()
            {
                Debug.Log(this.name + ": invoking trigger action: " + triggerAction);
                if (!string.IsNullOrWhiteSpace(triggerAction))
                {
                    Story.ActionManager.instance.Invoke(triggerAction);
                }
            }

            public virtual void Overload()
            {
                Debug.Log(this.name + ": invoking overload action: " + overloadAction);
                overloaded = true;
                CoreGame.Instance.State.player.ConsumeOverload();
                Story.ActionManager.instance.Invoke(overloadAction);
            }

            public virtual void Hack()
            {
                Debug.Log(this.name + ": invoking hack action: " + hackAction);
                hacked = true;
                CoreGame.Instance.State.player.ConsumeHack();
                Story.ActionManager.instance.Invoke(hackAction);
            }

            public virtual void Bug()
            {
                Debug.Log(this.name + ": invoking bug action: " + bugAction);
                bugged = true;
                hacked = true;
                CoreGame.Instance.State.player.ConsumeBug();
                Story.ActionManager.instance.Invoke(bugAction);
            }

            // public virtual void Keycard()
            // {
            //     Debug.Log(this.name + ": invoking hack action: " + keycardAction);
            //     CoreGame.Instance.State.player.ConsumeKeycard();
            //     Story.ActionManager.instance.Invoke(keycardAction);
            // }

            public virtual void Leave()
            {
                Debug.Log(this.name + ": invoking leave action: " + leaveAction);
                if (!string.IsNullOrWhiteSpace(leaveAction))
                {
                    Story.ActionManager.instance.Invoke(leaveAction);
                }
            }

            protected void CheckPlayerActions()
            {
                //FIXME: have 4 different sprites for each possible action. 
                // Show the sprite permanently if the object is hacked or overloaded
                if (sprite != null && !sprite.gameObject.activeInHierarchy)
                {
                    sprite.gameObject.SetActive(true);
                }

                if (!requireInteraction && !requireHack && !requireBug && !requireOverload)
                {
                    triggered = true;
                    Invoke();
                }

                if (!string.IsNullOrWhiteSpace(triggerAction) && inputController.InteractButtonUp() && !requireHack && !requireBug && !requireOverload)
                {
                    triggered = true;
                    Invoke();
                }
                if (!overloaded && !hacked && !bugged && !string.IsNullOrWhiteSpace(overloadAction) && CoreGame.Instance.State.player.CanOverload() && inputController.OverloadButtonUp() && !requireInteraction && !requireHack && !requireBug)
                {
                    triggered = true;
                    Overload();
                }
                // if (!string.IsNullOrWhiteSpace(keycardAction) && CoreGame.Instance.State.player.CanKeycard() && inputController.KeycardButtonUp() && !requireInteraction && !requireHack && !requireKeycard && !requireOverload)
                // {
                //     triggered = true;
                //     Keycard();
                // }
                if (!overloaded && !hacked && !bugged && !string.IsNullOrWhiteSpace(bugAction) && CoreGame.Instance.State.player.CanBug() && inputController.BugButtonUp() && !requireInteraction && !requireHack && !requireOverload)
                {
                    triggered = true;
                    Bug();
                }
                if (!overloaded && !hacked && !bugged && !string.IsNullOrWhiteSpace(hackAction) && CoreGame.Instance.State.player.CanHack() && inputController.HackButtonUp() && !requireInteraction && !requireBug && !requireOverload)
                {
                    triggered = true;
                    Hack();
                }
                if ((hacked || bugged) && inputController.InteractButtonUp())
                {
                    triggered = true;
                    Invoke();
                }

                if (allowRepeatInteractions)
                {
                    triggered = false;
                }
            }

            protected virtual void OnTriggerStay2D(Collider2D other)
            {
                if (!enabled)
                {
                    return;
                }

                if ((requireInteraction || requireHack || requireBug || requireOverload) && inputController == null)
                {
                    Debug.LogError(this.name + ": trigger has no inputController");
                    return;
                }

                if (other.CompareTag(TagManager.NPC) && !triggered && allowNPCAutoInteraction)
                {
                    npcTriggered = true;
                    Invoke();
                }

                //FIXME: allow triggering without leaving
                if (other.CompareTag(TagManager.PLAYER) && !triggered)
                {
                    CheckPlayerActions();
                }
            }

            protected virtual void OnTriggerExit2D(Collider2D other)
            {
                if (!enabled)
                {
                    return;
                }
                if (!string.IsNullOrWhiteSpace(triggerAction) && other.tag == TagManager.NPC && allowNPCAutoInteraction)
                {
                    npcTriggered = false;
                    Leave();
                }

                if (other.tag == TagManager.PLAYER)
                {
                    if (sprite != null && sprite.gameObject.activeInHierarchy)
                    {
                        sprite.gameObject.SetActive(false);
                    }

                    if (triggered)
                    {
                        triggered = false;
                        Leave();
                    }
                }


            }
        }
    }
}