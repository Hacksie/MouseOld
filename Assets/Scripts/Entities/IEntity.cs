using UnityEngine;
using System.Collections.Generic;

namespace HackedDesign
{
    public interface IEntity 
    {
        //void Initialize(bool pooled, Transform player);
        void UpdateBehaviour();
        void Animate();
        void SetPosition(Vector2 position);
        void AddCollider(GameObject collider);
        void RemoveCollider(GameObject collider);
        List<GameObject> GetColliders();
        void Activate();
        void Deactivate();
        Story.InfoEntity GetEntityDefinition();
        void SetEntityDefinition(Story.InfoEntity entity);

        void InvokeSeenPlayer();
        

        Transform Transform { get; }

        InteractionSpriteOverlay SpriteOverlay { get; }

        PolyNav.PolyNavAgent NavAgent { get; }

        float LastActionTime { get; set; }
        Vector2 Direction { get; set; }
        AbstractBehaviour Behaviour { get; set; }
    }
}