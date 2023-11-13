using UnityEngine;
using System;

// Used in two scenes, intro and main game
public class AudioManager : MonoBehaviour
{
    // Makes AudioManager accessible from other scripts
    private static AudioManager instance;
    public static AudioManager i { get { return instance; } }

    // Create a list of sounds in editor
    [SerializeField] private Sound[] sounds;

    // Initializes variables, called at start of game
    void Awake()
    {
        // Sets static reference
        instance = gameObject.GetComponent<AudioManager>();

        // Create sounds
        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.outputAudioMixerGroup = s.output;
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.panStereo = s.stereoPan;
        }
    }

    // Play sound
    public void Play(string name)
    {
        // Find sound
        Sound s = Array.Find(sounds, sound => sound.name == name);

        // If no sound exists, stop
        if(s == null){ return; }

        // Play sound
        s.source.Play();
    }

    // Stop sound
    public void Stop(string name)
    {
        // Find sound
        Sound s = Array.Find(sounds, sound => sound.name == name);
        
        /// If no sound exists, stop
        if(s == null){ return; }

        // Stop sound
        s.source.Stop();
    }
}
