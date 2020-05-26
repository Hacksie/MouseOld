using UnityEngine;

namespace HackedDesign.Entities
{
    class InteractionSpriteOverlay : MonoBehaviour
    {
        [Header("Game Objects")]
        [SerializeField] private Color normalColor = Color.white;
        [SerializeField] private Color highlightColor = Color.white;
    
        [SerializeField] private SpriteRenderer sprite = null;

        private void Start()
        {
            sprite.color = normalColor;
        }

        public void Show(bool flag)
        {
            sprite.color = flag ? highlightColor : normalColor;
            
            // if (sprite == null) return;

            // if (sprite.gameObject.activeInHierarchy != flag)
            // {
            //     sprite.gameObject.SetActive(flag);
            // }
        }
    }
}
