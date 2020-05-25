using UnityEngine;

namespace HackedDesign.Story
{
    [CreateAssetMenu(fileName = "Location", menuName = "Mouse/Content/Location")]
    [System.Serializable]
    public class Location : InfoEntity
    {
        public string title;
        public string corp;
    }
}
