using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace he
{
    public class AudioManager : MonoBehaviour
    {

        [SerializeField] AudioSource titleMusic;
        [SerializeField] AudioSource gameMusic;


        bool audioStopped = true;

        public bool AudioStopped { get => audioStopped; set => audioStopped = value; }


        AudioSource audioSource;


        void Start()
        {
            audioSource = titleMusic; //GetComponent<AudioSource>();
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }


        private void Update()
        {
            if (AudioStopped)
            {
                return;
            }

            if (!gameMusic.isPlaying)
            {
                gameMusic.Play();
            }
        }


        [ContextMenu("AudioPlay")]
        public void MusicTest()
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }

    }
}
