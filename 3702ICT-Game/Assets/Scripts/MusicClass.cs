using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
 public class MusicClass : MonoBehaviour
 {
     private AudioSource _audioSource;
     private void Awake()
     {
         _audioSource = GetComponent<AudioSource>();
        DontDestroyOnLoad(_audioSource);
     }
 
     public void PlayMusic()
     {
        if (!_audioSource.isPlaying){
            _audioSource.Play();
        } else {
            Destroy(gameObject);
        }
     }
 
     public void StopMusic()
     {
         _audioSource.Stop();
     }
 }
