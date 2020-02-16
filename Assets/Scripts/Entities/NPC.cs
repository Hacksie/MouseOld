
using UnityEngine;
using HackedDesign.Story;

namespace HackedDesign.Entity
{


    public class NPC : BaseEntity
    {

        public const string UNKNOWN_STRING = @"?";

        //[SerializeField]
        //SpriteRenderer actionBubble = null;

        //[SerializeField]
        //Collider2D actionTriggerCollider = null;

        [SerializeField]
        string character = "";

        //[SerializeField]
        //bool known = false;

        [SerializeField]
        UnityEngine.UI.Text text = null;

        new void Start()
        {
            base.Start();
            if (string.IsNullOrWhiteSpace(character))
            {
                Debug.LogError(this.name + ": character is null");
            }
            if (text == null)
            {
                Debug.LogError(this.name + ": text is null");
            }

            SetHandleText();
        }

        public override void UpdateBehaviour()
        {

            base.UpdateBehaviour();
            SetHandleText();
        }

        void SetHandleText()
        {
            
            if(text == null)
            {
                return; 
            }
            var c = InfoManager.instance.GetCharacter(character);
            text.text = c.handle;
            
            
        }
    }
}