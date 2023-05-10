using System;
using System.Collections;
using UnityEngine;

namespace Extensions
{
    public static class SoundPlayer
    {
        public static void PlaySoundThen(this MonoBehaviour behaviour, AudioSource sound, Action action)
        {
            behaviour.StartCoroutine(PlaySound(sound, action));
        }

        public static void PlaySound(this MonoBehaviour behaviour, AudioSource sound)
        {
            behaviour.StartCoroutine(PlaySound(sound, null));
        }

        private static IEnumerator PlaySound(AudioSource sound, Action action)
        {
            sound.Play();
            yield return new WaitWhile(() => sound.isPlaying);
            action?.Invoke();
        }
    }
}
