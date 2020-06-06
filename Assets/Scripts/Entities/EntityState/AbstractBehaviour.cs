using UnityEngine;

namespace HackedDesign
{
    public abstract class AbstractBehaviour : ScriptableObject, IBehaviour
    {
        public virtual void Begin()
        {
            
        }

        public virtual void UpdateBehaviour(IEntity entity)
        {
            UpdateInteractionSprite(entity.SpriteOverlay);
        }

        protected virtual void UpdateInteractionSprite(InteractionSpriteOverlay spriteOverlay)
        {
            spriteOverlay.SetSprite(EntityState.Interact);
        }
    }
}