using UnityEngine;

namespace HackedDesign
{
    public interface IEntity 
    {
        void Initialize(bool pooled);
        void UpdateBehaviour();
        void Animate();
        void SetPosition(Vector2 position);
        void Activate();
        void Deactivate();
        Story.InfoEntity GetEntityDefinition();
    }
}