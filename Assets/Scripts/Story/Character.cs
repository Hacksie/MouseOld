using UnityEngine;

namespace HackedDesign
{
    namespace Story
    {
        [CreateAssetMenu(fileName = "Character", menuName = "Mouse/Content/Character")]
        [System.Serializable]
        public class Character : InfoEntity
        {
            //public string fullName;
            public string handle;
            public string corp;
            public string serial;
            public Sprite avatar;
            public Sprite avatarTired;
            public Sprite avatarAngry;
            public Sprite avatarHappy;
            public Sprite avatarThinking;

            /*
            public CharacterSpriteManager.BodyTypes body;
            public int skin;
            public int eyes;
            public int shirt;
            public int pants;
            public int shoes;
            public int hair;
            public string shirtcolor;
            public string pantscolor;
            public string shoescolor;
            public string haircolor;
            public int spriteOffset;

            public void SetRandomAttributes()
            {
                
                body = body == CharacterSpriteManager.BodyTypes.RandomHuman ? (CharacterSpriteManager.BodyTypes)Random.Range(0, 2) : body;
                //Color c = ColorUtility.

                //FIXME: Pull these from config
                skin = skin < 0 ? Random.Range(0, 4) : skin;
                eyes = eyes < 0 ? Random.Range(1, 8) : eyes;
                shirt = shirt < 0 ? Random.Range(1, 2) : shirt;
                pants = pants < 0 ? Random.Range(1, 2) : pants;
                shoes = shoes < 0 ? Random.Range(1, 2) : shoes;
                hair = hair < 0 ? Random.Range(0, 11) : hair;
                shirtcolor = shirtcolor == "" ? CharacterSpriteManager.RandomColors[Random.Range(0, CharacterSpriteManager.RandomColors.Length)] : shirtcolor;
                pantscolor = pantscolor == "" ? CharacterSpriteManager.RandomColors[Random.Range(0, CharacterSpriteManager.RandomColors.Length)] : pantscolor;
                shoescolor = shoescolor == "" ? CharacterSpriteManager.RandomColors[Random.Range(0, CharacterSpriteManager.RandomColors.Length)] : shoescolor;
                haircolor = haircolor == "" ? CharacterSpriteManager.RandomColors[Random.Range(0, CharacterSpriteManager.RandomColors.Length)] : haircolor;
            }*/
        }
    }
}