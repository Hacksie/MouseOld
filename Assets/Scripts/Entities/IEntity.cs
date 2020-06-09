#nullable enable
using UnityEngine;
using System.Collections.Generic;
using HackedDesign.Story;

namespace HackedDesign
{
    public interface IEntity 
    {
        //void Initialize(bool pooled, Transform player);
        void Animate();
        void SetPosition(Vector2 position);
        void AddCollider(GameObject collider);
        void RemoveCollider(GameObject collider);
        List<GameObject> GetColliders();
        void Activate();
        void Deactivate();
        void UpdateBehaviour();
        void Invoke(GameObject invoker);
        void Overload(GameObject invoker);
        void Hack(GameObject invoker);


        Transform? Transform { get; }
        InteractionSpriteOverlay? SpriteOverlay { get; }
        PolyNav.PolyNavAgent? NavAgent { get; }

        float LastActionTime { get; set; }
        Vector2 Direction { get; set; }
        AbstractBehaviour? Behaviour { get; set; }
        InfoEntity? InfoEntity { get; set; }
        TripDetection?[]? Detections { get; }
        bool AllowHack { get; }
        bool AllowInteraction { get; }
        bool AllowOverload { get; }
        bool Hacked { get; }
        bool Overloaded { get; }
    }
}