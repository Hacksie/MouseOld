using UnityEngine;

namespace HackedDesign
{
    namespace Story
    {
        [CreateAssetMenu(fileName = "Character", menuName = "Mouse/Content/Character")]
        [System.Serializable]
        public class Character : InfoEntity
        {
            public string fullName;
            public string handle;
            public string corp;
            public string serial;
            public Sprite avatar;
            public Sprite avatarTired;
            public Sprite avatarAngry;
            public Sprite avatarHappy;
            public Sprite avatarThinking;
            public Sprite avatarSmirking;
        }
    }
}