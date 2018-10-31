﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign {
	namespace Triggers {
		public class TriggerFeedback : MonoBehaviour {

			public SpriteRenderer sprite;

			private void Start()
			{
				if(sprite != null && sprite.gameObject.activeInHierarchy)
				{
					sprite.gameObject.SetActive(false);
				}				
			}


			private void OnTriggerStay2D (Collider2D other) {
				if(sprite != null && !sprite.gameObject.activeInHierarchy)
				{
					sprite.gameObject.SetActive(true);
				}
			}

			private void OnTriggerExit2D (Collider2D other) {
				if(sprite != null && sprite.gameObject.activeInHierarchy)
				{
					sprite.gameObject.SetActive(false);
				}
			}			
		}

	}
}