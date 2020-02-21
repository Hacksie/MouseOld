using UnityEngine;

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
        public bool isEnemy = false;
        public string character;
        public string uniqueId;
        private Sprite[] bodySpritesheet;
        private Sprite[] eyesSpritesheet;
        private Sprite[] shirtSpritesheet;
        private Sprite[] pantsSpritesheet;
        private Sprite[] shoesSpritesheet;
        private Sprite[] hairSpritesheet;

        private int spriteOffset;

        public void Initialize(CharacterSpriteManager characterSpriteManager)
        {
            this.characterSpriteManager = characterSpriteManager;
            SetSpritesheets();
        }

        void SetSpritesheets()
        {
            if (isEnemy)
            {
                bodySpritesheet = characterSpriteManager.GetEnemySkin(uniqueId);
                hairSpritesheet = characterSpriteManager.GetEnemyHair(uniqueId);
                eyesSpritesheet = characterSpriteManager.GetEnemyEyes(uniqueId);
                shirtSpritesheet = characterSpriteManager.GetEnemyShirt(uniqueId);
                pantsSpritesheet = characterSpriteManager.GetEnemyPants(uniqueId);
                shoesSpritesheet = characterSpriteManager.GetEnemyShoes(uniqueId);
                if (hairSpriteRenderer != null)
                    hairSpriteRenderer.color = characterSpriteManager.GetEnemyHairColor(uniqueId);

                if (shirtSpriteRenderer != null)
                    shirtSpriteRenderer.color = characterSpriteManager.GetEnemyShirtColor(uniqueId);

                if (pantsSpriteRenderer != null)
                    pantsSpriteRenderer.color = characterSpriteManager.GetEnemyPantsColor(uniqueId);

                if (shoesSpriteRenderer != null)
                    shoesSpriteRenderer.color = characterSpriteManager.GetEnemyShoesColor(uniqueId);

                spriteOffset = characterSpriteManager.GetEnemySpriteOffset(uniqueId);
            }
            else
            {
                bodySpritesheet = characterSpriteManager.GetSkin(character);
                hairSpritesheet = characterSpriteManager.GetHair(character);
                eyesSpritesheet = characterSpriteManager.GetEyes(character);
                shirtSpritesheet = characterSpriteManager.GetShirt(character);
                pantsSpritesheet = characterSpriteManager.GetPants(character);
                shoesSpritesheet = characterSpriteManager.GetShoes(character);
                if (hairSpriteRenderer != null)
                    hairSpriteRenderer.color = characterSpriteManager.GetHairColor(character);

                if (shirtSpriteRenderer != null)
                    shirtSpriteRenderer.color = characterSpriteManager.GetShirtColor(character);

                if (pantsSpriteRenderer != null)
                    pantsSpriteRenderer.color = characterSpriteManager.GetPantsColor(character);

                if (shoesSpriteRenderer != null)
                    shoesSpriteRenderer.color = characterSpriteManager.GetShoesColor(character);

                spriteOffset = characterSpriteManager.GetSpriteOffset(character);
            }
        }



        public void UpdateSprites()
        {

            string currentFrameName = bodySpriteRenderer.sprite.name;

            if (currentFrameName.StartsWith("r2c", System.StringComparison.Ordinal))
            {
                currentFrameName = currentFrameName.Substring(3);
            }

            //FIXME: Better than it was, but still GC issues with substring;
            int ix = currentFrameName.LastIndexOf("_", System.StringComparison.Ordinal);
            string frame = currentFrameName.Substring(ix + 1);
            int frameIndex = 0;
            if (frame.Length > 0)
            {
                _ = int.TryParse(frame, out frameIndex);
            }


            frameIndex +=spriteOffset;

            if (bodySpriteRenderer != null)
                bodySpriteRenderer.sprite = (bodySpritesheet != null && bodySpritesheet.Length > 0) ? bodySpritesheet[frameIndex] : null;
            if (hairSpriteRenderer != null)
                hairSpriteRenderer.sprite = (hairSpritesheet != null && hairSpritesheet.Length > 0) ? hairSpritesheet[frameIndex] : null;
            if (eyesSpriteRenderer != null)
                eyesSpriteRenderer.sprite = (eyesSpritesheet != null && eyesSpritesheet.Length > 0) ? eyesSpritesheet[frameIndex] : null;
            if (shirtSpriteRenderer != null)
                shirtSpriteRenderer.sprite = (shirtSpritesheet != null && shirtSpritesheet.Length > 0) ? shirtSpritesheet[frameIndex] : null;
            if (pantsSpriteRenderer != null)
                pantsSpriteRenderer.sprite = (pantsSpritesheet != null && pantsSpritesheet.Length > 0) ? pantsSpritesheet[frameIndex] : null;
            if (shoesSpriteRenderer != null)
                shoesSpriteRenderer.sprite = (shoesSpritesheet != null && shoesSpritesheet.Length > 0) ? shoesSpritesheet[frameIndex] : null;

        }
    }
}