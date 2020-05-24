using UnityEngine;

namespace HackedDesign.Entities
{
    class InteractionSpriteOverlay : MonoBehaviour
    {
        [Header("Game Objects")]
        [SerializeField] private SpriteRenderer[] sprite = null;
        [SerializeField] private int index = 0;

        private void Start()
        {
            foreach(SpriteRenderer s in sprite)
            {
                s.gameObject.SetActive(false);

            }
        }

        public void Show(bool flag)
        {
            if(sprite == null || sprite.Length <= 0) return;
            
            if (sprite[index].gameObject.activeInHierarchy != flag)
            {
                sprite[index].gameObject.SetActive(flag);
            }
        }

        public void SetSprite(int index)
        {
            Logger.Log(this, "Set sprite", index.ToString());
            Show(false);
            this.index = index;
            Show(true);
        }
    }
}
