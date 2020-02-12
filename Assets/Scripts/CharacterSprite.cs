using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

namespace HackedDesign
{
    public class CharacterSprite : MonoBehaviour
    {
        public CharacterSpriteManager characterSpriteManager;

        [Header("Renderers")]
        public SpriteRenderer bodySpriteRenderer;
        public SpriteRenderer eyesSpriteRenderer;
        public SpriteRenderer hairSpriteRenderer;
        public SpriteRenderer shirtSpriteRenderer;
        public SpriteRenderer pantsSpriteRenderer;
        public SpriteRenderer shoesSpriteRenderer;

        [Header("Settings")]
        public CharacterSpriteManager.BodyTypes bodyType;
        //public int bodyIndex;
        public int skinIndex;
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

        void Start()
        {
            SetSpritesheets();
        }

        void SetSpritesheets()
        {
            bodyType = bodyType == CharacterSpriteManager.BodyTypes.RandomHuman ? (CharacterSpriteManager.BodyTypes)Random.Range(0, 2) : bodyType;
            skinIndex = skinIndex < 0 ? Random.Range(0, characterSpriteManager.maleBodySprites.Length) : skinIndex;
            eyesIndex = eyesIndex < 0 ? Random.Range(1, characterSpriteManager.maleEyesSprites.Length) : eyesIndex;
            shirtIndex = shirtIndex < 0 ? Random.Range(1, characterSpriteManager.maleShirtSprites.Length) : shirtIndex;
            pantsIndex = pantsIndex < 0 ? Random.Range(1, characterSpriteManager.malePantsSprites.Length) : pantsIndex;
            shoesIndex = shoesIndex < 0 ? Random.Range(1, characterSpriteManager.maleShoesSprites.Length) : shoesIndex;
            hairIndex = hairIndex < 0 ? Random.Range(0, characterSpriteManager.maleHairSprites.Length) : hairIndex;
            shirtColorIndex = shirtColorIndex < 0 ? Random.Range(0, characterSpriteManager.colors.Length) : shirtColorIndex;
            pantsColorIndex = pantsColorIndex < 0 ? Random.Range(0, characterSpriteManager.colors.Length) : pantsColorIndex;
            shoesColorIndex = shoesColorIndex < 0 ? Random.Range(0, characterSpriteManager.colors.Length) : shoesColorIndex;
            hairColorIndex = hairColorIndex < 0 ? Random.Range(0, characterSpriteManager.colors.Length) : hairColorIndex;

            switch (bodyType)
            {
                case CharacterSpriteManager.BodyTypes.Male:
                    bodySpritesheet = Resources.LoadAll<Sprite>(characterSpriteManager.maleBodyFolderPath + characterSpriteManager.maleBodySprites[skinIndex].name);
                    if (eyesIndex > 0)
                    {
                        eyesSpritesheet = Resources.LoadAll<Sprite>(characterSpriteManager.maleEyesFolderPath + characterSpriteManager.maleEyesSprites[eyesIndex].name);
                    }
                    if (shirtIndex > 0)
                    {
                        shirtSpritesheet = Resources.LoadAll<Sprite>(characterSpriteManager.maleShirtFolderPath + characterSpriteManager.maleShirtSprites[shirtIndex].name);
                    }
                    if (pantsIndex > 0)
                    {
                        pantsSpritesheet = Resources.LoadAll<Sprite>(characterSpriteManager.malePantsFolderPath + characterSpriteManager.malePantsSprites[pantsIndex].name);
                    }
                    if (shoesIndex > 0)
                    {
                        shoesSpritesheet = Resources.LoadAll<Sprite>(characterSpriteManager.maleShoesFolderPath + characterSpriteManager.maleShoesSprites[shoesIndex].name);
                    }
                    if (hairIndex > 0)
                    {
                        hairSpritesheet = Resources.LoadAll<Sprite>(characterSpriteManager.maleHairFolderPath + characterSpriteManager.maleHairSprites[hairIndex].name);
                    }

                    shirtSpriteRenderer.color = characterSpriteManager.colors[shirtColorIndex];
                    pantsSpriteRenderer.color = characterSpriteManager.colors[pantsColorIndex];
                    shoesSpriteRenderer.color = characterSpriteManager.colors[shoesColorIndex];
                    hairSpriteRenderer.color = characterSpriteManager.colors[hairColorIndex];

                    if (hairIndex == 0)
                    {
                        hairSpriteRenderer.sprite = null;
                    }
                    break;
                case CharacterSpriteManager.BodyTypes.Female:
                    bodySpritesheet = Resources.LoadAll<Sprite>(characterSpriteManager.femaleBodyFolderPath + characterSpriteManager.femaleBodySprites[skinIndex].name);
                    if (eyesIndex > 0)
                    {
                        eyesSpritesheet = Resources.LoadAll<Sprite>(characterSpriteManager.femaleEyesFolderPath + characterSpriteManager.femaleEyesSprites[eyesIndex].name);
                    }
                    if (shirtIndex > 0)
                    {
                        shirtSpritesheet = Resources.LoadAll<Sprite>(characterSpriteManager.femaleShirtFolderPath + characterSpriteManager.femaleShirtSprites[shirtIndex].name);
                    }
                    if (pantsIndex > 0)
                    {
                        pantsSpritesheet = Resources.LoadAll<Sprite>(characterSpriteManager.femalePantsFolderPath + characterSpriteManager.femalePantsSprites[pantsIndex].name);
                    }
                    if (shoesIndex > 0)
                    {
                        shoesSpritesheet = Resources.LoadAll<Sprite>(characterSpriteManager.femaleShoesFolderPath + characterSpriteManager.femaleShoesSprites[shoesIndex].name);
                    }
                    if (hairIndex > 0)
                    {
                        hairSpritesheet = Resources.LoadAll<Sprite>(characterSpriteManager.femaleHairFolderPath + characterSpriteManager.femaleHairSprites[hairIndex].name);
                    }

                    shirtSpriteRenderer.color = characterSpriteManager.colors[shirtColorIndex];
                    pantsSpriteRenderer.color = characterSpriteManager.colors[pantsColorIndex];
                    shoesSpriteRenderer.color = characterSpriteManager.colors[shoesColorIndex];
                    hairSpriteRenderer.color = characterSpriteManager.colors[hairColorIndex];

                    if (hairIndex == 0)
                    {
                        hairSpriteRenderer.sprite = null;
                    }
                    break;
                case CharacterSpriteManager.BodyTypes.Cat:
                    bodySpritesheet = Resources.LoadAll<Sprite>(characterSpriteManager.catBodyFolderPath + characterSpriteManager.catBodySprites[skinIndex].name);
                    break;
                case CharacterSpriteManager.BodyTypes.Drone:
                    bodySpritesheet = Resources.LoadAll<Sprite>(characterSpriteManager.droneBodyFolderPath + characterSpriteManager.droneBodySprites[skinIndex].name);
                    break;
            }
        }



