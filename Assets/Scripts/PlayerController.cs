
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


namespace HackedDesign
{

    [RequireComponent(typeof(Animator))]
    public class PlayerController : MonoBehaviour
    {
        [Range(0.0f, 10.0f)]
        [Tooltip("The movement speed of the controller.")]
        [SerializeField] private float baseMovementSpeed = 1.0f;
        [SerializeField] private float dashDistance = 2.5f;
        [SerializeField] private float dashCooldown = 5.0f;
        [SerializeField] private float dashTimeToComplete = 0.2f;
        [SerializeField] private float augmentFactor = 0.1f;
        [SerializeField] private bool isHacking = false;
        [SerializeField] private HackBar hackBar;
        [SerializeField] private float hackSpeed = 3.0f;

        private float hackTimer = 0;


        private Vector2 movementVector;
        private Animator anim;
        private float dashTimer = 0;

        private int directionXAnimId;
        private int directionYAnimId;
        private int isMovingAnimId;

        public bool IsDashing { get; private set; } = false;

        public float DashPercentageComplete
        {
            get
            {
                return (Time.time - dashTimer > dashCooldown) ? 1 : (Time.time - dashTimer) / dashCooldown;
            }
        }

        private readonly List<BaseTrigger> triggers = new List<BaseTrigger>();

        private void Awake()
        {
            anim = transform.GetComponent<Animator>();
            directionXAnimId = Animator.StringToHash("directionX");
            directionYAnimId = Animator.StringToHash("directionY");
            isMovingAnimId = Animator.StringToHash("isMoving");
        }


        public void MovementEvent(InputAction.CallbackContext context)
        {
            movementVector = context.ReadValue<Vector2>();

            isHacking = false;
        }

        public void InteractEvent(InputAction.CallbackContext context)
        {
            if (GameManager.Instance.CurrentState.PlayerActionAllowed)
            {
                if (context.performed)
                {
                    foreach (var trigger in triggers)
                    {
                        trigger.Invoke(gameObject);
                    }
                }
            }
            isHacking = false;
        }

        public void DashEvent(InputAction.CallbackContext context)
        {
            if (GameManager.Instance.CurrentState.PlayerActionAllowed && context.performed && (Time.time - dashTimer) > dashCooldown)
            {
                IsDashing = true;
                dashTimer = Time.time;
            }
            isHacking = false;
        }

        public void BugEvent(InputAction.CallbackContext context)
        {
            if (!GameManager.Instance.CurrentState.PlayerActionAllowed || !context.performed)
            {
                return;
            }

            for (int i = 0; i < triggers.Count; i++)
            {
                triggers[i].Bug(gameObject);
            }
            isHacking = false;
        }

        public void HackEvent(InputAction.CallbackContext context)
        {
            if (!GameManager.Instance.CurrentState.PlayerActionAllowed || !context.performed)
            {
                return;
            }



            for (int i = 0; i < triggers.Count; i++)
            {
                if (triggers[i].allowHack)
                {
                    isHacking = true;
                    hackTimer = Time.time;
                }
                //triggers[i].Hack(gameObject);
            }
        }

        public void OverloadEvent(InputAction.CallbackContext context)
        {
            if (!GameManager.Instance.CurrentState.PlayerActionAllowed || !context.performed)
            {
                return;
            }

            for (int i = 0; i < triggers.Count; i++)
            {
                triggers[i].Overload(gameObject);
            }
            isHacking = false;
        }

        public void StartEvent(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                GameManager.Instance.ToggleStart();
            }
            isHacking = false;
        }

        public void SelectEvent(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                GameManager.Instance.ToggleSelect();
                isHacking = false;
            }

        }

        public void RegisterTrigger(BaseTrigger trigger)
        {
            if (triggers.Contains(trigger))
            {
                return;
            }
            triggers.Add(trigger);
        }

        public void UnregisterTrigger(BaseTrigger trigger)
        {
            if (!triggers.Contains(trigger))
            {
                return;
            }
            triggers.Remove(trigger);
        }

        public void Animate()
        {
            if (anim == null)
                return;

            if (movementVector.sqrMagnitude > Vector2.kEpsilon)
            {
                anim.SetFloat(directionXAnimId, movementVector.x);
                anim.SetFloat(directionYAnimId, movementVector.y);
                anim.SetBool(isMovingAnimId, true);
            }
            else
            {
                anim.SetBool(isMovingAnimId, false);
            }
        }

        public void UpdateBehaviour()
        {
            UpdateHacking();
            if (IsDashing)
            {
                transform.Translate(movementVector * dashDistance * Time.deltaTime);
                if ((Time.time - dashTimer) > dashTimeToComplete)
                {
                    IsDashing = false;
                }
            }

            if (movementVector.sqrMagnitude <= Vector2.kEpsilon)
            {
                movementVector = Vector2.zero;
            }

            transform.Translate(movementVector * (baseMovementSpeed + (GameManager.Instance.GameState.Player.movementAugments * augmentFactor)) * Time.deltaTime);
        }

        public void UpdateHacking()
        {
            if (isHacking)
            {
                var hackValue = (Time.time - hackTimer) / hackSpeed;

                if (hackValue < 1)
                {
                    hackBar.Show();
                    hackBar.UpdateBar(hackValue);
                }
                else
                {
                    isHacking = false;
                    for (int i = 0; i < triggers.Count; i++)
                    {
                        triggers[i].Hack(gameObject);
                    }
                }
            }
            else
            {
                hackBar.Hide();
            }
        }

        public void Move(Vector2 position)
        {
            transform.position = position;
        }

        public void Show() => gameObject.SetActive(true);
        public void Hide() => gameObject.SetActive(false);
    }
}