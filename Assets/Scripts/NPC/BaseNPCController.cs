using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace HackedDesign {
	namespace NPC {
		public class BaseNPCController : MonoBehaviour {

			protected Animator anim; //The parent animator.

			public bool facePlayer = true;

			private List<SpriteRenderer> sprites = new List<SpriteRenderer> ();
			protected Transform player;
			protected Level.Level level;

			public Vector2 direction = Vector2.zero;
			public PolyNav.PolyNavAgent polyNavAgent;

			public LayerMask layerMask;

			// public List<Vector3> patrolPath;
			// public int patrolPathLength = 4;

			protected float visibilityDistance = 3.2f;

			Color randomColor;

			// Use this for initialization
			void Start () {
				anim = transform.GetComponent<Animator> ();
				sprites.Add (GetComponent<SpriteRenderer> ());
				sprites.AddRange (GetComponentsInChildren<SpriteRenderer> ());
				//polyNavAgent = GetComponent<PolyNav.PolyNavAgent>();
				//NavMeshAgent2D navMeshAgent = GetComponent<NavMeshAgent2D> ();

				randomColor = new Color (Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f));
				//player = CoreGame.instance.GetPlayer ().transform;

			}

			public void Initialize (Level.Level level, PolyNav.PolyNav2D polyNav2D) {
				this.player = CoreGame.instance.GetPlayer ().transform;
				this.level = level;
				FaceDirection (direction);
				if (this.polyNavAgent != null && this.polyNavAgent.isActiveAndEnabled) {
					this.polyNavAgent.map = polyNav2D;
				}
			}

			public void Activate () {
				//gameObject.SetActive(true);
			}

			public void Deactivate () {
				//gameObject.SetActive(false);
			}

			public virtual void UpdateBehaviour () {
				// FIXME: This happens because this can be called before the scene finishes loading

				UpdateLayer (player.position - transform.position);
			}

			public RaycastHit2D CanSeePlayer () {
				return Physics2D.Raycast (transform.position, (player.position - transform.position), visibilityDistance, layerMask);
			}

			public Vector3 DirectionToPlayer () {
				return (player.position - transform.position);
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

			public virtual void FaceDirection (Vector2 direction) {

				if (anim != null) {
					anim.SetFloat ("directionX", direction.x);
					anim.SetFloat ("directionY", direction.y);

					if (this.polyNavAgent != null && this.polyNavAgent.currentSpeed > 0.01f) {
						anim.SetBool ("isMoving", true);
					}
				}
			}

			public enum NPCState {
				STANDING,
				PATROLLING,
				SEEKING,
				HUNTING,
				FIGHTING,
				STUNNED
			}

		}
	}
}