        void LateUpdate() //FIXME: make this called as part of the game loop
        {
            //SetSpritesheets();
            currentFrameName = bodySpriteRenderer.sprite.name;

            if (currentFrameName.StartsWith("r2c"))
            {
                currentFrameName = currentFrameName.Substring(3);
            }

            //FIXME: Better than it was, but still GC issues with substring;
            int ix = currentFrameName.LastIndexOf("_");
            string frame = currentFrameName.Substring(ix + 1);
            if (frame.Length > 0)
            {
                int.TryParse(frame, out frameIndex);
            }

            //Debug.Log(frameIndex, ani)

            frameIndex += 128;
            if (bodySpriteRenderer != null)
                bodySpriteRenderer.sprite = bodySpritesheet[frameIndex];

            if (eyesSpriteRenderer != null && eyesIndex > 0)
                eyesSpriteRenderer.sprite = eyesSpritesheet[frameIndex];

            if (shirtSpriteRenderer != null && shirtIndex > 0)
                shirtSpriteRenderer.sprite = shirtSpritesheet[frameIndex];

            if (pantsSpriteRenderer != null && pantsIndex > 0)
                pantsSpriteRenderer.sprite = pantsSpritesheet[frameIndex];

            if (shoesSpriteRenderer != null && shoesIndex > 0)
                shoesSpriteRenderer.sprite = shoesSpritesheet[frameIndex];

            if (hairSpriteRenderer != null && hairIndex > 0)
            {
                hairSpriteRenderer.sprite = hairSpritesheet[frameIndex];

            }
        }
    }
}