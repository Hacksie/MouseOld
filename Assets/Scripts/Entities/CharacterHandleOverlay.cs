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

        [SerializeField]
        string character = "";

        private void Update()
        {
            if(CoreGame.Instance.IsInGame())
                SetHandleText();
        }

        void SetHandleText()
        {
            if (text == null)
            {
                return;
            }
            
            var character = InfoRepository.Instance.GetCharacter(this.character);
            text.text = character.handle;
        }
    }
}
