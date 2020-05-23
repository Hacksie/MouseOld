using UnityEngine;

namespace HackedDesign
{
    namespace Story
    {
        [CreateAssetMenu(fileName = "Trap", menuName = "Mouse/Content/Trap")]
        [System.Serializable]
        public class Trap : InfoEntity
        {
            public string uniqueId;
            public string handle;
            public string corp;
            public string serial;
        }
    }
}