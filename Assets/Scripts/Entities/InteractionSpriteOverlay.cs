using UnityEngine;

namespace HackedDesign.Entities
{
    class InteractionSpriteOverlay : MonoBehaviour
    {
        [Header("Game Objects")]
        [SerializeField]
        private SpriteRenderer sprite = null;
        [SerializeField]
        public float timeout = 2.0f;

        private float startTime;

        private void Start()
        {
            Show(false);
        }

        public void Show(bool flag)
        {
            if(sprite == null) return;
            
            if (sprite.gameObject.activeInHierarchy != flag)
            {
                sprite.gameObject.SetActive(flag);
            }

            if(flag)
            {
                startTime = Time.time;
            }
        }

        void Update()
        {
            //if(sprite.gameObject.activeInHierarchy && (Time.time - startTime) > timeout)
            //{
            //    sprite.gameObject.SetActive(false);
            //}
        }
    }
}
