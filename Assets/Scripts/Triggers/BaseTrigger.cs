using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HackedDesign.Triggers
{
    [RequireComponent(typeof(Collider2D))]
    public class BaseTrigger : MonoBehaviour, ITrigger
    {
        protected new Collider2D collider;
        public new bool enabled = true;

        [Header("Settings")]
        public bool autoInteraction = false;
        public bool allowInteraction = false;
        public bool allowBug = false;
        public bool allowHack = false;
        public bool allowOverload = false;

        public bool allowRepeatInteractions = false;
        public bool allowNPCAutoInteraction = false;

        [Header("Actions")]
        public UnityEvent entryActionEvent;
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
        private List<GameObject> colliders = new List<GameObject>();

        protected void Start()
        {
            if (!CompareTag(TagManager.TRIGGER))
            {
                Debug.LogError(this.name + ": trigger is not tagged: " + this.name);
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
                if (colliders[i].CompareTag(TagManager.NPC) && allowNPCAutoInteraction)
                {
                    Invoke(colliders[i].gameObject);
                    colliders.RemoveAt(i);
                }

                if (colliders[i].CompareTag(TagManager.PLAYER))
                {
                    if (CheckPlayerActions(colliders[i].gameObject, inputController))
                    {
                        colliders.RemoveAt(i);
                    }
                }
            }
        }

        public virtual void Entry(GameObject source)
        {
            entryActionEvent.Invoke();
        }

        public virtual void Invoke(GameObject source)
        {
            interactActionEvent.Invoke();
        }

        public virtual void Overload(GameObject source)
        {

            overloaded = true;
            overloadActionEvent.Invoke();
            CoreGame.Instance.state.player.ConsumeOverload();
        }

        public virtual void Hack(GameObject source)
        {
            hackActionEvent.Invoke();
            if (CoreGame.Instance.state.player.ConsumeHack())
            {
                hacked = true;
            }
        }

        public virtual void Bug(GameObject source)
        {
            bugged = true;
            hacked = true;
            bugActionEvent.Invoke();
            CoreGame.Instance.state.player.ConsumeBug();
        }

        public virtual void Leave(GameObject source)
        {
            leaveActionEvent.Invoke();
        }

        protected bool CheckPlayerActions(GameObject source, Input.IInputController inputController)
        {
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

            if (other.CompareTag(TagManager.PLAYER))
            {
                PlayerController playerController = other.gameObject.GetComponent<PlayerController>();
                playerController.RegisterTrigger(this);
                Entry(other.gameObject);
            }

            //if ((other.CompareTag(TagManager.PLAYER) || other.CompareTag(TagManager.NPC)) && !colliders.Contains(other.gameObject))
            //{
            //    colliders.Add(other.gameObject);
            //    Entry(other.gameObject);
            //}
        }

        //FIXME: Move input code outside of physics update
        protected virtual void OnTriggerStay2D(Collider2D other)
        {
            if (!enabled)
            {
                return;
            }

            /*    
            if (allowRepeatInteractions)
            {
                if ((other.CompareTag(TagManager.PLAYER) || other.CompareTag(TagManager.NPC)) && !colliders.Contains(other.gameObject))
                {
                    colliders.Add(other.gameObject);
                }
            }*/
        }

        protected virtual void OnTriggerExit2D(Collider2D other)
        {
            if (!enabled)
            {
                return;
            }

            if (other.CompareTag(TagManager.PLAYER))
            {
                PlayerController playerController = other.gameObject.GetComponent<PlayerController>();
                playerController.UnregisterTrigger(this);
                Leave(other.gameObject);
            }
            /*
            if ((other.CompareTag(TagManager.PLAYER) || other.CompareTag(TagManager.NPC)))
            {
                
                //Debug.Log(this.name + ": removed from collider list " + other.gameObject);
                if (colliders.Contains(other.gameObject))
                    colliders.Remove(other.gameObject);
            }*/
        }
    }

}