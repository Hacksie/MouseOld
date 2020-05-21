
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace HackedDesign
{

    public class ScanningEnemy : Entity
    {
        [Header("Referenced Game Objects")]
        [SerializeField] private Transform detection = null;

        [Header("Settings")]
        [SerializeField] private float rotateSpeed = 45.0f;

        public override void Initialize(bool pooled)
        {
            Logger.Log(this, "Initializing Scanning Enemy");
            base.Initialize(pooled);
            isMoving = false;
        }

        public override void UpdateBehaviour()
        {
            switch (State)
            {
                case EntityState.PASSIVE:
                    UpdatePassive();
                    break;
                case EntityState.ALERTED:
                    UpdateAlerted();
                    break;
                default:
                    break;
            }

            UpdateDetection();
        }        

        protected override void UpdatePassive()
        {
            this.direction = Quaternion.Euler(0, 0, this.rotateSpeed * Time.deltaTime) * direction;
            base.UpdatePassive();
        }

        private void UpdateDetection()
        {
            if (detection == null)
                return;

            if (direction != Vector2.zero)
            {
                detection.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.up, direction));
            }
        }

    }
}