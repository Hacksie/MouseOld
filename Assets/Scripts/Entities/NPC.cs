
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
        Story.Character character = null;

        //[SerializeField]
        //bool known = false;

        [SerializeField]
        UnityEngine.UI.Text text = null;

        new void Start()
        {
            base.Start();
            if (character == null)
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
            
            if (InfoManager.instance.knownEntities.Find(e => e.name == gameObject.name))
            {
                text.text = character.handle;
            }
            else
            {
                text.text = UNKNOWN_STRING;
            }
        }
    }
}