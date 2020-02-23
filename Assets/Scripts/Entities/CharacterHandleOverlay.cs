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
            SetHandleText();
        }

        void SetHandleText()
        {

            if (text == null)
            {
                return;
            }
            var c = InfoManager.instance.GetCharacter(character);
            text.text = c.handle;


        }



    }
}
