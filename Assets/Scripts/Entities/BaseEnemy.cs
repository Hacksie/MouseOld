using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign
{
    namespace Entity
    {
        public class BaseEnemy : BaseEntity
        {
            [Header("Game Objects")]
            public PolyNav.PolyNavAgent polyNavAgent;
            public Transform alertLight;

            [Header("Settings")]
            public EnemyState state = EnemyState.STANDING;
            public float patrolWait = 6.0f;
            public float patrolSpeed = 0.75f;
            public float huntSpeed = 1f;
            public bool hostile = true;

            [Header("State")]
            public float patrolLastCheck = 0;
            public Vector2Int currentDirection;
            public List<Vector2Int> currentDirections;

            



            public float huntingLastSeen = 0;
            public float seekTime = 5.0f;

            public float fightDistance = 1.5f;

            public Vector3 lastKnownLocation;



            void start()
            {
                if (polyNavAgent == null)
                {
                    Debug.LogError("Enemy without polyNavAgent set: " + this.name);
                }

                if (anim == null)
                {
                    Debug.LogError("Enemy without animator set: " + this.name);
                }
            }

            public void Initialize(PolyNav.PolyNav2D polyNav2D)
            {
                base.Initialize();
                FaceDirection(direction);
                if (this.polyNavAgent != null && this.polyNavAgent.isActiveAndEnabled)
                {
                    this.polyNavAgent.map = polyNav2D;
                }
            }

            public override void FaceDirection(Vector2 direction)
            {

                this.direction = NormaliseDirectionVector(direction);
                UpdateAlertLight(this.direction);

                if (anim != null)
                {
                    anim.SetFloat("moveX", this.direction.x);
                    anim.SetFloat("moveY", this.direction.y);

                    if (this.polyNavAgent != null && this.polyNavAgent.currentSpeed > 0.01f)
                    {
                        anim.SetBool("isMoving", true);
                    }
                }
            }

            public void UpdateAlertLight(Vector2Int direction)
            {
                if (direction == Vector2Int.zero)
                {
                    alertLight.rotation = Quaternion.Euler(0, 0, 180);
                }
                if (direction == Vector2Int.down)
                {
                    alertLight.rotation = Quaternion.Euler(0, 0, 180);
                }
                if (direction == Vector2Int.up)
                {
                    alertLight.rotation = Quaternion.Euler(0, 0, 0);
                }
                if (direction == Vector2Int.left)
                {
                    alertLight.rotation = Quaternion.Euler(0, 0, 90);
                }
                if (direction == Vector2Int.right)
                {
                    alertLight.rotation = Quaternion.Euler(0, 0, 270);
                }
            }

            public override void UpdateBehaviour()
            {

                switch (state)
                {
                    case EnemyState.STANDING:
                        UpdateStanding();
                        break;
                    case EnemyState.PATROLLING:
                        UpdatePatrolling();
                        break;
                    case EnemyState.SEEKING:
                        UpdateSeeking();
                        break;
                    case EnemyState.RESPONDING:
                        UpdateResponding();
                        break;
                    case EnemyState.HUNTING:
                        UpdateHunting();
                        break;
                    case EnemyState.FIGHTING:
                        UpdateFighting();
                        break;
                }
            }

            public void UpdateStanding()
            {

                RaycastHit2D hit = CanSeePlayer();

                if (hit.transform != null)
                {
                    if (hit.transform.gameObject.tag == TagManager.PLAYER)
                    {
                        state = EnemyState.HUNTING;
                    }
                }

            }

            public void UpdatePatrolling()
            {

                polyNavAgent.maxSpeed = patrolSpeed;

                RaycastHit2D hit = CanSeePlayer();

                if (hit.transform != null)
                {
                    //Debug.Log(hit.transform.gameObject.tag);

                    if (hostile && hit.transform.gameObject.CompareTag(TagManager.PLAYER))
                    {
                        state = EnemyState.HUNTING;
                        return;
                    }
                }

                if (CoreGame.Instance.State.alertTrap != null)
                {
                    Debug.Log("Enemy is responding to alert: " + this.name);
                    state = EnemyState.RESPONDING;
                    return;
                }

                if ((Time.time - patrolLastCheck) > (patrolWait))
                {

                    patrolLastCheck = Time.time;
                    // Keep patrolling
                    var location = CoreGame.Instance.State.currentLevel.ConvertWorldToLevelPos(transform.position);

                    var currentDirections = CoreGame.Instance.State.currentLevel.MovementDirections(location, false, false);
                    currentDirections.Randomize();

                    if (currentDirections.Count > 0)
                    {
                        currentDirection = currentDirections[0];

                        polyNavAgent.SetDestination(CoreGame.Instance.State.currentLevel.ConvertLevelPosToWorld(currentDirection));

                    }
                    // Change this to look at the actual current direction
                    FaceDirection(CoreGame.Instance.State.currentLevel.ConvertLevelPosToWorld(currentDirection) - (Vector2)transform.position);
                }

            }

            public void UpdateResponding()
            {

                RaycastHit2D hit = CanSeePlayer();

                if (hit.transform != null)
                {
                    //Debug.Log(hit.transform.gameObject.tag);

                    if (hostile && hit.transform.gameObject.CompareTag(TagManager.PLAYER))
                    {
                        state = EnemyState.HUNTING;
                        return;
                    }
                }

                if (CoreGame.Instance.State.alertTrap == null)
                {
                    Debug.Log("Alert camera cleared: " + this.name);
                    CoreGame.Instance.ClearAlert();
                    state = EnemyState.PATROLLING;
                    return;
                }

                if (CoreGame.Instance.State.currentLevel.ConvertWorldToLevelPos(this.transform.position) == CoreGame.Instance.State.currentLevel.ConvertWorldToLevelPos(CoreGame.Instance.State.alertTrap.transform.position))
                {
                    Debug.Log("Enemy is clearing alert: " + this.name);
                    CoreGame.Instance.ClearAlert();
                    state = EnemyState.PATROLLING;
                    return;
                }

                if (CoreGame.Instance.State.alertTrap != null)
                {

                    Vector3 pos = CoreGame.Instance.State.alertTrap.transform.position;
                    if (polyNavAgent.primeGoal != new Vector2(pos.x, pos.y))
                    {
                        polyNavAgent.SetDestination(pos);

                    }

                    FaceDirection(pos - transform.position);
                }
                else
                {

                }

            }

            public void UpdateHunting()
            {

                if (polyNavAgent != null)
                {

                    polyNavAgent.maxSpeed = huntSpeed;

                    if ((player.position - transform.position).magnitude < fightDistance)
                    {
                        state = EnemyState.FIGHTING;
                        return;
                    }

                    RaycastHit2D hit = CanSeePlayer();

                    if (hit.transform != null)
                    {

                        if (hit.transform.gameObject.CompareTag(TagManager.PLAYER))
                        {
                            huntingLastSeen = Time.time;
                            lastKnownLocation = player.position;
                            polyNavAgent.SetDestination(lastKnownLocation);

                            // Change this to look at the actual current direction
                            FaceDirection(lastKnownLocation - transform.position);
                            return;
                        }
                        else
                        {
                            state = EnemyState.SEEKING;
                            return;
                        }
                    }
                }

                //FaceDirection (player.position - transform.position);

            }

            public void UpdateSeeking()
            {
                if (polyNavAgent != null)
                {

                    if ((Time.time - huntingLastSeen) > seekTime)
                    {
                        state = EnemyState.PATROLLING;
                        return;
                    }

                    RaycastHit2D hit = CanSeePlayer();

                    if (hit.transform != null && hit.transform.gameObject.CompareTag(TagManager.PLAYER))
                    {
                        state = EnemyState.HUNTING;
                        return;
                    }
                }
            }

            public void UpdateFighting()
            {
                //FaceDirection (player.position - transform.position);
                if (hostile)
                {
                    CoreGame.Instance.GameOver();
                }
            }

            public void OnCollisionEnter2D(Collision2D collision)
            {
                if (collision.gameObject.CompareTag(TagManager.PLAYER))
                {
                    state = EnemyState.FIGHTING;
                    return;
                }

            }

            public enum EnemyState
            {
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