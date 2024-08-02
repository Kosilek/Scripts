using DredPack;
using System;
using System.Collections;
using UnityEngine;

namespace Kosilek.Manager
{
    public class AudioManager2248 : SimpleSingleton<AudioManager2248>
    {
        #region Main Value
        public bool isMusic;
        public bool isSound;
        public bool isVibration;
        #endregion

        #region AudioSource
        [SerializeField] private AudioSource mainSource;
        [SerializeField] private AudioSource effectSource;
        #endregion

        #region ScriptsComponent
        public AudioContainer audioContainer;
        #endregion

        #region Animatiom
        [SerializeField] private float moveSped;
        [SerializeField] private AnimationCurve animCurve;
        #endregion

        private float maxMusic;
        private float maxSound;

        private void Awake()
        {
            maxMusic = mainSource.volume;
            maxSound = effectSource.volume;
        }

        public void StartMainClip(bool isMenu)
        {
            if (!isMusic)
                return;
            Action action = isMenu ? StartClipMenu : StartClipGame;
            action.Invoke();
        }

        public void ActiveMusic()
        {
            var volume = isMusic ? maxMusic : 0f;
            mainSource.volume = volume;
        }

        public void ActiveSound()
        {
            var volume = isSound ? maxSound : 0f;
           // effectSource.volume = volume;
        }

        private void StartClipMenu()
        {
            mainSource.Stop();
            mainSource.clip = audioContainer.menuClip;
            SmoothTransitionMusic(mainSource);
            mainSource.Play();
        }

        public void StartClipMenuStartGame()
        {
            mainSource.Stop();
            mainSource.clip = audioContainer.menuClip;
            mainSource.Play();
        }

        private void StartClipGame()
        {
            mainSource.Stop();
            mainSource.clip = audioContainer.gameClip;
            SmoothTransitionMusic(mainSource);
            mainSource.Play();
        }

        public void StartEffectClip(AudioClip clip)
        {
           // Debug.Log("StartEffectClip");
            if (!isSound)
                return;
            effectSource.Stop();
            effectSource.clip = null;
            effectSource.clip = clip;
            effectSource.Play();
        }

        public void StartEffectClip(AudioClip clip, bool isActive)
        {
          //  Debug.Log("isActive = " + isActive);
            if (!isActive)
                return;
            effectSource.Stop();
            effectSource.clip = null;
            effectSource.clip = clip;
            effectSource.Play();
        }

        private void SmoothTransitionMusic(AudioSource audioSource)
        {
            var maxVolume = audioSource.volume;

            StartCoroutine(IE());

            IEnumerator IE()
            {
                StartCoroutine(CustomIenumerator.IEImageAlphaCor(audioSource.volume, 0f, moveSped, animCurve, _ => audioSource.volume = _));
                yield return new WaitForSeconds(0.75f);
                yield return StartCoroutine(CustomIenumerator.IEImageAlphaCor(audioSource.volume, maxVolume, moveSped, animCurve, _ => audioSource.volume = _));

            }
        }
    }
}