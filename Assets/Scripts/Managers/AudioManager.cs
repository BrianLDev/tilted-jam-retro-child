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
    [SerializeField]
    private AudioSource audioSourceSFX;
    [SerializeField]
    private AudioSource audioSourceMusic;
    
    public void PlayRandomClip(AudioClip[] clips) {
        if(clips.Length==0) {Debug.Log("empty clips");return;}
        PlayClip(clips[Random.Range(0,clips.Length)]);
    }
    
    public void PlayClip(AudioClip audioClip) {
        if(audioClip==null) {
            Debug.LogError("Missing audioclip: " + audioClip);
        }
        else {
            audioSourceSFX.PlayOneShot(audioClip);
        }
    }

    public void PlayClipUninterrupted(AudioClip audioClip) {
        if(audioClip==null) {
            Debug.LogError("Missing audioclip: " + audioClip);
        }
        else if (!audioSourceSFX.isPlaying) {
            audioSourceSFX.clip = audioClip;
            audioSourceSFX.Play();
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
