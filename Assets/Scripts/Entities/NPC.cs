
using UnityEngine;
using HackedDesign.Story;

namespace HackedDesign.Entity
{


    public class NPC : BaseEntity
    {

        

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

        public override void Initialize()
        {
            base.Initialize();
            if (string.IsNullOrWhiteSpace(character))
            {
                Debug.LogError(this.name + ": character is null");
            }
            if (text == null)
            {
                Debug.LogError(this.name + ": text is null");
            }

            //SetHandleText();
        }

        public override void UpdateBehaviour()
        {

            base.UpdateBehaviour();
            
        }


    }
}