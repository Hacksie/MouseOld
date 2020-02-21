using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HackedDesign.Triggers
{
    class Door : MonoBehaviour
    {
        public Animator animator;
        public BaseTrigger trigger;

        public bool open = false;
        public bool animate = true;

        public void Initialize()
        {

            Debug.Log("Door: Initialize door trigger");

            animator = GetComponent<Animator>();
            trigger = GetComponent<BaseTrigger>();
        }

        // FIXME: Create coregame loop
        public void Update()
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
