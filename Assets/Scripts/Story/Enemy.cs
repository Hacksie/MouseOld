using UnityEngine;

namespace HackedDesign
{
    namespace Story
    {
        [CreateAssetMenu(fileName = "Enemy", menuName = "Mouse/Content/Enemy")]
        [System.Serializable]
        public class Enemy : InfoEntity
        {
            public string uniqueId;
            public string handle;
            public string corp;
            public string serial;
        }
    }
}