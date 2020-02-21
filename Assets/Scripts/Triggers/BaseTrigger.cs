using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HackedDesign.Triggers
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

        [Header("Settings")]
        public bool autoInteraction = false;
        public bool allowInteraction = false;
        public bool allowBug = false;
        public bool allowHack = false;
        public bool allowOverload = false;
        
        public bool allowRepeatInteractions = false;
        public bool allowNPCAutoInteraction = false;

        [Header("Actions")]
        public UnityEvent interactActionEvent;
        public UnityEvent bugActionEvent;
        public UnityEvent hackActionEvent;
        public UnityEvent overloadActionEvent;
        public UnityEvent leaveActionEvent;

        [Header("State")]
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
            interactActionEvent.Invoke();
            //if (!string.IsNullOrWhiteSpace(triggerAction))
            //{
            //    Story.ActionManager.instance.Invoke(triggerAction);
            //}
        }

        public virtual void Overload(UnityEngine.GameObject source)
        {
            
            overloaded = true;
            overloadActionEvent.Invoke();
            CoreGame.Instance.state.player.ConsumeOverload();
            //if (!string.IsNullOrWhiteSpace(overloadAction))
            //{
            //    Story.ActionManager.instance.Invoke(overloadAction);
            //}
        }

        public virtual void Hack(UnityEngine.GameObject source)
        {
            hackActionEvent.Invoke();
            if (CoreGame.Instance.state.player.ConsumeHack())
            {
                hacked = true;
                //if (!string.IsNullOrWhiteSpace(hackAction))
                //{
                //    Story.ActionManager.instance.Invoke(hackAction);
                //}
            }
        }

        public virtual void Bug(UnityEngine.GameObject source)
        {
            bugged = true;
            hacked = true;
            bugActionEvent.Invoke();
            CoreGame.Instance.state.player.ConsumeBug();
            //if (!string.IsNullOrWhiteSpace(bugAction))
            //{
            //    Story.ActionManager.instance.Invoke(bugAction);
            //}
        }

        public virtual void Leave(UnityEngine.GameObject source)
        {
            leaveActionEvent.Invoke();
            //if (!string.IsNullOrWhiteSpace(leaveAction))
            //{
            //    Story.ActionManager.instance.Invoke(leaveAction);
            //}
        }

        protected bool CheckPlayerActions(GameObject source, Input.IInputController inputController)
        {
            //Debug.Log(!overloaded && !hacked && !bugged && CoreGame.Instance.state.player.CanHack() && inputController.HackButtonUp() && !requireBug);
            //FIXME: have 4 different sprites for each possible action. 
            // Show the sprite permanently if the object is hacked or overloaded
            if (sprite != null && !sprite.gameObject.activeInHierarchy)
            {
                sprite.gameObject.SetActive(true);
            }

            if (autoInteraction)
            {
                Invoke(source);
                return true;
            }

            if (inputController.InteractButtonUp() && allowInteraction)
            {
                Invoke(source);
                return true;
            }
            if (!overloaded && !hacked && !bugged && CoreGame.Instance.state.player.CanOverload() && inputController.OverloadButtonUp() && allowOverload)
            {
                Overload(source);
                return true;
            }
            // if (!string.IsNullOrWhiteSpace(keycardAction) && CoreGame.Instance.State.player.CanKeycard() && inputController.KeycardButtonUp() && !requireInteraction && !requireHack && !requireKeycard && !requireOverload)
            // {
            //     triggered = true;
            //     Keycard();
            // }
            if (!overloaded && !hacked && !bugged && CoreGame.Instance.state.player.CanBug() && inputController.BugButtonUp() && allowBug)
            {
                Bug(source);
                return true;
            }
            if (!overloaded && !hacked && !bugged && CoreGame.Instance.state.player.CanHack() && inputController.HackButtonUp() && allowHack)
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