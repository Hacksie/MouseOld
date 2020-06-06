using UnityEngine;

namespace HackedDesign
{
    [CreateAssetMenu(fileName = "Patrol Behaviour", menuName = "Mouse/Entities/Patrol Behaviour")]
    public class PatrolBehaviour : AbstractBehaviour
    {
        [SerializeField] private PatrolConfig patrolConfig;
        [SerializeField] private AbstractBehaviour playerSeenBehaviour;

        private Transform playerTransform;

        public override void Begin()
        {
            playerTransform = GameManager.Instance.Player.transform;
        }


        public override void UpdateBehaviour(IEntity entity)
        {
            
            if ((Time.time - entity.LastActionTime) >= patrolConfig.patrolTime)
            {
                Logger.Log(this, "Patrolling");
                entity.LastActionTime = Time.time;

                var pointsOfInterest = GetPointsOfInterestNearby(entity.Transform);
                

                if (pointsOfInterest.Length > 0)
                {
                    var point = pointsOfInterest[Random.Range(0, pointsOfInterest.Length)];
                    entity.NavAgent?.SetDestination(point.transform.position); // FIXME: this should probably be a function
                }
                else
                {
                    Logger.Log(this, "No points of interest nearby");
                }
            }

            if (entity.GetColliders().Contains(playerTransform.gameObject))
            {
                var hit = CanSeePlayer(entity);
                if (hit.collider != null && hit.collider.CompareTag(Tags.PLAYER))
                {
                    entity.Behaviour = playerSeenBehaviour;
                }
            }            

            base.UpdateBehaviour(entity);
        }

       protected RaycastHit2D CanSeePlayer(IEntity entity)
        {
            return Physics2D.Raycast(entity.Transform.position, (playerTransform.position - entity.Transform.position), this.patrolConfig.visibilityDistance, this.patrolConfig.playerLayerMask);
        }


        protected override void UpdateInteractionSprite(InteractionSpriteOverlay spriteOverlay)
        {
            spriteOverlay?.SetSprite(EntityState.Patrol);
        }

        protected Collider2D[] GetPointsOfInterestNearby(Transform transform)
        {
            return Physics2D.OverlapCircleAll(transform.position, this.patrolConfig.poiDistance, this.patrolConfig.poiLayerMask);
        }
    }
}