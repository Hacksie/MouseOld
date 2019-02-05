using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace HackedDesign {
    namespace NPC {
        public class BaseEnemy : BaseNPCController {

            EnemyState state = EnemyState.STANDING;

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
                        if (hit.transform.gameObject.tag == TagManager.PLAYER) {
                            state = EnemyState.HUNTING;

                        }
                    }
                }
            }

            public void UpdateSeeking () {

            }

            public void UpdateHunting () {
                FaceDirection (player.position - transform.position);

            }

            public void UpdateFighting () {
                FaceDirection (player.position - transform.position);

            }

            public void OnDrawGizmosSelected () {
                RaycastHit2D hit = CanSeePlayer ();

                Color debugColor = Color.white;
                if (hit.transform != null) {
                    if (hit.transform.gameObject.tag == TagManager.PLAYER) {
                        debugColor = Color.red;
                    } else {
                        debugColor = Color.yellow;
                    }
                }

                Debug.DrawLine (transform.position, player.transform.position, debugColor, 0.1f);
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