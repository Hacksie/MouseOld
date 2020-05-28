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

        [SerializeField] Character character;

        private void Awake()
        {
            SetHandleText();
        }

        private void Update()
        {
            
            /*
            if(GameManager.Instance.IsInGame())
                
                */
        }

        void SetHandleText()
        {
            if (text == null)
            {
                return;
            }
            
            //var character = InfoRepository.Instance.GetCharacter(this.character);
            text.text = character.handle;
        }
    }
}
