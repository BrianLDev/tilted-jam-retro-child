using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityUtilities;

public class AudioManager : SingletonMono<AudioManager> {

    public AudioClip musicIntro;
    public AudioClip musicMario;
    public AudioClip musicZelda;
    public AudioClip musicFinal;
    public AudioClip musicEnd;
    public AudioClip[] audioClips;
    [SerializeField]
    private AudioSource audioSourceSFX;
    [SerializeField]
    private AudioSource audioSourceMusic;

    private new void Awake() {
        base.Awake();
        SceneManager.sceneLoaded += OnSceneLoaded;  // subscribe to OnSceneLoaded event to play music for that scene
    }
    
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
            audioSourceMusic.loop = true;
            audioSourceMusic.Play();
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        Debug.Log("Checking for new song on scene " + scene.name);

        if (scene.name == "01-1-Intro") {
            Debug.Log("Playing intro music");
            PlayMusic(musicIntro);
        }
        else if (scene.name == "02-MarioScene") {
            Debug.Log("Playing mario music");
            PlayMusic(musicMario);
        }
        else if (scene.name == "04-FinalScene") {
            Debug.Log("Playing final music");
            PlayMusic(musicFinal);
        }
        else if (scene.name == "05-end") {
            Debug.Log("Playing end music");
            PlayMusic(musicEnd);
        }
    }

}
