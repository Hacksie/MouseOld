using UnityEngine;

namespace HackedDesign
{
    public interface IEntity 
    {
        void Initialize(bool pooled, Transform player);
        void UpdateBehaviour();
        void Animate();
        void SetPosition(Vector2 position);
        void AddCollider(GameObject collider);
        void RemoveCollider(GameObject collider);
        void Activate();
        void Deactivate();
        Story.InfoEntity GetEntityDefinition();
        void SetEntityDefinition(Story.InfoEntity entity);
    }
}