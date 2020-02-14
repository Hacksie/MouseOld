using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

namespace HackedDesign
{
    public class CharacterSpriteManager : MonoBehaviour
    {
        public static string[] RandomColors = {
            "#FFFFFFFF",
            "#5A5A5AFF",
            "#FFD98CFF",
            "#0056FFFF",
            "#BC8239FF",
            "#28FF00FF",
            "#A52DFFFF",
            "#FF3A00FF",
            "#A6033CFF"
        };


        [Header("Assets")]
        public Texture2D[] maleBodySprites;
        public string maleBodyFolderPath;
        public Texture2D[] maleEyesSprites;
        public string maleEyesFolderPath;
        public Texture2D[] maleShirtSprites;
        public string maleShirtFolderPath;
        public Texture2D[] malePantsSprites;
        public string malePantsFolderPath;
        public Texture2D[] maleShoesSprites;
        public string maleShoesFolderPath;
        public Texture2D[] maleHairSprites;
        public string maleHairFolderPath;

        public Texture2D[] femaleBodySprites;
        public string femaleBodyFolderPath;
        public Texture2D[] femaleEyesSprites;
        public string femaleEyesFolderPath;
        public Texture2D[] femaleShirtSprites;
        public string femaleShirtFolderPath;
        public Texture2D[] femalePantsSprites;
        public string femalePantsFolderPath;
        public Texture2D[] femaleShoesSprites;
        public string femaleShoesFolderPath;
        public Texture2D[] femaleHairSprites;
        public string femaleHairFolderPath;

        public Texture2D[] creatureBodySprites;
        public string creatureBodyFolderPath;


        //public Color[] colors;

        private Story.InfoManager infoManager;

        public void Initialize(Story.InfoManager infoManager)
        {
            this.infoManager = infoManager;

        }

        public Sprite[] GetBody(string id)
        {
            var character = infoManager.GetCharacter(id);
            switch (character.body)
            {
                case BodyTypes.Male:
                return Resources.LoadAll<Sprite>(maleBodyFolderPath + maleBodySprites[character.skin].name);

                case BodyTypes.Female:
                return Resources.LoadAll<Sprite>(femaleBodyFolderPath + femaleBodySprites[character.skin].name);

                case BodyTypes.Creature:
                return Resources.LoadAll<Sprite>(creatureBodyFolderPath + creatureBodySprites[character.skin].name);
            }
            return null;
        }

        public Sprite[] GetHair(string id)
        {
            var character = infoManager.GetCharacter(id);

            if(character.hair == 0)
                return null;
            switch ((BodyTypes)character.body)
            {
                case BodyTypes.Male:
                return Resources.LoadAll<Sprite>(maleHairFolderPath + maleHairSprites[character.hair].name);

                case BodyTypes.Female:
                return Resources.LoadAll<Sprite>(femaleHairFolderPath + femaleHairSprites[character.hair].name);
            }

            return null;
        }        

        public Sprite[] GetEyes(string id)
        {
            var character = infoManager.GetCharacter(id);

            if(character.eyes == 0)
                return null;

            switch ((BodyTypes)character.body)
            {
                case BodyTypes.Male:
                return Resources.LoadAll<Sprite>(maleEyesFolderPath + maleEyesSprites[character.eyes].name);

                case BodyTypes.Female:
                return Resources.LoadAll<Sprite>(femaleEyesFolderPath + femaleEyesSprites[character.eyes].name);
            }

            return null;
        }  

        public Sprite[] GetShirt(string id)
        {
            var character = infoManager.GetCharacter(id);

            if(character.shirt == 0)
                return null;

            switch ((BodyTypes)character.body)
            {
                case BodyTypes.Male:
                return Resources.LoadAll<Sprite>(maleShirtFolderPath + maleShirtSprites[character.shirt].name);

                case BodyTypes.Female:
                return Resources.LoadAll<Sprite>(femaleShirtFolderPath + femaleShirtSprites[character.shirt].name);
            }

            return null;
        }
        public Sprite[] GetPants(string id)
        {
            var character = infoManager.GetCharacter(id);

            if(character.pants == 0)
                return null;

            switch ((BodyTypes)character.body)
            {
                case BodyTypes.Male:
                return Resources.LoadAll<Sprite>(malePantsFolderPath + malePantsSprites[character.pants].name);

                case BodyTypes.Female:
                return Resources.LoadAll<Sprite>(femalePantsFolderPath + femalePantsSprites[character.pants].name);
            }

            return null;
        }
        public Sprite[] GetShoes(string id)
        {
            var character = infoManager.GetCharacter(id);

            if(character.shoes == 0)
                return null;

            switch ((BodyTypes)character.body)
            {
                case BodyTypes.Male:
                return Resources.LoadAll<Sprite>(maleShoesFolderPath + maleShoesSprites[character.shoes].name);

                case BodyTypes.Female:
                return Resources.LoadAll<Sprite>(femaleShoesFolderPath + femaleShoesSprites[character.shoes].name);
            }

            return null;
        }

        public Color GetHairColor(string id)
        {
            var character = infoManager.GetCharacter(id);

            Color color = Color.magenta;

            if(!string.IsNullOrWhiteSpace(character.haircolor))
            {
                ColorUtility.TryParseHtmlString(character.haircolor, out color);
            }

            return color;
        }

        public Color GetShirtColor(string id)
        {
            var character = infoManager.GetCharacter(id);

            Color color = Color.magenta;

            if(!string.IsNullOrWhiteSpace(character.shirtcolor))
            {
                ColorUtility.TryParseHtmlString(character.shirtcolor, out color);
            }

            return color;
        }  

        public Color GetPantsColor(string id)
        {
            var character = infoManager.GetCharacter(id);

            Color color = Color.magenta;

            if(!string.IsNullOrWhiteSpace(character.pantscolor))
            {
                ColorUtility.TryParseHtmlString(character.pantscolor, out color);
            }

            return color;
        }  

        public Color GetShoesColor(string id)
        {
            var character = infoManager.GetCharacter(id);

            Color color = Color.magenta;

            if(!string.IsNullOrWhiteSpace(character.shoescolor))
            {
                ColorUtility.TryParseHtmlString(character.shoescolor, out color);
            }

            return color;
        }                        
        

        public enum BodyTypes
        {
            Male,
            Female,
            Creature,
            RandomHuman
        }


    }
}