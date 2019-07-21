using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign {
    namespace NPC {
        public class BaseEnemy : BaseNPCController {

            public int patrolSpeed = 4;
            public float patrolLastCheck = 0;
            public Vector2Int currentDirection;
            public List<Vector2Int> currentDirections;

            public float huntingLastSeen = 0;
            public float seekTime = 5.0f;

            public float fightDistance = 1.5f;

            public Vector3 lastKnownLocation;

            public EnemyState state = EnemyState.PATROLLING;

            public override void UpdateBehaviour () {
                if (anim == null) {
                    return;
                }

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
                    case EnemyState.HUNTING:
                        UpdateHunting ();
                        break;
                    case EnemyState.FIGHTING:
                        UpdateFighting ();
                        break;
                }
            }

            public void UpdateStanding () {
                if (navMeshAgent != null) {

                    RaycastHit2D hit = CanSeePlayer ();

                    if (hit.transform != null) {
                        if (hit.transform.gameObject.tag == TagManager.PLAYER) {
                            state = EnemyState.HUNTING;
                        }
                    }
                }
            }

            public void UpdatePatrolling () {

                if (navMeshAgent != null) {

                    RaycastHit2D hit = CanSeePlayer ();

                    if (hit.transform != null) {
                        //Debug.Log(hit.transform.gameObject.tag);

                        if (hit.transform.gameObject.tag == TagManager.PLAYER) {
                            state = EnemyState.HUNTING;
                            return;
                        }
                    }

                    if (navMeshAgent.remainingDistance < 0.01f || ((Time.time - patrolLastCheck) > patrolSpeed)) {
                        patrolLastCheck = Time.time;
                        // Keep patrolling
                        var location = level.ConvertWorldToLevelPos (transform.position);

                        currentDirections = level.MovementDirections (location, false, false);
                        currentDirections.Randomize ();

                        if (currentDirections.Count > 0) {
                            currentDirection = currentDirections[0];
                            navMeshAgent.SetDestination (level.ConvertLevelPosToWorld (currentDirection));
                        }
                        FaceDirection (level.ConvertLevelPosToWorld (currentDirection) - transform.position);
                    }
                }
            }

            public void UpdateHunting () {

                if (navMeshAgent != null) {

                    if ((player.position - transform.position).magnitude < fightDistance) {
                        state = EnemyState.FIGHTING;
                        return;
                    }

                    RaycastHit2D hit = CanSeePlayer ();

                    if (hit.transform != null) {

                        if (hit.transform.gameObject.tag == TagManager.PLAYER) {
                            huntingLastSeen = Time.time;
                            lastKnownLocation = player.position;
                            navMeshAgent.SetDestination (lastKnownLocation);

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
                if (navMeshAgent != null) {

                    if ((Time.time - huntingLastSeen) > seekTime) {
                        state = EnemyState.PATROLLING;
                        return;
                    }

                    RaycastHit2D hit = CanSeePlayer ();

                    if (hit.transform.gameObject.tag == TagManager.PLAYER) {
                        state = EnemyState.HUNTING;
                        return;
                    }
                }
            }

            public void UpdateFighting () {
                //FaceDirection (player.position - transform.position);
                game.GameOver ();

            }

            public void OnCollisionEnter2D (Collision2D collision) {
                if (collision.gameObject.tag == "Player") {
                    state = EnemyState.FIGHTING;
                    return;
                }

            }

            public void OnDrawGizmos () {
                if (Application.isPlaying) {
                    RaycastHit2D hit = CanSeePlayer ();

                    Color debugColor = Color.white;
                    if (hit.transform != null) {
                        if (hit.transform.gameObject.tag == TagManager.PLAYER) {
                            debugColor = Color.red;
                        } else {
                            debugColor = Color.yellow;
                        }
                    }

                    Debug.DrawRay (transform.position, (player.position - transform.position), debugColor);

                    foreach (var d in currentDirections) {
                        Debug.DrawLine (transform.position, level.ConvertLevelPosToWorld (d), Color.magenta);
                    }
                }

            }

            public enum EnemyState {
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