using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.UIElements;

namespace HackedDesign
{
    public class ShadowPerf : MonoBehaviour
    {
        private ShadowCaster2D shadow;
        private Vector3 position;
        private Transform player;
        [SerializeField] private float disableDistance = 10.0f;
        // Start is called before the first frame update
        void Start()
        {
            shadow = GetComponent<ShadowCaster2D>();
            position = transform.position + new Vector3(3, 3);
            player = CoreGame.Instance.GetPlayer().transform;
        }

        // Update is called once per frame
        void Update()
        {
            shadow.castsShadows = (player.position - position).sqrMagnitude < (disableDistance * disableDistance);

        }
    }
}
