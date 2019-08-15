using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign {

	public class PlayerController : MonoBehaviour {

		private float movementEpsilon = 0.2f; //An axis value above this is considered movement.		

		[Range (0.0f, 10.0f)]
		[Tooltip ("The movement speed of the controller.")]
		[SerializeField]
		private float baseMovementSpeed = 1.0f;

		private Vector2 movementVector; 
		private Animator anim; 

		
		void Start () {
			anim = transform.GetComponent<Animator> ();
		}

		public void UpdateMovement (Input.IInputController inputController) {

			movementVector = inputController.GetMovementAxis ();

			if (movementVector.sqrMagnitude > (movementEpsilon * movementEpsilon)) {
				anim.SetFloat ("directionX", movementVector.x);
				anim.SetFloat ("directionY", movementVector.y);
				anim.SetBool ("isMoving", true);
			} else {
				movementVector = Vector2.zero;
				if (anim != null) {
					anim.SetBool ("isMoving", false);
				}
			}
		}

		public void UpdateTransform () {
			// Movement augments (0 - 10) are reduced by a factor of 10
			transform.Translate (movementVector * (baseMovementSpeed + (CoreGame.Instance.CoreState.player.movementAugments / 10.0f)) * Time.fixedDeltaTime);
		}
	}
}