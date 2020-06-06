using System;
using UnityEngine;
using HackedDesign.Story;


namespace HackedDesign.Entities
{
    class CharacterHandleOverlay : MonoBehaviour
    {
        public const string UNKNOWN_STRING = @"?";

        [SerializeField]
        UnityEngine.UI.Text text = null;

        [SerializeField] Character character = null;

        private void Awake()
        {
            if (character == null)
            {
                character = GetComponentInParent<Character>();
            }

        }

        private void Update()
        {
            SetHandleText();
        }

        void SetHandleText()
        {
            if (text == null)
            {
                return;
            }

            if (character != null)
            {
                text.text = character.handle;
            }
            else
            {
                text.text = "";
            }
        }
    }
}
