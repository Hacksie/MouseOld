using HackedDesign.Triggers;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace HackedDesign {

	[RequireComponent(typeof(Animator))]
	public class PlayerController : MonoBehaviour 
	{
		[Range (0.0f, 10.0f)]
		[Tooltip ("The movement speed of the controller.")]
		[SerializeField] private float baseMovementSpeed = 1.5f;
		[SerializeField] private float dashDistance = 2.5f;
		[SerializeField] private float dashCooldown = 5.0f;
		[SerializeField] private float dashTimeToComplete = 0.2f;

		private Vector2 movementVector; 
		private Animator anim;
		private bool dash = false;
		
		private float dashTimer = 0;

		public bool IsDashing
		{
			get
			{
				return dash;
			}
		}
		
		public float DashPercentageComplete
		{
			get
			{
				return (Time.time - dashTimer > dashCooldown) ? 1 : (Time.time - dashTimer) / dashCooldown;
			}
		}


		private readonly List<ITrigger> triggers = new List<ITrigger>();


		private void Start () 
		{
			anim = transform.GetComponent<Animator> ();
		}

		public void MovementEvent (InputAction.CallbackContext context) 
		{
			movementVector = context.ReadValue<Vector2>();
		}

		public void InteractEvent(InputAction.CallbackContext context)
		{
			if (CoreGame.Instance.state.state == GameState.GameStateEnum.PLAYING)
			{
				if (context.performed)
				{
					foreach (var trigger in triggers)
					{
						trigger.Invoke(gameObject);
					}
				}
			}
		}

		public void DashEvent(InputAction.CallbackContext context)
		{
			if (CoreGame.Instance.state.state == GameState.GameStateEnum.PLAYING)
			{
				if (context.performed)
				{
					if ((Time.time - dashTimer) > dashCooldown)
					{
						dash = true;
						dashTimer = Time.time;
					}
				}
			}
		}

		public void BugEvent(InputAction.CallbackContext context)
		{
			if (CoreGame.Instance.state.state == GameState.GameStateEnum.PLAYING)
			{
				if (context.performed)
				{
					foreach (var trigger in triggers)
					{
						trigger.Bug(gameObject);
					}
				}
			}
		}

		public void HackEvent(InputAction.CallbackContext context)
		{
			if (CoreGame.Instance.state.state == GameState.GameStateEnum.PLAYING)
			{
				if (context.performed)
				{
					foreach (var trigger in triggers)
					{
						trigger.Hack(gameObject);
					}
				}
			}
		}

		public void OverloadEvent(InputAction.CallbackContext context)
		{
			if (CoreGame.Instance.state.state == GameState.GameStateEnum.PLAYING)
			{
				if (context.performed)
				{
					foreach (var trigger in triggers)
					{
						trigger.Overload(gameObject);
					}
				}
			}
		}

		public void StartEvent(InputAction.CallbackContext context)
		{
			if (context.performed)
			{
				CoreGame.Instance.ToggleStart();
			}
		}

		public void SelectEvent(InputAction.CallbackContext context)
		{
			if (context.performed)
			{
				CoreGame.Instance.ToggleSelect();
			}
		}

		public void RegisterTrigger(ITrigger trigger)
		{
			if(!triggers.Contains(trigger))
			{
				triggers.Add(trigger);
			}
		}

		public void UnregisterTrigger(ITrigger trigger)
		{
			if (triggers.Contains(trigger))
			{
				triggers.Remove(trigger);
			}
		}

		public void Update()
		{
			UpdateTransform();
		}

		public void LateUpdate()
		{
			if (movementVector.sqrMagnitude > Vector2.kEpsilon)
			{
				//anim.SetFloat ("moveX", movementVector.x);
				//anim.SetFloat ("moveY", movementVector.y);
				anim.SetFloat("directionX", movementVector.x);
				anim.SetFloat("directionY", movementVector.y);
				anim.SetBool("isMoving", true);
			}
			else
			{
				movementVector = Vector2.zero;
				if (anim != null)
				{
					anim.SetBool("isMoving", false);
				}
			}
		}

		public void UpdateTransform () {
			// Movement augments (0 - 10) are reduced by a factor of 10
			if(dash)
			{
				transform.Translate(movementVector * dashDistance * Time.deltaTime); 
				//performedDashDistance += dashDistance * Time.deltaTime;
				if ((Time.time - dashTimer)>  dashTimeToComplete)
				{
					dash = false;
				}
			}

			transform.Translate (movementVector * (baseMovementSpeed + (CoreGame.Instance.state.player.movementAugments / 10.0f)) * Time.deltaTime);
		}
	}
}