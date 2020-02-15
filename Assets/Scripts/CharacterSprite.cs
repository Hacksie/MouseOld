﻿using UnityEngine;

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
        public string character;

        private Sprite[] bodySpritesheet;
        private Sprite[] eyesSpritesheet;
        private Sprite[] shirtSpritesheet;
        private Sprite[] pantsSpritesheet;
        private Sprite[] shoesSpritesheet;
        private Sprite[] hairSpritesheet;

        public void Initialize(CharacterSpriteManager characterSpriteManager)
        {
            this.characterSpriteManager = characterSpriteManager;
            SetSpritesheets();
        }

        void SetSpritesheets()
        {
            bodySpritesheet = characterSpriteManager.GetBody(character);
            hairSpritesheet = characterSpriteManager.GetHair(character);
            eyesSpritesheet = characterSpriteManager.GetEyes(character);
            shirtSpritesheet = characterSpriteManager.GetShirt(character);
            pantsSpritesheet = characterSpriteManager.GetPants(character);
            shoesSpritesheet = characterSpriteManager.GetShoes(character);
            if(hairSpriteRenderer != null)
                hairSpriteRenderer.color = characterSpriteManager.GetHairColor(character);
            
            if(shirtSpriteRenderer != null)
                shirtSpriteRenderer.color = characterSpriteManager.GetShirtColor(character);

            if(pantsSpriteRenderer != null)
                pantsSpriteRenderer.color = characterSpriteManager.GetPantsColor(character);

            if(shoesSpriteRenderer != null)
                shoesSpriteRenderer.color = characterSpriteManager.GetShoesColor(character);
        }



        public void UpdateSprites()
        {

            //SetSpritesheets();
            string currentFrameName = bodySpriteRenderer.sprite.name;

            if (currentFrameName.StartsWith("r2c"))
            {
                currentFrameName = currentFrameName.Substring(3);
            }

            //FIXME: Better than it was, but still GC issues with substring;
            int ix = currentFrameName.LastIndexOf("_");
            string frame = currentFrameName.Substring(ix + 1);
            int frameIndex = 0;
            if (frame.Length > 0)
            {
                int.TryParse(frame, out frameIndex);
            }


            frameIndex += characterSpriteManager.GetSpriteOffset(character);

            if(bodySpriteRenderer!=null)
                bodySpriteRenderer.sprite = (bodySpritesheet != null && bodySpritesheet.Length > 0) ? bodySpritesheet[frameIndex] : null;
            if(hairSpriteRenderer!=null)
                hairSpriteRenderer.sprite = (hairSpritesheet != null && hairSpritesheet.Length > 0) ? hairSpritesheet[frameIndex] : null;
            if(eyesSpriteRenderer!=null)
                eyesSpriteRenderer.sprite = (eyesSpritesheet != null && eyesSpritesheet.Length > 0) ? eyesSpritesheet[frameIndex] : null;
            if(shirtSpriteRenderer!=null)
                shirtSpriteRenderer.sprite = (shirtSpritesheet != null && shirtSpritesheet.Length > 0) ? shirtSpritesheet[frameIndex] : null;
            if(pantsSpriteRenderer!=null)
                pantsSpriteRenderer.sprite = (pantsSpritesheet != null && pantsSpritesheet.Length > 0) ? pantsSpritesheet[frameIndex] : null;
            if(shoesSpriteRenderer!=null)
                shoesSpriteRenderer.sprite = (shoesSpritesheet != null && shoesSpritesheet.Length > 0) ? shoesSpritesheet[frameIndex] : null;

        }
    }
}