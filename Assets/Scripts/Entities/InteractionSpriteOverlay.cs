using UnityEngine;
using static HackedDesign.Entity;

namespace HackedDesign
{
    public class InteractionSpriteOverlay : MonoBehaviour
    {
        [Header("Game Objects")]       
        [SerializeField] private SpriteRenderer spriteRenderer = null;
        

        [Header("Settings")]
        [SerializeField] private Color normalColor = Color.white;
        [SerializeField] private Color highlightColor = Color.white;

        private void Awake()
        {
            SetSpriteRenderer();
        }

        private void OnEnable()
        {
            SetSpriteRenderer();
        }

        private void SetSpriteRenderer()
        {
            if (spriteRenderer == null)
            {
                spriteRenderer = GetComponentInChildren<SpriteRenderer>();
                spriteRenderer.color = normalColor;
            }
        }

        public void SetSprite(Sprite sprite)
        {
            spriteRenderer.sprite = sprite;
        }

        public void ShowInteract()
        {
            spriteRenderer.color = highlightColor;
        }

        public void HideInteract()
        {
            spriteRenderer.color = normalColor;
        }

        // public void Show(bool flag)
        // {
        //     spriteRenderer.color = flag ? highlightColor : normalColor;
        // }
    }
}
