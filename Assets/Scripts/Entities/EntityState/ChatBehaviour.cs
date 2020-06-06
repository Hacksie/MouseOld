using UnityEngine;

namespace HackedDesign
{
    [CreateAssetMenu(fileName = "Chat Behaviour", menuName = "Mouse/Entities/Chat Behaviour")]
    public class ChatBehaviour : AbstractBehaviour
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
            spriteOverlay.SetSprite(EntityState.Chat);
        }
    }
}