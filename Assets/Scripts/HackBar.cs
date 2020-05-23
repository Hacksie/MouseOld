using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign
{
    public class HackBar : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer frame = null;
        [SerializeField] private SpriteRenderer bar = null;

        void Awake()
        {
            Hide();
        }

        public void Show()
        {
            if (!frame.gameObject.activeInHierarchy)
            {
                frame.gameObject.SetActive(true);
            }
        }

        public void Hide()
        {
            if (frame.gameObject.activeInHierarchy)
            {
                frame.gameObject.SetActive(false);
            }
        }

        public void UpdateBar(float value)
        {
            bar.transform.localScale = new Vector3(value, 1, 1);
        }
    }
}