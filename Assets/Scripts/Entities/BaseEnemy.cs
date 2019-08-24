using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign {
    namespace Entity {
        public class BaseEnemy : BaseEntity {

			public PolyNav.PolyNavAgent polyNavAgent;            

            public float patrolWait = 6.0f;
            public float patrolSpeed = 0.75f;
            public float huntSpeed = 1f;
            public float patrolLastCheck = 0;
            public Vector2Int currentDirection;
            public List<Vector2Int> currentDirections;

            public float huntingLastSeen = 0;
            public float seekTime = 5.0f;

            public float fightDistance = 1.5f;

            public Vector3 lastKnownLocation;

            public EnemyState state = EnemyState.PATROLLING;

            void start () {
                if (polyNavAgent == null) {
                    Debug.LogError ("Enemy without polyNavAgent set: " + this.name);
                }

                if (anim == null) {
                    Debug.LogError ("Enemy without animator set: " + this.name);
                }
            }

			public void Initialize (PolyNav.PolyNav2D polyNav2D) {
				base.Initialize();
				FaceDirection (direction);
				if (this.polyNavAgent != null && this.polyNavAgent.isActiveAndEnabled) {
					this.polyNavAgent.map = polyNav2D;
				}
			}       

			public override void FaceDirection (Vector2 direction) {

				if (anim != null) {
					anim.SetFloat ("directionX", direction.x);
					anim.SetFloat ("directionY", direction.y);

					if (this.polyNavAgent != null && this.polyNavAgent.currentSpeed > 0.01f) {
						anim.SetBool ("isMoving", true);
					}
				}
			}                 

            public override void UpdateBehaviour () {

                switch (state) {
                    case EnemyState.STANDING:
                        UpdateStanding ();
                        break;
                    case EnemyState.PATROLLING:
                        UpdatePatrolling ();
                        break;
                    case EnemyState.SEEKING:
                        UpdateSeeking ();
                        break;
                    case EnemyState.RESPONDING:
                        UpdateResponding ();
                        break;
                    case EnemyState.HUNTING:
                        UpdateHunting ();
                        break;
                    case EnemyState.FIGHTING:
                        UpdateFighting ();
                        break;
                }
            }

            public void UpdateStanding () {

                RaycastHit2D hit = CanSeePlayer ();

                if (hit.transform != null) {
                    if (hit.transform.gameObject.tag == TagManager.PLAYER) {
                        state = EnemyState.HUNTING;
                    }
                }

            }

            public void UpdatePatrolling () {

                polyNavAgent.maxSpeed = patrolSpeed;

                RaycastHit2D hit = CanSeePlayer ();

                if (hit.transform != null) {
                    //Debug.Log(hit.transform.gameObject.tag);

                    if (hit.transform.gameObject.tag == TagManager.PLAYER) {
                        state = EnemyState.HUNTING;
                        return;
                    }
                }

                if (CoreGame.Instance.CoreState.alertTrap != null) {
                    Debug.Log("Enemy is responding to alert: " + this.name);
                    state = EnemyState.RESPONDING;
                    return;
                }

                if ((Time.time - patrolLastCheck) > (patrolWait)) {

                    patrolLastCheck = Time.time;
                    // Keep patrolling
                    var location = CoreGame.Instance.CoreState.level.ConvertWorldToLevelPos (transform.position);

                    var currentDirections = CoreGame.Instance.CoreState.level.MovementDirections (location, false, false);
                    currentDirections.Randomize ();

                    if (currentDirections.Count > 0) {
                        currentDirection = currentDirections[0];

                        polyNavAgent.SetDestination (CoreGame.Instance.CoreState.level.ConvertLevelPosToWorld (currentDirection));

                    }
                    // Change this to look at the actual current direction
                    FaceDirection (CoreGame.Instance.CoreState.level.ConvertLevelPosToWorld (currentDirection) - (Vector2)transform.position);
                }

            }

            public void UpdateResponding () {
                
                RaycastHit2D hit = CanSeePlayer ();

                if (hit.transform != null) {
                    //Debug.Log(hit.transform.gameObject.tag);

                    if (hit.transform.gameObject.tag == TagManager.PLAYER) {
                        state = EnemyState.HUNTING;
                        return;
                    }
                }

                if(CoreGame.Instance.CoreState.alertTrap == null)
                {
                    Debug.Log("Alert camera cleared: " + this.name);
                    CoreGame.Instance.ClearAlert();
                    state = EnemyState.PATROLLING;
                    return;                    
                }

                if (CoreGame.Instance.CoreState.level.ConvertWorldToLevelPos(this.transform.position) == CoreGame.Instance.CoreState.level.ConvertWorldToLevelPos(CoreGame.Instance.CoreState.alertTrap.transform.position) ) {
                    Debug.Log("Enemy is clearing alert: " + this.name);
                    CoreGame.Instance.ClearAlert();
                    state = EnemyState.PATROLLING;
                    return;
                }

                if (CoreGame.Instance.CoreState.alertTrap != null) {

                    Vector3 pos = CoreGame.Instance.CoreState.alertTrap.transform.position;
                    if (polyNavAgent.primeGoal != new Vector2 (pos.x, pos.y)) {
                        polyNavAgent.SetDestination (pos);

                    }

                    FaceDirection (pos - transform.position);
                } else {

                }

            }

            public void UpdateHunting () {

                if (polyNavAgent != null) {

                    polyNavAgent.maxSpeed = huntSpeed;

                    if ((player.position - transform.position).magnitude < fightDistance) {
                        state = EnemyState.FIGHTING;
                        return;
                    }

                    RaycastHit2D hit = CanSeePlayer ();

                    if (hit.transform != null) {

                        if (hit.transform.gameObject.tag == TagManager.PLAYER) {
                            huntingLastSeen = Time.time;
                            lastKnownLocation = player.position;
                            polyNavAgent.SetDestination (lastKnownLocation);

                            // Change this to look at the actual current direction
                            FaceDirection (lastKnownLocation - transform.position);
                            return;
                        } else {
                            state = EnemyState.SEEKING;
                            return;
                        }
                    }
                }

                //FaceDirection (player.position - transform.position);

            }

            public void UpdateSeeking () {
                if (polyNavAgent != null) {

                    if ((Time.time - huntingLastSeen) > seekTime) {
                        state = EnemyState.PATROLLING;
                        return;
                    }

                    RaycastHit2D hit = CanSeePlayer ();

                    if (hit.transform != null && hit.transform.gameObject.tag == TagManager.PLAYER) {
                        state = EnemyState.HUNTING;
                        return;
                    }
                }
            }

            public void UpdateFighting () {
                //FaceDirection (player.position - transform.position);
                CoreGame.Instance.GameOver ();
            }

            public void OnCollisionEnter2D (Collision2D collision) {
                if (collision.gameObject.tag == TagManager.PLAYER) {
                    state = EnemyState.FIGHTING;
                    return;
                }

            }

            public void OnDrawGizmos () {
                // if (CoreGame.instance.state == GameState.PLAYING && this.player) {
                //     RaycastHit2D hit = CanSeePlayer ();

                //     Color debugColor = Color.white;
                //     if (hit.transform != null) {
                //         if (hit.transform.gameObject.tag == TagManager.PLAYER) {
                //             debugColor = Color.red;
                //         } else {
                //             debugColor = Color.yellow;
                //         }
                //     }

                //     Debug.DrawRay (transform.position, (player.position - transform.position), debugColor);

                //     foreach (var d in currentDirections) {
                //         Debug.DrawLine (transform.position, level.ConvertLevelPosToWorld (d), Color.magenta);
                //     }
                // }

            }

            public enum EnemyState {
                STANDING,
                PATROLLING,
                SEEKING,
                RESPONDING,
                HUNTING,
                FIGHTING,
                STUNNED
            }
        }
    }
}