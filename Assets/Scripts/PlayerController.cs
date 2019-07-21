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
		//public CharacterAnimator anim;

		public bool lyingDown = false;

		// Use this for initialization
		void Start () {
			anim = transform.GetComponent<Animator> ();
			//var anim = GetComponent<CharacterAnimator>();

			//AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo (0); //could replace 0 by any other animation layer index
			//anim.Play (state.fullPathHash, -1, Random.Range (0f, 1f));

		}

		// Update is called once per frame
		public void UpdateMovement (Input.IInputController inputController) {

			if (lyingDown) {

				return;
			}

			//StandUp();

			moveVector = inputController.GetMovementAxis ();

			//If horizontal or vertical axis is above the threshold value (moveSense), set the move state to Walk.
			if (moveVector.sqrMagnitude > (moveSense * moveSense)) {

				//moveState = MoveState.Walk;
				//Pass the moveVector axes to the animators move variables and set animator's isMoving to true.
				anim.SetFloat ("directionX", moveVector.x);
				anim.SetFloat ("directionY", moveVector.y);
				anim.SetBool ("isMoving", true);

				//transform.Translate (moveVector * moveSpeed * Time.deltaTime);

			} else {
				//If there's no input, set the state to stand again and change Animator's isMoving to false.
				//moveState = MoveState.Stand;
				moveVector = Vector2.zero;
				//anim.SetMoving(false);

				anim.SetBool ("isMoving", false);
			}
		}

		public void LieDown () {
			lyingDown = true;
			//anim.SetBool ("isMoving", false);
			//anim.Play ("Dead");
		}

		public void StandUp() {
			lyingDown = false;
			//anim.Play ("Stand Tree");
		}

		public void UpdateTransform () {
			transform.Translate (moveVector * moveSpeed * Time.fixedDeltaTime);
		}

		private void OnCollisionEnter2D (Collision2D other) {
			//Debug.Log(other.gameObject.name);

		}
	}
}