using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign
{
    namespace Triggers
    {
        public class TriggerFeedback : MonoBehaviour
        {
            public BaseTrigger trigger;
            public SpriteRenderer sprite;

            private void Start()
            {
                trigger = GetComponent<BaseTrigger>();

                if (sprite != null && sprite.gameObject.activeInHierarchy)
                {
                    sprite.gameObject.SetActive(false);
                }
            }


            private void OnTriggerStay2D(Collider2D other)
            {
                if (trigger != null && trigger.enabled && other.tag == TagManager.PLAYER && sprite != null && !sprite.gameObject.activeInHierarchy)
                {
                    sprite.gameObject.SetActive(true);
                }
            }

            private void OnTriggerExit2D(Collider2D other)
            {
                if (trigger != null && trigger.enabled && other.tag == TagManager.PLAYER && sprite != null && sprite.gameObject.activeInHierarchy)
                {
                    sprite.gameObject.SetActive(false);
                }
            }
        }

    }
}