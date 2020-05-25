using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private AudioSource sfx = null;
        [SerializeField] private AudioSource music = null;
        [SerializeField] private AudioClip select = null;
        [SerializeField] private AudioClip accept = null;

        public void PlayAccept()
        {
            sfx.clip = accept;
            sfx.Play();
        }

        public void PlaySelect()
        {
            sfx.clip = select;
            sfx.Play();
        }
    }
}