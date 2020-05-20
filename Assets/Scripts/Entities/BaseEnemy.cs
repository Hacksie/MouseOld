using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign
{
    namespace Entities
    {
        public class BaseEnemy : MonoBehaviour
        {
            [Header("Game Objects")]
            protected Animator anim;
            public PolyNav.PolyNavAgent polyNavAgent;
            public Transform alert;

            [Header("Settings")]
            public LayerMask playerLayerMask;
            public string enemy = "";
            public EnemyState state = EnemyState.STANDING;
            public float patrolWait = 6.0f;
            public float patrolSpeed = 0.75f;
            public float huntSpeed = 1f;
            public bool hostile = true;

            [Header("State")]
            public Vector2Int direction = Vector2Int.zero;
            public float patrolLastCheck = 0;
            public Vector2Int currentDirection;
            public List<Vector2Int> currentDirections;


            public float huntingLastSeen = 0;
            public float seekTime = 5.0f;

            public float fightDistance = 1.5f;

            public Vector3 lastKnownLocation;

            protected Transform player;

            protected float visibilityDistance = 3.2f;


            protected void Start()
            {
                anim = transform.GetComponent<Animator>();
                if (anim == null)
                {
                    Logger.LogError(this.name, "Enemy without animation set");
                }
                if (polyNavAgent == null)
                {
                    Logger.LogError(this.name, "Enemy without polyNavAgent set");
                }
            }

            public void Initialize(PolyNav.PolyNav2D polyNav2D, PlayerController player)
            {
                this.player = player.transform;


                //base.Initialize();
                FaceDirection(direction);
                if (this.polyNavAgent != null && this.polyNavAgent.isActiveAndEnabled)
                {
                    this.polyNavAgent.map = polyNav2D;
                }
            }

            public void Animate()
            {
                if (anim != null)
                {
                    if (this.polyNavAgent != null && this.polyNavAgent.currentSpeed > 0.01f)
                    {
                        anim.SetFloat("moveX", this.polyNavAgent.movingDirection.x);
                        anim.SetFloat("moveY", this.polyNavAgent.movingDirection.y);
                        anim.SetBool("isMoving", true);
                    }
                    else
                    {
                        anim.SetFloat("moveX", 0);
                        anim.SetFloat("moveY", 0);
                        anim.SetBool("isMoving", false);
                    }
                }
            }

            public void FaceDirection(Vector2 direction)
            {
                this.direction = NormaliseDirectionVector(direction);
            }

            protected Vector2Int NormaliseDirectionVector(Vector2 direction)
            {
                return Vector2Int.RoundToInt(direction.normalized);
            }

            public RaycastHit2D CanSeePlayer()
            {
                return Physics2D.Raycast(transform.position, (player.position - transform.position), visibilityDistance, playerLayerMask);
            }

            public void UpdateAlertLight(Vector2Int direction)
            {
                if (direction == Vector2Int.zero)
                {
                    alert.rotation = Quaternion.Euler(0, 0, 180);
                }
                if (direction == Vector2Int.down)
                {
                    alert.rotation = Quaternion.Euler(0, 0, 180);
                }
                if (direction == Vector2Int.up)
                {
                    alert.rotation = Quaternion.Euler(0, 0, 0);
                }
                if (direction == Vector2Int.left)
                {
                    alert.rotation = Quaternion.Euler(0, 0, 90);
                }
                if (direction == Vector2Int.right)
                {
                    alert.rotation = Quaternion.Euler(0, 0, 270);
                }
            }

            public void UpdateBehaviour()
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

                Animate();
            }

            public void UpdateStanding()
            {

                RaycastHit2D hit = CanSeePlayer();

                if (hit.transform != null)
                {
                    if (hit.transform.gameObject.CompareTag(TagManager.PLAYER))
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
                    if (hostile && hit.transform.gameObject.CompareTag(TagManager.PLAYER))
                    {
                        state = EnemyState.HUNTING;
                        return;
                    }
                }

                if (GameManager.Instance.state.alertTrap != null)
                {
                    Logger.Log(name, "Enemy is responding to alert");
                    state = EnemyState.RESPONDING;
                    return;
                }

                if ((Time.time - patrolLastCheck) > (patrolWait))
                {

                    patrolLastCheck = Time.time;
                    // Keep patrolling
                    var location = GameManager.Instance.state.currentLevel.ConvertWorldToLevelPos(transform.position);

                    var currentDirections = GameManager.Instance.state.currentLevel.MovementDirections(location, false, false);
                    currentDirections.Randomize();

                    if (currentDirections.Count > 0)
                    {
                        currentDirection = currentDirections[0];

                        polyNavAgent.SetDestination(GameManager.Instance.state.currentLevel.ConvertLevelPosToWorld(currentDirection));

                    }
                    // Change this to look at the actual current direction
                    FaceDirection(GameManager.Instance.state.currentLevel.ConvertLevelPosToWorld(currentDirection) - (Vector2)transform.position);
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

                if (GameManager.Instance.state.alertTrap == null)
                {
                    Debug.Log("Alert camera cleared: " + this.name);
                    //CoreGame.Instance.ClearAlert();
                    state = EnemyState.PATROLLING;
                    return;
                }

                if (GameManager.Instance.state.currentLevel.ConvertWorldToLevelPos(transform.position) == GameManager.Instance.state.currentLevel.ConvertWorldToLevelPos(GameManager.Instance.state.alertTrap.transform.position))
                {
                    Debug.Log("Enemy is clearing alert: " + this.name);
                    //CoreGame.Instance.ClearAlert();
                    state = EnemyState.PATROLLING;
                    return;
                }

                if (GameManager.Instance.state.alertTrap != null)
                {

                    Vector3 pos = GameManager.Instance.state.alertTrap.transform.position;
                    if (polyNavAgent.primeGoal != new Vector2(pos.x, pos.y))
                    {
                        polyNavAgent.SetDestination(pos);

                    }

                    FaceDirection(pos - transform.position);
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
                    GameManager.Instance.GameOver();
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