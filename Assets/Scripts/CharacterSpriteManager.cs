using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

namespace HackedDesign
{
    public class CharacterSpriteManager : MonoBehaviour
    {
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

        public Texture2D[] catBodySprites;
        public string catBodyFolderPath;

        public Color[] colors;

        public enum BodyTypes
        {
            Male,
            Female,
            Cat,
            Drone,
            Android,
            RandomHuman
        }
    }
}