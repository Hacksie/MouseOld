using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign {

    public class CharacterAnimator : MonoBehaviour {
        // Start is called before the first frame update

        public SpriteRenderer spriteRenderer;

        public float lastAnimation;

        public Sprite[] StandUp;
        public Sprite[] StandLeft;
        public Sprite[] StandRight;
        public Sprite[] StandDown;

        public Sprite[] WalkUp;
        public Sprite[] WalkLeft;
        public Sprite[] WalkRight;
        public Sprite[] WalkDown;

        public Vector2 direction;
        public bool isMoving;


        void Start () {

        }

        public void SetDirection(Vector2 direction)
        {
            this.direction = direction;
            

        }

        public void SetMoving(bool isMoving)
        {
            this.isMoving = isMoving;
        }

        // Update is called once per frame
        void FixedUpdate () {
            var intTime = (int)Time.time;

            Debug.Log("ANIM:" + (intTime * StandLeft.Length) % intTime);

            if(this.direction.x < 0) {
                spriteRenderer.sprite = StandLeft[(intTime * StandLeft.Length) % intTime - 1];
            }
            else if(this.direction.x > 0) {
                spriteRenderer.sprite = StandRight[(intTime * StandRight.Length) % intTime - 1];
            }            
            else if(this.direction.y > 0) {
                spriteRenderer.sprite = StandUp[(intTime * StandUp.Length) % intTime - 1];
            }
            else if(this.direction.y < 0) {
                spriteRenderer.sprite = StandDown[(intTime * StandDown.Length) % intTime - 1];
            }            
        }
    }

}