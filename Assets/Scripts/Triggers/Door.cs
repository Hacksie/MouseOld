using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HackedDesign.Triggers
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(BaseTrigger))]
    public class Door : MonoBehaviour
    {
        public Animator animator;
        public BaseTrigger trigger;

        public bool open = false;
        public bool animate = true;

        private void Start()
        {
            animator = GetComponent<Animator>();
            trigger = GetComponent<BaseTrigger>();
        }

        public void Initialize()
        {
        }

        // FIXME: Create coregame loop
        //private void LateUpdate()
        //{
        //    UpdateAnimation();
        //}

        public void UpdateAnimation()
        {
            if (animate && animator != null)
            {
                animator.SetBool("open", open);
            }
        }

        public void Open()
        {
            open = true;
        }

        public void Close()
        {
            open = false;
        }
    }
}
