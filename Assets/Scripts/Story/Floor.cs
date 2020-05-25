using UnityEngine;

namespace HackedDesign.Story
{
    [CreateAssetMenu(fileName = "Floor", menuName = "Mouse/Content/Floor")]
    [System.Serializable]
    public class Floor : InfoEntity
    {
        public string locationId;
        public bool random;
    }
}
