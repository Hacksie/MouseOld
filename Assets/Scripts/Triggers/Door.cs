using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HackedDesign
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(BaseTrigger))]
    public class Door : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private bool animate = true;
        [SerializeField] private bool requireSecurityDisable = true;

        private Animator animator;
        private BaseTrigger trigger;

        private int count = 0;


        private void Awake()
        {
            animator = GetComponent<Animator>();
            trigger = GetComponent<BaseTrigger>();
        }

        public void Initialize()
        {
        }

        public void UpdateAnimation()
        {
            if (animate && animator != null && animator.enabled)
            {
                animator.SetBool("open", count != 0);
            }
        }

        public void Open()
        {
            if (!requireSecurityDisable || GameManager.Instance.Data.CurrentLevel.entryTriggered)
            {
                count++;
            }
        }

        public void Close()
        {
            if (count > 0)
            {
                count--;
            }
        }
    }
}
