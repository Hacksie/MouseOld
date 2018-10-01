using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign {

	public class ClothingItem : MonoBehaviour {

		public ClothingColour colour; 

		private SpriteRenderer sprite;

		// Use this for initialization
		void Start () {
			sprite = GetComponent<SpriteRenderer>();
		}

		// Update is called once per frame
		void Update () {
			sprite.color = colour? colour.color : Color.white;
		}

		private Color RandomColor()
		{
			return Color.white;
		}
	}
}