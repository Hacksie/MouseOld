using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private AudioSource sfx;
        [SerializeField] private AudioSource music;
        [SerializeField] private AudioClip select;
        [SerializeField] private AudioClip accept;

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