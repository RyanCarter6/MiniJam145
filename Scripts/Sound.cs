using UnityEngine.Audio;
using UnityEngine;

// Used by AudioManager to create sounds
[System.Serializable]
public class Sound 
{
    public string name;

    public AudioClip clip;
    public AudioMixerGroup output;

    [Range(0f, 1f)]
    public float volume;

    [Range(.1f, 3f)]
    public float pitch;

    [Range(-1f, 1f)]
    public float stereoPan;

    public bool loop;

    [HideInInspector]
    public AudioSource source;

}