using UnityEngine;

namespace HackedDesign
{
    [CreateAssetMenu(fileName = "Hunt Behaviour", menuName = "Mouse/Entities/Hunt Behaviour")]
    public class HuntBehaviour : AbstractBehaviour
    {
        [SerializeField] private bool facePlayer = false;

        private Transform playerTransform;

        public override void Begin()
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

        protected override void UpdateInteractionSprite(InteractionSpriteOverlay spriteOverlay)
        {
            spriteOverlay.SetSprite(EntityState.Hunt);
        }
    }
}