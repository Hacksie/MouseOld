using UnityEngine;

namespace HackedDesign
{
    [CreateAssetMenu(fileName = "Patrol Config", menuName = "Mouse/Entities/Patrol Config")]
    public class PatrolConfig : ScriptableObject
    {
        [SerializeField] public LayerMask playerLayerMask = 0;
        [SerializeField] public float visibilityDistance = 3.2f;
        [SerializeField] public float patrolSpeed = 0.5f;
        [SerializeField] public float patrolTime = 10.0f;
        [SerializeField] public LayerMask poiLayerMask = 0;
        [SerializeField] public float poiDistance = 10.0f;
        [SerializeField] public bool hostile = false;
    }
}