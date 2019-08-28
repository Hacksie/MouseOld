﻿using System.Collections;
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

            [Header("")]
            public SpriteRenderer sprite;

            [Header("Trigger settings")]
            public bool requireInteraction = false;
            public bool allowRepeatTriggers = false;

            [Header("Trigger actions")]
            public string triggerAction;
            public string leaveAction;

            [Header("Trigger state")]
            public bool hasBeenTriggered = false;
            public bool hasBeenLeft = false;

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
                Story.ActionManager.instance.Invoke(triggerAction);
            }

            public virtual void Leave()
            {
                Story.ActionManager.instance.Invoke(leaveAction);
            }

            // TODO: Make door close trigger
            protected virtual void OnTriggerStay2D(Collider2D other)
            {

                if (!enabled)
                {
                    return;
                }

                if (requireInteraction && inputController == null)
                {
                    Debug.LogError(this.name + ": trigger has no inputController");
                    return;
                }

                if (other.tag == TagManager.PLAYER && !hasBeenTriggered || (allowRepeatTriggers && hasBeenTriggered))
                {
                    if (sprite != null && !sprite.gameObject.activeInHierarchy)
                    {
                        sprite.gameObject.SetActive(true);
                    }

                    if (!requireInteraction || (requireInteraction && inputController.InteractButtonUp()))
                    {
                        hasBeenTriggered = true;
                        Invoke();
                    }

                }
            }

            protected virtual void OnTriggerExit2D(Collider2D other)
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


            }
        }
    }
}