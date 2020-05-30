
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace HackedDesign
{

    public class LaserTrap : Entity
    {
        [Header("Settings")]
        [SerializeField] private float resetTime = 30.0f;

        private bool tripped = false;
        private float resetTimer = 0;

        public override void Initialize(bool pooled, Transform playerTransform)
        {
            Logger.Log(this, "Initializing Laser Trap");
            base.Initialize(pooled, playerTransform);
            isMoving = false;
        }

        public override void UpdateBehaviour()
        {
        
            switch (State)
            {
                case EntityState.Passive:
                    UpdatePassive();
                    break;
                case EntityState.Alerted:
                    UpdateAlerted();
                    break;
                default:
                    break;
            }
            UpdateDetection();
        }

        protected override void UpdatePassive()
        {
            if (colliders.Contains(playerTransform.gameObject) && !tripped)
            {
                tripped = true;
                resetTimer = Time.time;
                GameManager.Instance.IncreaseAlert();
            }
        }

        private void UpdateDetection()
        {

        }

    }
}