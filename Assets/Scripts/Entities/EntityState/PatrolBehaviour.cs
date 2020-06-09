#nullable enable
using UnityEngine;

namespace HackedDesign
{
    [CreateAssetMenu(fileName = "Patrol Behaviour", menuName = "Mouse/Entities/Patrol Behaviour")]
    public class PatrolBehaviour : AbstractBehaviour
    {
        [SerializeField] private PatrolConfig? patrolConfig = null;
        [SerializeField] private AbstractBehaviour? playerSeenBehaviour = null;

        private Transform? playerTransform = null;

        public override void Begin(IEntity entity)
        {
            playerTransform = GameManager.Instance.Player.transform;

            if (entity.NavAgent != null && entity.NavAgent.isActiveAndEnabled)
            {
                entity.NavAgent.map = GameManager.Instance.PolyNav;
            }
        }


        public override void UpdateBehaviour(IEntity entity)
        {
            if (this.patrolConfig != null && (Time.time - entity.LastActionTime) >= this.patrolConfig.patrolTime)
            {
                Logger.Log(this, "Patrolling");
                entity.LastActionTime = Time.time;

                var pointsOfInterest = GetPointsOfInterestNearby(entity.Transform, this.patrolConfig);


                if (pointsOfInterest == null || pointsOfInterest.Length == 0)
                {
                    Logger.Log(this, "No points of interest nearby");
                }
                else
                {
                    var point = pointsOfInterest[Random.Range(0, pointsOfInterest.Length)];
                    entity.NavAgent?.SetDestination(point.transform.position); // FIXME: this should probably be a function on entity
                }
            }

            if (playerTransform != null && entity.Transform != null && this.patrolConfig != null && entity.GetColliders().Contains(playerTransform.gameObject))
            {
                var hit = CanSeePlayer(entity.Transform, playerTransform, this.patrolConfig);
                if (hit.collider != null && hit.collider.CompareTag(Tags.PLAYER))
                {
                    entity.Behaviour = playerSeenBehaviour;
                }
            }

            UpdateDetections(entity);

            base.UpdateBehaviour(entity);
        }

        private void UpdateDetections(IEntity entity)
        {
            if (entity.Detections is null || entity.Detections.Length == 0)
                return;

            if (entity.Direction != Vector2.zero)
            {
                var rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.up, entity.Direction));
                foreach (var detection in entity.Detections)
                {
                    if(detection is null)
                    {
                        continue;
                    }
                    detection.transform.rotation = rotation;
                }
            }
        }

        protected RaycastHit2D CanSeePlayer(Transform entityTransform, Transform playerTransform, PatrolConfig patrolConfig) => Physics2D.Raycast(entityTransform.position, (playerTransform.position - entityTransform.position), patrolConfig.visibilityDistance, patrolConfig.playerLayerMask);
        protected Collider2D[]? GetPointsOfInterestNearby(Transform? transform, PatrolConfig patrolConfig) => transform is null? null : Physics2D.OverlapCircleAll(transform.position, patrolConfig.poiDistance, patrolConfig.poiLayerMask);
    }
}