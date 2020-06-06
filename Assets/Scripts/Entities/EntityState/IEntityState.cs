using UnityEngine;

namespace HackedDesign
{
    public interface IBehaviour
    {
        //void Initialize(Transform transform, InteractionSpriteOverlay spriteOverlay, PolyNav.PolyNavAgent agent);
        void Begin();
        void UpdateBehaviour(IEntity entity);
    }
}