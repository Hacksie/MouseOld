
using UnityEngine;

namespace HackedDesign
{
    namespace Entity
    {
        
        public class NPC : BaseEntity
        {

            public const string UNKNOWN_STRING = @"???";
            [SerializeField]
            Story.Character character;

            [SerializeField]
            bool known;

            [SerializeField]
            UnityEngine.UI.Text text;

            new void Start()
            {
                base.Start();
                if(character == null)
                {
                    Debug.Log(this.name + ": character is null");
                }
                if(text == null)
                {
                    Debug.Log(this.name + ": text is null");
                }

                known = character.known;

                SetHandleText();                

                
            }

            public override void UpdateBehaviour ()
            {
                base.UpdateBehaviour();
                SetHandleText();
            }

            void SetHandleText()
            {
                if(known) {
                text.text = character.handle;
                } else
                {
                    text.text = UNKNOWN_STRING;
                }

            }
            
        }
    }
}