using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityUtilities;

public class AudioManager : SingletonMono<AudioManager> {

    public AudioClip musicIntro;
    public AudioClip musicMario;
    public AudioClip musicZelda;
    public AudioClip musicFinal;
    public AudioClip[] audioClips;
    
    private AudioSource audioSourceSFX;
    private AudioSource audioSourceMusic;
    
    
    public void PlayClip(AudioClip audioClip) {
        if(audioClip==null) {
            Debug.LogError("Missing audioclip: " + audioClip);
        }
        else {
            audioSourceSFX.PlayOneShot(audioClip);
        }
    }

    public void PlayMusic(AudioClip music) {
        if(music==null) {
            Debug.LogError("Missing music: " + music);
        }
        else {
            audioSourceMusic.clip = music;
            audioSourceMusic.Play();
        }
    }
}
