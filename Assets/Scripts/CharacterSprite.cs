using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

namespace HackedDesign
{
    public class CharacterSprite : MonoBehaviour
    {
        [Header("Renderers")]
        public SpriteRenderer bodySpriteRenderer;
        public SpriteRenderer eyesSpriteRenderer;
        public SpriteRenderer hairSpriteRenderer;
        public SpriteRenderer shirtSpriteRenderer;
        public SpriteRenderer pantsSpriteRenderer;
        public SpriteRenderer shoesSpriteRenderer;

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

        public Color[] colors;

        [Header("Settings")]
        public bool isMale = false;

        public int bodyIndex;
        public int eyesIndex;
        public int shirtIndex;
        public int pantsIndex;
        public int shoesIndex;
        public int hairIndex;
        public int shirtColorIndex;
        public int pantsColorIndex;
        public int shoesColorIndex;
        public int hairColorIndex;

        private Sprite[] bodySpritesheet;
        private Sprite[] eyesSpritesheet;
        private Sprite[] shirtSpritesheet;
        private Sprite[] pantsSpritesheet;
        private Sprite[] shoesSpritesheet;
        private Sprite[] hairSpritesheet;
        private string currentFrameName; //Name of the current frame. Used to find the current frame's number.
        private int frameIndex = 0; //The index of the current frame. Used to set index of new frame.    


        void SetSpritesheets()
        {
            bodySpritesheet = isMale ? Resources.LoadAll<Sprite>(maleBodyFolderPath + maleBodySprites[bodyIndex].name) : Resources.LoadAll<Sprite>(femaleBodyFolderPath + femaleBodySprites[bodyIndex].name);
            eyesSpritesheet = isMale ? Resources.LoadAll<Sprite>(maleEyesFolderPath + maleEyesSprites[eyesIndex].name) : Resources.LoadAll<Sprite>(femaleEyesFolderPath + femaleEyesSprites[eyesIndex].name);
            shirtSpritesheet = isMale ? Resources.LoadAll<Sprite>(maleShirtFolderPath + maleShirtSprites[shirtIndex].name) : Resources.LoadAll<Sprite>(femaleShirtFolderPath + femaleShirtSprites[shirtIndex].name);
            pantsSpritesheet = isMale ? Resources.LoadAll<Sprite>(malePantsFolderPath + malePantsSprites[pantsIndex].name) : Resources.LoadAll<Sprite>(femalePantsFolderPath + femalePantsSprites[pantsIndex].name);
            shoesSpritesheet = isMale ? Resources.LoadAll<Sprite>(maleShoesFolderPath + maleShoesSprites[shoesIndex].name) : Resources.LoadAll<Sprite>(femaleShoesFolderPath + femaleShoesSprites[shoesIndex].name);
            if(hairIndex>-1)
            {
                hairSpritesheet = isMale ? Resources.LoadAll<Sprite>(maleHairFolderPath + maleHairSprites[hairIndex].name) : Resources.LoadAll<Sprite>(femaleHairFolderPath + femaleHairSprites[hairIndex].name);
            }
        }

        void LateUpdate() //FIXME: make this called as part of the game loop
        {
            SetSpritesheets();
            currentFrameName = bodySpriteRenderer.sprite.name;
            currentFrameName = Regex.Replace(currentFrameName, "r2c", "");

            int.TryParse(Regex.Replace(currentFrameName, "[^0-9]", ""), out frameIndex);

            frameIndex += 128;

            bodySpriteRenderer.sprite = bodySpritesheet[frameIndex];
            eyesSpriteRenderer.sprite = eyesSpritesheet[frameIndex];
            shirtSpriteRenderer.sprite = shirtSpritesheet[frameIndex];
            pantsSpriteRenderer.sprite = pantsSpritesheet[frameIndex];
            shoesSpriteRenderer.sprite = shoesSpritesheet[frameIndex];
            
            shirtSpriteRenderer.color = colors[shirtColorIndex];
            pantsSpriteRenderer.color = colors[pantsColorIndex];
            shoesSpriteRenderer.color = colors[shoesColorIndex];
            if(hairIndex>-1)
            {
                hairSpriteRenderer.sprite = hairSpritesheet[frameIndex];
                hairSpriteRenderer.color = colors[hairColorIndex];
            }
            else {
                hairSpriteRenderer.sprite = null;
            }
        }
    }
}