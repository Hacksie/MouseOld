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
            public List<GameObject> colliders = new List<GameObject>();

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

            public virtual void Initialize()
            {
                triggered = false;
                npcTriggered = false;
                collider = GetComponent<Collider2D>();
                colliders.Clear();

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

            public virtual void UpdateTrigger(Input.IInputController inputController)
            {

                for (int i = colliders.Count - 1; i >= 0; i--)
                {
                    var other = colliders[i];

                    if (other.CompareTag(TagManager.NPC) && allowNPCAutoInteraction)
                    {
                        Invoke(other.gameObject);
                        colliders.RemoveAt(i);
                    }

                    if (other.CompareTag(TagManager.PLAYER))
                    {
                        if (CheckPlayerActions(other.gameObject, inputController))
                        {
                            colliders.RemoveAt(i);
                        }
                    }
                }
            }

            public virtual void Invoke(UnityEngine.GameObject source)
            {
                //Debug.Log(this.name + ": " + source.name + " invoked trigger action: " + triggerAction);
                if (!string.IsNullOrWhiteSpace(triggerAction))
                {
                    Story.ActionManager.instance.Invoke(triggerAction);
                }
            }

            public virtual void Overload(UnityEngine.GameObject source)
            {
                //Debug.Log(this.name + ": " + source.name + " invoked overload action: " + overloadAction);
                overloaded = true;
                CoreGame.Instance.state.player.ConsumeOverload();
                if (!string.IsNullOrWhiteSpace(overloadAction))
                {
                    Story.ActionManager.instance.Invoke(overloadAction);
                }
            }

            public virtual void Hack(UnityEngine.GameObject source)
            {
                //Debug.Log(this.name + ": " + source.name + " invoked hack action: " + hackAction);


                if (CoreGame.Instance.state.player.ConsumeHack())
                {
                    hacked = true;
                    if (!string.IsNullOrWhiteSpace(hackAction))
                    {
                        Story.ActionManager.instance.Invoke(hackAction);
                    }
                }
            }

            public virtual void Bug(UnityEngine.GameObject source)
            {
                //Debug.Log(this.name + ": " + source.name + " invoked bug action: " + bugAction);
                bugged = true;
                hacked = true;
                CoreGame.Instance.state.player.ConsumeBug();
                if (!string.IsNullOrWhiteSpace(bugAction))
                {
                    Story.ActionManager.instance.Invoke(bugAction);
                }
            }

            // public virtual void Keycard()
            // {
            //     Debug.Log(this.name + ": invoking hack action: " + keycardAction);
            //     CoreGame.Instance.State.player.ConsumeKeycard();
            //     Story.ActionManager.instance.Invoke(keycardAction);
            // }

            public virtual void Leave(UnityEngine.GameObject source)
            {
                //Debug.Log(this.name + ": " + source.name + " invoked action: " + leaveAction);
                if (!string.IsNullOrWhiteSpace(leaveAction))
                {
                    Story.ActionManager.instance.Invoke(leaveAction);
                }
            }

            protected bool CheckPlayerActions(GameObject source, Input.IInputController inputController)
            {
                //FIXME: have 4 different sprites for each possible action. 
                // Show the sprite permanently if the object is hacked or overloaded
                if (sprite != null && !sprite.gameObject.activeInHierarchy)
                {
                    sprite.gameObject.SetActive(true);
                }

                if (!requireInteraction && !requireHack && !requireBug && !requireOverload)
                {
                    Invoke(source);
                    return true;
                }

                if (inputController.InteractButtonUp() && !requireHack && !requireBug && !requireOverload)
                {
                    Invoke(source);
                    return true;
                }
                if (!overloaded && !hacked && !bugged && CoreGame.Instance.state.player.CanOverload() && inputController.OverloadButtonUp() && !requireOverload)
                {
                    Overload(source);
                    return true;
                }
                // if (!string.IsNullOrWhiteSpace(keycardAction) && CoreGame.Instance.State.player.CanKeycard() && inputController.KeycardButtonUp() && !requireInteraction && !requireHack && !requireKeycard && !requireOverload)
                // {
                //     triggered = true;
                //     Keycard();
                // }
                if (!overloaded && !hacked && !bugged && CoreGame.Instance.state.player.CanBug() && inputController.BugButtonUp() && !requireBug)
                {
                    Bug(source);
                    return true;
                }
                if (!overloaded && !hacked && !bugged && CoreGame.Instance.state.player.CanHack() && inputController.HackButtonUp() && !requireBug)
                {
                    Hack(source);
                    return true;
                }
                if ((hacked || bugged) && inputController.InteractButtonUp())
                {
                    Invoke(source);
                    return true;
                }

                return false;
            }

            protected virtual void OnTriggerEnter2D(Collider2D other)
            {
                if (!enabled)
                {
                    return;
                }

                if ((other.CompareTag(TagManager.PLAYER) || other.CompareTag(TagManager.NPC)) && !colliders.Contains(other.gameObject))
                {
                    colliders.Add(other.gameObject);
                }
            }

            //FIXME: Move input code outside of physics update
            protected virtual void OnTriggerStay2D(Collider2D other)
            {
                if (!enabled)
                {
                    return;
                }

                if (allowRepeatInteractions)
                {
                    if ((other.CompareTag(TagManager.PLAYER) || other.CompareTag(TagManager.NPC)) && !colliders.Contains(other.gameObject))
                    {
                        colliders.Add(other.gameObject);
                    }
                }
            }

            protected virtual void OnTriggerExit2D(Collider2D other)
            {
                if (!enabled)
                {
                    return;
                }

                if ((other.CompareTag(TagManager.PLAYER) || other.CompareTag(TagManager.NPC)))
                {
                    Leave(other.gameObject);
                    //Debug.Log(this.name + ": removed from collider list " + other.gameObject);
                    if (colliders.Contains(other.gameObject))
                        colliders.Remove(other.gameObject);

                    if (other.CompareTag(TagManager.PLAYER))
                    {
                        if (sprite != null && sprite.gameObject.activeInHierarchy)
                        {
                            sprite.gameObject.SetActive(false);
                        }
                    }


                }

                /*
                if (other.CompareTag(TagManager.NPC) && allowNPCAutoInteraction)
                {
                    npcTriggered = false;
                    Leave(other.gameObject);
                }

                if (other.CompareTag(TagManager.PLAYER))
                {
                    if (sprite != null && sprite.gameObject.activeInHierarchy)
                    {
                        sprite.gameObject.SetActive(false);
                    }

                    if (triggered)
                    {
                        triggered = false;
                        Leave(other.gameObject);
                    }
                }*/


            }
        }
    }
}