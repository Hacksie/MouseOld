using UnityEngine;

namespace HackedDesign
{
    [CreateAssetMenu(fileName = "Passive Behaviour", menuName = "Mouse/Entities/Passive Behaviour")]
    public class PassiveBehaviour : AbstractBehaviour
    {
        [SerializeField] private bool facePlayer = false;

        private Transform playerTransform;

        public override void Begin(IEntity entity)
        {

            playerTransform = GameManager.Instance.Player.transform;
        }

        public override void UpdateBehaviour(IEntity entity)
        {
            if (facePlayer)
            {
                entity.Direction = (playerTransform.position - entity.Transform.position).normalized;
            }

            base.UpdateBehaviour(entity);
        }
    }
}