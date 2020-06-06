using UnityEngine;

namespace HackedDesign
{
    public abstract class AbstractBehaviour : ScriptableObject, IBehaviour
    {
        [SerializeField] protected Sprite overlaySprite = null;

        public virtual void Begin(IEntity entity)
        {

        }

        public virtual void UpdateBehaviour(IEntity entity)
        {
            UpdateInteractionSprite(entity.SpriteOverlay);
        }

        protected virtual void UpdateInteractionSprite(InteractionSpriteOverlay spriteOverlay)
        {
            spriteOverlay.SetSprite(overlaySprite);
        }
    }
}