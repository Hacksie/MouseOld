using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace HackedDesign {
	public class NPCController : MonoBehaviour {

		private Animator anim; //The parent animator.

		public bool facePlayer = true;

		private List<SpriteRenderer> sprites = new List<SpriteRenderer> ();
		private Transform player;

		public Vector2 direction = Vector2.zero;
		public NavMeshAgent2D navMeshAgent;

		public List<Vector3> patrolPath;

		// Use this for initialization
		void Start () {
			anim = transform.GetComponent<Animator> ();
			sprites.Add (GetComponent<SpriteRenderer> ());
			sprites.AddRange (GetComponentsInChildren<SpriteRenderer> ());
			//player = CoreGame.instance.GetPlayer ().transform;

		}

		public void Initialize (Transform player) {
			this.player = player;
			FaceDirection (direction);

		}

		public void Activate () {
			//gameObject.SetActive(true);
		}

		public void Deactivate () {
			//gameObject.SetActive(false);
		}

		// Update is called once per frame
		public void UpdateBehaviour () {
			// FIXME: 
			if (anim != null) {

				if (navMeshAgent != null) {
					direction = navMeshAgent.velocity;
					FaceDirection (direction);
				}
				// else if (facePlayer) {

				// 	FaceDirection (playerDirection);
				// }

				UpdateLayer (player.position - transform.position);
			}
		}

		public void UpdateLayer (Vector2 direction) {

			if (direction.y >= 0) {
				for (int i = 0; i < sprites.Count; i++) {
					sprites[i].sortingOrder = 160 + i;
				}
			} else {
				for (int i = 0; i < sprites.Count; i++) {
					sprites[i].sortingOrder = 100 + i;
				}
			}
		}

		public void OnTriggerStay2D (Collider2D other) {

		}

		public void FaceDirection (Vector2 direction) {

			if (anim != null) {
				anim.SetFloat ("moveX", direction.x);
				anim.SetFloat ("moveY", direction.y);
			}
		}

	}
}