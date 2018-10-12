using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign {

	public class PlayerController : MonoBehaviour {

		private float moveSense = 0.2f; //An axis value above this is considered movement.		

		[Range (0.0f, 10.0f)]
		[Tooltip ("The movement speed of the controller.")]
		public float moveSpeed = 5.0f;
		private Vector2 moveVector; //The vector used to apply movement to the controller.		
		private Animator anim; //The parent animator.

		// Use this for initialization
		void Start () {
			anim = transform.GetComponent<Animator> ();

			//AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo (0); //could replace 0 by any other animation layer index
			//anim.Play (state.fullPathHash, -1, Random.Range (0f, 1f));

		}

		// Update is called once per frame
		public void UpdateMovement (Input.IInputController inputController) {

			moveVector = inputController.GetMovementAxis ();

			

			//If horizontal or vertical axis is above the threshold value (moveSense), set the move state to Walk.
			if (moveVector.sqrMagnitude > (moveSense * moveSense)) {
				//moveState = MoveState.Walk;
				//Pass the moveVector axes to the animators move variables and set animator's isMoving to true.
				anim.SetFloat ("moveX", moveVector.x);
				anim.SetFloat ("moveY", moveVector.y);
				anim.SetBool ("isMoving", true);

				//transform.Translate (moveVector * moveSpeed * Time.deltaTime);

			} else {
				//If there's no input, set the state to stand again and change Animator's isMoving to false.
				//moveState = MoveState.Stand;
				moveVector = Vector2.zero;

				anim.SetBool ("isMoving", false);
			}
		}

		public void UpdateTransform () {
			transform.Translate (moveVector * moveSpeed * Time.fixedDeltaTime);
		}

		private void OnCollisionEnter2D(Collision2D other)
		{
			//Debug.Log(other.gameObject.name);
			
		}
	}
}